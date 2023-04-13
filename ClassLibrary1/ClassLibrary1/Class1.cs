using System.Data;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Windows.Controls;
using System.Windows;
using System;
using System.Collections.Generic;
using AdvanceHelperWPF;

namespace AHlibrary
{
    /// <summary>
    /// Класс DBconnect предоставляет функции для работы с базой данных,
    /// а именно для подключения, сохранения таблицы и других
    /// различных действий, необходимых для работы
    /// </summary>
    public class DBconnect
    {

        DbDataAdapter adapter;
        DataTable dt;
        MySqlConnection conn;
        public string UserLogin;
        FileHandler fileHandler = new FileHandler();
        string dbusername;
        string dbname;
        string password;

        /// <summary>
        /// Инициализирует строки для подключения к базе данных
        /// </summary>
        public void dbConnectionStrings()
        {
            dbusername = fileHandler.GetPath("config.txt", "Имя пользователя базы данных: ");
            dbname = fileHandler.GetPath("config.txt", "Название базы данных: ");
            password = fileHandler.GetPath("config.txt", "Пароль базы данных: ");
        }

        /// <summary>
        /// Выполняет подключение к базе данных,
        /// после чего выгружает данные из
        /// определенной таблицы в DataGrid
        /// </summary>
        /// <param name="table">Передает таблицу, которая должна
        /// быть выведена в DataGrid</param>
        /// <param name="dataGrid">Передает элемент dataGrid,
        /// в который будут переданы данные из
        /// выбранной таблицы</param>
        public void DB(string table, DataGrid dataGrid) //Подключение к БД
        {
            try
            {
                dbConnectionStrings();
                conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};");
                string sql = "SELECT * FROM " + table;
                adapter = new MySqlDataAdapter(sql, conn);
                conn.Open();
                MySqlCommandBuilder myCommandBuilder = new MySqlCommandBuilder(adapter as MySqlDataAdapter);
                adapter.InsertCommand = myCommandBuilder.GetInsertCommand();
                adapter.UpdateCommand = myCommandBuilder.GetUpdateCommand();
                adapter.DeleteCommand = myCommandBuilder.GetDeleteCommand();

                dt = new DataTable();
                adapter.Fill(dt); //загрузка данных
                dataGrid.ItemsSource = dt.DefaultView; //привязка к DataGrid
            }
            catch (MySqlException) { MessageBox.Show("Отсутствует подключение к базе данных"); }
        }

