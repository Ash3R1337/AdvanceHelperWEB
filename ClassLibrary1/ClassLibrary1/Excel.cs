using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Diagnostics;
using System.Linq;
using AdvanceHelperWPF;
using System.Collections.Generic;

namespace AHlibrary
{
    /// <summary>
    /// Набор функций для работы
    /// с документами Excel
    /// </summary>
    public class Excel
    {
        MySqlConnection conn;
        string dbusername;
        string dbname;
        string password;
        string userLogin;
        FileHandler fileHandler = new FileHandler();

        /// <summary>
        /// Добавляет строки подключения из файла
        /// </summary>
        public void dbConnectionStrings()
        {
            dbusername = fileHandler.GetPath("config.txt", "Имя пользователя базы данных: ");
            dbname = fileHandler.GetPath("config.txt", "Название базы данных: ");
            password = fileHandler.GetPath("config.txt", "Пароль базы данных: ");
        }

        /// <summary>
        /// Создает Excel-документ
        /// </summary>
        /// <param name="FileName"></param>
        public void ExcelCreateDocument(string FileName, string UserLogin)
        {
            userLogin = UserLogin;
            ExcelWorksheet sheet;
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                try
                {
                    sheet = excelPackage.Workbook.Worksheets.Add("Ведомость"); //Каждый раз содается новый документ
                    ExcelGenerator(sheet);
                    FileInfo excelFile = new FileInfo(FileName + ".xlsx");
                    excelPackage.SaveAs(excelFile);
                    Process.Start(FileName + ".xlsx");
                }
                catch (InvalidOperationException) { MessageBox.Show("Документ уже открыт"); }
            }
        }

