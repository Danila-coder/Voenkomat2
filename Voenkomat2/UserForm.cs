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
        }

        private void ConnectToDatabase()
        {
            //    try
            //    {
            //        // Укажите путь к вашей базе данных SQLite
            //        string connectionString = "Data Source=Voenkomat.db;Version=3;";
            //        sqliteConn = new SQLiteConnection(connectionString);
            //        sqliteConn.Open();

            //        // SQL-запрос для обновления статуса всех отсрочек
            //        string updateQuery = @"
            //    UPDATE Отсрочка
            //    SET Статус = 
            //        CASE
            //            WHEN DATE('now') BETWEEN [Дата начала] AND [Дата окончания] THEN 'Активна'
            //            ELSE 'Закрыта'
            //        END
            //";

            //        // Выполняем запрос для обновления статусов
            //        SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, sqliteConn);
            //        updateCommand.ExecuteNonQuery();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Ошибка подключения или выполнения запроса: " + ex.Message);
            //    }

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
            comboBox1.Items.Add("Высшее");
            comboBox1.Items.Add("Среднее");
            comboBox1.Items.Add("Незаконченное высшее");
            comboBox1.Items.Add("Среднее специальное");

            // Выбор первого элемента по умолчанию (если нужно)
            comboBox1.SelectedIndex = 0;

            // Добавление значений в ComboBox для статуса
            comboBox2.Items.Add("Служит");
            comboBox2.Items.Add("Пенсионер");
            comboBox2.Items.Add("Студент");

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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обработке клика по ячейке: " + ex.Message);
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
                string query = "SELECT Медосмотр.ID, Медосмотр.[Дата осмотра], Медосмотр.Результат, Врач.ФИО AS Врач " +
                       "FROM Медосмотр " +
                       "JOIN Врач ON Медосмотр.Врач = Врач.ID " +
                       "WHERE Медосмотр.ID_Гражданина = @citizenId";

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

                // Форматируем дату для вставки в базу данных
                string formattedDate = examDate.ToString("dd-MM-yyyy");

                // SQL запрос на добавление новой записи в таблицу Медосмотр
                string query = "INSERT INTO Медосмотр (ID_Гражданина, [Вид осмотра], [Дата осмотра], Результат, Врач) " +
                               "VALUES (@citizenId, @examTypeId, @examDate, @result, @doctorId)";

                // Создаем команду SQLite
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);

                // Добавляем параметры в команду
                command.Parameters.AddWithValue("@citizenId", citizenId);
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
