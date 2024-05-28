using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MyLibrary
{
    public partial class HomePage : Form
    {
        public MySqlConnection Conn { get; set; }
        private const string placeholderText = "Пошук книг";
        private bool isPlaceholderActive;
        public bool LibBool { get; set; }
        public string TextGenre { get; set; }
        public string TextHouse { get; set; }
        public string idUser {  get; set; }
        private GenreForm genreForm;
        private ReturnBookForm returnBookForm;
        private PublishingHouseForm publishingHouseForm;
        public List<string> genreList;
        private string sql;
        private MySqlCommand cmd;
        DataConnect dataConn;


        public HomePage(MySqlConnection conn, string ID, bool libBool)
        {
            InitializeComponent();

            this.LibBool = libBool;

            this.Conn = conn;
            this.idUser = ID;

            this.Resize += new EventHandler(MainForm_Resize);
            CenterBox();
            SetTableLayoutPanel();
            this.dataGridBook.DataSource = Table();
            SetSizeDataTable();

            SetupPlaceholder();
            textBox1.Enter += RemovePlaceholder;
            textBox1.Leave += SetPlaceholder;

            DateOnClientPage();

            comboBox1.SelectedIndex = 0;

            ClientBookTable();
        }

        public HomePage()
        {
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            if (LibBool)
            {
                this.label11.Visible = true;
                this.textBox2.Visible = true;
                this.button1.Visible = true;
                this.button2.Visible = true;
                tableLayoutPanel10.Visible = true;
            }
            else
            {
                this.tabControl.Controls.Remove(tpControl);
                this.tabControl.Controls.Remove(tpClient);
            }

        }

        public MySqlConnection ConnOpen()
        {
            if (Conn == null)
            {
                dataConn = new DataConnect();
                Conn = dataConn.Connect;
                LibBool = dataConn.ReadAndDisplayTextFile() == "MyLibrarian";
            }

            if (Conn.State == System.Data.ConnectionState.Open)
            {
                Conn.Close();
            }

            Conn.Open();
            return Conn;
        }

        //Функції центрування об'єктів пошуку
        private void MainForm_Resize(object sender, EventArgs e)
        {
            CenterBox();
            SetTableLayoutPanel();
            SetSizeDataTable();
        }
        private void CenterBox()
        {
            panel1.Left = (splitContainer1.Panel1.ClientSize.Width - panel1.Width) / 2;
            panel1.Top = (splitContainer1.Panel1.ClientSize.Height - panel1.Height) / 2;
            tableLayoutPanel6.Left = (groupBox3.Width - tableLayoutPanel6.Width) / 2;
            tableLayoutPanel6.Top = (groupBox3.Height - tableLayoutPanel6.Height) / 2;

        }



        //Функції для створення фантомного тексту "Пошук книг"
        private void SetupPlaceholder()
        {
            textBox1.Text = placeholderText;
            textBox1.ForeColor = Color.Gray;
            isPlaceholderActive = true;
        }
        private void RemovePlaceholder(object sender, EventArgs e)
        {
            if (isPlaceholderActive)
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
                isPlaceholderActive = false;
            }
        }
        private void SetPlaceholder(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = placeholderText;
                textBox1.ForeColor = Color.Gray;
                isPlaceholderActive = true;
            }
        }

        private DataTable SearchTable(string category, string searchValue)
        {
            if (category == "ВСІ")
            {
                textBox1.Text = placeholderText;
                textBox1.ForeColor = Color.Gray;
                isPlaceholderActive = true;
                ClientBookTable();
                return Table();
            }
            string sql = @"SELECT book.id, title, publishing_house.name AS publishing_house, year_of_publication,
                              GROUP_CONCAT(DISTINCT genre.name ORDER BY genre.name ASC) AS genres,
                              GROUP_CONCAT(DISTINCT COALESCE(pen_name, CONCAT(first_name, ' ', last_name)) ORDER BY last_name ASC) AS authors,
                              book.amount_of_all, book.amount_of_stoke
                       FROM book
                       JOIN book_genre ON book.id = book_genre.book_id
                       JOIN genre ON book_genre.genre_id = genre.id
                       JOIN book_author ON book.id = book_author.book_id
                       JOIN author ON book_author.author_id = author.id
                       JOIN publishing_house ON book.publishing_id = publishing_house.id
                       WHERE";

            // Додавання умов фільтрації
            if (category == "НАЗВА")
            {
                sql += " title LIKE @searchValue";
            }
            else if (category == "АВТОР")
            {
                sql += " (pen_name LIKE @searchValue OR CONCAT(first_name, ' ', last_name) LIKE @searchValue)";
            }
            else if (category == "ЖАНР")
            {
                sql += " genre.name LIKE @searchValue";
            }
            else if (category == "ВИДАВНИЦТВО")
            {
                sql += " publishing_house.name LIKE @searchValue";
            }

            sql += @" GROUP BY book.id, title, publishing_house.name, year_of_publication, book.amount_of_all, book.amount_of_stoke";

            using (MySqlCommand cmd = new MySqlCommand(sql, ConnOpen()))
            {
                cmd.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%");

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        //Блок функцій для роботи із пошуком (альфа)
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == placeholderText || this.textBox1.Text == "") return;

            SearchClick();
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchClick();
            }
        }

        private void SearchClick()
        {
            if (this.textBox1.Text == placeholderText || this.textBox1.Text == "")
            {
                this.dataGridBook.DataSource = Table();
            }
            else this.dataGridBook.DataSource = SearchTable(comboBox1.Text, textBox1.Text);
            SetSizeDataTable();
        }

        private void SetSizeDataTable()
        {
            dataGridBook.Columns[7].Visible = LibBool;

            dataGridBook.Columns[0].Width = (int)(dataGridBook.Width * 0.04);
            dataGridBook.Columns[1].Width = (int)(dataGridBook.Width * 0.35);
            dataGridBook.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridBook.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridBook.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridBook.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridBook.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridBook.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        private void SetTableLayoutPanel()
        {
            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                tableLayoutPanel1.RowStyles[i].Height = 25F;
                tableLayoutPanel1.RowStyles[i].SizeType = SizeType.Percent;
            }
            tableLayoutPanel2.ColumnStyles[0].Width = 5F;
            tableLayoutPanel2.ColumnStyles[1].Width = 40F;
            tableLayoutPanel2.ColumnStyles[2].Width = 35F;
            tableLayoutPanel2.ColumnStyles[3].Width = 20F;

            tableLayoutPanel2.ColumnStyles[0].SizeType = SizeType.Percent;
            tableLayoutPanel2.ColumnStyles[1].SizeType = SizeType.Percent;
            tableLayoutPanel2.ColumnStyles[2].SizeType = SizeType.Percent;
            tableLayoutPanel2.ColumnStyles[3].SizeType = SizeType.Percent;

            tableLayoutPanel3.RowStyles[0].Height = 9F;
            tableLayoutPanel3.RowStyles[1].Height = 49F;
            tableLayoutPanel3.RowStyles[2].Height = 12F;
            tableLayoutPanel3.RowStyles[3].Height = 12F;
            tableLayoutPanel3.RowStyles[4].Height = 9F;
            tableLayoutPanel3.RowStyles[5].Height = 9F;

            tableLayoutPanel3.RowStyles[0].SizeType = SizeType.Percent;
            tableLayoutPanel3.RowStyles[1].SizeType = SizeType.Percent;
            tableLayoutPanel3.RowStyles[2].SizeType = SizeType.Percent;
            tableLayoutPanel3.RowStyles[3].SizeType = SizeType.Percent;
            tableLayoutPanel3.RowStyles[4].SizeType = SizeType.Percent;
            tableLayoutPanel3.RowStyles[5].SizeType = SizeType.Percent;

            tableLayoutPanel8.ColumnStyles[0].Width = 15F;
            tableLayoutPanel8.ColumnStyles[1].Width = 25F;
            tableLayoutPanel8.ColumnStyles[2].Width = 20F;
            tableLayoutPanel8.ColumnStyles[3].Width = 20F;
            tableLayoutPanel8.ColumnStyles[4].Width = 20F;

            tableLayoutPanel8.ColumnStyles[0].SizeType = SizeType.Percent;
            tableLayoutPanel8.ColumnStyles[1].SizeType = SizeType.Percent;
            tableLayoutPanel8.ColumnStyles[2].SizeType = SizeType.Percent;
            tableLayoutPanel8.ColumnStyles[3].SizeType = SizeType.Percent;
            tableLayoutPanel8.ColumnStyles[4].SizeType = SizeType.Percent;

            for (int i = 0; i < tableLayoutPanel2.RowCount; i++)
            {
                tableLayoutPanel2.RowStyles[i].Height = 25F;
                tableLayoutPanel2.RowStyles[i].SizeType = SizeType.Percent;
            }

            tableLayoutPanel7.RowStyles[0].Height = 0F;
            for (int i = 1; i < tableLayoutPanel7.RowCount - 1; i++)
            {
                tableLayoutPanel7.RowStyles[i].Height = 1F;
                tableLayoutPanel7.RowStyles[i].SizeType = SizeType.Percent;
            }
            tableLayoutPanel7.RowStyles[5].Height = 95F;

        }



        private void button5_Click(object sender, EventArgs e)
        {

            if (genreForm == null || genreForm.IsDisposed)
            {
                genreForm = new GenreForm(this);
            }
            genreForm.Show();
        }
        public void UpdateTextGenre() => this.tbGenre.Text = TextGenre;
        public void UpdateTextHouse() => this.tbPubHouse.Text = TextHouse;

        private void cbAuthor_Click(object sender, EventArgs e)
        {
            this.cbAuthor.Items.Clear();
            sql = "select * from author";
            cmd = new MySqlCommand(sql, ConnOpen());

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name;
                    if (String.IsNullOrEmpty(reader["pen_name"].ToString()))
                    {
                        name = reader["first_name"].ToString() + " " + reader["last_name"].ToString();
                    }
                    else name = reader["pen_name"].ToString();
                    this.cbAuthor.Items.Add(name);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbAuthor.Text))
            {
                string authorName = cbAuthor.Text;
                if (CheckAuthorExists(ref authorName))
                {
                    lbAuthor.Items.Add(authorName);
                    cbAuthor.Text = "";
                }
                else
                {
                    DialogResult result = MessageBox.Show($"Чи бажаєте ви додати автора {authorName}?", "Додати автора", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        AddAuthor(authorName);
                        lbAuthor.Items.Add(authorName);
                        cbAuthor.Text = "";
                    }
                }
            }
        }

        private bool CheckAuthorExists(ref string authorName)
        {
            sql = "select count(*) from author where pen_name = @name or CONCAT(first_name, ' ', last_name) = @name";
            cmd = new MySqlCommand(sql, ConnOpen());
            cmd.Parameters.AddWithValue("@name", authorName);
            int count = Convert.ToInt32(cmd.ExecuteScalar());

            if (count == 0)
            {
                sql = "select count(*) from author where CONCAT(last_name, ' ', first_name) = @name";
                cmd = new MySqlCommand(sql, ConnOpen());
                cmd.Parameters.AddWithValue("@name", authorName);
                count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count != 0)
                {
                    ReverseName(ref authorName);
                    CheckAuthorExists(ref authorName);
                }
            }
            return count > 0;
        }

        public string ReverseName(ref string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                return fullName;
            }

            string[] parts = fullName.Split(' ');

            if (parts.Length != 2)
            {
                throw new ArgumentException("Full name must contain exactly one space.");
            }

            string firstName = parts[0];
            string lastName = parts[1];

            return $"{lastName} {firstName}";
        }

        private void AddAuthor(string authorName)
        {
            sql = "insert into author (last_name, pen_name) values (@last, @name)";
            cmd = new MySqlCommand(sql, ConnOpen());
            cmd.Parameters.AddWithValue("@last", authorName);
            cmd.Parameters.AddWithValue("@name", authorName);
            cmd.ExecuteNonQuery();
        }


        private void DateOnClientPage()
        {
            sql = "select * from " + (LibBool ? "librarian" : "client");
            cmd = new MySqlCommand(sql, ConnOpen());

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader["id"].ToString() != idUser) continue;
                    lbIDSub.Text = reader["id"].ToString();
                    lbNameSub.Text = reader["last_name"].ToString() + " " + reader["first_name"].ToString();
                    lbPhoneSub.Text = reader["phone"].ToString();
                    if (!LibBool)
                    {
                        SetDateWithColor(ref lbDateSub, reader["sub_end_date"].ToString());
                    }
                    else
                        label6.Text = "";

                    break;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (publishingHouseForm == null || publishingHouseForm.IsDisposed)
            {
                publishingHouseForm = new PublishingHouseForm(this);
            }
            publishingHouseForm.Show();
        }

        private List<string> listItemsAuthor()
        {
            List<string> listBoxItems = new List<string>();
            try
            {
                if (String.IsNullOrEmpty(tbTitile.Text) || lbAuthor.Items.Count == 0 || String.IsNullOrEmpty(tbGenre.Text) || String.IsNullOrEmpty(tbPubHouse.Text) || String.IsNullOrEmpty(tbYear.Text) || String.IsNullOrEmpty(tbAmount.Text))
                {
                    MessageBox.Show("Не всі поля заповнені");
                    return null;
                }

                foreach (var item in lbAuthor.Items)
                {
                    listBoxItems.Add(item.ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return listBoxItems;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                CreateRowTableBook NewRow = new CreateRowTableBook(tbTitile.Text, listItemsAuthor(), tbGenre.Text, tbPubHouse.Text, tbYear.Text, tbAmount.Text);

                NewRow.CreateBook();
                ClearPageOblick();
                MessageBox.Show("Книга додана");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невдалось додати книгу: " + ex);
            }
        }

        private DataTable Table()
        {
            sql = File.ReadAllText("HeadBook.txt");
            using (cmd = new MySqlCommand(sql, ConnOpen()))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            CreateClient cc = new CreateClient(tbClLN.Text, tbClFN.Text, tbClPh.Text, tbClP.Text);
            cc.AddClient();
            tbClLN.Text = ""; tbClFN.Text = ""; tbClPh.Text = ""; tbClP.Text = "";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string date = AddMonth(tbClCount.Text);

            sql = "UPDATE client SET sub_end_date = @Date WHERE id = @id";
            cmd = new MySqlCommand(sql, ConnOpen());

            cmd.Parameters.AddWithValue("@Date", date);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(tbClId.Text));

            try
            {
                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine($"Number of rows updated: {rowsAffected}");
                MessageSub(date);
                tbClId.Text = ""; tbClCount.Text = "";
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySqlException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private string AddMonth(string countText)
        {
            DateTime today = DateTime.Today;
            int count = Convert.ToInt32(countText);
            return today.AddMonths(count).ToString("yyyy-MM-dd");
        }

        private void MessageSub(string date)
        {
            sql = "select id, first_name, last_name from client";
            cmd = new MySqlCommand(sql, ConnOpen());
            string name = "(Клієнт не знайдений)";
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (tbClId.Text == reader[0].ToString())
                    {
                        name = reader["first_name"].ToString() + " " + reader["last_name"].ToString();
                    }
                }
            }

            DateTime dateTime = DateTime.ParseExact(date, "yyyy-MM-dd", null);

            date = dateTime.ToString("dd.MM.yyyy");

            MessageBox.Show("Підписку " + name + " продовжено до " + date);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sql = File.ReadAllText("HeadBook.txt");
            cmd = new MySqlCommand(sql, ConnOpen());
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (tbSerchBook.Text == reader[0].ToString())
                    {
                        tbTitile.Text = reader[1].ToString();
                        tbGenre.Text = reader[2].ToString();
                        AddItemsToListBox(reader[3].ToString());
                        tbPubHouse.Text = reader[4].ToString();
                        tbYear.Text = reader[5].ToString();
                        tbAmount.Text = reader[6].ToString();
                    }
                }
            }

            button7.Enabled = false;
            button12.Enabled = true;
            button11.Enabled = true;
        }
        private void AddItemsToListBox(string text)
        {
            string[] items = text.Split(',');
            foreach (string item in items)
            {
                lbAuthor.Items.Add(item);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var selectedIndices = new int[lbAuthor.SelectedIndices.Count];
            lbAuthor.SelectedIndices.CopyTo(selectedIndices, 0);
            for (int i = selectedIndices.Length - 1; i >= 0; i--)
            {
                lbAuthor.Items.RemoveAt(selectedIndices[i]);
            }
        }

        private void ClearPageOblick()
        {
            tbTitile.Text = "";
            lbAuthor.Items.Clear();
            tbGenre.Text = "";
            tbPubHouse.Text = "";
            tbYear.Text = "";
            tbAmount.Text = "";
            tbSerchBook.Text = "";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ClearPageOblick();
            button7.Enabled = true;
            button12.Enabled = false;
            button11.Enabled = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string rowIndexText = tbSerchBook.Text;
            int rowIndex;

            if (!int.TryParse(rowIndexText, out rowIndex))
            {
                MessageBox.Show("Please enter a valid numerical ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (ConnOpen().State != ConnectionState.Open)
                {
                    ConnOpen().Open();
                }

                string deleteQuery = "DELETE FROM book_author WHERE book_id = @id";

                using (MySqlCommand command = new MySqlCommand(deleteQuery, ConnOpen()))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", rowIndex);
                    int rowsAffected = command.ExecuteNonQuery();
                }

                deleteQuery = "DELETE FROM book_genre WHERE book_id = @id";

                using (MySqlCommand command = new MySqlCommand(deleteQuery, ConnOpen()))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", rowIndex);
                    int rowsAffected = command.ExecuteNonQuery();
                }

                deleteQuery = "DELETE FROM book_publishing_house WHERE book_id = @id";

                using (MySqlCommand command = new MySqlCommand(deleteQuery, ConnOpen()))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", rowIndex);
                    int rowsAffected = command.ExecuteNonQuery();
                }

                deleteQuery = "DELETE FROM book WHERE id = @id";

                using (MySqlCommand command = new MySqlCommand(deleteQuery, ConnOpen()))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", rowIndex);
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"MySqlException: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ClearPageOblick();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {

            UpdateRowTableBook updateRowTableBook = new UpdateRowTableBook(tbSerchBook.Text, tbTitile.Text, listItemsAuthor(), tbGenre.Text, tbPubHouse.Text, tbYear.Text, tbAmount.Text);
            updateRowTableBook.UpdateBook();
            MessageBox.Show("Книга успішно оновлена.");
        }

        private string IDBook()
        {
            DataGridViewCell selectedCell = dataGridBook.SelectedRows[0].Cells["ID"];
            object cellValue = selectedCell.Value;
            string idBook;
            if (cellValue != null)
            {
                idBook = cellValue.ToString();
            }
            else
            {
                idBook = string.Empty; // або будь-яке інше значення за замовчуванням, яке вам потрібно
            }
            return idBook;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string idBook = IDBook();
            if (!IsCountIssue(idBook))
            {
                MessageBox.Show("На абонент вже зареєстровано 3 книги.");
                return;
            }


            DateTime day = DateTime.Today;

            sql = "INSERT INTO issue_of_book (book_id, client_id, librarian_id, date, return_book) VALUES (@Book, @Client, @Librarian, @Date, @Return)";
            using (MySqlCommand cmd = new MySqlCommand(sql, ConnOpen()))
            {
                cmd.Parameters.AddWithValue("@Book", idBook);
                cmd.Parameters.AddWithValue("@Client", textBox2.Text);
                cmd.Parameters.AddWithValue("@Librarian", idUser);
                cmd.Parameters.AddWithValue("@Date", day.AddMonths(1).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Return", "false");
                cmd.ExecuteNonQuery();
            }

            PlusOrMinusAmount(idBook, true);

            dataGridBook.DataSource = Table();
            MessageBox.Show("Операція пройшла іспішно");
        }

        public void PlusOrMinusAmount(string idBook, bool minus)
        {
            int currentAmount = 0;
            sql = "SELECT amount_of_stoke FROM book WHERE id = @BookId";
            using (cmd = new MySqlCommand(sql, ConnOpen()))
            {
                cmd.Parameters.AddWithValue("@BookId", idBook);
                object result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out currentAmount))
                {
                    if (minus)
                        currentAmount--;
                    else
                        currentAmount++;

                    // Оновити значення amount_of_stoke
                    sql = "UPDATE book SET amount_of_stoke = @NewAmount WHERE id = @BookId";
                    using (MySqlCommand updateCmd = new MySqlCommand(sql, ConnOpen()))
                    {
                        updateCmd.Parameters.AddWithValue("@NewAmount", currentAmount);
                        updateCmd.Parameters.AddWithValue("@BookId", idBook);
                        updateCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("Поле порожне");
                return;
            }
            returnBookForm = new ReturnBookForm(this, textBox2.Text);
            returnBookForm.ShowDialog();
            dataGridBook.DataSource = Table();
        }

        private void ClientBookTable()
        {
            if (textBox3.Text == "") return;

            sql = "select client.id AS client_id, CONCAT(client.first_name, ' ', client.last_name) AS full_name_client, client.phone, client.sub_end_date from client where id = @client_id";
            using (cmd = new MySqlCommand(sql, ConnOpen()))
            {
                cmd.Parameters.AddWithValue("@client_id", textBox3.Text);
                cmd.ExecuteNonQuery();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        this.lbIDSub.Text = reader["client_id"].ToString();
                        this.lbNameSub.Text = reader["full_name_client"].ToString();
                        this.lbPhoneSub.Text = reader["phone"].ToString();
                        SetDateWithColor(ref lbDateSub, reader["sub_end_date"].ToString());
                    }
                }
            }

            string [,]text = new string[4,3];
            int i = 0;
            sql = File.ReadAllText("ClientIssue.txt");

            using (cmd = new MySqlCommand(sql, ConnOpen()))
            {
                cmd.Parameters.AddWithValue("@client_id", textBox3.Text);
                cmd.ExecuteNonQuery();

                try
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            text[0, i] = reader["title"].ToString();
                            text[1, i] = reader["author"].ToString();
                            text[2, i] = reader["publishing_house"].ToString();
                            text[3, i] = reader["date"].ToString();
                            i++;
                        }
                    }

                }
                catch
                {
                    return;
                }
                finally
                {
                    this.NameBook1.Text = text[0, 0];
                    this.NameBook2.Text = text[0, 1];
                    this.NameBook3.Text = text[0, 2];

                    this.AuthorBook1.Text = text[1, 0];
                    this.AuthorBook2.Text = text[1, 1];
                    this.AuthorBook3.Text = text[1, 2];

                    this.Publishing1.Text = text[2, 0];
                    this.Publishing2.Text = text[2, 1];
                    this.Publishing3.Text = text[2, 2];


                    SetDateWithColor(ref BookDate1, text[3, 0]);
                    SetDateWithColor(ref BookDate2, text[3, 1]);
                    SetDateWithColor(ref BookDate3, text[3, 2]);
                }

            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ClientBookTable();
        }

        private bool IsCountIssue(string clientID)
        {
            sql = "select count(return_book) as 'c' from issue_of_book where return_book like 'false' group by client_id, return_book;";

            using (cmd = new MySqlCommand(sql, ConnOpen()))
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@client_id", clientID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int n = Convert.ToInt32(reader["c"].ToString());
                        return n < 3;
                    }
                }
            }

            return false;
        }

        private void SetDateWithColor(ref Label label, string dateText)
        {
            if (string.IsNullOrEmpty(dateText))
            {
                label.Text = "";
                return;
            }
            DateTime date = new DateTime();
            date = Convert.ToDateTime(dateText);
            if (Convert.ToDateTime(dateText) < DateTime.Today) 
                label.ForeColor = Color.Red;
            else 
                label.ForeColor = Color.Black;

            label.Text = date.ToString("dd.MM.yyyy");
        }
    }
}