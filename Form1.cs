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

namespace Генератор_паролей
{
    public partial class Form1 : Form
    {
        private DataBase dataBase;
        public Form1()
        {
            InitializeComponent();
            dataBase = new DataBase();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            int len_pass = (int)numericUpDown1.Value;
            int quantity = (int)numericUpDown2.Value;
            bool symbols = checkBox1.Checked;
            Random rand = new Random();
            Random type = new Random();
            for (int j = 0; j < quantity; j++)
            {
                for (int i = 0; i < len_pass; i++)
                {
                    char value;
                    int type_num = type.Next(0, 3);
                    if (type_num == 0)
                    {
                        value = Convert.ToChar(rand.Next(0, 9) + 48);
                    }
                    else if (type_num == 1 && symbols == true)
                    {
                        value = (char)rand.Next(33, 47);
                       
                        if (value == 39)
                        {
                            value = '"';
                        }
                    }
                    else
                    {
                       

                        if (type.Next(0, 2) == 0)
                        {
                            value = (char)rand.Next(65, 91);
                        }
                        else
                        {
                            value = (char)rand.Next(97, 123);
                        }
                    }
                    richTextBox1.Text += value.ToString();
                }
                richTextBox1.Text += "\n";

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataBase.SavePassword(textBox1.Text, richTextBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2(dataBase.GetAllData());
            frm.Show();
        }
    }

    class DataBase
    {
        private SQLiteConnection SQLiteConn;
        private string tableName, loginColName, passwordColName;

        public DataBase()
        {
            SQLiteConn = new SQLiteConnection("DATA SOURCE=password_gen.bd;");
            SQLiteConn.Open();

            tableName = "passwords";
            loginColName = "login";
            passwordColName = "password";

            string CreateTableQuery = $"CREATE TABLE IF NOT EXISTS [{tableName}] (" +
                $"{loginColName} TEXT NOT NULL," +
                $"{passwordColName} TEXT NOT NULL" +
                $");";
            new SQLiteCommand(CreateTableQuery, SQLiteConn).ExecuteNonQuery();
        }

        public void SavePassword(string login, string password)
        {
            string PutDataQuery;
            string SelectQuery = $"SELECT [{loginColName}], [{passwordColName}] FROM [{tableName}] " +
                $"WHERE [{loginColName}] = '{login}';";

            SQLiteCommand SelectCommand = new SQLiteCommand(SelectQuery, SQLiteConn);
            SQLiteDataReader SelectReader = SelectCommand.ExecuteReader();
            if (SelectReader.Read())
            {
                PutDataQuery = $"UPDATE [{tableName}] SET [{passwordColName}] = '{password}' " +
                    $"WHERE [{loginColName}] = '{login}';";
            }
            else
            {
                PutDataQuery = $"INSERT INTO [{tableName}] ([{loginColName}], [{passwordColName}]) " +
                    $"VALUES ('{login}', '{password}');";
            }
            new SQLiteCommand(PutDataQuery, SQLiteConn).ExecuteNonQuery();
        }

        public Dictionary<string, string> GetAllData()
        {
            string SelectQuery = $"SELECT [{loginColName}], [{passwordColName}] FROM [{tableName}];";
            SQLiteCommand SelectCommand = new SQLiteCommand(SelectQuery, SQLiteConn);
            SQLiteDataReader SelectReader = SelectCommand.ExecuteReader();

            Dictionary<string, string> result = new Dictionary<string, string>();
            while (SelectReader.Read())
            {
                result.Add(SelectReader[0].ToString(), SelectReader[1].ToString());
            }

            return result;
        }
    }
}
