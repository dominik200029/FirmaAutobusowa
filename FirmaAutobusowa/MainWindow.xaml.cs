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
                    FormPanel.Children.Add(CreateTextBox("DriverNameBox", "Imię"));
                    FormPanel.Children.Add(CreateTextBox("LastNameBox", "Nazwisko"));
                    FormPanel.Children.Add(CreateTextBox("CategoryBox", "Kategoria PJ"));
                    FormPanel.Children.Add(CreateTextBox("SerialBox", "Nr prawa jazdy"));
                    break;

                case DatabaseService.Table.CLIENT:
                    FormPanel.Children.Add(CreateTextBox("ClientNameBox", "Nazwa klienta"));
                    FormPanel.Children.Add(CreateTextBox("AddressBox", "Adres"));
                    FormPanel.Children.Add(CreateTextBox("PhoneBox", "Telefon"));
                    break;

                case DatabaseService.Table.CONTRACT:
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


        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            //usuwanie z bazy
        }

    }
}