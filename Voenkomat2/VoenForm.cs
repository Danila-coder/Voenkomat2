//using System;
//using System.Data;
//using System.Data.SQLite;
//using System.Windows.Forms;

//namespace Voenkomat2
//{
//    public partial class VoenForm : Form
//    {
//        private SQLiteConnection sqliteConn;

//        public VoenForm()
//        {
//            InitializeComponent();
//            ConnectToDatabase();
//            LoadHistoryData();
//            dataGridView1.RowPostPaint += DataGridView1_RowPostPaint;

//            comboBoxCategory.Items.Add("");
//            comboBoxCategory.Items.Add("Гражданин");
//            comboBoxCategory.Items.Add("Медосмотр");
//            comboBoxCategory.Items.Add("Военная служба");
//            comboBoxCategory.Items.Add("Отсрочка");

//            comboBoxAction.Items.Add("");
//            comboBoxAction.Items.Add("Добавление");
//            comboBoxAction.Items.Add("Обновление");
//            comboBoxAction.Items.Add("Удаление");


//            comboBoxCategory.SelectedIndex = 0;
//            comboBoxAction.SelectedIndex = 0;
//        }

//        // Метод для подключения к базе данных SQLite
//        private void ConnectToDatabase()
//        {
//            try
//            {
//                // Укажите путь к вашей базе данных SQLite
//                string connectionString = "Data Source=Voenkomat.db;Version=3;";  // Путь к базе данных
//                sqliteConn = new SQLiteConnection(connectionString);  // Создаем подключение
//                sqliteConn.Open();  // Открываем соединение с базой данных
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message);  // Обработка ошибки подключения
//            }
//        }

//        // Метод для загрузки данных из таблицы "История" и отображения в DataGridView
//        private void LoadHistoryData()
//        {
//            try
//            {
//                // Модифицированный запрос, исключающий столбец ID
//                string query = "SELECT Таблица, Действие, [Старое значение], [Новое значение], [Дата изменения] FROM История";
//                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(query, sqliteConn);
//                DataTable dataTable = new DataTable();  // Создаем таблицу для хранения данных

//                // Заполняем таблицу данными из запроса
//                dataAdapter.Fill(dataTable);

//                // Привязываем данные к DataGridView
//                dataGridView1.DataSource = dataTable;  // dataGridViewHistory — это ваш элемент на форме

//                // Применяем форматирование для длинных текстов в других столбцах
//                FormatLongTextColumns(dataTable);

//                // Настройка отображения многострочного текста в DataGridView
//                EnableMultiLineDisplay();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);  // Обработка ошибки
//            }
//        }

//        // Метод для форматирования длинных текстов в таблице
//        private void FormatLongTextColumns(DataTable dataTable)
//        {
//            foreach (DataRow row in dataTable.Rows)
//            {
//                // Например, форматируем строку для столбца "Старое значение" и других длинных строк
//                if (row["Старое значение"] != DBNull.Value)
//                {
//                    row["Старое значение"] = FormatLongString(row["Старое значение"].ToString());
//                }

//                if (row["Новое значение"] != DBNull.Value)
//                {
//                    row["Новое значение"] = FormatLongString(row["Новое значение"].ToString());
//                }
//            }
//        }

//        // Метод для добавления переносов строк в длинную строку
//        private string FormatLongString(string input)
//        {
//            // Разбиваем строку на части по запятой и создаем многострочный текст
//            var formattedText = input.Replace(", ", ",\n");

//            // Возвращаем отформатированную строку
//            return formattedText;
//        }

//        // Метод для разрешения многострочного отображения текста в DataGridView
//        private void EnableMultiLineDisplay()
//        {
//            // Устанавливаем режим переноса текста для всех ячеек в каждом столбце
//            foreach (DataGridViewColumn column in dataGridView1.Columns)
//            {
//                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;  // Разрешаем перенос текста
//            }

//            // Подгоняем высоту строк автоматически
//            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;  // Автоматически меняем высоту строк под все ячейки
//            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;  // Автоматически подстраиваем ширину столбцов
//        }

//        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
//        {
//            // Получаем стиль для рисования номера строки (используем стиль шрифта из DataGridView)
//            using (SolidBrush brush = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
//            {
//                // Получаем позицию строки на экране и рисуем номер строки
//                string rowNumber = (e.RowIndex + 1).ToString();

//                // Рисуем номер строки в левом верхнем углу каждой строки
//                e.Graphics.DrawString(rowNumber, dataGridView1.RowHeadersDefaultCellStyle.Font, brush,
//                    e.RowBounds.Location.X + 19, e.RowBounds.Location.Y + 51);
//            }
//        }
//    }
//}

