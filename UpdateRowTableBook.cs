using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MyLibrary
{
    internal class UpdateRowTableBook
    {
        MySqlConnection conn = new HomePage().ConnOpen();
        private string id;
        private string name;
        private List<string> author;
        private List<string> genre;
        private string pubHouse;
        private string year;
        private string amount;
        private string sql = null;
        private MySqlCommand cmd;
        string gapId;

        public UpdateRowTableBook(string Id, string Name, List<string> Author, string Genre, string PubHouse, string Year, string Amount)
        {
            id = Id;
            name = Name;
            author = Author;
            genre = ConvertStringToList(Genre);
            pubHouse = PubHouse;
            year = Year;
            amount = Amount;
        }

        public static List<string> ConvertStringToList(string input)
        {
            // Розділяємо рядок за комами і створюємо список
            List<string> result = new List<string>(input.Split(','));
            return result;
        }

        private string SearchID(string type, string text)
        {
            string id = null;
            if (type == "a") sql = "author";
            else if (type == "g") sql = "genre";
            else if (type == "p") sql = "publishing_house";
            sql = "SELECT * FROM " + sql;
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (type == "a")
                    {
                        if (text == (reader["first_name"].ToString() + " " + reader["last_name"].ToString()) || text == reader["pen_name"].ToString())
                        {
                            id = reader["id"].ToString();
                            break;
                        }
                    }
                    else if (type == "g")
                    {
                        if (text == reader["name"].ToString())
                        {
                            id = reader["id"].ToString();
                            break;
                        }
                    }
                    else if (type == "p")
                    {
                        if (text == reader["name"].ToString())
                        {
                            id = reader["id"].ToString();
                            break;
                        }
                    }
                }
            }
            return id;
        }
        public void UpdateBook()
        {
            string amAll = "";
            string amStoke = "";
            string amClient = "";

            // Отримання amount_of_all та amount_of_stoke
            sql = "SELECT amount_of_all, amount_of_stoke FROM book WHERE id = @Id";
            using (cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        amAll = reader["amount_of_all"].ToString();
                        amStoke = reader["amount_of_stoke"].ToString();
                    }
                }
            }

            // Обчислення кількості книг у клієнтів
            amClient = (Convert.ToInt32(amAll) - Convert.ToInt32(amStoke)).ToString();

            // Перевірка валідності списання книг
            if (Convert.ToInt32(amAll) - Convert.ToInt32(amClient) < Convert.ToInt32(amount))
            {
                MessageBox.Show("Не можна списати більше книг ніж є на складі");
                return;
            }

            // Оновлення запису в таблиці book
            sql = "UPDATE book SET title = @Title, amount_of_all = @AmountAll, amount_of_stoke = @AmountStoke, publishing_id = @PubHouse, year_of_publication = @Year WHERE id = @Id";
            using (cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Title", name);
                cmd.Parameters.AddWithValue("@AmountAll", amount);
                cmd.Parameters.AddWithValue("@AmountStoke", (Convert.ToInt32(amount) - Convert.ToInt32(amClient)).ToString());
                cmd.Parameters.AddWithValue("@PubHouse", PublisingId());
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.ExecuteNonQuery();
            }

            // Оновлення жанрів та авторів книги
            UpdateBookToGenre();
            UpdateBookToAuthor();
        }


        private void UpdateBookToGenre()
        {
            // Видаляємо існуючі зв'язки
            string deleteSql = "DELETE FROM book_genre WHERE book_id = @BookId";
            using (MySqlCommand deleteCmd = new MySqlCommand(deleteSql, conn))
            {
                deleteCmd.Parameters.AddWithValue("@BookId", id);
                deleteCmd.ExecuteNonQuery();
            }

            // Додаємо нові зв'язки
            string insertSql = "INSERT INTO book_genre (book_id, genre_id) VALUES (@Book, @Genre)";
            using (MySqlCommand insertCmd = new MySqlCommand(insertSql, conn))
            {
                foreach (string text in genre)
                {
                    gapId = SearchID("g", text);
                    insertCmd.Parameters.Clear();
                    insertCmd.Parameters.AddWithValue("@Book", id);
                    insertCmd.Parameters.AddWithValue("@Genre", gapId);
                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        private void UpdateBookToAuthor()
        {
            // Видаляємо існуючі зв'язки
            string deleteSql = "DELETE FROM book_author WHERE book_id = @BookId";
            using (MySqlCommand deleteCmd = new MySqlCommand(deleteSql, conn))
            {
                deleteCmd.Parameters.AddWithValue("@BookId", id);
                deleteCmd.ExecuteNonQuery();
            }

            // Додаємо нові зв'язки
            string insertSql = "INSERT INTO book_author (book_id, author_id) VALUES (@Book, @Author)";
            using (MySqlCommand insertCmd = new MySqlCommand(insertSql, conn))
            {
                foreach (string text in author)
                {
                    gapId = SearchID("a", text);
                    insertCmd.Parameters.Clear();
                    insertCmd.Parameters.AddWithValue("@Book", id);
                    insertCmd.Parameters.AddWithValue("@Author", gapId);
                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        private string PublisingId()
        {
            return SearchID("p", pubHouse);
        }
    }
}
