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
        FileHandler fileHandler = new FileHandler();

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
        public void ExcelCreateDocument(string FileName)
        {
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
            int SpecialtiesCount = GetRowsCount("специальности");
            for (int i = 1; i <= SpecialtiesCount; i++)
            {
                sheet.Cells[sheet.Dimension.End.Row + 1, sheet.Dimension.Start.Column, sheet.Dimension.End.Row + 1, sheet.Dimension.End.Column].Merge = true;
                sheet.Cells[sheet.Dimension.End.Row + 1, 1].Value = GetValueById("специальности", i, "Код_специальности", "Наименование");
                sheet.Cells[sheet.Dimension.End.Row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                FillRows(i, sheet);
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

        /// <summary>
        /// Заполняет строки значениями
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sheet"></param>
        public void FillRows(int id, ExcelWorksheet sheet)
        {
            conn.Open();
            string sql = "SELECT Индекс, Наименование_предмета, Титул_РП, РП, Титул_ФОС, ФОС, ВнутрРец, ЭкспЗакл, ВСРС, МУПР, ФИО " +
                "FROM материалы JOIN предметы ON материалы.Код_предмета = предметы.Код_предмета " +
                "JOIN преподаватели ON материалы.Код_преподавателя = преподаватели.Код_преподавателя " +
                $"WHERE Код_специальности = {id}";
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
        /// <returns></returns>
        public int GetRowsCount(string table)
        {
            conn.Open();
            string sql = "SELECT count(*) FROM " + table;
            MySqlCommand command = new MySqlCommand(sql, conn);
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
        /// <returns></returns>
        public string GetValueById(string table, int id, string ColName, string value)
        {
            conn.Open();
            string sql = $"SELECT {value} FROM {table} WHERE {ColName} = '{id}'";
            MySqlCommand command = new MySqlCommand(sql, conn);
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
        public int GetID(string table, int TableId, string ColName, string value)
        {
            conn.Open();
            string sql = $"SELECT {value} FROM {table} WHERE {ColName} = '{TableId}'";
            MySqlCommand command = new MySqlCommand(sql, conn);
            int result = Convert.ToInt32(command.ExecuteScalar());
            conn.Close();
            return result;
        }

        public void ExcelAddValues(string Student, string Work, string Estimate, string FileName) //Заполнение ячеек таблицы
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(FileName + ".xlsx"))
            {
                ExcelWorksheet sheet = excelPackage.Workbook.Worksheets["Практические работы"];
                int Column = 0;
                int Row = 0;
                for (int i = sheet.Dimension.Start.Column + 2; i < sheet.Dimension.End.Column; i++) //Поиск колонки
                    if (sheet.Cells[2, i].Value.ToString() == Work)
                        Column = i;
                for (int j = sheet.Dimension.Start.Row + 2; j <= sheet.Dimension.End.Row; j++) //Поиск строки
                    if (sheet.Cells[j, 2].Value.ToString() == Student)
                        Row = j;
                /*Добавление значения в таблицу*/
                switch (Estimate)
                {
                    case "5":
                        sheet.Cells[Row, Column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        /*sheet.Cells[Row, Column].Style.Font.Color.SetColor(Color.White);*/
                        sheet.Cells[Row, Column].Value = "5";
                        break;
                    case "4":
                        sheet.Cells[Row, Column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        /*sheet.Cells[Row, Column].Style.Font.Color.SetColor(Color.White);*/
                        sheet.Cells[Row, Column].Value = "4";
                        break;
                    case "3":
                        sheet.Cells[Row, Column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        /*sheet.Cells[Row, Column].Style.Font.Color.SetColor(Color.White);*/
                        sheet.Cells[Row, Column].Value = "3";
                        break;
                    case "2":
                        sheet.Cells[Row, Column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        /*sheet.Cells[Row, Column].Style.Font.Color.SetColor(Color.White);*/
                        sheet.Cells[Row, Column].Value = "2";
                        break;
                    case "Не выставлена":
                        sheet.Cells[Row, Column].Value = " ";
                        break;
                }
                excelPackage.Save();
                MessageBox.Show("Работа была успешно отмечена");
            }
        }
    }
}
