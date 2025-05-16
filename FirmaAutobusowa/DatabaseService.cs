using System.Data;
using MySql.Data.MySqlClient;
using Dapper;
using FirmaAutobusowa.Source;

namespace FirmaAutobusowa
{
    internal class DatabaseService
    {
        public enum Table{ BUS, CLIENT, DRIVER, CONTRACT};
        private readonly string _connectionString; // tu dane z logowaniem do bazy

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);
        public int ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            using (var conn = Connection) // łączymy sie z bazą, uzywamy using zeby miec pewność ze zamknie połaczenie z bazą po wykonaniu kodu
            { // ogolnie using uzywamy zawsze jak chcemy zwolnić zasoby po zakonczeniu. czyli np. łaczymy sie z baza, wykonujemy zapytanie i zamykamy od razu
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query; //zapytanie sql

                    foreach (var param in parameters)
                    {
                        var dbParam = cmd.CreateParameter();
                        dbParam.ParameterName = param.Key;
                        dbParam.Value = param.Value;
                        cmd.Parameters.Add(dbParam);
                    }

                    return cmd.ExecuteNonQuery(); // a tu wykonuje zapytanie
                }
            }
        }


        public IEnumerable<T> GetAllItemsFromTable<T>(Table table)
        {
            // zwraca wszystkie pozycje z tabeli
            using var db = Connection;
            string tableName = table.ToString();
            string sql = $"SELECT * FROM {tableName}";
            return db.Query<T>(sql);
        }

        public void AddTableItem(Table table, object item)
        {
            // dodaje do tabeli kolejną pozycje
            // item to klasa tabeli którą chcemy wyswietlic(Bus, Driver, Contract, Client)
            using var db = Connection;

            switch (table)
            {
                
                case Table.BUS:
                    var bus = item as Bus;
                    if (bus == null) throw new ArgumentException("BUS");
                    var sqlBus = @"INSERT INTO Bus 
                (Registration_number, Number_of_seats, Year_of_production, Mark, Model, Date_of_next_inspection)
                VALUES (@Registration_number, @Number_of_seats, @Year_of_production, @Mark, @Model, @Date_of_next_inspection)";
                    db.Execute(sqlBus, bus);
                    break;

                case Table.CLIENT:
                    var client = item as Client;
                    if (client == null) throw new ArgumentException("CLIENT");
                    var sqlClient = @"INSERT INTO Client (Name, Address, Phone_number) VALUES (@Name, @Address, @Phone_number)";
                    db.Execute(sqlClient, client);
                    break;

                case Table.DRIVER:
                    var driver = item as Driver;
                    if (driver == null) throw new ArgumentException("DRIVER");
                    var sqlDriver = @"INSERT INTO Driver (Name, Last_name, Driving_license_category, Driving_license_serial_number) 
                              VALUES (@Name, @Last_name, @Driving_license_category, @Driving_license_serial_number)";
                    db.Execute(sqlDriver, driver);
                    break;

                case Table.CONTRACT:
                    var contract = item as Contract;
                    if (contract == null) throw new ArgumentException("CONTRACT");
                    var sqlContract = @"INSERT INTO Contract (Origin_address, Destination_address, Start_date, Planned_end_date, Real_end_date, Driver_ID, Registration_number, Status, Client_ID, Requested_seats) 
                                VALUES (@Origin_address, @Destination_address, @Start_date, @Planned_end_date, @Real_end_date, @Driver_ID, @Registration_number, @Status, @Client_ID, @Requested_seats)";
                    db.Execute(sqlContract, contract);
                    break;

                default:
                    throw new ArgumentException("Nieznana tabela"); // nigdy sie nie stanie raczej, bo trza wybrać z comboBoxa tabele
            }
        }

        
    }
}
