using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualBasic.FileIO; // Не забудьте добавить ссылку на Microsoft.VisualBasic

/*
1. Добавление ссылки на Microsoft.VisualBasic
Чтобы использовать Microsoft.VisualBasic.FileIO, нужно добавить ссылку на сборку Microsoft.VisualBasic в ваш проект. Вот как это сделать:

В Visual Studio:
Откройте ваш проект.

В Solution Explorer (Обозреватель решений) нажмите правой кнопкой мыши на ваш проект и выберите Add → Reference (Добавить → Ссылка).

В открывшемся окне выберите Assemblies (Сборки) → Framework.

Найдите и поставьте галочку напротив Microsoft.VisualBasic.

Нажмите OK.
*/

namespace ConsoleApp8
{
    class Program
    {
        static void Main(string[] args)
        {
            //Модуль 8.Работа с файлами - 8.2. Классы для работы с дисками и директориями
            /*

            Файлы и каталоги, которыми оперируют ваши программы, расположены на физических дисках, поэтому изучать уместно с них, как с самого нижнего уровня.

            Для представления диска в пространстве имен System.IO имеется класс DriveInfo.

            Он имеет статический метод GetDrives(), возвращающий имена всех логических дисков компьютера.

            Также в классе есть много свойств, откуда вы можете получить полезную информацию: 

                AvailableFreeSpace: указывает на объем доступного свободного места на диске в байтах;
                DriveFormat: получает имя файловой системы;
                DriveType: представляет тип диска;
                IsReady: готов ли диск (например, DVD-диск может быть не вставлен в дисковод);
                Name: получает имя диска;
                TotalFreeSpace: получает общий объем свободного места на диске в байтах;
                TotalSize: общий размер диска в байтах;
                VolumeLabel: получает или устанавливает метку тома.

            Попробуем получить информацию о дисках на компьютере: 

                Название: C:\
                Тип: Fixed
                Объем: 1023398637568
                Свободно: 633033805824
                Метка:
                Название: D:\
                Тип: CDRom

            Таким образом, класс DriveInfo предоставляет нам удобный высокоуровневый интерфейс получения информации с дисковой системы машины.

            Теперь поднимемся на один уровень выше, к тому, что располагается на диске — каталогам (или папкам, как для вас более привычно).

            Для работы с ними в языке С#  предусмотрены два класса: Directory и DirectoryInfo.

            Класс Directory предоставляет ряд статических методов для управления каталогами.  
            Наиболее используемые:

                CreateDirectory(path): создает каталог по указанному пути;
                Delete(path): удаляет каталог по указанному пути;
                Exists(path): определяет, существует ли каталог по указанному пути; 
                GetDirectories(path): получает список каталогов в каталоге path;
                GetFiles(path): получает список файлов в каталоге path;
                Move(source, destination): перемещает каталог;
                GetParent(path): получение родительского каталог
    
              Класс DirectoryInfo по функционалу во многом похож на Directory и позволяет нам создавать, удалять, перемещать и производить другие операции с каталогами. 

            Наиболее используемые:

                Create(): создает каталог;
                CreateSubdirectory(path): создает подкаталог по указанному пути path;
                Delete(): удаляет каталог;
                Свойство Exists: определяет, существует ли каталог;
                GetDirectories(): получает список каталогов;
                GetFiles(): получает список файлов;
                MoveTo(destDirName): перемещает каталог;
                Свойство Parent: получение родительского каталога;
                Свойство Root: получение корневого каталога.

             Попробуем получить все файлы и папки корневого каталога: 

Папки:
/$Recycle.Bin
/DISTR
/Documents and Settings
/E
/MyDoc
/OneDriveTemp
/PerfLogs
/Program Files
/Program Files (x86)
/ProgramData
/Recovery
/SAP
/source
/System Volume Information
/Users
/Windows
/Дизайн
/Михаил Русаков
/Песни
/Фото

Файлы:
/bootTel.dat
/DumpStack.log.tmp
/hiberfil.sys
/pagefile.sys
/swapfile.sys

            Задание 8.2.1
            Напишите метод, который считает количество файлов и папок в корне вашего диска и выводит итоговое количество объектов.

            Еще несколько вариантов использования

            Создание новой директории в каталоге текущего пользователя (luft):

                DirectoryInfo dirInfo = new DirectoryInfo(@"C:\source\repos\ConsoleApp8"); 
                if (!dirInfo.Exists)
                     dirInfo.Create();

                dirInfo.CreateSubdirectory("NewFolder");

            Задание 8.2.2
            Добавьте в метод из задания 8.2.1 создание новой директории в корне вашего диска, а после вновь выведите количество элементов уже после создания нового. 

            Убедитесь, что их количество увеличилось, либо корректно вывелось сообщение об ошибке (если у вас нет прав на запись).

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(@"C:\");
                if (dirInfo.Exists)
                {
                    Console.WriteLine(dirInfo.GetDirectories().Length + dirInfo.GetFiles().Length);
                }

                DirectoryInfo newDirectory = new DirectoryInfo(@"C:\newDirectory");
                if (!newDirectory.Exists)
                    newDirectory.Create();

                Console.WriteLine(dirInfo.GetDirectories().Length + dirInfo.GetFiles().Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Получение информации о каталоге: 

                Console.WriteLine($"Название каталога: {dirInfo.Name}");
                Console.WriteLine($"Полное название каталога: {dirInfo.FullName}");Console.WriteLine($"Время создания каталога: 
                {dirInfo.CreationTime}");Console.WriteLine($"Корневой каталог: {dirInfo.Root}");

            При удалении каталога мы должны явно указать программе, что стоит также удалить все содержимое (файлы и подкаталоги), иначе получим ошибку.

            Для этого при вызове метода Delete  передаем флаг true, что означает удаление со всем содержимым:

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(@"/Users/luft/SkillFactory");
                dirInfo.Delete(true); // Удаление со всем содержимым
                Console.WriteLine("Каталог удален");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Задание 8.2.3
            Добавьте в задание 8.2.2 удаление вновь созданной директории и проверьте: теперь ваша программа не должна оставлять после себя следов!

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(@"C:\");
                if (dirInfo.Exists)
                {
                    Console.WriteLine(dirInfo.GetDirectories().Length + dirInfo.GetFiles().Length);
                }

                DirectoryInfo newDirectory = new DirectoryInfo(@"C:\newDirectory");
                if (!newDirectory.Exists)
                    newDirectory.Create();

                Console.WriteLine(dirInfo.GetDirectories().Length + dirInfo.GetFiles().Length);

                DirectoryInfo dirInfoNew = new DirectoryInfo(@"C:\newDirectory");
                dirInfoNew.Delete(true); // Удаление со всем содержимым
                Console.WriteLine("Каталог удален");

                Console.WriteLine(dirInfo.GetDirectories().Length + dirInfo.GetFiles().Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Задание 8.2.4
            Создайте на рабочем столе папку testFolder. Напишите метод, с помощью которого можно будет переместить её в корзину. 

            try
            {
                // Путь к папке, которую нужно переместить в корзину
                string folderPath = @"C:\testFolder";

                // Перемещаем папку в корзину
                FileSystem.DeleteDirectory(folderPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);

                Console.WriteLine("Папка успешно перемещена в корзину.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
            }

            Задание 8.2.5
            Как получить корневой каталог в системе?

            Несколько верных вариантов ответа

                1. Через свойство DirectoryInfo.Root    X
                2. Через свойство DirectoryInfo.Parent  
                3. Через Directory.GetDirectoryRoot     X
                4. Все варианты возможны

            Задание 8.2.6
            Что сделает данный код при запуске, если путь указан верно?

            var di = new DirectoryInfo("\C:\Documents\");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

                удалит все файлы и директории внутри директории Documents.                          X
                удалит все файлы и директории, но только если во вложенных директориях нет файлов
                выбросит исключение
                код не скомпилируется

            здесь сначала мы пробегаемся по файлам, удаляя каждый, а далее по папкам, удаляя каждую рекурсивно, т.е. со всем содержимым, 
            потому что в метод Delete() передан флаг true.

                Итоги

            В данном юните мы, изучая работу с файловой системой компьютера, перешли на один уровень выше (от системного диска к расположенным на нем директориям и каталогам).
            Далее рассмотрим работу с файлами — основной сущностью, с которой имеет дело операционная система и пользователь. 



            */

            //// получим системные диски
            //DriveInfo[] drives = DriveInfo.GetDrives();

            //// Пробежимся по дискам и выведем их свойства
            //foreach (DriveInfo drive in drives)
            //{
            //    Console.WriteLine($"Название: {drive.Name}");
            //    Console.WriteLine($"Тип: {drive.DriveType}");
            //    if (drive.IsReady)
            //    {
            //        Console.WriteLine($"Объем: {drive.TotalSize}");
            //        Console.WriteLine($"Свободно: {drive.TotalFreeSpace}");
            //        Console.WriteLine($"Метка: {drive.VolumeLabel}");
            //    }
            //}

            //GetCatalogs(); //   Вызов метода получения директорий


            // Получаем все диски
            DriveInfo[] drives = DriveInfo.GetDrives();

            // Фильтруем диски (только фиксированные)
            foreach (var drive in drives.Where(d => d.DriveType == DriveType.Fixed))
            {
                // Выводим информацию о диске
                WriteDriveInfo(drive);

                // Получаем корневую директорию диска
                DirectoryInfo root = drive.RootDirectory;

                // Получаем все папки в корневой директории
                var folders = root.GetDirectories();

                // Выводим информацию о папках
                WriteFolderInfo(folders);

                Console.WriteLine();
                Console.WriteLine();
            }
        }

