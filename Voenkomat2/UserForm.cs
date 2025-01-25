using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SQLite;
using System.Windows.Forms.VisualStyles;

namespace Voenkomat2
{
    public partial class UserForm : Form
    {
        private SQLiteConnection sqliteConn;

        public UserForm()
        {
            InitializeComponent();
            ConnectToDatabase();
            LoadData();
            SubscribeToRowPostPaint();


            FillComboBoxes();
            FillDefermentComboBox();
            LoadComboBoxes();
            LoadRankComboBox();
            LoadStatusComboBox();
        }

        private void ConnectToDatabase()
        {
            try
            {
                // Укажите путь к вашей базе данных SQLite
                string connectionString = "Data Source=Voenkomat.db;Version=3;";
                sqliteConn = new SQLiteConnection(connectionString);
                sqliteConn.Open();

                // SQL-запрос для обновления статуса всех отсрочек
                string updateQuery = @"
        UPDATE Отсрочка
        SET Статус = 
            CASE 
                WHEN DATE('now') BETWEEN 
                    strftime('%Y-%m-%d', substr([Дата начала], 7, 4) || '-' || substr([Дата начала], 4, 2) || '-' || substr([Дата начала], 1, 2)) 
                    AND 
                    strftime('%Y-%m-%d', substr([Дата окончания], 7, 4) || '-' || substr([Дата окончания], 4, 2) || '-' || substr([Дата окончания], 1, 2)) 
                THEN 'Активна'
                ELSE 'Закрыта'
            END
    ";

                // Выполняем запрос для обновления статусов
                SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, sqliteConn);
                updateCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения или выполнения запроса: " + ex.Message);
            }


        }

