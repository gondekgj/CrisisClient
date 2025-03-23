using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testapp4
{
    public partial class Form2 : Form
    {
        private Form1 _form1;
        public Form2(Form1 form1)
        {
            InitializeComponent();
            _form1 = form1;

            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(projectDirectory, "savedText.txt");

            string portDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string portPath = Path.Combine(projectDirectory, "savedText2.txt");

            string IDDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string IDPath = Path.Combine(projectDirectory, "savedText3.txt");

            //set the file path inside the project directory
            filePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "savedText.txt");
            portPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "savedText2.txt");
            IDPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "savedText3.txt");

            //load text from the file when the form loads
            this.Load += new EventHandler(Form2_Load);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(projectDirectory, "savedText.txt");
            string portDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string portPath = Path.Combine(projectDirectory, "savedText2.txt");
            string IDDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string IDPath = Path.Combine(projectDirectory, "savedText3.txt");

            //load ip
            try

            {
                if (File.Exists(filePath))
                {
                    textBox1.Text = File.ReadAllText(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading text: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //load port
            try

            {
                if (File.Exists(portPath))
                {
                    textBox2.Text = File.ReadAllText(portPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading text: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //load ID
            try

            {
                if (File.Exists(IDPath))
                {
                    textBox3.Text = File.ReadAllText(IDPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading text: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //saves things after button
        private void button1_Click(object sender, EventArgs e)
        {
            string textToSave = textBox1.Text;
            string portToSave = textBox2.Text;
            string IDToSave = textBox3.Text;

            if (!string.IsNullOrWhiteSpace(textToSave))
            {
                try
                {
                    //get directories
                    string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string filePath = Path.Combine(projectDirectory, "savedText.txt");
                    string portDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string portPath = Path.Combine(projectDirectory, "savedText2.txt");
                    string IDDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string IDPath = Path.Combine(projectDirectory, "savedText3.txt");

                    //writes text
                    File.WriteAllText(filePath, textToSave);
                    File.WriteAllText(portPath, portToSave);
                    File.WriteAllText(IDPath, IDToSave);

                    MessageBox.Show($"Config saved successfully");
                    //debug text - MessageBox.Show($"Text saved successfully at: {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _form1.ExecuteAction();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter some text before saving.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //na
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
