//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Data.SQLite;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace Voenkomat2
//{
//    public partial class AdminForm : Form
//    {
//        private SQLiteConnection sqliteConn;

//        public AdminForm()
//        {
//            InitializeComponent();
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

//    }
//}

//using System.Data.SQLite;

//namespace Voenkomat2
//{
//    public partial class AdminForm : Form
//    {
//        private SQLiteConnection sqliteConn;

//        public AdminForm()
//        {
//            InitializeComponent();
//            InitializeComboBox();  // Инициализация ComboBox
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

//        // Метод для инициализации ComboBox с двумя вариантами
//        private void InitializeComboBox()
//        {
//            comboBox.Items.Clear();  // Очищаем ComboBox
//            comboBox.Items.Add("Вид осмотра");  // Добавляем первый элемент
//            comboBox.Items.Add("Основание отсрочки");  // Добавляем второй элемент
//            comboBox.SelectedIndex = 0;  // Устанавливаем выбранный элемент по умолчанию
//        }
//    }
//}

using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Voenkomat2
{
    public partial class AdminForm : Form
    {
        private SQLiteConnection sqliteConn;

        public AdminForm()
        {
            InitializeComponent();
            InitializeComboBox();  // Инициализация ComboBox
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

        // Метод для инициализации ComboBox с двумя вариантами
        private void InitializeComboBox()
        {
            comboBox.Items.Clear();  // Очищаем ComboBox
            comboBox.Items.Add("Вид осмотра");  // Добавляем первый элемент
            comboBox.Items.Add("Основание отсрочки");  // Добавляем второй элемент
            comboBox.SelectedIndex = 0;  // Устанавливаем выбранный элемент по умолчанию
            comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;  // Обработчик события изменения выбранного элемента
        }

        // Обработчик события изменения выбранного элемента в ComboBox
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Проверяем, что выбранный элемент не пустой
            if (comboBox.SelectedIndex == 0) // Вид осмотра
            {
                LoadDataFromDatabase("Вид осмотра", "Наименование");
            }
            else if (comboBox.SelectedIndex == 1) // Основание отсрочки
            {
                LoadDataFromDatabase("Основание отсрочки", "Название");
            }
        }

        // Метод для загрузки данных из базы данных и отображения в ListView
        private void LoadDataFromDatabase(string tableName, string columnName)
        {
            // Очищаем ListView перед добавлением новых данных
            listViewData.Items.Clear();

            try
            {
                ConnectToDatabase();  // Подключаемся к базе данных

                string query = $"SELECT ID, {columnName} FROM [{tableName}]";
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);
                SQLiteDataReader reader = command.ExecuteReader();

                // Проходим по данным и добавляем их в ListView
                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["ID"].ToString());
                    item.SubItems.Add(reader[columnName].ToString());
                    listViewData.Items.Add(item);
                }
                reader.Close(); // Закрываем reader
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }
    }
}




