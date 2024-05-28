using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace MyLibrary
{
    internal class CreateClient
    {
        private MySqlConnection conn = new HomePage().ConnOpen();
        private string lastName;
        private string firstName;
        private string phone;
        private string password;
        private string subDate;
        public CreateClient(string LastName, string FirstName, string Phone, string Password)
        {
            lastName = LastName; firstName = FirstName; phone = Phone; password = Password;
            DateTime today = DateTime.Today;
            subDate = today.ToString("yyyy-MM-dd");
        }

        public void AddClient()
        {
            string sql = "INSERT INTO client (first_name, last_name, phone, password, sub_end_date) VALUES (@First, @Last, @Phone,  @Password, @Sub)";
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@First", firstName);
                cmd.Parameters.AddWithValue("@Last", lastName);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Sub", subDate);
                cmd.ExecuteNonQuery();
            }

            MessageID();
        }

        private void MessageID()
        {
            MessageBox.Show("ID клієнта: " + lastId());
        }

        private string lastId()
        {
            string selectSql = "SELECT LAST_INSERT_ID()";
            using (MySqlCommand cmd = new MySqlCommand(selectSql, conn))
            {
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    string lastInsertId = result.ToString();
                    Console.WriteLine($"Last inserted ID: {lastInsertId}");
                    return lastInsertId;
                }
            }
            return null;
        }

    }
}
