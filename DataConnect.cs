using System;
using System.Windows.Forms;
using MyLibrary;
using MySql.Data.MySqlClient;
using System.IO;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

internal class DataConnect
{
    private string filePath = "IsLibrian.txt";
    public MySqlConnection Connect { get; private set; }

    public DataConnect(bool librarian = false, bool first = false)
    {
        if (first) CreateOrOverwriteTextFile(librarian);
        DBUtilConnect();
    }

    private void DBUtilConnect()
    {
        string user = ReadAndDisplayTextFile();
        try
        {
            Connect = DBUtils.GetDBConnection(user);
        }
        catch (Exception)
        {
            MessageBox.Show("Не вдалося під'єднатись до бази даних, перезавантажте програму.", "Помилка підключення", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void CreateOrOverwriteTextFile(bool l)
    {
        string text = "MyUser";
        if (l) text = "MyLibrarian";
        File.WriteAllText(filePath, text);
    }

    public string ReadAndDisplayTextFile()
    {
        if (File.Exists(filePath))
        {
            string text = File.ReadAllText(filePath);
            return text;
        }

        MessageBox.Show("File does not exist.");
        return null;
    }
}
