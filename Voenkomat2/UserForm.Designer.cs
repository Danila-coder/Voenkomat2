namespace Voenkomat2
{
    partial class UserForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            buttonRecord = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            dateTimePicker1 = new DateTimePicker();
            textBox4 = new TextBox();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            textBoxSearch = new TextBox();
            label8 = new Label();
            buttonSearch = new Button();
            buttonClearFilter = new Button();
            buttonSaveChanges = new Button();
            dataGridView2 = new DataGridView();
            comboBox3 = new ComboBox();
            dateTimePicker2 = new DateTimePicker();
            dateTimePicker3 = new DateTimePicker();
            buttonAddDeferment = new Button();
            label9 = new Label();
            label10 = new Label();
            label12 = new Label();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            label16 = new Label();
            label15 = new Label();
            label14 = new Label();
            textBoxResult = new TextBox();
            buttonAddExam = new Button();
            dateTimePickerExamDate = new DateTimePicker();
            comboBoxDoctor = new ComboBox();
            dataGridView3 = new DataGridView();
            tabPage3 = new TabPage();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(949, 478);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            // 
            // buttonRecord
            // 
            buttonRecord.Location = new Point(287, 656);
            buttonRecord.Name = "buttonRecord";
            buttonRecord.Size = new Size(124, 29);
            buttonRecord.TabIndex = 1;
            buttonRecord.Text = "Создать";
            buttonRecord.UseVisualStyleBackColor = true;
            buttonRecord.Click += buttonRecord_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(3, 507);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(257, 27);
            textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(3, 560);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(257, 27);
            textBox2.TabIndex = 3;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(3, 613);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(257, 27);
            textBox3.TabIndex = 4;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(3, 666);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(257, 27);
            dateTimePicker1.TabIndex = 5;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(287, 559);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(254, 27);
            textBox4.TabIndex = 6;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(287, 505);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(254, 28);
            comboBox1.TabIndex = 7;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(287, 612);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(254, 28);
            comboBox2.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 486);
            label1.Name = "label1";
            label1.Size = new Size(73, 20);
            label1.TabIndex = 9;
            label1.Text = "Фамилия";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 537);
            label2.Name = "label2";
            label2.Size = new Size(39, 20);
            label2.TabIndex = 10;
            label2.Text = "Имя";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 590);
            label3.Name = "label3";
            label3.Size = new Size(72, 20);
            label3.TabIndex = 11;
            label3.Text = "Отчество";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 643);
            label4.Name = "label4";
            label4.Size = new Size(116, 20);
            label4.TabIndex = 12;
            label4.Text = "Дата рождения";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(287, 483);
            label5.Name = "label5";
            label5.Size = new Size(104, 20);
            label5.TabIndex = 13;
            label5.Text = "Образование";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(287, 537);
            label6.Name = "label6";
            label6.Size = new Size(51, 20);
            label6.TabIndex = 14;
            label6.Text = "Адрес";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(287, 590);
            label7.Name = "label7";
            label7.Size = new Size(52, 20);
            label7.TabIndex = 15;
            label7.Text = "Статус";
            // 
            // textBoxSearch
            // 
            textBoxSearch.Location = new Point(3, 27);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new Size(386, 27);
            textBoxSearch.TabIndex = 16;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(3, 4);
            label8.Name = "label8";
            label8.Size = new Size(220, 20);
            label8.TabIndex = 17;
            label8.Text = "Введите значение для  поиска";
            // 
            // buttonSearch
            // 
            buttonSearch.Location = new Point(3, 60);
            buttonSearch.Name = "buttonSearch";
            buttonSearch.Size = new Size(94, 29);
            buttonSearch.TabIndex = 18;
            buttonSearch.Text = "Искать";
            buttonSearch.UseVisualStyleBackColor = true;
            buttonSearch.Click += buttonSearch_Click;
            // 
            // buttonClearFilter
            // 
            buttonClearFilter.Location = new Point(103, 60);
            buttonClearFilter.Name = "buttonClearFilter";
            buttonClearFilter.Size = new Size(94, 29);
            buttonClearFilter.TabIndex = 19;
            buttonClearFilter.Text = "Сброс";
            buttonClearFilter.UseVisualStyleBackColor = true;
            buttonClearFilter.Click += buttonClearFilter_Click;
            // 
            // buttonSaveChanges
            // 
            buttonSaveChanges.Location = new Point(417, 656);
            buttonSaveChanges.Name = "buttonSaveChanges";
            buttonSaveChanges.Size = new Size(124, 29);
            buttonSaveChanges.TabIndex = 20;
            buttonSaveChanges.Text = "Редактировать";
            buttonSaveChanges.UseVisualStyleBackColor = true;
            buttonSaveChanges.Click += buttonSaveChanges_Click;
            // 
            // dataGridView2
            // 
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(6, 6);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(568, 539);
            dataGridView2.TabIndex = 21;
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(6, 571);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(182, 28);
            comboBox3.TabIndex = 22;
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Location = new Point(6, 625);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(182, 27);
            dateTimePicker2.TabIndex = 23;
            // 
            // dateTimePicker3
            // 
            dateTimePicker3.Location = new Point(215, 572);
            dateTimePicker3.Name = "dateTimePicker3";
            dateTimePicker3.Size = new Size(182, 27);
            dateTimePicker3.TabIndex = 24;
            // 
            // buttonAddDeferment
            // 
            buttonAddDeferment.Location = new Point(403, 623);
            buttonAddDeferment.Name = "buttonAddDeferment";
            buttonAddDeferment.Size = new Size(124, 29);
            buttonAddDeferment.TabIndex = 26;
            buttonAddDeferment.Text = "Создать";
            buttonAddDeferment.UseVisualStyleBackColor = true;
            buttonAddDeferment.Click += buttonAddDeferment_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(6, 602);
            label9.Name = "label9";
            label9.Size = new Size(94, 20);
            label9.TabIndex = 27;
            label9.Text = "Дата начала";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(215, 548);
            label10.Name = "label10";
            label10.Size = new Size(121, 20);
            label10.TabIndex = 28;
            label10.Text = "Дата окончания";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(6, 548);
            label12.Name = "label12";
            label12.Size = new Size(87, 20);
            label12.TabIndex = 30;
            label12.Text = "Основание";
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(dataGridView1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(textBox3);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(dateTimePicker1);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(comboBox2);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(buttonSaveChanges);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(textBox4);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(buttonRecord);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(960, 701);
            panel1.TabIndex = 31;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.Fixed3D;
            panel2.Controls.Add(textBoxSearch);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(buttonSearch);
            panel2.Controls.Add(buttonClearFilter);
            panel2.Location = new Point(556, 487);
            panel2.Name = "panel2";
            panel2.Size = new Size(396, 207);
            panel2.TabIndex = 32;
            // 
            // panel3
            // 
            panel3.BorderStyle = BorderStyle.Fixed3D;
            panel3.Controls.Add(tabControl1);
            panel3.Location = new Point(983, 12);
            panel3.Name = "panel3";
            panel3.Size = new Size(596, 701);
            panel3.TabIndex = 32;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(3, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(586, 691);
            tabControl1.TabIndex = 33;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dateTimePicker2);
            tabPage1.Controls.Add(dataGridView2);
            tabPage1.Controls.Add(buttonAddDeferment);
            tabPage1.Controls.Add(dateTimePicker3);
            tabPage1.Controls.Add(label9);
            tabPage1.Controls.Add(label10);
            tabPage1.Controls.Add(label12);
            tabPage1.Controls.Add(comboBox3);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(578, 658);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Отсрочки";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label16);
            tabPage2.Controls.Add(label15);
            tabPage2.Controls.Add(label14);
            tabPage2.Controls.Add(textBoxResult);
            tabPage2.Controls.Add(buttonAddExam);
            tabPage2.Controls.Add(dateTimePickerExamDate);
            tabPage2.Controls.Add(comboBoxDoctor);
            tabPage2.Controls.Add(dataGridView3);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(578, 658);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Медосмотры";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(6, 548);
            label16.Name = "label16";
            label16.Size = new Size(75, 20);
            label16.TabIndex = 9;
            label16.Text = "Результат";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(6, 601);
            label15.Name = "label15";
            label15.Size = new Size(43, 20);
            label15.TabIndex = 8;
            label15.Text = "Врач";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(6, 495);
            label14.Name = "label14";
            label14.Size = new Size(104, 20);
            label14.TabIndex = 7;
            label14.Text = "Дата осмотра";
            // 
            // textBoxResult
            // 
            textBoxResult.Location = new Point(6, 571);
            textBoxResult.Name = "textBoxResult";
            textBoxResult.Size = new Size(250, 27);
            textBoxResult.TabIndex = 5;
            // 
            // buttonAddExam
            // 
            buttonAddExam.Location = new Point(272, 623);
            buttonAddExam.Name = "buttonAddExam";
            buttonAddExam.Size = new Size(94, 29);
            buttonAddExam.TabIndex = 1;
            buttonAddExam.Text = "Создать";
            buttonAddExam.UseVisualStyleBackColor = true;
            buttonAddExam.Click += buttonAddExam_Click;
            // 
            // dateTimePickerExamDate
            // 
            dateTimePickerExamDate.Location = new Point(6, 518);
            dateTimePickerExamDate.Name = "dateTimePickerExamDate";
            dateTimePickerExamDate.Size = new Size(250, 27);
            dateTimePickerExamDate.TabIndex = 4;
            // 
            // comboBoxDoctor
            // 
            comboBoxDoctor.FormattingEnabled = true;
            comboBoxDoctor.Location = new Point(6, 624);
            comboBoxDoctor.Name = "comboBoxDoctor";
            comboBoxDoctor.Size = new Size(250, 28);
            comboBoxDoctor.TabIndex = 3;
            // 
            // dataGridView3
            // 
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Location = new Point(6, 6);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.RowHeadersWidth = 51;
            dataGridView3.Size = new Size(566, 486);
            dataGridView3.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(578, 658);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Служба";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // UserForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1583, 721);
            Controls.Add(panel3);
            Controls.Add(panel1);
            Name = "UserForm";
            Text = "UserForm";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Button buttonRecord;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private DateTimePicker dateTimePicker1;
        private TextBox textBox4;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private TextBox textBoxSearch;
        private Label label8;
        private Button buttonSearch;
        private Button buttonClearFilter;
        private Button buttonSaveChanges;
        private DataGridView dataGridView2;
        private ComboBox comboBox3;
        private DateTimePicker dateTimePicker2;
        private DateTimePicker dateTimePicker3;
        private Button buttonAddDeferment;
        private Label label9;
        private Label label10;
        private Label label12;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private DataGridView dataGridView3;
        private Button buttonAddExam;
        private TextBox textBoxResult;
        private DateTimePicker dateTimePickerExamDate;
        private ComboBox comboBoxDoctor;
        private Label label16;
        private Label label15;
        private Label label14;
        private TabPage tabPage3;
    }
}