        public static void WriteDriveInfo(DriveInfo drive)
        {
            Console.WriteLine($"Название: {drive.Name}");
            Console.WriteLine($"Тие: {drive.DriveType}");
            if (drive.IsReady)
            {
                Console.WriteLine($"Объем: {drive.TotalSize}");
                Console.WriteLine($"Свободно: {drive.TotalFreeSpace}");
                Console.WriteLine($"Метка: {drive.VolumeLabel}");
            }

        }

        public static void WriteFolderInfo(DirectoryInfo[] folders)
        {
            Console.WriteLine();
            Console.WriteLine("Папки: ");
            Console.WriteLine();

            foreach (var folder in folders)
            {
                Console.WriteLine(folder.Name);
            }
        }

        static void GetCatalogs()
        {
            string dirName = @"/"; // Прописываем путь к корневой директории MacOS (для Windows скорее всего тут будет "C:\\")
            if (Directory.Exists(dirName)) // Проверим, что директория существует
            {
                Console.WriteLine("Папки:");
                string[] dirs = Directory.GetDirectories(dirName);  // Получим все директории корневого каталога

                foreach (string d in dirs) // Выведем их все
                    Console.WriteLine(d);

                Console.WriteLine();
                Console.WriteLine("Файлы:");
                string[] files = Directory.GetFiles(dirName);// Получим все файлы корневого каталога

                foreach (string s in files)   // Выведем их все
                    Console.WriteLine(s);
            }
        }
    }
    }
