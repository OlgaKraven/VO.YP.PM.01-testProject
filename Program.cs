using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace StudentManagerApp
{
    public class Program
    {
        static List<Student> students = new List<Student>();
        static string filePath = "students.json";

        public static void Main(string[] args)
        {
            LoadData();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n--- Меню управления студентами ---");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Показать всех студентов");
                Console.WriteLine("3. Найти по ID");
                Console.WriteLine("4. Редактировать данные");
                Console.WriteLine("5. Удалить студента");
                Console.WriteLine("6. Сохранить и выйти");
                Console.Write("Выбор: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddStudent(); break;
                    case "2": ShowAll(); break;
                    case "3": FindStudent(); break;
                    case "4": EditStudent(); break;
                    case "5": DeleteStudent(); break;
                    case "6": SaveData(); exit = true; break;
                    default: Console.WriteLine("Неверный выбор."); break;
                }
            }
        }

        static void AddStudent()
        {
            try
            {
                Console.Write("Введите ID: ");
                int id = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Введите ФИО: ");
                string? name = Console.ReadLine() ?? "";
                Console.Write("Введите группу: ");
                string? group = Console.ReadLine() ?? "";
                Console.Write("Введите средний балл: ");
                double grade = double.Parse(Console.ReadLine() ?? "0");

                students.Add(new Student { Id = id, FullName = name, Group = group, AverageGrade = grade });
                Console.WriteLine("✅ Студент добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка ввода: {ex.Message}");
            }
        }

        static void ShowAll()
        {
            Console.WriteLine("\n--- Список студентов ---");
            if (students.Count == 0) { Console.WriteLine("(пока пусто)"); return; }
            foreach (var s in students)
                s.ShowInfo();
        }

        static void FindStudent()
        {
            Console.Write("Введите ID студента: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Некорректный ID."); return; }
            var s = students.Find(x => x.Id == id);
            if (s != null) s.ShowInfo();
            else Console.WriteLine("❌ Студент не найден.");
        }

        static void EditStudent()
        {
            Console.Write("Введите ID для редактирования: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Некорректный ID."); return; }
            var s = students.Find(x => x.Id == id);
            if (s != null)
            {
                Console.Write("Новое имя: ");
                s.FullName = Console.ReadLine() ?? s.FullName;
                Console.Write("Новая группа: ");
                s.Group = Console.ReadLine() ?? s.Group;
                Console.Write("Новый средний балл: ");
                if (double.TryParse(Console.ReadLine(), out double grade)) s.AverageGrade = grade;
                Console.WriteLine("✅ Данные обновлены!");
            }
            else Console.WriteLine("❌ Студент не найден.");
        }

        static void DeleteStudent()
        {
            Console.Write("Введите ID для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Некорректный ID."); return; }
            int removed = students.RemoveAll(s => s.Id == id);
            Console.WriteLine(removed > 0 ? "🗑️ Студент удалён." : "❌ Студент не найден.");
        }

        static void SaveData()
        {
            try
            {
                string json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
                Console.WriteLine($"💾 Данные сохранены в {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            }
        }

        static void LoadData()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    var loaded = JsonSerializer.Deserialize<List<Student>>(json);
                    if (loaded != null) students = loaded;
                    Console.WriteLine("📂 Данные загружены из файла.");
                }
                else
                {
                    // создать файл с начальными данными
                    students = new List<Student>
                    {
                        new Student { Id = 1, FullName = "Иванов Иван", Group = "ИС-23", AverageGrade = 4.5 },
                        new Student { Id = 2, FullName = "Петров Пётр", Group = "ИС-23", AverageGrade = 3.9 }
                    };
                    SaveData();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки: {ex.Message}");
            }
        }
    }
}
