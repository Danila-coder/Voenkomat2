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
        }

        private void ConnectToDatabase()
        {
            try
            {
                // Укажите путь к вашей базе данных SQLite
                string connectionString = "Data Source=Voenkomat.db;Version=3;";
                sqliteConn = new SQLiteConnection(connectionString);
                sqliteConn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                // SQL запрос для получения данных, исключая поле ID
                string query = "SELECT Фамилия, Имя, Отчество, [Дата рождения], Образование, Адрес, Статус FROM Гражданин";
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Заполнение DataGridView данными
                dataGridView1.DataSource = dataTable;
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

                // SQL запрос для вставки данных
                string query = "INSERT INTO Гражданин (Фамилия, Имя, Отчество, [Дата рождения], Образование, Адрес, Статус) " +
                               "VALUES (@lastName, @firstName, @middleName, @birthDate, @education, @address, @status)";

                // Создаем команду
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@middleName", middleName);
                command.Parameters.AddWithValue("@birthDate", birthDate.ToString("yyyy-MM-dd"));
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


    }
}
