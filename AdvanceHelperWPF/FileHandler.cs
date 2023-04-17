using System.IO;
using System.Windows;

namespace AdvanceHelperWPF
{
    public class FileHandler
    {
        public string GetPath(string FileName, string SubLine)
        {
            try
            {
                string DirPathStr = "";
                using (StreamReader sr = new StreamReader(FileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith(SubLine))
                            return DirPathStr = line.Substring(SubLine.Length);
                    }
                    return DirPathStr;
                }
            }
            catch (System.IO.FileNotFoundException) {MessageBox.Show("Файл config.txt не обнаружен"); return ""; }

        }

        public void FileSave(string file, string DirPath, string SubLine) //Сохранение файла
        {
            string tempFile = Path.GetTempFileName();
            if (!File.Exists(file))
                File.WriteAllText(file, SubLine + DirPath); //Создание файла и сохранение пути
            else
            {
                using (var sr = new StreamReader(file)) //Если файл создан, то новый сохраненный путь будет перезаписан
                using (var sw = new StreamWriter(tempFile))
                {
                    string line;
                    bool pathSaved = false;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith(SubLine))
                        {
                            sw.WriteLine(SubLine + DirPath);
                            pathSaved = true;
                        }
                        else
                        {
                            sw.WriteLine(line);
                        }
                    }

                    if (!pathSaved)
                    {
                        sw.WriteLine(SubLine + DirPath);
                    }
                }

                File.Delete(file);
                File.Move(tempFile, file);
            }
        }
    }
}
