using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using FirmaAutobusowa.Source;

namespace FirmaAutobusowa
{
    public partial class MainWindow : Window
    {
        private DatabaseService _db;

        public MainWindow()
        {
            InitializeComponent();
            //USERNAME="root"; PASSWORD="baza123"
            _db = new DatabaseService("server=localhost;database=firmaautobusowa;uid=root;pwd=baza123;");
            Table.ItemsSource = Enum.GetValues(typeof(DatabaseService.Table)); // zaladuj do comboBoxa dane nazw tabel w bazie
        }
        private void LoadTableData(DatabaseService.Table table)
        {
            // laduje tabele do gridu na podstawie wybranej tabeli z bazy z listy
            switch (table)
            {
                case DatabaseService.Table.BUS:
                    TableGrid.ItemsSource = _db.GetAllItemsFromTable<Bus>(DatabaseService.Table.BUS);
                    break;
                case DatabaseService.Table.CLIENT:
                    TableGrid.ItemsSource = _db.GetAllItemsFromTable<Client>(DatabaseService.Table.CLIENT);
                    break;
                case DatabaseService.Table.DRIVER:
                    TableGrid.ItemsSource = _db.GetAllItemsFromTable<Driver>(DatabaseService.Table.DRIVER);
                    break;
                case DatabaseService.Table.CONTRACT:
                    TableGrid.ItemsSource = _db.GetAllItemsFromTable<Contract>(DatabaseService.Table.CONTRACT);
                    break;
                default:
                    MessageBox.Show("Nieznana tabela");
                    break;
            }
        }

        private void TableComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // jak zmienie nazwe tabeli z listy
            if (Table.SelectedItem is DatabaseService.Table selectedTable)
            {
                LoadTableData(selectedTable); // to zaladuje konkretne boxy
                GenerateForm(selectedTable); // i stworze je
            }
        }
        private void GenerateForm(DatabaseService.Table table)
        {
            // tworze pola do wyboru danych wpisania na podstawie wybranej tabeli z bazy
            FormPanel.Children.Clear();

            switch (table)
            {
                case DatabaseService.Table.BUS:
                    FormPanel.Children.Add(CreateTextBox("RegBox", "Rejestracja"));
                    FormPanel.Children.Add(CreateTextBox("SeatsBox", "Miejsca"));
                    FormPanel.Children.Add(CreateTextBox("YearBox", "Rok"));
                    FormPanel.Children.Add(CreateTextBox("MarkBox", "Marka"));
                    FormPanel.Children.Add(CreateTextBox("ModelBox", "Model"));
                    FormPanel.Children.Add(CreateDatePicker("InspectPicker"));
                    break;

                case DatabaseService.Table.DRIVER:
                    FormPanel.Children.Add(CreateTextBox("DriverIDBox", "ID kierowcy"));
                    FormPanel.Children.Add(CreateTextBox("DriverNameBox", "Imię"));
                    FormPanel.Children.Add(CreateTextBox("LastNameBox", "Nazwisko"));
                    FormPanel.Children.Add(CreateTextBox("CategoryBox", "Kategoria PJ"));
                    FormPanel.Children.Add(CreateTextBox("SerialBox", "Nr prawa jazdy"));
                    break;

                case DatabaseService.Table.CLIENT:
                    FormPanel.Children.Add(CreateTextBox("ClientIDBox", "ID klienta"));
                    FormPanel.Children.Add(CreateTextBox("ClientNameBox", "Nazwa klienta"));
                    FormPanel.Children.Add(CreateTextBox("AddressBox", "Adres"));
                    FormPanel.Children.Add(CreateTextBox("PhoneBox", "Telefon"));
                    break;

                case DatabaseService.Table.CONTRACT:
                    FormPanel.Children.Add(CreateTextBox("ContractIDBox", "ID kontraktu"));
                    FormPanel.Children.Add(CreateTextBox("OriginBox", "Start"));
                    FormPanel.Children.Add(CreateTextBox("DestinationBox", "Cel"));
                    FormPanel.Children.Add(CreateDatePicker("StartDatePicker"));
                    FormPanel.Children.Add(CreateDatePicker("PlannedEndDatePicker"));
                    FormPanel.Children.Add(CreateDatePicker("RealEndDatePicker"));
                    FormPanel.Children.Add(CreateTextBox("DriverIdBox", "ID kierowcy"));
                    FormPanel.Children.Add(CreateTextBox("RegNumberBox", "Nr rejestracyjny"));
                    FormPanel.Children.Add(CreateTextBox("StatusBox", "Status"));
                    FormPanel.Children.Add(CreateTextBox("ClientIdBox", "ID klienta"));
                    FormPanel.Children.Add(CreateTextBox("SeatsRequestedBox", "Żądane miejsca"));
                    break;
            }
        }

