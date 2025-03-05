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
                size += DirSize(d);
            }
            return size;
        }
    }
}
