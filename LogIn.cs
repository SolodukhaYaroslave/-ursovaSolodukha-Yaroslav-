using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace MyLibrary
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void btEnterLogIn_Click(object sender, EventArgs e)
        {
            bool lib = this.checkBox1.Checked;
            DataConnect dataConn = new DataConnect(lib, true);
            MySqlConnection Conn = dataConn.Connect;
            Conn.Open();
            if (LogInDataBaceCheck(Conn, this.tbID.Text, this.tbPassword.Text, lib))
            {
                HomePage homePage = new HomePage(Conn, tbID.Text, lib);
                homePage.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Не правильно введено пароль чи id");
            }
        }

        private static bool LogInDataBaceCheck(MySqlConnection conn, string TextId, string TextPass, bool lib)
        {
            string id, pass;
            string sql = "select id, password from " + (lib ? "librarian" : "client");

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = reader["id"].ToString();
                        pass = reader["password"].ToString();
                        
                        if (id == TextId && pass == TextPass)
                        {
                            return true;
                        }
                    }
                }
                
            }
            return false;
        }
    }
}
