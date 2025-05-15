namespace FirmaAutobusowa.Source
{
    internal class Bus
    {
        public string Registration_number { get; set; }
        public int Number_of_seats { get; set; }
        public int Year_of_production { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public DateTime Date_of_next_inspection { get; set; }

        public Bus() { } // pusty konstruktor dla Dappera
        public Bus(string registrationNumber, int numberOfSeats, int yearOfProduction, string mark, string model, DateTime dateOfNextInspection)
        {
            if (string.IsNullOrWhiteSpace(registrationNumber)) throw new ArgumentException("Numer rejestracyjny nie może być pusty.");
            if (numberOfSeats <= 0) throw new ArgumentException("Liczba miejsc musi być większa od zera.");
            if (string.IsNullOrWhiteSpace(mark)) throw new ArgumentException("Marka nie może być pusta.");
            if (yearOfProduction < 1900 || yearOfProduction > DateTime.Now.Year) throw new ArgumentException("Nieprawidłowy rok produkcji.");
            if (dateOfNextInspection < DateTime.Today) throw new ArgumentException("Przegląd nie może być w przeszłości.");

            Registration_number = registrationNumber;
            Number_of_seats = numberOfSeats;
            Year_of_production = yearOfProduction;
            Mark = mark;
            Model = model;
            Date_of_next_inspection = dateOfNextInspection;
        }
    }
}
