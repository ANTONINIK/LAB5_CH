using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace LAB5_CH
{
    class Program
    {
        static void Main(string[] args)
        {
            ResearchTeam RT1 = new ResearchTeam("Armada", 5, "MATH", TimeFrame.TwoYears);
            Person participant0 = new Person("Ivan", "Ivanov", new DateTime(2001, 1, 10));
            RT1.AddPapers(new Paper("Integral", participant0, new DateTime(2020, 3, 1)));

            Console.WriteLine("Задание 1 \n");
                ResearchTeam RT2 = RT1.DeepCopy() as ResearchTeam;
                Console.WriteLine(RT1);
                Console.WriteLine(RT2);

            Console.WriteLine("Задание 2 \n");
                Console.WriteLine("Название файла");
                string FileName = Console.ReadLine();
                if (File.Exists(FileName))
                {
                    if (RT2.Load(FileName))
                    {
                        Console.WriteLine(RT2);
                    }
                    else
                        Console.WriteLine("Ошибка загрузки");
                }
                else
                {
                    Console.WriteLine("Файл не найден");
                    try
                    {
                        File.Create(FileName).Close();
                        Console.WriteLine("Файл успешно создан");
                    }
                    catch
                    {
                        Console.WriteLine("Ошибка создания файла");
                    }

                }
            Console.WriteLine("Задание 3-4 \n");
                if (RT2.AddFromConsole())
                {
                    Console.WriteLine("Публикация добавлена");
                    if (RT2.Save(FileName))
                    {
                        Console.WriteLine("Файл успешно сохранен");
                        Console.WriteLine(RT2);
                    }
                    else
                        Console.WriteLine("Сохранить не удалось");
                }
                else
                    Console.WriteLine("Проблемы при добавлении публикации");

            Console.WriteLine("Задание 5-6 \n");
                if (ResearchTeam.Load(FileName,  RT2))
                {
                    Console.WriteLine("Файл успешно считан");
                    Console.WriteLine(RT2);
                }
                else
                    Console.WriteLine("Невозможно чтение файла");
                if (RT2.AddFromConsole())
                    Console.WriteLine("Публикация добавлена");
                else
                    Console.WriteLine("Ошибка при добавлении публикации");
                if (ResearchTeam.Save(FileName, RT2))
                {
                    Console.WriteLine("Файл сохранен");
                    Console.WriteLine(RT2);
                }
                else
                    Console.WriteLine("Сохранить не удалось");
        }
    }
}