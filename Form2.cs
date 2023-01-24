using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Генератор_паролей
{
    public partial class Form2 : Form
    {
        private Dictionary<string, string> loginPasswordData;

        public Form2(Dictionary<string, string> loginPasswordData)
        {
            InitializeComponent();
            this.loginPasswordData = loginPasswordData;

            foreach (string login in this.loginPasswordData.Keys)
            {
                comboBox1.Items.Add(login);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = loginPasswordData[comboBox1.SelectedItem.ToString()];
        }
    }
}
