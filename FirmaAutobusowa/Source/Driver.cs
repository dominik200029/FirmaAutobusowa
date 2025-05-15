namespace FirmaAutobusowa.Source
{
    internal class Driver
    {
        public int Driver_ID { get; set; }
        public string Name { get; set; }
        public string Last_name { get; set; }
        public string Driving_license_category { get; set; }
        public int Driving_license_serial_number { get; set; }

        public Driver() { } // pusty konstruktor dla Dappera
        public Driver(int driverId, string name, string lastName, string licenseCategory, int licenseSerial)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Imię nie może być puste.");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Nazwisko nie może być puste.");
            if (string.IsNullOrWhiteSpace(licenseCategory)) throw new ArgumentException("Kategoria prawa jazdy nie może być pusta.");
            if (licenseSerial <= 0) throw new ArgumentException("Numer prawa jazdy musi być większy od 0.");

            Driver_ID = driverId;
            Name = name;
            Last_name = lastName;
            Driving_license_category = licenseCategory;
            Driving_license_serial_number = licenseSerial;
        }
    }
}
