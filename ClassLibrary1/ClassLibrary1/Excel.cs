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
                if (!File.Exists(FileName + ".xlsx"))
                {
                    sheet = excelPackage.Workbook.Worksheets.Add("Практические работы");
                    ExcelGenerator(sheet/*, Students*/);
                    FileInfo excelFile = new FileInfo(FileName + ".xlsx");
                    excelPackage.SaveAs(excelFile);
                    Process.Start(FileName + ".xlsx");
                }
                else
                {
                    Process.Start(FileName + ".xlsx");
                }
            }
        }

        public void ExcelGenerator(ExcelWorksheet sheet/*, List<string> list*/) //Формирование Excel документа
        {
            conn = new MySqlConnection("server=localhost;user=root;database=projectdb;port=3306;password=root;");
            string sql = "SELECT Код_предмета FROM предметы";
            var cmd = new MySqlCommand(sql, conn);
            conn.Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                sheet.Cells[1, 1].Value = "Номер";
                sheet.Cells[1, 2].Value = "ФИО студента";
                sheet.Cells[1, 3].Value = "Практические работы";
                sheet.Cells["A1:A2"].Merge = true;
                sheet.Cells["B1:B2"].Merge = true;
                /*Добавление колонок*/
                int index = 3;
                while (reader.Read())
                {
                    int result = reader.GetInt32(0);
                    sheet.Cells[2, index].Value = result;
                    index += 1;
                }
                sheet.Cells[1, 3, 1, index - 1].Merge = true;
                /*Добавление строк*/
                /*for (int i = 0; i < list.Count; i++)
                {
                    sheet.Cells[i + 3, 1].Value = i + 1;
                    sheet.Cells[i + 3, 2].Value = list[i];
                }*/
                /*Стили таблицы*/
                sheet.Cells.Style.Font.Size = 14;
                sheet.Cells.Style.Font.Name = "Times New Roman";
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                sheet.Column(2).Width = 22;
                sheet.Cells[1, 1, 2, index].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[1, 1, 2, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                /*sheet.Cells[3, 1, list.Count + 2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[1, 1, list.Count + 2, index - 1].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                sheet.Cells[1, 1, list.Count + 2, index - 1].Style.Border.Right.Style = ExcelBorderStyle.Thick;
                sheet.Cells[1, 1, list.Count + 2, index - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                sheet.Cells[1, 1, list.Count + 2, index - 1].Style.Border.Left.Style = ExcelBorderStyle.Thick;*/
            }
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
