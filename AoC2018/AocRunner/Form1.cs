using System;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;

namespace AocRunner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.comboBox1.DataSource = Program.AvailableDays.Keys.ToList();
        }
        

         //task1
        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            var day = Program.AvailableDays[comboBox1.Text];
            textBox2.Text = await Task<string>.Factory.StartNew(() => day.Task1(textBox1.Lines.ToList()));
            
            button1.Enabled = true;
            progressBar1.Value = 0;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            var day = Program.AvailableDays[comboBox1.Text];
            textBox3.Text =  await Task<string>.Factory.StartNew(() => day.Task2(textBox1.Lines.ToList()));

            button2.Enabled = true;
            progressBar1.Value = 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