        private TextBox CreateTextBox(string name, string placeholder)
        {
            var box = new TextBox
            {
                Name = name,
                Width = 100,
                Margin = new Thickness(5, 0, 5, 0),
                Tag = placeholder, 
                Text = placeholder,  //stąd wiemy jakie pole w GUI odpowiada za jaką zmienną
                Foreground = Brushes.Gray
            };

            box.GotFocus += RemovePlaceholder; // jak chce wpisać np. numer rejestracji, to wiem w które pole
            box.LostFocus += AddPlaceholder; // jak wpisuje to znika nazwa pola, ale przy usunieciu tekstu znow bedzie widoczna

            return box;
        }
        private void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text == (string)tb.Tag)
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void AddPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = (string)tb.Tag;
                tb.Foreground = Brushes.Gray;
            }
        }


        private DatePicker CreateDatePicker(string name)
        {
            return new DatePicker
            {
                Name = name,
                Width = 150,
                Margin = new Thickness(5, 0, 5, 0)
            };
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedTable = (DatabaseService.Table)Table.SelectedItem;

                switch (selectedTable)
                {
                    // tu w zaleznosci od wybranej tabeli iterujemy po stackPanelu a potem po jego elementach
                    //albo textbox albo datapicker i ustawiamy wlasciwosci obiektow odpowiedzialnych za dana tabele na podstawie tego co w textboxie
                    // i potem sa pobierane do zapytania INSERT w metodzie AddTableItem w DatabaseService
                    case DatabaseService.Table.BUS:
                        var bus = new Bus();

                        foreach (var child in FormPanel.Children)
                        {
                            if (child is TextBox tb)
                            {
                                switch (tb.Name)
                                {
                                    case "RegBox":
                                        bus.Registration_number = tb.Text;
                                        break;
                                    case "SeatsBox":
                                        bus.Number_of_seats = int.Parse(tb.Text);
                                        break;
                                    case "YearBox":
                                        bus.Year_of_production = short.Parse(tb.Text);
                                        break;
                                    case "MarkBox":
                                        bus.Mark = tb.Text;
                                        break;
                                    case "ModelBox":
                                        bus.Model = tb.Text;
                                        break;
                                }
                            }
                            else if (child is DatePicker dp && dp.Name == "InspectPicker")
                            {
                                bus.Date_of_next_inspection = dp.SelectedDate ?? DateTime.Now;
                            }
                        }

                        _db.AddTableItem(DatabaseService.Table.BUS, bus);
                        break;

                    case DatabaseService.Table.CLIENT:
                        var client = new Client();

                        foreach (var child in FormPanel.Children)
                        {
                            if (child is TextBox tb)
                            {
                                switch (tb.Name)
                                {
                                    case "ClientIDBox":
                                        client.Client_ID = int.Parse(tb.Text);
                                        break;
                                    case "ClientNameBox":
                                        client.Name = tb.Text;
                                        break;
                                    case "AddressBox":
                                        client.Address = tb.Text;
                                        break;
                                    case "PhoneBox":
                                        client.Phone_number = int.Parse(tb.Text);
                                        break;
                                }
                            }
                        }

                        _db.AddTableItem(DatabaseService.Table.CLIENT, client);
                        break;

                    case DatabaseService.Table.DRIVER:
                        var driver = new Driver();

                        foreach (var child in FormPanel.Children)
                        {
                            if (child is TextBox tb)
                            {
                                switch (tb.Name)
                                {
                                    case "DriverIDBox":
                                        driver.Driver_ID = int.Parse(tb.Text);
                                        break;
                                    case "DriverNameBox":
                                        driver.Name = tb.Text;
                                        break;
                                    case "LastNameBox":
                                        driver.Last_name = tb.Text;
                                        break;
                                    case "CategoryBox":
                                        driver.Driving_license_category = tb.Text;
                                        break;
                                    case "SerialBox":
                                        driver.Driving_license_serial_number = int.Parse(tb.Text);
                                        break;
                                }
                            }
                        }

                        _db.AddTableItem(DatabaseService.Table.DRIVER, driver);
                        break;
                    case DatabaseService.Table.CONTRACT:
                        var contract = new Contract();

                        foreach (var child in FormPanel.Children)
                        {
                            switch (child)
                            {
                                case TextBox tb:
                                    switch (tb.Name)
                                    {
                                        case "ContractIDBox":
                                            contract.Contract_ID = int.Parse(tb.Text);
                                            break;
                                        case "OriginBox":
                                            contract.Origin_address = tb.Text;
                                            break;
                                        case "DestinationBox":
                                            contract.Destination_address = tb.Text;
                                            break;
                                        case "DriverIdBox":
                                            contract.Driver_ID = int.Parse(tb.Text);
                                            break;
                                        case "RegNumberBox":
                                            contract.Registration_number = tb.Text;
                                            break;
                                        case "StatusBox":
                                            contract.Status = tb.Text;
                                            break;
                                        case "ClientIdBox":
                                            contract.Client_ID = int.Parse(tb.Text);
                                            break;
                                        case "RequestedSeatsBox":
                                            contract.Requested_seats = int.Parse(tb.Text);
                                            break;
                                    }
                                    break;

                                case DatePicker dp:
                                    switch (dp.Name)
                                    {
                                        case "StartDatePicker":
                                            contract.Start_date = dp.SelectedDate ?? DateTime.Now;
                                            break;
                                        case "PlannedEndDatePicker":
                                            contract.Planned_end_date = dp.SelectedDate ?? DateTime.Now;
                                            break;
                                        case "RealEndDatePicker":
                                            contract.Real_end_date = dp.SelectedDate; // może być null
                                            break;
                                    }
                                    break;
                            }
                        }

                        _db.AddTableItem(DatabaseService.Table.CONTRACT, contract);
                        break;
                }

                LoadTableData(selectedTable); // odświeżenie widoku po dodaniu pozycji do tabeli
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (Table.SelectedItem is not DatabaseService.Table selectedTable)
            {
                MessageBox.Show("Wybierz tabele"); // jak nic nie wybrane, to wybierz (czyli jak wcisniesz usun ale nie masz zadnej tabeli wybranej)
                return;
            }

            var selectedItem = TableGrid.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Zaznacz pozycje do usuniecia");
                return;
            }

            string query = "";
            Dictionary<string, object> parameters = new();

            // w zaleznosci od tabeli, usun zaznaczony wiersz z tabeli z bazy
            // czyli zapytanie po PRIMARY KEY
            switch (selectedTable)
            {
                case DatabaseService.Table.DRIVER:
                    var driver = selectedItem as Driver;
                    query = "DELETE FROM Driver WHERE Driver_ID = @id";
                    parameters.Add("@id", driver.Driver_ID);
                    break;

                case DatabaseService.Table.BUS:
                    var bus = selectedItem as Bus;
                    query = "DELETE FROM Bus WHERE Registration_number = @id";
                    parameters.Add("@id", bus.Registration_number);
                    break;

                case DatabaseService.Table.CLIENT:
                    var client = selectedItem as Client;
                    query = "DELETE FROM Client WHERE Client_ID = @id";
                    parameters.Add("@id", client.Client_ID);
                    break;

                case DatabaseService.Table.CONTRACT:
                    var contract = selectedItem as Contract;
                    query = "DELETE FROM Contract WHERE Contract_ID = @id";
                    parameters.Add("@id", contract.Contract_ID);
                    break;

                default:
                    MessageBox.Show("Nieobsługiwana tabela");
                    return;
            }

            try
            {
                int rowsAffected = _db.ExecuteNonQuery(query, parameters);
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Pozycja usunieta");
                    LoadTableData(selectedTable); // przeładuj dane
                }
                else
                {
                    MessageBox.Show("Nie udało się usunac");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas usuwania: {ex.Message}");
            }
        }
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            if (Table.SelectedItem is not DatabaseService.Table selectedTable)
            {
                MessageBox.Show("Wybierz tabele"); // jak nic nie wybrane, to wybierz (czyli jak wcisniesz usun ale nie masz zadnej tabeli wybranej)
                return;
            }

            var selectedItem = TableGrid.SelectedItem;
            LoadTableData(selectedTable); // przeładuj dane

        }
        private void EditButton_Click(object sender, EventArgs e)
        {
            if (TableGrid.SelectedItem == null)
            {
                MessageBox.Show("Proszę zaznaczyć pozycję do edycji.");
                return;
            }

            var selectedTable = (DatabaseService.Table)Table.SelectedItem;

            string query = "";
            Dictionary<string, object> parameters = new();

            switch (selectedTable)
            {
                case DatabaseService.Table.BUS:
                    var selectedBus = (Bus)TableGrid.SelectedItem;

                    int seats = TryParseInt(GetTextBoxValueIfChanged("SeatsBox", "Miejsca"), selectedBus.Number_of_seats);
                    short year = TryParseShort(GetTextBoxValueIfChanged("YearBox", "Rok"), (short)selectedBus.Year_of_production);
                    string mark = GetTextBoxValueIfChanged("MarkBox", "Marka") ?? selectedBus.Mark;
                    string model = GetTextBoxValueIfChanged("ModelBox", "Model") ?? selectedBus.Model;
                    DateTime inspect = GetDatePickerValueOrDefault("InspectPicker", selectedBus.Date_of_next_inspection);

                    query = @"UPDATE Bus SET 
                        Number_of_seats = @seats,
                        Year_of_production = @year,
                        Mark = @mark,
                        Model = @model,
                        Date_of_next_inspection = @inspect
                      WHERE Registration_number = @reg";

                    parameters = new()
            {
                {"@seats", seats},
                {"@year", year},
                {"@mark", mark},
                {"@model", model},
                {"@inspect", inspect},
                {"@reg", selectedBus.Registration_number}
            };
                    break;

                case DatabaseService.Table.CLIENT:
                    var selectedClient = (Client)TableGrid.SelectedItem;

                    string clientName = GetTextBoxValueIfChanged("ClientNameBox", "Nazwa klienta") ?? selectedClient.Name;
                    string address = GetTextBoxValueIfChanged("AddressBox", "Adres") ?? selectedClient.Address;
                    int phone = TryParseInt(GetTextBoxValueIfChanged("PhoneBox", "Telefon"), selectedClient.Phone_number);

                    query = @"UPDATE Client SET 
                        Name = @name,
                        Address = @address,
                        Phone_number = @phone
                      WHERE Client_ID = @id";

                    parameters = new()
            {
                {"@name", clientName},
                {"@address", address},
                {"@phone", phone},
                {"@id", selectedClient.Client_ID}
            };
                    break;

                case DatabaseService.Table.DRIVER:
                    var selectedDriver = (Driver)TableGrid.SelectedItem;

                    string firstName = GetTextBoxValueIfChanged("DriverNameBox", "Imię") ?? selectedDriver.Name;
                    string lastName = GetTextBoxValueIfChanged("LastNameBox", "Nazwisko") ?? selectedDriver.Last_name;
                    string category = GetTextBoxValueIfChanged("CategoryBox", "Kategoria PJ") ?? selectedDriver.Driving_license_category;
                    int serial = TryParseInt(GetTextBoxValueIfChanged("SerialBox", "Nr prawa jazdy"), selectedDriver.Driving_license_serial_number);

                    query = @"UPDATE Driver SET 
                        Name = @name,
                        Last_name = @lastName,
                        Driving_license_category = @category,
                        Driving_license_serial_number = @serial
                      WHERE Driver_ID = @id";

                    parameters = new()
            {
                {"@name", firstName},
                {"@lastName", lastName},
                {"@category", category},
                {"@serial", serial},
                {"@id", selectedDriver.Driver_ID}
            };
                    break;

                case DatabaseService.Table.CONTRACT:
                    var selectedContract = (Contract)TableGrid.SelectedItem;

                    string origin = GetTextBoxValueIfChanged("OriginBox", "Start") ?? selectedContract.Origin_address;
                    string destination = GetTextBoxValueIfChanged("DestinationBox", "Cel") ?? selectedContract.Destination_address;

                    DateTime startDate = GetDatePickerValueOrDefault("StartDatePicker", selectedContract.Start_date);
                    DateTime plannedEndDate = GetDatePickerValueOrDefault("PlannedEndDatePicker", selectedContract.Planned_end_date);
                    DateTime? realEndDate = GetDatePickerNullableValue("RealEndDatePicker"); // może być null

                    string status = GetTextBoxValueIfChanged("StatusBox", "Status") ?? selectedContract.Status;

                    int driverId = TryParseInt(GetTextBoxValueIfChanged("DriverIdBox", "ID kierowcy"), selectedContract.Driver_ID);
                    string regNum = GetTextBoxValueIfChanged("RegNumberBox", "Nr rejestracyjny") ?? selectedContract.Registration_number;
                    int clientId = TryParseInt(GetTextBoxValueIfChanged("ClientIdBox", "ID klienta"), selectedContract.Client_ID);
                    int requestedSeats = TryParseInt(GetTextBoxValueIfChanged("SeatsRequestedBox", "Żądane miejsca"), selectedContract.Requested_seats);

                    query = @"UPDATE Contract SET
                        Origin_address = @origin,
                        destination_address = @destination,
                        start_date = @startDate,
                        planned_end_date = @plannedEndDate,
                        real_end_date = @realEndDate,
                        status = @status,
                        driver_id = @driverId,
                        registration_number = @regNum,
                        client_ID = @clientId,
                        requested_seats = @requestedSeats
                      WHERE Contract_ID = @id";

                    parameters = new()
            {
                {"@origin", origin},
                {"@destination", destination},
                {"@startDate", startDate},
                {"@plannedEndDate", plannedEndDate},
                {"@realEndDate", (object?)realEndDate ?? DBNull.Value},
                {"@status", status},
                {"@driverId", driverId},
                {"@regNum", regNum},
                {"@clientId", clientId},
                {"@requestedSeats", requestedSeats},
                {"@id", selectedContract.Contract_ID}
            };
                    break;

                default:
                    MessageBox.Show("Nieznana tabela");
                    return;
            }

            try
            {
                int rowsAffected = _db.ExecuteNonQuery(query, parameters);
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Edycja zakończona sukcesem.");
                    LoadTableData(selectedTable);
                }
                else
                {
                    MessageBox.Show("Nie zmieniono żadnego rekordu.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas aktualizacji: {ex.Message}");
            }
        }

        private string GetTextBoxValueIfChanged(string name, string placeholder)
        {
            var tb = FormPanel.Children.OfType<TextBox>().FirstOrDefault(t => t.Name == name);
            if (tb == null)
                throw new Exception($"Brak TextBoxa: {name}");

            if (tb.Text == placeholder)
                return null;

            return tb.Text;
        }

        private int TryParseInt(string input, int defaultValue)
        {
            return int.TryParse(input, out int val) ? val : defaultValue;
        }

        private short TryParseShort(string input, short defaultValue)
        {
            return short.TryParse(input, out short val) ? val : defaultValue;
        }

        private DateTime GetDatePickerValueOrDefault(string name, DateTime defaultValue)
        {
            var dp = FormPanel.Children.OfType<DatePicker>().FirstOrDefault(d => d.Name == name);
            return dp?.SelectedDate ?? defaultValue;
        }

        private DateTime? GetDatePickerNullableValue(string name)
        {
            var dp = FormPanel.Children.OfType<DatePicker>().FirstOrDefault(d => d.Name == name);
            return dp?.SelectedDate;
        }



    }
}