        private void LoadData()
        {
            try
            {
                // Добавляем ID в запрос для выборки данных
                string query = "SELECT ID, Фамилия, Имя, Отчество, [Дата рождения], Образование, Адрес, Статус FROM Гражданин";
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Заполнение DataGridView данными
                dataGridView1.DataSource = dataTable;

                // Скрываем столбец ID (если не нужно его показывать)
                dataGridView1.Columns["ID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        // Заполнение ComboBox значениями
        private void FillComboBoxes()
        {
            // Добавление значений в ComboBox для образования
            comboBox1.Items.Add("Начальное");
            comboBox1.Items.Add("Среднее");
            comboBox1.Items.Add("Среднее специальное");
            comboBox1.Items.Add("Высшее");
            comboBox1.Items.Add("Неоконченное высшее");
            comboBox1.Items.Add("Магистратура");
            comboBox1.Items.Add("Аспирантура");

            // Выбор первого элемента по умолчанию (если нужно)
            comboBox1.SelectedIndex = 0;

            // Добавление значений в ComboBox для статуса
            comboBox2.Items.Add("На учёте");
            comboBox2.Items.Add("Служит");
            comboBox2.Items.Add("Запас");
            comboBox2.Items.Add("Отсрочка");

            // Выбор первого элемента по умолчанию (если нужно)
            comboBox2.SelectedIndex = 0;
        }

        private void buttonRecord_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем данные из текстовых полей, DateTimePicker и ComboBox
                string lastName = textBox1.Text;  // Фамилия
                string firstName = textBox2.Text; // Имя
                string middleName = textBox3.Text; // Отчество
                DateTime birthDate = dateTimePicker1.Value; // Дата рождения
                string education = comboBox1.SelectedItem?.ToString(); // Образование (выбор из ComboBox)
                string address = textBox4.Text; // Адрес (ввод в TextBox)
                string status = comboBox2.SelectedItem?.ToString(); // Статус (выбор из ComboBox)

                // Проверка, чтобы не вставлять пустые значения из ComboBox
                if (string.IsNullOrEmpty(education) || string.IsNullOrEmpty(status))
                {
                    MessageBox.Show("Выберите значение в обоих ComboBox.");
                    return;
                }

                // Проверка на возраст гражданина (должен быть старше или равен 18 лет)
                int age = DateTime.Now.Year - birthDate.Year;
                if (DateTime.Now < birthDate.AddYears(age)) age--;  // Корректировка возраста, если день рождения в этом году еще не был

                if (age < 18)
                {
                    MessageBox.Show("Гражданин должен быть старше 18 лет.");
                    return;
                }

                // SQL запрос для вставки данных
                string query = "INSERT INTO Гражданин (Фамилия, Имя, Отчество, [Дата рождения], Образование, Адрес, Статус) " +
                               "VALUES (@lastName, @firstName, @middleName, @birthDate, @education, @address, @status)";

                // Создаем команду
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@middleName", middleName);
                command.Parameters.AddWithValue("@birthDate", birthDate.ToString("dd-MM-yyyy"));
                command.Parameters.AddWithValue("@education", education);
                command.Parameters.AddWithValue("@address", address);
                command.Parameters.AddWithValue("@status", status);

                // Выполняем команду для вставки данных
                command.ExecuteNonQuery();

                // После добавления, перезагружаем данные в DataGridView
                LoadData();

                // Очистить текстовые поля после добавления
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear(); // Очистить поле ввода адреса

                // Установить индекс на первый элемент в ComboBox после добавления записи
                comboBox1.SelectedIndex = 0; // Установить первый элемент (например, "Высшее")
                comboBox2.SelectedIndex = 0; // Установить первый элемент (например, "Служит")

                // Сбросить дату на текущую
                dateTimePicker1.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении записи: " + ex.Message);
            }
        }

        // Подписка на событие RowPostPaint для всех DataGridView
        private void SubscribeToRowPostPaint()
        {
            // Подписываем на событие RowPostPaint для текущего DataGridView
            dataGridView1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(DataGridView_RowPostPaint);
            dataGridView2.RowPostPaint += new DataGridViewRowPostPaintEventHandler(DataGridView_RowPostPaint);
            dataGridView3.RowPostPaint += new DataGridViewRowPostPaintEventHandler(DataGridView_RowPostPaint);
            dataGridView4.RowPostPaint += new DataGridViewRowPostPaintEventHandler(DataGridView_RowPostPaint);
        }

        // Обработчик события RowPostPaint
        private void DataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv != null && e.RowIndex >= 0)
            {
                // Получаем номер строки
                string rowNumber = (e.RowIndex + 1).ToString();

                // Используем шрифт для заголовка строки, можно изменить на свой
                Font rowHeaderFont = dgv.RowHeadersDefaultCellStyle.Font;

                // Создаем кисть для рисования
                using (Brush brush = new SolidBrush(dgv.RowHeadersDefaultCellStyle.ForeColor))
                {
                    // Рисуем номер строки в области, где находится заголовок строки
                    e.Graphics.DrawString(rowNumber, rowHeaderFont, brush, e.RowBounds.Location.X + 24, e.RowBounds.Location.Y + 4);
                }
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = textBoxSearch.Text.Trim(); // Получаем текст из поля поиска

            if (string.IsNullOrEmpty(searchQuery))
            {
                MessageBox.Show("Пожалуйста, заполните поле поиска.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Выполнить поиск
                SearchData(searchQuery);
            }
        }

        private void buttonClearFilter_Click(object sender, EventArgs e)
        {
            // Загружаем все данные без фильтрации
            LoadData();
        }

        private void SearchData(string searchQuery)
        {
            try
            {
                // Разделяем поисковый запрос на отдельные слова
                string[] searchTerms = searchQuery.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Строим SQL запрос с операторами LIKE
                string query = "SELECT Фамилия, Имя, Отчество, [Дата рождения], Образование, Адрес, Статус FROM Гражданин WHERE ";

                for (int i = 0; i < searchTerms.Length; i++)
                {
                    string searchTerm = "%" + searchTerms[i] + "%"; // Используем % для LIKE

                    if (i > 0)
                    {
                        query += " AND "; // Добавляем логический оператор AND
                    }

                    // Добавляем условие поиска для каждого поля
                    query += "(Фамилия LIKE @SearchTerm" + i + " OR Имя LIKE @SearchTerm" + i + " OR Отчество LIKE @SearchTerm" + i + " ";
                    query += "OR Образование LIKE @SearchTerm" + i + " OR Статус LIKE @SearchTerm" + i + ")";
                }

                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);

                // Добавляем параметры для защиты от SQL инъекций
                for (int i = 0; i < searchTerms.Length; i++)
                {
                    command.Parameters.AddWithValue("@SearchTerm" + i, "%" + searchTerms[i] + "%");
                }

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Привязываем результат к DataGridView
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при поиске данных: " + ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Получаем индекс строки, по которой был клик
                int rowIndex = e.RowIndex;

                // Проверяем, что клик был на допустимой строке (не на заголовке)
                if (rowIndex >= 0)
                {
                    // Получаем значения из ячеек для выбранной строки
                    DataGridViewRow row = dataGridView1.Rows[rowIndex];

                    // Получаем ID выбранного гражданина
                    int citizenId = Convert.ToInt32(row.Cells["ID"].Value);

                    // Заполнение текстовых полей и комбинированных списков
                    textBox1.Text = row.Cells["Фамилия"].Value?.ToString() ?? string.Empty;
                    textBox2.Text = row.Cells["Имя"].Value?.ToString() ?? string.Empty;
                    textBox3.Text = row.Cells["Отчество"].Value?.ToString() ?? string.Empty;
                    textBox4.Text = row.Cells["Адрес"].Value?.ToString() ?? string.Empty;

                    // Для ComboBox
                    comboBox1.SelectedItem = row.Cells["Образование"].Value?.ToString() ?? comboBox1.Items[0].ToString();
                    comboBox2.SelectedItem = row.Cells["Статус"].Value?.ToString() ?? comboBox2.Items[0].ToString();

                    // Работа с датой рождения
                    object birthDateValue = row.Cells["Дата рождения"].Value;
                    if (birthDateValue != DBNull.Value)
                    {
                        DateTime birthDate = Convert.ToDateTime(birthDateValue);
                        dateTimePicker1.Value = birthDate;
                    }
                    else
                    {
                        dateTimePicker1.Value = DateTime.Now;
                    }

                    // Загружаем отсрочки для выбранного гражданина в DataGridView2
                    LoadDeferments(citizenId);

                    // Загружаем медосмотры для выбранного гражданина в DataGridView3
                    LoadMedicalExams(citizenId);

                    // Загружаем cлужбу для выбранного гражданина в DataGridView4
                    LoadMilitaryService(citizenId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обработке клика по ячейке: " + ex.Message);
            }
        }


        private void LoadMilitaryService(int citizenId)
        {
            try
            {
                // SQL-запрос для получения всех данных о военной службе для выбранного гражданина
                string query = @"
                        SELECT [Военная служба].ID, [Военная служба].[Начало службы], [Военная служба].[Окончание службы], 
                               [Военная служба].Подразделение, [Военная служба].Звание, [Военная служба].[Статус службы]
                        FROM [Военная служба]
                        WHERE [Военная служба].ID_Гражданина = @citizenId";

                // Создаем команду SQLite
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);

                // Добавляем параметр для ID гражданина
                command.Parameters.AddWithValue("@citizenId", citizenId);

                // Создаем адаптер данных
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                // Создаем таблицу данных
                DataTable dataTable = new DataTable();

                // Заполняем таблицу данными
                dataAdapter.Fill(dataTable);

                // Привязываем результат к DataGridView
                dataGridView4.DataSource = dataTable;

                // Если нужно скрыть столбец ID (по желанию)
                dataGridView4.Columns["ID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных военной службы: " + ex.Message);
            }
        }

        private void buttonAddMilitaryService_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем данные из элементов управления
                DateTime startDate = dateTimePickerStart.Value;  // Дата начала службы
                DateTime endDate = dateTimePickerEnd.Value;      // Дата окончания службы
                string subdivision = textBoxSubdivision.Text;    // Подразделение
                string rank = comboBoxRank.SelectedItem?.ToString(); // Звание (из ComboBox)
                string status = comboBoxStatus.SelectedItem?.ToString();  // Статус службы

                // Проверка на корректность дат
                if (endDate < startDate)
                {
                    MessageBox.Show("Дата окончания службы не может быть раньше даты начала службы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверяем, что все обязательные поля заполнены
                if (string.IsNullOrEmpty(subdivision) || string.IsNullOrEmpty(rank) || string.IsNullOrEmpty(status))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                // Получаем ID гражданина, для которого добавляется запись о военной службе
                int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex; // Или как-то иначе получаем ID гражданина
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                int citizenId = Convert.ToInt32(selectedRow.Cells["ID"].Value); // Получаем ID гражданина

                // Шаг 1: Проверяем, есть ли активные отсрочки у гражданина
                bool hasActiveDeferment = CheckActiveDeferments(citizenId);

                if (hasActiveDeferment)
                {
                    // Если есть активная отсрочка, выводим сообщение и останавливаем выполнение
                    MessageBox.Show("У гражданина есть активная отсрочка, нельзя добавлять запись о военной службе.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Шаг 2: Добавляем запись о военной службе
                string query = @"
        INSERT INTO [Военная служба] (ID_Гражданина, [Начало службы], [Окончание службы], Подразделение, Звание, [Статус службы])
        VALUES (@citizenId, @startDate, @endDate, @subdivision, @rank, @status)";

                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);
                command.Parameters.AddWithValue("@citizenId", citizenId);
                command.Parameters.AddWithValue("@startDate", startDate.ToString("dd-MM-yyyy"));
                command.Parameters.AddWithValue("@endDate", endDate.ToString("dd-MM-yyyy"));
                command.Parameters.AddWithValue("@subdivision", subdivision);
                command.Parameters.AddWithValue("@rank", rank);
                command.Parameters.AddWithValue("@status", status);

                // Выполняем команду для добавления данных
                command.ExecuteNonQuery();

                // Перезагружаем данные для DataGridView
                LoadMilitaryService(citizenId);

                // Очищаем элементы управления после добавления записи
                dateTimePickerStart.Value = DateTime.Now;
                dateTimePickerEnd.Value = DateTime.Now;
                textBoxSubdivision.Clear();
                comboBoxRank.SelectedIndex = 0;  // Сбросить на первый элемент
                comboBoxStatus.SelectedIndex = 0;  // Сбросить на первый элемент
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении записи о военной службе: " + ex.Message);
            }
        }

        private bool CheckActiveDeferments(int citizenId)
        {
            try
            {
                // SQL запрос для проверки наличия активной отсрочки
                string query = @"
        SELECT COUNT(*) 
        FROM Отсрочка 
        WHERE ID_Гражданина = @citizenId 
        AND [Дата начала] <= @currentDate 
        AND [Дата окончания] >= @currentDate 
        AND Статус = 'Активна'";  // Проверка на активную отсрочку

                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);
                command.Parameters.AddWithValue("@citizenId", citizenId);
                command.Parameters.AddWithValue("@currentDate", DateTime.Now);  // Текущая дата

                int activeDefermentCount = Convert.ToInt32(command.ExecuteScalar()); // Получаем количество активных отсрочек

                return activeDefermentCount > 0;  // Если есть хотя бы одна активная отсрочка, возвращаем true
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при проверке отсрочек: " + ex.Message);
                return false;
            }
        }

        private void LoadRankComboBox()
        {
            try
            {
                // Очищаем текущие элементы
                comboBoxRank.Items.Clear();

                // Добавляем значения для звания
                comboBoxRank.Items.Add("Рядовой");
                comboBoxRank.Items.Add("Сержант");
                comboBoxRank.Items.Add("Лейтенант");
                comboBoxRank.Items.Add("Капитан");
                comboBoxRank.Items.Add("Майор");
                comboBoxRank.Items.Add("Полковник");
                comboBoxRank.Items.Add("Генерал");

                // Устанавливаем первый элемент по умолчанию
                if (comboBoxRank.Items.Count > 0)
                {
                    comboBoxRank.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке званий: " + ex.Message);
            }
        }

        private void LoadStatusComboBox()
        {
            try
            {
                // Очищаем текущие элементы
                comboBoxStatus.Items.Clear();

                // Добавляем значения для статуса службы
                comboBoxStatus.Items.Add("Активная служба");
                comboBoxStatus.Items.Add("Демобилизован");
                comboBoxStatus.Items.Add("Арестован");
                comboBoxStatus.Items.Add("Отпуск");

                // Устанавливаем первый элемент по умолчанию
                if (comboBoxStatus.Items.Count > 0)
                {
                    comboBoxStatus.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке статусов службы: " + ex.Message);
            }
        }

        private int selectedRecordId;

        private void dataGridView4_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count > 0)
            {
                // Получаем данные выбранной строки
                DataGridViewRow selectedRow = dataGridView4.SelectedRows[0];

                // Загружаем данные в элементы управления (ComboBox, TextBox)
                textBoxSubdivision.Text = selectedRow.Cells["Подразделение"].Value.ToString();
                comboBoxRank.SelectedItem = selectedRow.Cells["Звание"].Value.ToString();
                comboBoxStatus.SelectedItem = selectedRow.Cells["Статус службы"].Value.ToString();

                // Сохраняем ID записи, чтобы потом использовать его для обновления
                selectedRecordId = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                // Запрещаем редактирование дат (не даём пользователю изменять их)
                dateTimePickerStart.Value = Convert.ToDateTime(selectedRow.Cells["Начало службы"].Value);
                dateTimePickerEnd.Value = Convert.ToDateTime(selectedRow.Cells["Окончание службы"].Value);

                // Можно оставить датам статус "Только для чтения"
                dateTimePickerStart.Enabled = false;
                dateTimePickerEnd.Enabled = false;
            }
        }

        private void buttonSaveMilitaryService_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем данные из элементов управления
                DateTime startDate = dateTimePickerStart.Value;
                DateTime endDate = dateTimePickerEnd.Value;
                string subdivision = textBoxSubdivision.Text;
                string rank = comboBoxRank.SelectedItem?.ToString();
                string status = comboBoxStatus.SelectedItem?.ToString();

                // Проверка на корректность дат
                if (endDate < startDate)
                {
                    MessageBox.Show("Дата окончания службы не может быть раньше даты начала службы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверяем, что все обязательные поля заполнены
                if (string.IsNullOrEmpty(subdivision) || string.IsNullOrEmpty(rank) || string.IsNullOrEmpty(status))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                // SQL-запрос для обновления данных в таблице "Военная служба"
                string query = @"
        UPDATE [Военная служба]
        SET Подразделение = @subdivision,
            Звание = @rank,
            [Статус службы] = @status
        WHERE ID = @recordId";

                // Создаем команду
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);

                // Добавляем параметры в команду
                command.Parameters.AddWithValue("@subdivision", subdivision);
                command.Parameters.AddWithValue("@rank", rank);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@recordId", selectedRecordId); // ID записи для обновления

                // Выполняем команду для обновления данных
                command.ExecuteNonQuery();

                // Обновляем только текущую запись в DataGridView (не все данные о гражданине)
                UpdateRowInDataGridView(selectedRecordId);

                // Очищаем элементы управления после сохранения
                textBoxSubdivision.Clear();
                comboBoxRank.SelectedIndex = 0;
                comboBoxStatus.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении записи о военной службе: " + ex.Message);
            }
        }

