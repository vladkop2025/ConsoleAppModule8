using System;
using System.IO;
using System.Text.Json;

namespace ConsoleApp8
{
    class Program
    {
        static void Main(string[] args)
        {
            //Модуль 8.Работа с файлами -  8.4.Классы для работы с бинарными данными
            /*
            Выше мы рассматривали в основном работу с данными, хранящимися в файлах в текстовом формате. Но любое число или символьную последовательность из файла или оперативной 
            памяти можно представить в виде двоичного (бинарного) кода.

            Операции с двоичными данными выполняются быстрее, так как нет необходимости в кодировке и преобразовании. Одна из основных причин, по которой данные хранятся в форматах, 
            отличных от двоичного, — это удобство для чтения человеком.

            В случае, когда нет необходимости чтения данных человеком, имеет смысл использовать бинарный формат.

            Для записи в файловый поток или чтения из него данных в двоичном формате в платформе .NET в пространстве имен System.IO предусмотрены классы BinaryWriter и BinaryReader.

            Эти классы инкапсулируют в себе поток. При создании объекта в конструктор передаётся Stream в качестве параметра.  При этом BinaryReader — входной поток, 
            BinaryWriter — выходной. Это видно из сигнатур конструкторов:

                BinaryWriter(Stream output)
                BinaryReader(Stream input)

            сли же требуется считать/записать данные в отличный от двоичного формат, нужно использовать расширенные версии конструктора:

                BinaryWriter(Stream output, Encoding encoding)
                BinaryReader(Stream input, Encoding encoding)

            Stream в сигнатуре метода означает поток. Поток, передаваемый в конструктор, должен быть открыт, иначе код выбросит исключение. 

                BinaryWriter BW = new BinaryWriter(
                    ew FileStream("C:/temp/Test/Configuration.cfg", FileMode.Create)
                    );
                BW.Close();

                Для чтения в классе BinaryReader определены методы для каждого типа данных: 

                    ReadByte
                    ReadChar
                    ReadDecimal
                    ReadDouble
                    ReadInt16, ReadInt32, ReadInt64
                    ReadSingle
                    ReadString и др.
                    BinaryWriter имеет похожие методы записи.

            Если вы записали двоичные данные в файл через BinaryWriter, для считывания стоит применять  BinaryReader.

            Результаты программы ниже 

                    Из файла считано:

                Дробь: 20,666
                Строка: Тестовая строка
                Целое: 55
                Булево значение: False

            Задание 8.4.1
            По ссылке лежит бинарный файл. В нём записана дата создания и имя операционной системы, на которой он был создан (формат данных — строка).

            Скачайте файл и поместите его на рабочий стол.

            Напишите программу, которая считает из него данные и позволит вам ответить на следующие вопросы:


            // сохраняем путь к файлу (допустим, вы его скачали на рабочий стол)
            string filePath = @"C:\Users\\vladk\Desktop\BinaryFile.bin";

            // при запуске проверим, что файл существует
            if (File.Exists(filePath))
            {
                // строковая переменная, в которую будем считывать данные
                string stringValue;

                // считываем, после использования высвобождаем задействованный ресурс BinaryReader
                using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    stringValue = reader.ReadString();
                }

                // Вывод
                Console.WriteLine("Из файла считано:");
                Console.WriteLine(stringValue);
            }

            Результат программы
            Из файла считано:

                Файл создан 11/02/2020 12:21:47 на компьютере c ОС Unix 19.6.0.0

            1. Когда файл был создан?
            Формат ответа: DD.MM в hh:mm

                11.02 12:21

            2. На какой операционной системе создан файл?

                Unix 19.6.0.0
 
            Теперь, когда мы рассмотрели, как сохранять и считывать информацию с текстовых и бинарных файлов, нужно определиться, как эти данные преобразовывать
            для удобного представления в вашей программе.

            Для этого C# предоставляет механизм бинарной сериализации. Она служит для преобразования объекта в поток байтов для последующей удобной записи в файл 
            или хранения в памяти. 

            Впоследствии можно выполнить обратный процесс — десериализацию (преобразование массива байт в ранее сохраненный объект).

            Чтобы объект определенного класса можно было сериализовать, этот класс должен иметь  атрибут Serializable:



            Попробуем применить это на практике: 

                // Объект для сериализации
                var pet = new Pet("Rex", 2);
                Console.WriteLine("Объект создан");

                // Сериализация
                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonString = JsonSerializer.Serialize(pet, options);
                File.WriteAllText("myPets.json", jsonString);
                Console.WriteLine("Объект сериализован");

                // Десериализация
                jsonString = File.ReadAllText("myPets.json");
                var newPet = JsonSerializer.Deserialize<Pet>(jsonString);
                Console.WriteLine("Объект десериализован");

            Console.WriteLine($"Имя: {newPet.Name} --- Возраст: {newPet.Age}");
            Console.ReadLine();

            Наш простейший класс Pet объявлен с атрибутом Serializable. Это делает возможной сериализацию его объектов. 
            
            В этом примере мы последовательно выполняем операции сериализации и десериализации, для которых нам необходим поток, в который нужно либо сохранять, 
            либо из которого считывать данные.

            Поток предоставлен объектом FileStream, с помощью которого мы пишем нужный нам объект Pet в файл myPets.json. Сериализация с помощью метода 
            JsonSerializer.Serialize добавляет все данные об объекте Pet в файл myPets.json, затем идёт приведение к типу Pet.

            Как видно из примера, при использовании сериализации процесс сохранения объектов в бинарном формате значительно проще, чем с использованием 
            связки классов BinaryWriter/BinaryReader.

            Хотя в данном примере мы взяли лишь один объект Pet, точно так же мы можем использовать и любую коллекцию подобных объектов.

            В примерах выше мы рассмотрели основные операции с директориями и файлами. Как вы уже знаете, операции записи/чтения из файла называются о
            перациями ввода-вывода, и поддержка их реализована в пространстве имен System.IO. 

            Результат программы:

                Объект создан
                Объект сериализован
                Объект десериализован
                Имя: Rex --- Возраст: 2

            Задание 8.4.3
            Дан класс:

          [Serializable]
            public class Contact
        {
            public string Name { get; set; }
            public long PhoneNumber { get; set; }
            public string Email { get; set; }

            public Contact(string name, long phoneNumber, string email)
            {
                Name = name;
                PhoneNumber = phoneNumber;
                Email = email;
            }
        }

            Доработайте его и сериализуйте в бинарный формат.


            Contact contact = new Contact("John Doe", 1234567890, "john.doe@example.com");

            // Сериализация объекта в бинарный файл
            using (FileStream fs = new FileStream("contact.bin", FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                contact.Serialize(writer);
            }

            // Десериализация объекта из бинарного файла
            Contact deserializedContact;
            using (FileStream fs = new FileStream("contact.bin", FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                deserializedContact = Contact.Deserialize(reader);
            }

            // Вывод десериализованных данных
            Console.WriteLine($"Name: {deserializedContact.Name}, Phone: {deserializedContact.PhoneNumber}, Email: {deserializedContact.Email}");

            Результат программы:

                Name: John Doe, Phone: 1234567890, Email: john.doe@example.com

            Задание 8.4.4
            В чём плюсы хранения данных в бинарном формате?
            Несколько верных вариантов ответа

                1. Удобство чтения человеком
                2. Более быстрая обработка программой   X
                3. Нет необходимости в сериализации     X
                4. Всё вышеперечисленное
                5. Очевидных плюсов нет
                
            Задание 8.4.5
            В каких случаях вам стоит использовать бинарный формат для хранения данных?
            Несколько верных вариантов ответа

                1. Ваша программа сохраняет данные о заказах за день в Excel-файл, размер файла: 100 КБ. Менеджер каждый день в конце рабочего дня просматривает заказы
                2. Ваша программа делает дамп базы данных раз в неделю и сохраняет его в облаке X
                3. Ваша программа сохраняет в архив старые документы                            X
                4. Во всех вышеперечисленных случаях
                5. Ни в одном

            При операциях ввода-вывода ваши программы обращаются к внешнему ресурсу, в данном случае — файлу. Для эффективного управления доступом к ресурсу во многих 
            примерах была использована новая для вас конструкция — Using. 

            Давайте рассмотрим подробнее её применение в следующем юните.

         */

            Contact contact = new Contact("John Doe", 1234567890, "john.doe@example.com");

            // Сериализация объекта в бинарный файл
            using (FileStream fs = new FileStream("contact.bin", FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                contact.Serialize(writer);
            }

            // Десериализация объекта из бинарного файла
            Contact deserializedContact;
            using (FileStream fs = new FileStream("contact.bin", FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                deserializedContact = Contact.Deserialize(reader);
            }

            // Вывод десериализованных данных
            Console.WriteLine($"Name: {deserializedContact.Name}, Phone: {deserializedContact.PhoneNumber}, Email: {deserializedContact.Email}");

            //// Пишем
            //WriteValues();
            //// Считываем
            //ReadValues();

            //// Объект для сериализации
            //var pet = new Pet("Rex", 2);
            //Console.WriteLine("Объект создан");

            //// Сериализация
            //var options = new JsonSerializerOptions { WriteIndented = true };
            //var jsonString = JsonSerializer.Serialize(pet, options);
            //File.WriteAllText("myPets.json", jsonString);
            //Console.WriteLine("Объект сериализован");

            //// Десериализация
            //jsonString = File.ReadAllText("myPets.json");
            //var newPet = JsonSerializer.Deserialize<Pet>(jsonString);
            //Console.WriteLine("Объект десериализован");

            //Console.WriteLine($"Имя: {newPet.Name} --- Возраст: {newPet.Age}");
            //Console.ReadLine();

        }


        [Serializable]
        public class Contact
        {
            public string Name { get; set; }
            public long PhoneNumber { get; set; }
            public string Email { get; set; }

            public Contact(string name, long phoneNumber, string email)
            {
                Name = name;
                PhoneNumber = phoneNumber;
                Email = email;
            }

            // Метод для сериализации объекта в бинарный формат
            public void Serialize(BinaryWriter writer)
            {
                writer.Write(Name);
                writer.Write(PhoneNumber);
                writer.Write(Email);
            }

            // Метод для десериализации объекта из бинарного формата
            public static Contact Deserialize(BinaryReader reader)
            {
                string name = reader.ReadString();
                long phoneNumber = reader.ReadInt64();
                string email = reader.ReadString();

                return new Contact(name, phoneNumber, email);
            }
        }
        class Pet
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public Pet(string name, int age)
            {
                Name = name;
                Age = age;
            }
        }

        //[Serializable] // Атрибут сериализации
        //class Person
        //{
        //    // Простая модель класса
        //    public string Name { get; set; }
        //    public int Year { get; set; }

        //    // Метод-конструктор
        //    public Person(string name, int year)
        //    {
        //        Name = name;
        //        Year = year;
        //    }
        //}


        const string SettingsFileName = "Settings.cfg";

        static void WriteValues()
        {
            // Создаем объект BinaryWriter и указываем, куда будет направлен поток данных
            using (BinaryWriter writer = new BinaryWriter(File.Open(SettingsFileName, FileMode.Create)))
            {
                // Записываем данные в разном формате
                writer.Write(20.666F);
                writer.Write("Тестовая строка");
                writer.Write(55);
                writer.Write(false);
            }
        }

        static void ReadValues()
        {
            float FloatValue;
            string StringValue;
            int IntValue;
            bool BooleanValue;

            if (File.Exists(SettingsFileName))
            {
                // Создаем объект BinaryReader и инициализируем его возвратом метода File.Open.
                using (BinaryReader reader = new BinaryReader(File.Open(SettingsFileName, FileMode.Open)))
                {
                    // Применяем специализированные методы Read для считывания соответствующего типа данных
                    FloatValue = reader.ReadSingle();
                    StringValue = reader.ReadString();
                    IntValue = reader.ReadInt32();
                    BooleanValue = reader.ReadBoolean();
                }

                Console.WriteLine("Из файла считано:");

                Console.WriteLine("Дробь: " + FloatValue);
                Console.WriteLine("Строка: " + StringValue);
                Console.WriteLine("Целое: " + IntValue);
                Console.WriteLine("Булево значение: " + BooleanValue);
            }
        }

    }
}
