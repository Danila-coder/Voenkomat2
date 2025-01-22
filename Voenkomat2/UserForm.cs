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

                // Чтение данных
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
    
    }
}