        private void UpdateRowInDataGridView(int recordId)
        {
            // Найдите строку по ID и обновите её
            foreach (DataGridViewRow row in dataGridView4.Rows)
            {
                if (Convert.ToInt32(row.Cells["ID"].Value) == recordId)
                {
                    row.Cells["Подразделение"].Value = textBoxSubdivision.Text;
                    row.Cells["Звание"].Value = comboBoxRank.SelectedItem?.ToString();
                    row.Cells["Статус службы"].Value = comboBoxStatus.SelectedItem?.ToString();
                    row.Cells["Начало службы"].Value = dateTimePickerStart.Value.ToString("dd-MM-yyyy");
                    row.Cells["Окончание службы"].Value = dateTimePickerEnd.Value.ToString("dd-MM-yyyy");
                    break;
                }
            }
        }

        private void LoadDeferments(int citizenId)
        {
            try
            {
                // SQL запрос для получения данных отсрочек без поля "Дата оформления"
                string query = "SELECT ID, [Дата начала], [Дата окончания], [Основание отсрочки], Статус FROM Отсрочка WHERE ID_Гражданина = @citizenId";
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);
                command.Parameters.AddWithValue("@citizenId", citizenId);

                // Создаем DataTable для хранения данных
                DataTable dt = new DataTable();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                adapter.Fill(dt);

                // Привязываем DataTable к DataGridView
                dataGridView2.DataSource = dt;

                // Скрыть столбец ID
                dataGridView2.Columns["ID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке отсрочек: " + ex.Message);
            }
        }

        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем данные из текстовых полей и комбинированных списков
                string lastName = textBox1.Text;  // Фамилия
                string firstName = textBox2.Text; // Имя
                string middleName = textBox3.Text; // Отчество
                DateTime birthDate = dateTimePicker1.Value; // Дата рождения
                string education = comboBox1.SelectedItem?.ToString(); // Образование
                string address = textBox4.Text; // Адрес
                string status = comboBox2.SelectedItem?.ToString(); // Статус

                // Проверяем, что все обязательные поля заполнены
                if (string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(middleName) ||
                    string.IsNullOrEmpty(education) || string.IsNullOrEmpty(status))
                {
                    MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                    return;
                }

                // Получаем ID выбранной строки, чтобы обновить запись в базе данных
                int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                int id = Convert.ToInt32(selectedRow.Cells["ID"].Value); // Получаем ID из скрытого столбца

                // SQL запрос для обновления данных
                string query = "UPDATE Гражданин SET Фамилия = @lastName, Имя = @firstName, Отчество = @middleName, " +
                               "[Дата рождения] = @birthDate, Образование = @education, Адрес = @address, Статус = @status " +
                               "WHERE ID = @id";

                // Создаем команду
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@middleName", middleName);
                command.Parameters.AddWithValue("@birthDate", birthDate.ToString("dd-MM-yyyy"));
                command.Parameters.AddWithValue("@education", education);
                command.Parameters.AddWithValue("@address", address);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@id", id); // Используем ID для обновления записи

                // Выполняем команду
                command.ExecuteNonQuery();

                // Перезагружаем данные в DataGridView
                LoadData();

                // Очистить текстовые поля после сохранения
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                comboBox1.SelectedIndex = 0; // Возвращаем ComboBox в начальное состояние
                comboBox2.SelectedIndex = 0;
                dateTimePicker1.Value = DateTime.Now;

                MessageBox.Show("Данные успешно обновлены.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении изменений: " + ex.Message);
            }
        }

        private void buttonAddDeferment_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем данные из элементов управления
                DateTime startDate = dateTimePicker2.Value;  // Дата начала отсрочки
                DateTime endDate = dateTimePicker3.Value;    // Дата окончания отсрочки
                string defermentReason = comboBox3.SelectedItem?.ToString(); // Основание отсрочки

                // Проверка на корректность дат
                if (endDate < startDate)
                {
                    MessageBox.Show("Дата окончания отсрочки не может быть раньше даты начала отсрочки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверяем, что все обязательные поля заполнены
                if (string.IsNullOrEmpty(defermentReason))
                {
                    MessageBox.Show("Пожалуйста, выберите основание отсрочки.");
                    return;
                }

                // Получаем ID гражданина, для которого добавляется отсрочка
                int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                int citizenId = Convert.ToInt32(selectedRow.Cells["ID"].Value); // Получаем ID гражданина

                // Определяем статус
                string status = (startDate <= DateTime.Now && DateTime.Now <= endDate) ? "Активна" : "Закрыта";

                // SQL запрос для вставки данных отсрочки без поля "Дата оформления"
                string query = "INSERT INTO Отсрочка (ID_Гражданина, [Дата начала], [Дата окончания], [Основание отсрочки], Статус) " +
                               "VALUES (@citizenId, @startDate, @endDate, @defermentReason, @status)";

                // Создаем команду
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);
                command.Parameters.AddWithValue("@citizenId", citizenId);
                command.Parameters.AddWithValue("@startDate", startDate.ToString("dd-MM-yyyy"));
                command.Parameters.AddWithValue("@endDate", endDate.ToString("dd-MM-yyyy"));
                command.Parameters.AddWithValue("@defermentReason", defermentReason);
                command.Parameters.AddWithValue("@status", status);

                // Выполняем команду для вставки данных
                command.ExecuteNonQuery();

                // После добавления, перезагружаем данные отсрочек в DataGridView2
                LoadDeferments(citizenId);

                // Очистить элементы управления после добавления
                dateTimePicker2.Value = DateTime.Now;
                dateTimePicker3.Value = DateTime.Now;
                comboBox3.SelectedIndex = 0;  // Устанавливаем первый элемент (например, "Основание 1")
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении отсрочки: " + ex.Message);
            }
        }

        private void FillDefermentComboBox()
        {
            try
            {
                // Очистить ComboBox перед заполнением
                comboBox3.Items.Clear();

                // SQL запрос для получения всех оснований отсрочки
                string query = "SELECT Название FROM [Основание отсрочки]";
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);
                SQLiteDataReader reader = command.ExecuteReader();

                // Заполняем ComboBox данными из таблицы
                while (reader.Read())
                {
                    string defermentReason = reader["Название"].ToString();
                    comboBox3.Items.Add(defermentReason);  // Добавляем название основания отсрочки
                }

                // Установить первый элемент по умолчанию (если есть элементы)
                if (comboBox3.Items.Count > 0)
                {
                    comboBox3.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке оснований отсрочки: " + ex.Message);
            }
        }

        private void LoadMedicalExams(int citizenId)
        {
            try
            {
                // SQL-запрос для получения всех данных медосмотра с включением названия вида осмотра
                string query = @"
                                SELECT Медосмотр.ID, [Вид осмотра].Наименование AS [Вид осмотра], Медосмотр.[Дата осмотра], Медосмотр.Результат, Врач.ФИО AS Врач
                                FROM Медосмотр
                                JOIN Врач ON Медосмотр.Врач = Врач.ID
                                JOIN [Вид осмотра] ON Медосмотр.[Вид осмотра] = [Вид осмотра].ID
                                WHERE Медосмотр.ID_Гражданина = @citizenId";


                // Создаем команду SQLite
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);

                // Добавляем параметр для ID гражданина
                command.Parameters.AddWithValue("@citizenId", citizenId);

                // Создаем адаптер данных
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);

                // Создаем таблицу данных
                DataTable dataTable = new DataTable();

                // Заполняем таблицу данными
                dataAdapter.Fill(dataTable);

                // Привязываем результат к DataGridView3
                dataGridView3.DataSource = dataTable;

                // Если нужно скрыть столбец ID (по желанию)
                dataGridView3.Columns["ID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных медосмотра: " + ex.Message);
            }
        }

        private void LoadComboBoxes()
        {
            try
            {
                // Запрос для получения всех врачей
                string doctorQuery = "SELECT ID, ФИО FROM Врач";
                SQLiteDataAdapter doctorAdapter = new SQLiteDataAdapter(doctorQuery, sqliteConn);
                DataTable doctorTable = new DataTable();
                doctorAdapter.Fill(doctorTable);

                // Заполнение комбобокса Врач
                comboBoxDoctor.DataSource = doctorTable;
                comboBoxDoctor.DisplayMember = "ФИО";  // Показывать ФИО врача
                comboBoxDoctor.ValueMember = "ID";     // Использовать ID врача для отправки на сервер

                // Запрос для получения всех видов осмотра
                string examTypeQuery = "SELECT ID, Наименование FROM [Вид осмотра]";
                SQLiteDataAdapter examTypeAdapter = new SQLiteDataAdapter(examTypeQuery, sqliteConn);
                DataTable examTypeTable = new DataTable();
                examTypeAdapter.Fill(examTypeTable);

                // Заполнение комбобокса Вид осмотра
                comboBoxExamType.DataSource = examTypeTable;
                comboBoxExamType.DisplayMember = "Наименование";  // Показывать наименование вида осмотра
                comboBoxExamType.ValueMember = "ID";              // Использовать ID вида осмотра для отправки в базу данных
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void buttonAddExam_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем значения из комбобоксов и текстового поля
                int doctorId = Convert.ToInt32(comboBoxDoctor.SelectedValue);      // Преобразуем в int
                DateTime examDate = dateTimePickerExamDate.Value;
                string result = textBoxResult.Text;
                int citizenId = 1; // Пример ID гражданина, этот параметр можно получить из выбранной строки DataGridView1

                // Получаем ID выбранного вида осмотра из комбобокса
                int examTypeId = Convert.ToInt32(comboBoxExamType.SelectedValue); // ID вида осмотра

                // Форматируем дату для вставки в базу данных
                string formattedDate = examDate.ToString("dd-MM-yyyy");

                // SQL запрос на добавление новой записи в таблицу Медосмотр
                string query = "INSERT INTO Медосмотр (ID_Гражданина, [Вид осмотра], [Дата осмотра], Результат, Врач) " +
                               "VALUES (@citizenId, @examTypeId, @examDate, @result, @doctorId)";

                // Создаем команду SQLite
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);

                // Добавляем параметры в команду
                command.Parameters.AddWithValue("@citizenId", citizenId);
                command.Parameters.AddWithValue("@examTypeId", examTypeId);  // Добавляем ID вида осмотра
                command.Parameters.AddWithValue("@examDate", formattedDate);  // Используем отформатированную дату
                command.Parameters.AddWithValue("@result", result);
                command.Parameters.AddWithValue("@doctorId", doctorId);

                // Выполняем команду
                command.ExecuteNonQuery();

                // Перезагружаем данные для DataGridView3
                LoadMedicalExams(citizenId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении записи медосмотра: " + ex.Message);
            }
        }
    }
}
