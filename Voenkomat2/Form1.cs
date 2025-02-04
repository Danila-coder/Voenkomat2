namespace Voenkomat2
{
    public partial class Form1 : Form
    {
        // ������� ������
        private const string userUsername = "user";
        private const string userPassword = "user123";
        private const string voenVoenname = "voen";
        private const string voenPassword = "voen123";
        private const string adminAdminname = "admin";
        private const string adminPassword = "admin123";

        public Form1()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (username == userUsername && password == userPassword)
            {
                MessageBox.Show("����������� �������!", "�����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UserForm mainForm = new UserForm();
                mainForm.Show();
                this.Hide();

                // ���� ������ ��������� ���������� ��� �������� ������ �����
                mainForm.FormClosed += (s, args) => Application.Exit();
            }
            else if (username == voenVoenname && password == voenPassword)
            {
                MessageBox.Show("����������� �������!", "�����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                VoenForm mainForm = new VoenForm();
                mainForm.Show();
                this.Hide();

                // ���� ������ ��������� ���������� ��� �������� ������ �����
                mainForm.FormClosed += (s, args) => Application.Exit();
            }
            else if (username == adminAdminname && password == adminPassword)
            {
                MessageBox.Show("����������� �������!", "�����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AdminForm mainForm = new AdminForm();
                mainForm.Show();
                this.Hide();

                // ���� ������ ��������� ���������� ��� �������� ������ �����
                mainForm.FormClosed += (s, args) => Application.Exit();
            }
            else
            {
                MessageBox.Show("�������� ����� ��� ������!", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
