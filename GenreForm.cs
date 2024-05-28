using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace MyLibrary
{
    public partial class GenreForm : Form
    {
        private HomePage homePage;
        public GenreForm(HomePage homePage)
        {
            this.homePage = homePage;
            InitializeComponent();
        }

        public List<string> GetGenres()
        {
            List<string> genres = new List<string>();

            string query = "SELECT name FROM genre";

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, homePage.ConnOpen());

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        genres.Add(reader["name"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при отриманні жанрів: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            genres.Sort();
            return genres;
        }

        private void GenreForm_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (var genre in GetGenres())
            {
                listBox1.Items.Add(genre);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            homePage.genreList = SelectedItems().Cast<string>().ToList();
            string selectedItemsText = string.Join(", ", homePage.genreList);
            homePage.TextGenre = selectedItemsText;
            homePage.UpdateTextGenre();

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SetItemChecked(i, false);
            }

            this.Hide();
        }

        private CheckedListBox.CheckedItemCollection SelectedItems()
        {
            return listBox1.CheckedItems;
        }

    }
}
