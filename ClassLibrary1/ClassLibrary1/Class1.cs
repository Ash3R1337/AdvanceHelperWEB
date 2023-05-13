using System.Data;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using AdvanceHelperWPF;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

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
        /// <summary>
        /// 
        /// </summary>
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
        public void DB(string table, System.Windows.Controls.DataGrid dataGrid) //Подключение к БД
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
                        Teacher teacher = new Teacher(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetString(7), reader.GetString(8));
                        teacher.Id = reader.GetInt32(0);
                        teacher.Name = reader.GetString(1);
                        teacher.BirthDate = reader.GetString(2);
                        teacher.Subdivision = reader.GetString(3);
                        teacher.ImagePath = reader.GetString(4);
                        teacher.WorkExp = reader.GetString(5);
                        teacher.Specialization = reader.GetString(6);
                        teacher.Phone = reader.GetString(7);
                        teacher.Email = reader.GetString(8);
                        teachers.Add(teacher);
                    }
                }
                return teachers;
            }
        }

        /// <summary>
        /// Получение информации о грамотах преподавателей из БД
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
        /// Получает количество из таблицы по id
        /// </summary> 
        /// <param name="table"></param>
        /// <param name="fieldCount"></param>
        /// <param name="field"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetCount(string table, string fieldCount, string field, int id)
        {
            dbConnectionStrings();
            using (conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};"))
            {
                conn.Open();
                string sql = $"SELECT count({fieldCount}) FROM {table} WHERE {field} = @id";
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.Parameters.AddWithValue("@id", id);
                int result = Convert.ToInt32(command.ExecuteScalar());
                return result;
            }
        }

        /// <summary>
        /// Получает количество по строке
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fieldCount"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetCountByString(string fieldCount, string table, string value)
        {
            dbConnectionStrings();
            using (conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};"))
            {
                conn.Open();
                string sql = $"SELECT count({fieldCount}) FROM {table} WHERE {fieldCount} = @value";
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.Parameters.AddWithValue("@value", value);
                string result = Convert.ToString(command.ExecuteScalar());
                return result;
            }
        }

        /// <summary>
        /// Выбрать общее количество из таблицы
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fieldCount"></param>
        /// <returns></returns>
        public int GetAllCount(string table, string fieldCount)
        {
            dbConnectionStrings();
            using (conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};"))
            {
                conn.Open();
                string sql = $"SELECT count({fieldCount}) FROM {table}";
                MySqlCommand command = new MySqlCommand(sql, conn);
                int result = Convert.ToInt32(command.ExecuteScalar());
                return result;
            }
        }

        /// <summary>
        /// Получает количество материалов, у которых есть
        /// все документы
        /// </summary>
        /// <param name="table"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public int GetCountByDocs(string field, string table)
        {
            try
            {
                dbConnectionStrings();
                using (conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};"))
                {
                    conn.Open();
                    string sql = $"SELECT count({field}) FROM {table} WHERE Титул_РП and РП and Титул_ФОС and ФОС and ВнутрРец and ЭкспЗакл and ВСРС and МУПР = 1";
                    MySqlCommand command = new MySqlCommand(sql, conn);
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    return result;
                }
            }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return 0; }
        }

        /// <summary>
        /// Получает количество материалов, у которых есть
        /// все документы по id преподавателя
        /// </summary>
        /// <param name="field"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public int GetDocsCountByTeacher(string field, string table, int id)
        {
            try
            {
                dbConnectionStrings();
                using (conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};"))
                {
                    conn.Open();
                    string sql = $"SELECT count({field}) FROM {table} WHERE Титул_РП and РП and Титул_ФОС and ФОС and ВнутрРец and ЭкспЗакл and ВСРС and МУПР = 1 and Код_преподавателя = @id";
                    MySqlCommand command = new MySqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@id", id);
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    return result;
                }
            }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return 0; }
        }

        /// <summary>
        /// Получение значения из таблицы
        /// </summary>
        /// <param name="table">Из какой таблицы получить элемент</param>
        /// <param name="inputItem">В каком столбце делать сравнение</param>
        /// <param name="field">Откуда получить значение</param>
        /// <param name="value">С помощью какой строки получить элемент</param>
        /// <returns></returns>
        public string GetValueByString(string field, string table, string inputItem, string value)
        {
            dbConnectionStrings();
            using (conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};"))
            {
                conn.Open();
                string sql = $"SELECT {field} FROM {table} WHERE {inputItem} = @value";
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.Parameters.AddWithValue("@value", value);
                string result = Convert.ToString(command.ExecuteScalar());
                return result;
            }
        }

        /// <summary>
        /// Выбирает преподавателя, который сделал большее количество материалов
        /// </summary>
        /// <returns></returns>
        public string GetMaxTeacher(string field, string table)
        {
            dbConnectionStrings();
            using (conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};"))
            {
                conn.Open();
                string sql = $"SELECT {table}.{field} FROM {table} JOIN материалы ON {table}.Код_преподавателя = материалы.Код_преподавателя GROUP BY {table}.Код_преподавателя, {table}.{field} ORDER BY COUNT(*) DESC LIMIT 1;";
                MySqlCommand command = new MySqlCommand(sql, conn);
                string result = Convert.ToString(command.ExecuteScalar());
                return result;
            }
        }

        /// <summary>
        /// Выбирает преподавателя, который сделал меньшее количество материалов
        /// </summary>
        /// <returns></returns>
        public string GetMinTeacher(string field, string table)
        {
            dbConnectionStrings();
            using (conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};"))
            {
                conn.Open();
                string sql = $"SELECT {table}.{field} FROM {table} JOIN материалы ON {table}.Код_преподавателя = материалы.Код_преподавателя GROUP BY {table}.Код_преподавателя, {table}.{field} ORDER BY COUNT(*) ASC LIMIT 1;";
                MySqlCommand command = new MySqlCommand(sql, conn);
                string result = Convert.ToString(command.ExecuteScalar());
                return result;
            }
        }

        /// <summary>
        /// Поиск строки в таблице
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="table"></param>
        /// <param name="cond"></param>
        /// <param name="SearchName"></param>
        public void TableSearch(System.Windows.Controls.DataGrid dataGrid, string table, string cond, string SearchName)
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
        public void FillCombobox(System.Windows.Controls.ComboBox comboBox, string field, string table)
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
        public void SaveTable()
        {
            try
            {
                adapter.Update(dt);
                MessageBox.Show("Таблица была успешно сохранена");
            }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        /// <summary>
        /// Шифрование пароля MD5
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string EncryptPassword(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "");
                return hashedPassword;
            }
        }
        /// <summary>
        /// Сохранение таблицы пользователей с шифрованием пароля
        /// </summary>
        public void SaveUsersTable()
        {
            if (!dt.Columns.Contains("Пароль")) // Если столбца "Пароль" нет в таблице, выходим из метода
                return;
            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState == DataRowState.Deleted) // Если строка удалена, пропускаем ее
                    continue;
                if (row.IsNull("Пароль")) // Если у строки нет значения в столбце "Пароль", пропускаем ее
                    continue;
                string password = row["Пароль"].ToString();
                if (password.Length == 32) // Зашифрованный пароль уже есть в таблице, пропускаем шифрование
                    continue;
                // Шифруем пароль
                string hashedPassword = EncryptPassword(password);
                row["Пароль"] = hashedPassword;
            }
            SaveTable();
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
        /// Получает логин пользователя по коду в таблице подразделение
        /// </summary>
        /// <param name="field"></param>
        /// <param name="table"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetUserLoginFromSubdivision(string field, string table, int id)
        {
            dbConnectionStrings();
            using (conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};"))
            {
                conn.Open();
                string sql = $"SELECT пользователи.{field} FROM {table} JOIN пользователи ON {table}.Код_пользователя = пользователи.Код_пользователя WHERE {table}.Код_подразделения = @id";
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.Parameters.AddWithValue("@id", id);
                string result = Convert.ToString(command.ExecuteScalar());
                return result;
            }
        }


        /// <summary>
        /// Проверка авторизации пользователя
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="passwordBox"></param>
        public bool AuthCheck(System.Windows.Controls.TextBox textBox, PasswordBox passwordBox)
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

                if (table.Rows.Count > 0) return true;
                else { MessageBox.Show($"Неправильный логин или пароль."); return false; }
            }
            catch (MySqlException) { MessageBox.Show("Отсутствует подключение к базе данных"); return false; }
        }
    }
}
