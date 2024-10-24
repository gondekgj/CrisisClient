namespace testapp4
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            comboBox1 = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(44, 27);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(176, 23);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(44, 65);
            label1.Name = "label1";
            label1.Size = new Size(93, 15);
            label1.TabIndex = 1;
            label1.Text = "System Uptime: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(44, 94);
            label2.Name = "label2";
            label2.Size = new Size(101, 15);
            label2.TabIndex = 2;
            label2.Text = "Software Uptime: ";
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(44, 122);
            label3.Name = "label3";
            label3.Size = new Size(139, 15);
            label3.TabIndex = 3;
            label3.Text = "Loading Resource Usage ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(44, 149);
            label4.Name = "label4";
            label4.Size = new Size(145, 15);
            label4.TabIndex = 4;
            label4.Text = "Software Resource Usage: ";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(498, 195);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(comboBox1);
            Controls.Add(label1);
            Controls.Add(label2);
            Name = "Form1";
            Text = "Uptime Tracker";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private System.Windows.Forms.Timer timer1;
        private Label label2;
        private ComboBox comboBox1;
        private Label label3;
        private Label label4;
    }
}
