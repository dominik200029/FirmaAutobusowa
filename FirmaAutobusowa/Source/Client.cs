namespace FirmaAutobusowa.Source
{
    internal class Client
    {
        public int Client_ID { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; } // ? oznacza ze moze byc NULLem
        public int Phone_number { get; set; }

        public Client() { } // pusty konstruktor dla Dappera
        public Client(int clientId, string name, string? address, int phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nazwa klienta nie może być pusta.");
            if (phoneNumber <= 0) throw new ArgumentException("Numer telefonu musi być dodatni.");

            Client_ID = clientId;
            Name = name;
            Address = address;
            Phone_number = phoneNumber;
        }
    }
}
