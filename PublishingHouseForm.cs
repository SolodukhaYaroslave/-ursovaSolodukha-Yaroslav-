using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace MyLibrary
{
    public partial class PublishingHouseForm : Form
    {
        private HomePage homePage;
        public PublishingHouseForm(HomePage homePage)
        {
            this.homePage = homePage;
            InitializeComponent();
        }

        public List<string> GetGenres()
        {
            List<string> genres = new List<string>();

            string query = "SELECT name FROM publishing_house"; // Замість "genre_name" використовуйте правильну назву стовпця

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, homePage.Conn);

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

        private void PublishingHouseForm_Load(object sender, EventArgs e)
        {
            checkedListBox2.Items.Clear();
            foreach (var genre in GetGenres())
            {
                checkedListBox2.Items.Add(genre);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            homePage.TextHouse = SelectedItems();
            homePage.UpdateTextHouse();
            this.Hide();
        }

        private string SelectedItems()
        {
            if (checkedListBox2.SelectedItem != null)
            {
                string selectedItem = checkedListBox2.SelectedItem.ToString();
                return selectedItem;
            }
            return null;
        }
    }
}
