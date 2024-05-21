using Npgsql;
using System;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace SuperheroProject
{
    class Program
    {
        static string connectionString = "Host=localHost; Port=5432; Username=postgres; Password=1212; Database=superheroo";

        static void Main(string[] args)
        {
            //CreateDatabase();

            while (true)
            {
                Console.WriteLine("1. Добавить нового супергероя");
                Console.WriteLine("2. Показать всех супергероев");
                Console.WriteLine("3. Найти супергероя по ID");
                Console.WriteLine("4. Найти супергероя по Имени");
                Console.WriteLine("5. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddSuperhero();
                        break;
                    case "2":
                        ShowSuperheroes();
                        break;
                    case "3":
                        Console.Write("Введите ID супергероя: ");
                        int id = int.Parse(Console.ReadLine());
                        ReadSuperheroById(connectionString, id);
                        break;
                    case "4":
                        Console.Write("Введите имя супергероя: ");
                        string name = Console.ReadLine();
                        ReadSuperheroByName(connectionString, name);
                        break;
                    case "5":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Некорректный выбор.");
                        break;
                }
            }
        }


        //static void CreateDatabase()
        //{
        //    using (var connection = new NpgsqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        string createTableQuery = @" CREATE TABLE IF NOT EXISTS superheroes (
        //                Id SERIAL PRIMARY KEY,
        //                Name TEXT NOT NULL,
        //                Superpower TEXT NOT NULL,
        //                Universe TEXT NOT NULL,
        //                Experience INT NOT NULL";

        //        using (var command = new NpgsqlCommand(createTableQuery, connection))
        //        {
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //}

        static void AddSuperhero()
        {
            Console.Write("Введите имя супергероя: ");
            string name = Console.ReadLine();

            Console.Write("Введите его суперспособность: ");
            string superpower = Console.ReadLine();

            Console.WriteLine("Введите вселенную супергероя: ");
            string universe = Console.ReadLine();

            Console.WriteLine("Введите стаж супергероя: ");
            int experience = int.Parse(Console.ReadLine());

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO superheroes (Name, Superpower,Universe,Experience) VALUES (@Name, @Superpower,@Universe,@Experience)";

                using (var command = new NpgsqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Superpower", superpower);
                    command.Parameters.AddWithValue("@Universe", universe);
                    command.Parameters.AddWithValue("@Experience", experience);
             
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Супергерой добавлен успешно!");
            }
        }

        static void ShowSuperheroes()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM superheroes";

                using (var command = new NpgsqlCommand(selectQuery, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("Список супергероев:");
                            Console.WriteLine();
                            while (reader.Read())
                            {
                                Console.WriteLine($"Имя: {reader["Name"]}, Суперспособность: {reader["Superpower"]}, Вселенная: {reader["Universe"]}, Стаж: {reader["Experience"]}");
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Нет супергероев в базе данных.");
                        }
                    }
                }
            }
        }
        private static void ReadSuperheroByName(string connectionString, string? name)
        {
            using(var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT id,name,superpower,universe,experience FROM superheroes WHERE Name = @Name";
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = selectQuery;
                    command.Parameters.AddWithValue ("name", name);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Console.WriteLine($"Имя: {reader["Name"]}, Суперспособность: {reader["Superpower"]}, Вселенная: {reader["Universe"]}, Стаж: {reader["Experience"]}");
                        }
                        else
                        {
                            Console.WriteLine("Супергерой с указанным именем не найден.");
                        }
                    }
                }
            }
        }

        private static void ReadSuperheroById(string connectionString, int id)
        {
            using( var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT id,name,superpower,universe,experience FROM superheroes WHERE Id = @Id";
                using( var command = connection.CreateCommand())
                {
                    command.CommandText = selectQuery;
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Console.WriteLine($"Имя: {reader["Name"]}, Суперспособность: {reader["Superpower"]}, Вселенная: {reader["Universe"]}, Стаж: {reader["Experience"]}");
                        }
                        else
                        {
                            Console.WriteLine("Супергерой с указанным ID не найден.");
                        }
                    }
                }

            }
        }

    }
}