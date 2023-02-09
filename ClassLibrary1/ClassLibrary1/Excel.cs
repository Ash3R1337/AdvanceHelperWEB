using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.IO;
using System.Diagnostics;

namespace AHlibrary
{
    /// <summary>
    /// Набор функций для работы
    /// с документами Excel
    /// </summary>
    public class Excel
    {

        DbDataAdapter adapter;
        DataTable dt;
        MySqlConnection conn;

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
                /*if (!File.Exists(FileName + ".xlsx")) //Если документ создат, то он просто открывается
                {
                    sheet = excelPackage.Workbook.Worksheets.Add("Ведомость");
                    ExcelGenerator(sheet);
                    FileInfo excelFile = new FileInfo(FileName + ".xlsx");
                    excelPackage.SaveAs(excelFile);
                    Process.Start(FileName + ".xlsx");
                }
                else
                {
                    Process.Start(FileName + ".xlsx");
                }*/
            }
        }

        public void ExcelGenerator(ExcelWorksheet sheet) //Формирование Excel документа
        {
            conn = new MySqlConnection("server=localhost;user=root;database=projectdb;port=3306;password=root;");
            string sql = "SELECT Код_предмета FROM предметы";
            var cmd = new MySqlCommand(sql, conn);
            conn.Open();
            /* using (MySqlDataReader reader = cmd.ExecuteReader())
             {*/
            sheet.Cells[1, 1].Value = "Индекс";
            sheet.Cells[1, 2].Value = "Наименование ОП";
            sheet.Cells[1, 3].Value = "Титул РП";
            sheet.Cells[1, 4].Value = "РП";
            sheet.Cells[1, 5].Value = "Титул ФОС";
            sheet.Cells[1, 6].Value = "ФОС";
            sheet.Cells[1, 7].Value = "Внутр. рец.";
            sheet.Cells[1, 8].Value = "Эксп. закл.";
            sheet.Cells[1, 9].Value = "ВСРС";
            sheet.Cells[1, 10].Value = "МУПР";
            sheet.Cells[1, 11].Value = "Ответственный";
            /*Добавление колонок*/
            /*int index = 3;
            while (reader.Read())
            {
                int result = reader.GetInt32(0);
                sheet.Cells[2, index].Value = result;
                index += 1;
            }*/
            /*sheet.Cells[1, 3, 1, index - 1].Merge = true;*/
            /*Добавление строк*/
            int SubdivisionCount = GetRowsCount("подразделение");
            int SubjectsCount = GetRowsCount("предметы");
            for (int i = 1; i <= SubdivisionCount; i++)
            {
                sheet.Cells[sheet.Dimension.End.Row + 1, sheet.Dimension.Start.Column, sheet.Dimension.End.Row + 1, sheet.Dimension.End.Column].Merge = true;
                sheet.Cells[sheet.Dimension.End.Row + 1, 1].Value = GetValueById("подразделение", i, "Код_подразделения", "Цикловая_комиссия");
                for (int j = 1; j <= SubjectsCount; j++)
                {
                    int LastRow = sheet.Dimension.End.Row + 1;
                    sheet.Cells[LastRow, 1].Value = GetValueById("предметы", j, "Код_предмета", "Наименование_предмета");
                    sheet.Cells[LastRow, 3].Value = GetValueById("материалы", j, "Код_Материала", "Титул_РП");
                    sheet.Cells[LastRow, 4].Value = GetValueById("материалы", j, "Код_Материала", "РП");
                    sheet.Cells[LastRow, 5].Value = GetValueById("материалы", j, "Код_Материала", "Титул_ФОС");
                    sheet.Cells[LastRow, 6].Value = GetValueById("материалы", j, "Код_Материала", "ФОС");
                    sheet.Cells[LastRow, 7].Value = GetValueById("материалы", j, "Код_Материала", "ВнутрРец");
                    sheet.Cells[LastRow, 8].Value = GetValueById("материалы", j, "Код_Материала", "ЭкспЗакл");
                    sheet.Cells[LastRow, 9].Value = GetValueById("материалы", j, "Код_Материала", "ВСРС");
                    sheet.Cells[LastRow, 10].Value = GetValueById("материалы", j, "Код_Материала", "МУПР");
                    int teacherID = GetID("материалы", j, "Код_Материала", "Код_преподавателя");
                    sheet.Cells[LastRow, 11].Value = GetValueById("преподаватели", teacherID, "Код_преподавателя", "ФИО");
                }
                //sheet.Cells[i + 3, 2].Value = list[i];
            }
            /*Стили таблицы*/
            sheet.Cells.Style.Font.Size = 14;
            sheet.Cells.Style.Font.Name = "Times New Roman";
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            sheet.Column(2).Width -= 1;
            sheet.Column(3).Width -= 1;
            sheet.Column(5).Width -= 1;
            sheet.Column(7).Width -= 1;
            sheet.Column(8).Width -= 1;
            sheet.Cells[sheet.Dimension.Address].Style.WrapText = true;
            sheet.Cells[sheet.Dimension.Address].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            sheet.Cells[sheet.Dimension.Address].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Column(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[sheet.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet.Cells[sheet.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            sheet.Cells[sheet.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells[sheet.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet.Row(1).Height = 50;
            /* }*/
        }

        /// <summary>
        /// Получает количество строк в таблице
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public int GetRowsCount(string table)
        {
            string sql = "SELECT count(*) FROM " + table;
            MySqlCommand command = new MySqlCommand(sql, conn);
            int result = Convert.ToInt32(command.ExecuteScalar());
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
            string sql = $"SELECT {value} FROM {table} WHERE {ColName} = '{id}'";
            MySqlCommand command = new MySqlCommand(sql, conn);
            string result = Convert.ToString(command.ExecuteScalar());
            return result;
        }

        public int GetID(string table, int TableId, string ColName, string value)
        {
            string sql = $"SELECT {value} FROM {table} WHERE {ColName} = '{TableId}'";
            MySqlCommand command = new MySqlCommand(sql, conn);
            int result = Convert.ToInt32(command.ExecuteScalar());
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

        public void WorksCheck(string FileName)
        {
            /*ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            string connectString = "Data Source=.\\SQLEXPRESS1;Initial Catalog=Application;" + "Integrated Security=true;";
            using (SqlConnection myConnection = new SqlConnection(connectString))
            {
                myConnection.Open();
                DateTime CurDate = DateTime.Today;
                using (ExcelPackage excelPackage = new ExcelPackage(FileName + ".xlsx"))
                {
                    ExcelWorksheet sheet = excelPackage.Workbook.Worksheets["Практические работы"];
                    for (int i = sheet.Dimension.Start.Column + 2; i < sheet.Dimension.End.Column; i++)
                        for (int j = sheet.Dimension.Start.Row + 2; j <= sheet.Dimension.End.Row; j++)
                        {
                            SqlCommand command = new SqlCommand($"SELECT Дата_окончания FROM Works WHERE Номер_работы = {sheet.Cells[2, i].Value}", myConnection);
                            SqlDataReader reader;
                            reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                DateTime EndDate = reader.GetDateTime(0);
                                if (string.IsNullOrEmpty((string)sheet.Cells[j, i].Value) && (CurDate > EndDate))
                                {
                                    sheet.Cells[j, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[j, i].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                }
                                if (!((sheet.Cells[j, i].Style.Fill.BackgroundColor.Rgb == Color.DodgerBlue.ToArgb().ToString("X")) || (sheet.Cells[j, i].Style.Fill.BackgroundColor.Rgb == Color.LightGreen.ToArgb().ToString("X")) || (sheet.Cells[j, i].Style.Fill.BackgroundColor.Rgb == Color.Khaki.ToArgb().ToString("X"))))
                                {
                                    if (!string.IsNullOrEmpty((string)sheet.Cells[j, i].Value) && (CurDate <= EndDate))
                                    {
                                        sheet.Cells[j, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        sheet.Cells[j, i].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
                                    }
                                    else if (!string.IsNullOrEmpty((string)sheet.Cells[j, i].Value) && (CurDate > EndDate) && (CurDate <= EndDate.AddDays(7)))
                                    {
                                        sheet.Cells[j, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        sheet.Cells[j, i].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                                    }
                                    else if (!string.IsNullOrEmpty((string)sheet.Cells[j, i].Value) && CurDate > EndDate.AddDays(7))
                                    {
                                        sheet.Cells[j, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        sheet.Cells[j, i].Style.Fill.BackgroundColor.SetColor(Color.Khaki);
                                    }
                                }
                            }
                            reader.Close();
                        }

                    excelPackage.Save();
                }
            }*/
        }
    }
}
