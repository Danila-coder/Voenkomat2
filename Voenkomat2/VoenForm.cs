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
        }

        // Метод для подключения к базе данных SQLite
        private void ConnectToDatabase()
        {
            try
            {
                // Укажите путь к вашей базе данных SQLite
                string connectionString = "Data Source=Voenkomat.db;Version=3;";  // Путь к базе данных
                sqliteConn = new SQLiteConnection(connectionString);  // Создаем подключение
                sqliteConn.Open();  // Открываем соединение с базой данных
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message);  // Обработка ошибки подключения
            }
        }

        // Метод для загрузки данных из таблицы "История" и отображения в DataGridView
        private void LoadHistoryData()
        {
            try
            {
                // Модифицированный запрос, исключающий столбец ID
                string query = "SELECT Таблица, Действие, [Старое значение], [Новое значение], [Дата изменения] FROM История";
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(query, sqliteConn);
                DataTable dataTable = new DataTable();  // Создаем таблицу для хранения данных

                // Заполняем таблицу данными из запроса
                dataAdapter.Fill(dataTable);

                // Привязываем данные к DataGridView
                dataGridView1.DataSource = dataTable;  // dataGridViewHistory — это ваш элемент на форме

                // Применяем форматирование для длинных текстов в других столбцах
                FormatLongTextColumns(dataTable);

                // Настройка отображения многострочного текста в DataGridView
                EnableMultiLineDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);  // Обработка ошибки
            }
        }

        // Метод для форматирования длинных текстов в таблице
        private void FormatLongTextColumns(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                // Например, форматируем строку для столбца "Старое значение" и других длинных строк
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
            // Разбиваем строку на части по запятой и создаем многострочный текст
            var formattedText = input.Replace(", ", ",\n");

            // Возвращаем отформатированную строку
            return formattedText;
        }

        // Метод для разрешения многострочного отображения текста в DataGridView
        private void EnableMultiLineDisplay()
        {
            // Устанавливаем режим переноса текста для всех ячеек в каждом столбце
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;  // Разрешаем перенос текста
            }

            // Подгоняем высоту строк автоматически
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;  // Автоматически меняем высоту строк под все ячейки
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;  // Автоматически подстраиваем ширину столбцов
        }

        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Получаем стиль для рисования номера строки (используем стиль шрифта из DataGridView)
            using (SolidBrush brush = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                // Получаем позицию строки на экране и рисуем номер строки
                string rowNumber = (e.RowIndex + 1).ToString();

                // Рисуем номер строки в левом верхнем углу каждой строки
                e.Graphics.DrawString(rowNumber, dataGridView1.RowHeadersDefaultCellStyle.Font, brush,
                    e.RowBounds.Location.X + 19, e.RowBounds.Location.Y + 51);
            }
        }

    }
}