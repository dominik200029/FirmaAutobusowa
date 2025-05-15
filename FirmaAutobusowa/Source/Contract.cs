namespace FirmaAutobusowa.Source
{
    internal class Contract
    {
        public int Contract_ID { get; set; }
        public string Origin_address { get; set; }
        public string Destination_address { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime Planned_end_date { get; set; }
        public DateTime? Real_end_date { get; set; }
        public int Driver_ID { get; set; }
        public string Registration_number { get; set; }
        public string Status { get; set; }
        public int Client_ID { get; set; }
        public int Requested_seats { get; set; }

        public Contract() { } // pusty konstruktor dla Dappera
        public Contract(int contractId, string origin, string destination, DateTime start, DateTime plannedEnd, DateTime? realEnd, int driverId, string regNumber, string status, int clientId, int requestedSeats)
        {
            if (string.IsNullOrWhiteSpace(origin) || string.IsNullOrWhiteSpace(destination))
                throw new ArgumentException("Adres początkowy i końcowy nie mogą być puste.");

            if (start > plannedEnd)
                throw new ArgumentException("Data zakończenia nie może być wcześniejsza niż data rozpoczęcia.");

            if (requestedSeats <= 0)
                throw new ArgumentException("Liczba miejsc musi być dodatnia.");

            Contract_ID = contractId;
            Origin_address = origin;
            Destination_address = destination;
            Start_date = start;
            Planned_end_date = plannedEnd;
            Real_end_date = realEnd;
            Driver_ID = driverId;
            Registration_number = regNumber;
            Status = status;
            Client_ID = clientId;
            Requested_seats = requestedSeats;
        }
    }
}
