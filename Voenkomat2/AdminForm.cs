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
            InitializeListView();  // Инициализация ListView
        }

        private void ConnectToDatabase()
        {
            try
            {
                string connectionString = "Data Source=Voenkomat.db;Version=3;";  // Путь к базе данных
                sqliteConn = new SQLiteConnection(connectionString);  // Создаем подключение
                sqliteConn.Open();  // Открываем соединение с базой данных
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message);  // Обработка ошибки подключения
            }
        }

        private void InitializeComboBox()
        {
            comboBox.Items.Clear();  // Очищаем ComboBox
            comboBox.Items.Add("Вид осмотра");  // Добавляем первый элемент
            comboBox.Items.Add("Основание отсрочки");  // Добавляем второй элемент
            comboBox.SelectedIndex = 0;  // Устанавливаем выбранный элемент по умолчанию
            comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;  // Обработчик события изменения выбранного элемента
        }

        private void InitializeListView()
        {
            listViewData.View = View.Details;  // Установим отображение в виде деталей
            listViewData.FullRowSelect = true;  // Включаем полное выделение строки
            listViewData.MultiSelect = false;  // Разрешаем выбирать только одну строку
            listViewData.ItemSelectionChanged += ListViewData_ItemSelectionChanged;  // Подписываемся на событие выбора строки
            buttonDelete.Click += buttonDelete_Click;  // Подписываемся на событие клика кнопки удаления
        }

        private void ListViewData_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                string selectedItemText = e.Item.Text;  // Это будет само значение элемента
                string selectedColumnText = string.Empty;

                // Если в ListView есть дополнительные подколонки, то можно взять второй элемент
                if (e.Item.SubItems.Count > 1)
                {
                    selectedColumnText = e.Item.SubItems[1].Text;  // Наименование/Название
                }

                Console.WriteLine($"Вы выбрали: {selectedColumnText} (ID: {selectedItemText})");
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex == 0) // Вид осмотра
            {
                LoadDataFromDatabase("Вид осмотра", "Наименование");
            }
            else if (comboBox.SelectedIndex == 1) // Основание отсрочки
            {
                LoadDataFromDatabase("Основание отсрочки", "Название");
            }
        }

        private void LoadDataFromDatabase(string tableName, string columnName)
        {
            listViewData.Items.Clear();  // Очищаем ListView перед добавлением новых данных

            try
            {
                ConnectToDatabase();  // Подключаемся к базе данных

                string query = $"SELECT ID, {columnName} FROM [{tableName}]";
                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);
                SQLiteDataReader reader = command.ExecuteReader();

                // Проверка на наличие колонок
                if (listViewData.Columns.Count == 0)
                {
                    listViewData.Columns.Add(columnName);  // Колонка для Наименования/Названия
                }

                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader[columnName].ToString());  // Наименование/Название
                    item.Tag = reader["ID"];  // Сохраняем ID в свойстве Tag, чтобы использовать его при необходимости
                    listViewData.Items.Add(item);
                }

                reader.Close();

                // Скрываем колонку ID, устанавливаем минимальную ширину
                listViewData.Columns[0].Width = 0;  // Устанавливаем ширину равной 0, чтобы скрыть колонку

                // Автоматически подгоняем ширину колонок по содержимому
                listViewData.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            // Проверяем, есть ли выбранная строка
            if (listViewData.SelectedItems.Count > 0)
            {
                // Получаем первую выбранную строку
                ListViewItem selectedItem = listViewData.SelectedItems[0];

                // Получаем ID элемента (используем Tag)
                string id = selectedItem.Tag.ToString();

                // Удаляем строку из ListView
                listViewData.Items.Remove(selectedItem);

                // Удаляем строку из базы данных
                DeleteFromDatabase(id);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления.");
            }
        }

        // Метод для удаления строки из базы данных
        private void DeleteFromDatabase(string id)
        {
            try
            {
                ConnectToDatabase();  // Подключаемся к базе данных

                string selectedTable = comboBox.SelectedItem.ToString();
                // Строим SQL-запрос для удаления элемента по ID
                string query = $"DELETE FROM [{selectedTable}] WHERE ID = @id";

                SQLiteCommand command = new SQLiteCommand(query, sqliteConn);
                command.Parameters.AddWithValue("@id", id);

                // Выполняем запрос
                command.ExecuteNonQuery();

                MessageBox.Show("Запись удалена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении из базы данных: " + ex.Message);
            }
            finally
            {
                sqliteConn.Close();  // Закрываем подключение
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedTable = comboBox.SelectedItem.ToString();
                string inputValue = textBoxInput.Text;

                if (string.IsNullOrEmpty(inputValue))
                {
                    MessageBox.Show("Пожалуйста, введите значение для добавления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string columnName = string.Empty;
                if (selectedTable == "Вид осмотра")
                {
                    columnName = "Наименование";
                }
                else if (selectedTable == "Основание отсрочки")
                {
                    columnName = "Название";
                }

                if (string.IsNullOrEmpty(columnName))
                {
                    MessageBox.Show("Не удалось определить колонку для добавления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string query = $"INSERT INTO [{selectedTable}] ({columnName}) VALUES (@inputValue);";

                using (SQLiteConnection connection = new SQLiteConnection("Data Source=Voenkomat.db;Version=3;"))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@inputValue", inputValue);
                        command.ExecuteNonQuery();
                    }
                }

                LoadDataFromDatabase(selectedTable, columnName);
                MessageBox.Show("Данные успешно добавлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxInput.Clear();  // Очистить текстовое поле после добавления
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
