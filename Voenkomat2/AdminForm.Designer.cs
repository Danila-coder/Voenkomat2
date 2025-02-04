namespace Voenkomat2
{
    partial class AdminForm
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
            buttonAdd = new Button();
            buttonDelete = new Button();
            comboBox = new ComboBox();
            listViewData = new ListView();
            textBoxInput = new TextBox();
            SuspendLayout();
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(12, 140);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(94, 29);
            buttonAdd.TabIndex = 0;
            buttonAdd.Text = "Добавить";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(118, 140);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(94, 29);
            buttonDelete.TabIndex = 1;
            buttonDelete.Text = "Удалить";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // comboBox
            // 
            comboBox.FormattingEnabled = true;
            comboBox.Location = new Point(8, 12);
            comboBox.Name = "comboBox";
            comboBox.Size = new Size(203, 28);
            comboBox.TabIndex = 2;
            comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            // 
            // listViewData
            // 
            listViewData.AllowColumnReorder = true;
            listViewData.FullRowSelect = true;
            listViewData.Location = new Point(217, 12);
            listViewData.Name = "listViewData";
            listViewData.Size = new Size(290, 330);
            listViewData.TabIndex = 3;
            listViewData.UseCompatibleStateImageBehavior = false;
            listViewData.View = View.Details;
            // 
            // textBoxInput
            // 
            textBoxInput.Location = new Point(9, 107);
            textBoxInput.Name = "textBoxInput";
            textBoxInput.Size = new Size(203, 27);
            textBoxInput.TabIndex = 4;
            // 
            // AdminForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(519, 354);
            Controls.Add(textBoxInput);
            Controls.Add(listViewData);
            Controls.Add(comboBox);
            Controls.Add(buttonDelete);
            Controls.Add(buttonAdd);
            Name = "AdminForm";
            Text = "Администратор";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonAdd;
        private Button buttonDelete;
        private ComboBox comboBox;
        private ListView listViewData;
        private TextBox textBoxInput;
    }
}