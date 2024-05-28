using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace MyLibrary
{
    internal class CreateRowTableBook
    {
        MySqlConnection conn = new HomePage().ConnOpen();
        private string name;
        private List<string> author;
        private List<string> genre;
        private string pubHouse;
        private string year;
        private string amount;
        string bookID;
        string gapId;

        public CreateRowTableBook(string Name, List<string> Author, string Genre, string PubHouse, string Year, string Amount)
        {
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
            string sql = null;
            if (type == "a") sql = "author";
            else if (type == "g") sql = "genre";
            else if (type == "p") sql = "publishing_house";
            sql = "select * from " + sql;
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

        public void CreateBook()
        {
            string sql = "INSERT INTO book (title, amount_of_all, amount_of_stoke, publishing_id, year_of_publication) VALUES (@Title, @AmountAll, @AmountStoke, @PubHouse, @Year)";
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Title", name);
                cmd.Parameters.AddWithValue("@AmountAll", amount);
                cmd.Parameters.AddWithValue("@AmountStoke", amount);
                cmd.Parameters.AddWithValue("@PubHouse", PublisingId());
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.ExecuteNonQuery();
            }

            bookID = lastId();
            CreateBookToGenre();
            CreateBookToAuthor();
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

        private void CreateBookToGenre()
        {
            string sql = "INSERT INTO book_genre (book_id, genre_id) VALUES (@Book, @Genre)";
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                foreach (string text in genre)
                {
                    gapId = SearchID("g", text);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Book", bookID);
                    cmd.Parameters.AddWithValue("@Genre", gapId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void CreateBookToAuthor()
        {
            string sql = "INSERT INTO book_author (book_id, author_id) VALUES (@Book, @Author)";
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                foreach (string text in author)
                {
                    gapId = SearchID("a", text);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Book", bookID);
                    cmd.Parameters.AddWithValue("@Author", gapId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private string PublisingId()
        {
            return SearchID("p", pubHouse);
        }
    }
}