using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Voenkomat2
{
    public partial class VoenForm : Form
    {
        private SQLiteConnection sqliteConn;

        public VoenForm()
        {
            InitializeComponent();
            ConnectToDatabase();
            LoadHistoryData();
            dataGridView1.RowPostPaint += DataGridView1_RowPostPaint;

            comboBoxCategory.Items.Add("");
            comboBoxCategory.Items.Add("Гражданин");
            comboBoxCategory.Items.Add("Медосмотр");
            comboBoxCategory.Items.Add("Военная служба");
            comboBoxCategory.Items.Add("Отсрочка");

            comboBoxAction.Items.Add("");
            comboBoxAction.Items.Add("Добавление");
            comboBoxAction.Items.Add("Обновление");
            comboBoxAction.Items.Add("Удаление");

            comboBoxCategory.SelectedIndex = 0;
            comboBoxAction.SelectedIndex = 0;

            // Привязываем обработчики для кнопок
            this.buttonSearch.Click += new EventHandler(this.buttonSearch_Click);
            this.buttonReset.Click += new EventHandler(this.buttonReset_Click);
        }

        // Метод для подключения к базе данных SQLite
        private void ConnectToDatabase()
        {
            try
            {
                string connectionString = "Data Source=Voenkomat.db;Version=3;";
                sqliteConn = new SQLiteConnection(connectionString);
                sqliteConn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message);
            }
        }

        // Метод для загрузки данных из таблицы "История" и отображения в DataGridView
        private void LoadHistoryData()
        {
            try
            {
                string query = "SELECT Таблица, Действие, [Старое значение], [Новое значение], [Дата изменения] FROM История";
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(query, sqliteConn);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;

                // Применяем форматирование для длинных текстов в других столбцах
                FormatLongTextColumns(dataTable);

                // Настройка отображения многострочного текста в DataGridView
                EnableMultiLineDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        // Метод для форматирования длинных текстов в таблице
        private void FormatLongTextColumns(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                if (row["Старое значение"] != DBNull.Value)
                {
                    row["Старое значение"] = FormatLongString(row["Старое значение"].ToString());
                }

                if (row["Новое значение"] != DBNull.Value)
                {
                    row["Новое значение"] = FormatLongString(row["Новое значение"].ToString());
                }
            }
        }

        // Метод для добавления переносов строк в длинную строку
        private string FormatLongString(string input)
        {
            var formattedText = input.Replace(", ", ",\n");
            return formattedText;
        }

        // Метод для разрешения многострочного отображения текста в DataGridView
        private void EnableMultiLineDisplay()
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void SearchHistoryData()
        {
            try
            {
                // Стартовый запрос
                string query = "SELECT Таблица, Действие, [Старое значение], [Новое значение], [Дата изменения] FROM История WHERE 1=1";

                // Фильтрация по категории
                if (!string.IsNullOrEmpty(comboBoxCategory.SelectedItem?.ToString()))
                {
                    query += " AND Таблица = @Table";
                }

                // Фильтрация по действию
                if (!string.IsNullOrEmpty(comboBoxAction.SelectedItem?.ToString()))
                {
                    query += " AND Действие = @Action";
                }

                // Форматируем даты в нужный формат (dd-MM-yyyy HH:mm:ss)
                string startDate = dateTimePicker1.Value.ToString("dd-MM-yyyy HH:mm:ss");
                string endDate = dateTimePicker2.Value.ToString("dd-MM-yyyy HH:mm:ss");

                // Добавляем фильтрацию по диапазону дат и времени
                query += " AND [Дата изменения] BETWEEN @StartDate AND @EndDate";

                // Создаем команду SQL
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);

                // Добавляем параметры для защиты от SQL инъекций
                if (!string.IsNullOrEmpty(comboBoxCategory.SelectedItem?.ToString()))
                {
                    command.Parameters.AddWithValue("@Table", comboBoxCategory.SelectedItem.ToString());
                }

                if (!string.IsNullOrEmpty(comboBoxAction.SelectedItem?.ToString()))
                {
                    command.Parameters.AddWithValue("@Action", comboBoxAction.SelectedItem.ToString());
                }

                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                // Выполняем запрос
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Проверка на наличие записей
                if (dataTable.Rows.Count > 0)
                {
                    // Записи найдены, выводим количество найденных записей
                    MessageBox.Show($"Записи найдены. Количество: {dataTable.Rows.Count}", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Записей не найдено
                    MessageBox.Show("Записей не найдено", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Отображаем результаты в DataGridView
                dataGridView1.DataSource = dataTable;

                // Применяем форматирование для длинных текстов
                FormatLongTextColumns(dataTable);
                EnableMultiLineDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при поиске данных: " + ex.Message);
            }
        }




        // Обработчик нажатия кнопки buttonSearch (поиск)
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            SearchHistoryData();
        }

        // Метод для сброса фильтров и возврата к исходному отображению данных
        private void buttonReset_Click(object sender, EventArgs e)
        {
            LoadHistoryData(); // Загружаем все данные без фильтрации
        }

        // Метод для отображения номеров строк в DataGridView
        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                string rowNumber = (e.RowIndex + 1).ToString();
                e.Graphics.DrawString(rowNumber, dataGridView1.RowHeadersDefaultCellStyle.Font, brush,
                    e.RowBounds.Location.X + 19, e.RowBounds.Location.Y + 51);
            }
        }
    }
}