        /// <summary>
        /// Получение информации о преподавателях из БД
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public List<Teacher> GetTeachersFromDatabase(string table)
        {
            List<Teacher> teachers = new List<Teacher>();
            dbConnectionStrings();
            using (conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};"))
            {
                conn.Open();
                string sql = "SELECT * FROM " + table + " WHERE Дата_рождения AND Код_подразделения AND Путь_изображения IS NOT NULL";
                MySqlCommand command = new MySqlCommand(sql, conn);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Teacher teacher = new Teacher(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                        teacher.Id = reader.GetInt32(0);
                        teacher.Name = reader.GetString(1);
                        teacher.BirthDate = reader.GetString(2);
                        teacher.Subdivision = reader.GetString(3);
                        teacher.ImagePath = reader.GetString(4);
                        teachers.Add(teacher);
                    }
                }
                return teachers;
            }
        }

        /// <summary>
        /// Получение информации о грамот преподавателей из БД
        /// </summary>
        /// <param name="table"></param>
        /// <param name="IdTeacher"></param>
        /// <returns></returns>
        public List<Certificate> GetCertificatesFromDatabase(string table, int IdTeacher)
        {
            List<Certificate> certificates = new List<Certificate>();
            dbConnectionStrings();
            using (conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};"))
            {
                conn.Open();
                string sql = $"SELECT Код_грамоты, Наименование_грамоты, Путь_изображения FROM {table} WHERE Преподаватель = {IdTeacher}";
                MySqlCommand command = new MySqlCommand(sql, conn);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Certificate certificate = new Certificate(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                        certificate.Id = reader.GetInt32(0);
                        certificate.Name = reader.GetString(1);
                        certificate.FilePath = reader.GetString(2);
                        certificates.Add(certificate);
                    }
                }
                return certificates;
            }
        }

        /// <summary>
        /// Получение значения из таблицы
        /// </summary>
        /// <param name="table">Из какой таблицы получить элемент</param>
        /// <param name="inputItem">В каком столбце делать сравнение</param>
        /// <param name="field">Откуда получить значение</param>
        /// <param name="textBox">С помощью какой строки получить элемент</param>
        /// <returns></returns>
        public string GetValueByString(string table, string inputItem, string field, string textBox)
        {
            string sql = $"SELECT {field} FROM {table} WHERE {inputItem} = '{textBox}'";
            MySqlCommand command = new MySqlCommand(sql, conn);
            string result = Convert.ToString(command.ExecuteScalar());
            return result;
        }

        /// <summary>
        /// Поиск строки в таблице
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="table"></param>
        /// <param name="cond"></param>
        /// <param name="SearchName"></param>
        public void TableSearch(DataGrid dataGrid, string table, string cond, string SearchName)
        {
            string sql = "SELECT * FROM " + table + " WHERE " + cond + " LIKE '" + SearchName + "%'";
            adapter = new MySqlDataAdapter(sql, conn);
            MySqlCommandBuilder myCommandBuilder = new MySqlCommandBuilder(adapter as MySqlDataAdapter);
            adapter.InsertCommand = myCommandBuilder.GetInsertCommand();
            adapter.UpdateCommand = myCommandBuilder.GetUpdateCommand();
            adapter.DeleteCommand = myCommandBuilder.GetDeleteCommand();
            dt = new DataTable();
            adapter.Fill(dt);
            dataGrid.ItemsSource = dt.DefaultView;
        }

        /// <summary>
        /// Заполнение Combobox
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="field"></param>
        /// <param name="table"></param>
        public void FillCombobox(ComboBox comboBox, string field, string table)
        {
            dbConnectionStrings();
            using (conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};"))
            {
                conn.Open();
                string sql = "SELECT " + field + " FROM " + table;
                adapter = new MySqlDataAdapter(sql, conn);
                dt = new DataTable();
                adapter.Fill(dt);
                comboBox.ItemsSource = dt.DefaultView;
                comboBox.DisplayMemberPath = field;
            }
        }

        /// <summary>
        /// Сохраняет все измененные данные
        /// из DataGrid в базу данных   
        /// </summary>
        public void SaveTable() //Сохранение БД
        {
            adapter.Update(dt);
        }

        /// <summary>
        /// Получает количество строк в таблице
        /// </summary>
        /// <param name="table"></param>
        public int GetRowsCount(string table)
        {
            string sql = "SELECT count(*) FROM " + table;
            MySqlCommand command = new MySqlCommand(sql, conn);
            int result = Convert.ToInt32(command.ExecuteScalar());
            return result;
        }


        /// <summary>
        /// Проверка авторизации пользователя
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="passwordBox"></param>
        public bool AuthCheck(TextBox textBox, PasswordBox passwordBox)
        {
            try
            {
                dbConnectionStrings();
                MySqlConnection conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};");
                string sql = "SELECT * FROM пользователи WHERE Логин = @login and Пароль = MD5(@pass)";
                conn.Open();

                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();

                MySqlCommand command = new MySqlCommand(sql, conn);
                command.Parameters.Add("@login", MySqlDbType.VarChar, 25);
                command.Parameters.Add("@pass", MySqlDbType.VarChar, 25);

                command.Parameters["@login"].Value = textBox.Text;
                command.Parameters["@pass"].Value = passwordBox.Password;

                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    return true;
                    //    UserRole(); // метод, который будет открывать разные формы в зависимости от пользователя
                }
                else { MessageBox.Show($"Неправильный логин или пароль."); return false; }
            }
            catch (MySqlException) { MessageBox.Show("Отсутствует подключение к базе данных"); return false; }
        }

        //public void UserRole()
        //{
        //    string UserName = TextBox1.Text;

        //    string connStr = "server=localhost; port=3306; username=root; password= root; database=bd;";
        //    string sql = "SELECT User_Role FROM `user` WHERE `Name` = @un";

        //    MySqlConnection conn = new MySqlConnection(connStr);
        //    conn.Open();

        //    MySqlParameter nameParam = new MySqlParameter("@un", UserName);

        //    MySqlCommand command = new MySqlCommand(sql, conn);
        //    command.Parameters.Add(nameParam);

        //    string Form_Role = command.ExecuteScalar().ToString();

        //    Switch(Form_Role):
        //    {
        //        case "Администратор": Form.ActiveForm.Close(); Form1 f1 = new Form1(); f1.Show();
        //        break;
        //        default:  Form.ActiveForm.Close(); Form2 f2 = new Form2(); f2.Show();
        //    }
        //    conn.Close();
        //}
    }
}
