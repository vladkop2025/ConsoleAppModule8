using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp8
{
    public static class DirectoryExtention
    {
        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            try
            {
                FileInfo[] fis = d.GetFiles();
                //подсчет размера всех файлов в выбранной директории
                foreach (FileInfo fi in fis)
                {
                    size += fi.Length;
                }

                DirectoryInfo[] dis = d.GetDirectories();
                foreach (DirectoryInfo di in dis)
                //подсчет размера всех директорий, вложенных в выбранную директорию
                {
                    size += DirSize(di);
                }
            }

            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Нет доступа к директории: {d.FullName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Console.WriteLine($"Размер директории: {FormatSize(size)}");
            return size;
        }

        static string FormatSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (bytes >= 1024 && order < sizes.Length - 1)
            {
                order++;
                bytes /= 1024;
            }
            return $"{bytes:0.##} {sizes[order]}";
        }
    }
}
