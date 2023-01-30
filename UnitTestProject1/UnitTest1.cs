using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdvanceHelperWEB;
using System;
using System.Windows.Controls;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FileSort_Test() //Проверка метода сортировки файлов
        {
            string DirPath = "D:\\DiskPC\\Флешка\\Работы\\Курсовая";
            Start start = new Start();
            start.FileSort(DirPath);
        }

        [TestMethod]
        public void FileSave_Test() //Проверка сохранения файла
        {
            string FileName = "save.txt";
            string DirPath = "D:\\DiskPC\\Флешка\\Работы\\Курсовая";
            Start start = new Start();
            start.FileSave(FileName, DirPath);
        }

        [TestMethod]
        public void CreateCatalog_Test() //Проверка создания каталога
        {
            string DirPath = "D:\\DiskPC\\Флешка\\Работы\\Курсовая";
            Start start = new Start();
            bool actual = start.CreateCatalog(DirPath);
            Assert.IsTrue(actual);
        }
    }
}
