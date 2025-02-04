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

        // Метод для поиска данных
        private void SearchHistoryData()
        {
            try
            {
                // Получаем значения фильтров
                string category = comboBoxCategory.SelectedItem?.ToString();
                string action = comboBoxAction.SelectedItem?.ToString();
                string startDate = dateTimePicker1.Value.ToString("dd-MM-yyyy HH:mm:ss");
                string endDate = dateTimePicker2.Value.ToString("dd-MM-yyyy HH:mm:ss");

                // Выводим значения для проверки
                MessageBox.Show($"Категория: {category}, Действие: {action}, Дата начала: {startDate}, Дата окончания: {endDate}");

                // Стартовый запрос
                string query = "SELECT Таблица, Действие, [Старое значение], [Новое значение], [Дата изменения] FROM История WHERE 1=1";

                // Фильтрация по категории, если выбрана не пустая категория
                if (!string.IsNullOrEmpty(category) && category != "")
                {
                    query += " AND Таблица = @Table";
                }

                // Фильтрация по действию, если выбрано не пустое действие
                if (!string.IsNullOrEmpty(action) && action != "")
                {
                    query += " AND Действие = @Action";
                }

                // Добавляем фильтрацию по диапазону дат и времени
                query += " AND [Дата изменения] BETWEEN @StartDate AND @EndDate";

                // Создаем команду SQL
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);

                // Добавляем параметры для защиты от SQL инъекций
                if (!string.IsNullOrEmpty(category) && category != "")
                {
                    command.Parameters.AddWithValue("@Table", category);
                }

                if (!string.IsNullOrEmpty(action) && action != "")
                {
                    command.Parameters.AddWithValue("@Action", action);
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
                    MessageBox.Show($"Записи найдены. Количество: {dataTable.Rows.Count}", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
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

        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                // Проверяем, является ли текущая строка последней
                if (e.RowIndex == dataGridView1.Rows.Count - 1)
                {
                    return;
                }
                // Получаем номер строки
                string rowNumber = (e.RowIndex + 1).ToString();
                // Вычисляем позицию по оси X для номера строки
                int xPosition = e.RowBounds.Left + 16; // Отступ от левого края
                // Вычисляем позицию по оси Y для номера строки
                int yPosition = e.RowBounds.Top + (e.RowBounds.Height - dataGridView1.RowHeadersDefaultCellStyle.Font.Height) / 2;
                // Отображаем номер строки в заголовке
                e.Graphics.DrawString(rowNumber, dataGridView1.RowHeadersDefaultCellStyle.Font, brush, xPosition, yPosition);
            }
        }
    }
}
