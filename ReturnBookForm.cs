using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MyLibrary
{
    public partial class ReturnBookForm : Form
    {
        private HomePage homePage;
        private string idClient;
        private string[] idBook = new string[3];
        public ReturnBookForm(HomePage homePage, string clientId)
        {
            this.homePage = homePage;
            InitializeComponent();
            idClient = clientId;
        }

        public List<string> GetBook()
        {

            List<string> books = new List<string>();

            string sql = File.ReadAllText("ClientIssue.txt");

            try
            {
                string text; int i = 0;
                MySqlCommand cmd = new MySqlCommand(sql, homePage.ConnOpen());
                cmd.Parameters.AddWithValue("@client_id", idClient);
                cmd.ExecuteNonQuery();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        text = reader["title"].ToString() + " Автори: " + reader["author"].ToString() + " Видавництво: " + reader["publishing_house"].ToString();
                        books.Add(text);
                        idBook[i] = reader["book_id"].ToString();
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при отриманні книг: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return books;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ReturnBookForm_Load(object sender, EventArgs e)
        {
            this.checkedListBox1.Items.Clear();
            foreach (var book in GetBook())
            {
                checkedListBox1.Items.Add(book);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string idbook;
            string sql = "UPDATE issue_of_book SET return_book = @Return WHERE book_id = @BookId and client_id = @Client";
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    idbook = this.idBook[i];



                    using (MySqlCommand updateCmd = new MySqlCommand(sql, homePage.ConnOpen()))
                    {
                        updateCmd.Parameters.AddWithValue("@Return", "true");
                        updateCmd.Parameters.AddWithValue("@BookId", idbook);
                        updateCmd.Parameters.AddWithValue("@Client", idClient);
                        updateCmd.ExecuteNonQuery();
                    }

                    homePage.PlusOrMinusAmount(idbook, false);
                }
            }

            MessageBox.Show("Операція пройшла іспішно");

            checkedListBox1.Items.Clear();
            homePage.Show();
            this.Close();
        }

        private CheckedListBox.CheckedItemCollection SelectedItems()
        {
            return checkedListBox1.CheckedItems;
        }
    }
}