        /// <summary>
        /// Заполняет Excel-документ
        /// </summary>
        /// <param name="sheet"></param>
        public void ExcelGenerator(ExcelWorksheet sheet) //Формирование Excel документа
        {
            try
            {
                dbConnectionStrings();
                conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};");
                /*Добавление колонок*/
                sheet.Cells[1, 1].Value = "Индекс";
                sheet.Cells[1, 2].Value = "Наименование\nОП";
                sheet.Cells[1, 3].Value = "Титул\nРП";
                sheet.Cells[1, 4].Value = "РП";
                sheet.Cells[1, 5].Value = "Титул\nФОС";
                sheet.Cells[1, 6].Value = "ФОС";
                sheet.Cells[1, 7].Value = "Внутр.\nрец.";
                sheet.Cells[1, 8].Value = "Эксп.\nзакл.";
                sheet.Cells[1, 9].Value = "ВСРС";
                sheet.Cells[1, 10].Value = "МУПР";
                sheet.Cells[1, 11].Value = "Ответственный";
                sheet.Cells[sheet.Dimension.Address].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                /*Добавление строк*/
                int UserId = GetIDbyString("Код_пользователя", "пользователи", "Логин", userLogin); //Получение id пользователя
                int SubId = GetID("Код_подразделения", "подразделение", "Код_пользователя", UserId); //Получение id подразделения
                List<int> specialitiesIds = GetSpecialitiesIds("специальности", SubId);
                for (int i = 0; i < specialitiesIds.Count; i++)
                {
                    sheet.Cells[sheet.Dimension.End.Row + 1, sheet.Dimension.Start.Column, sheet.Dimension.End.Row + 1, sheet.Dimension.End.Column].Merge = true;
                    sheet.Cells[sheet.Dimension.End.Row + 1, 1].Value = GetValueById("Наименование", "специальности", "Код_специальности", specialitiesIds[i], SubId); //Наименование специальности
                    sheet.Cells[sheet.Dimension.End.Row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    FillRows(specialitiesIds[i], sheet);
                }
                FindAndReplace(sheet, "True", "+");
                FindAndReplace(sheet, "False", " ");
                /*Стили таблицы*/
                sheet.Cells.Style.Font.Size = 14;
                sheet.Cells.Style.Font.Name = "Times New Roman";
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns(10, 62);
                sheet.Cells[sheet.Dimension.Address].Style.WrapText = true;
                sheet.Cells[sheet.Dimension.Address].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                sheet.Column(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Column(11).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                sheet.Column(11).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[sheet.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                sheet.Cells[sheet.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells[sheet.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells[sheet.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                sheet.Row(1).Height = 50;
            }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        /// <summary>
        /// Заполняет строки значениями
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sheet"></param>
        public void FillRows(int id, ExcelWorksheet sheet)
        {
            dbConnectionStrings();
            conn.Open();
            string sql = "SELECT Индекс, Наименование_предмета, Титул_РП, РП, Титул_ФОС, ФОС, ВнутрРец, ЭкспЗакл, ВСРС, МУПР, ФИО " +
                "FROM материалы JOIN предметы ON материалы.Код_предмета = предметы.Код_предмета " +
                "JOIN преподаватели ON материалы.Код_преподавателя = преподаватели.Код_преподавателя " +
                $"WHERE CONCAT(',', Код_специальности, ',') LIKE '%,{id},%'";
            MySqlCommand command = new MySqlCommand(sql, conn);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int LastRow = sheet.Dimension.End.Row + 1;
                for (int col = 1; col <= reader.FieldCount; col++)
                {
                    sheet.Cells[LastRow, col].Value = reader.GetValue(col - 1).ToString();
                }
            }
            conn.Close();
        }

        /// <summary>
        /// Находит строку во всем документе и заменяет
        /// на заданную строку
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="valueToSearch"></param>
        /// <param name="valueToReplace"></param>
        public void FindAndReplace(ExcelWorksheet sheet, string valueToSearch, string valueToReplace)
        {
            var query = from cell in sheet.Cells[sheet.Dimension.Address]
                        where cell.Value?.ToString().Contains(valueToSearch) == true
                        select cell;

            foreach (var cell in query)
            {
                cell.Value = cell.Value.ToString().Replace(valueToSearch, valueToReplace);
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            }
        }

        /// <summary>
        /// Получает количество строк в таблице
        /// </summary>
        /// <param name="table"></param>
        /// <param name="SubId"></param>
        /// <returns></returns>
        public int GetRowsCount(string table, int SubId)
        {
            dbConnectionStrings();
            conn.Open();
            string sql = $"SELECT count(*) FROM {table} WHERE Код_подразделения = @SubId";
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.AddWithValue("@SubId", SubId);
            int result = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();
            return result;
        }

        /// <summary>
        /// Получает значение по идентификатору поля
        /// </summary>
        /// <param name="table"></param>
        /// <param name="id"></param>
        /// <param name="ColName"></param>
        /// <param name="value"></param>
        /// <param name="SubId"></param>
        /// <returns></returns>
        public string GetValueById(string value, string table, string ColName, int id, int SubId)
        {
            dbConnectionStrings();
            conn.Open();
            string sql = $"SELECT {value} FROM {table} WHERE {ColName} = @id AND Код_подразделения = @SubId";
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@SubId", SubId);
            string result = Convert.ToString(command.ExecuteScalar());
            conn.Close();
            return result;
        }

        /// <summary>
        /// Получает количество строк в таблице с определенным
        /// идентификатором
        /// </summary>
        /// <param name="table"></param>
        /// <param name="id"></param>
        /// <param name="ColName"></param>
        /// <returns></returns>
        public int GetRowsCountById(string table, int id, string ColName)
        {
            dbConnectionStrings();
            conn.Open();
            string sql = $"SELECT count(*) FROM {table} WHERE {ColName} = {id}";
            MySqlCommand command = new MySqlCommand(sql, conn);
            int result = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();
            return result;
        }

        /// <summary>
        /// Получает идентификатор
        /// </summary>
        /// <param name="table"></param>
        /// <param name="TableId"></param>
        /// <param name="ColName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int GetID(string value, string table, string ColName, int TableId)
        {
            dbConnectionStrings();
            conn.Open();
            string sql = $"SELECT {value} FROM {table} WHERE {ColName} = @TableId";
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.AddWithValue("@TableId", TableId);
            int result = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();
            return result;
        }

        /// <summary>
        /// Получение идентификатора по строке
        /// </summary>
        /// <param name="value"></param>
        /// <param name="table"></param>
        /// <param name="ColName"></param>
        /// <param name="TableString"></param>
        /// <returns></returns>
        public int GetIDbyString(string value, string table, string ColName, string TableString)
        {
            dbConnectionStrings();
            conn.Open();
            string sql = $"SELECT {value} FROM {table} WHERE {ColName} = @TableString";
            MySqlCommand command = new MySqlCommand(sql, conn);
            command.Parameters.AddWithValue("@TableString", TableString);
            int result = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();
            return result;
        }

        /// <summary>
        /// Получает все идентификаторы специальностей из таблицы специальности
        /// </summary>
        /// <param name="table"></param>
        /// <param name="SubId"></param>
        /// <returns></returns>
        public List<int> GetSpecialitiesIds(string table, int SubId)
        {
            List<int> specialitiesIds = new List<int>();
            dbConnectionStrings();
            using (conn = new MySqlConnection($"server=localhost;user={dbusername};database={dbname};port=3306;password={password};"))
            {
                conn.Open();
                string sql = $"SELECT Код_специальности FROM {table} WHERE Код_подразделения = @SubId";
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.Parameters.AddWithValue("@SubId", SubId);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int specialityCode = (int)reader["Код_специальности"];
                        specialitiesIds.Add(specialityCode);
                    }
                }
                return specialitiesIds;
            }
        }
    }
}
