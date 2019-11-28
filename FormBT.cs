using System;
using System.Windows.Forms;
namespace BT_UP
{

    public partial class FormBT : Form
    {
        Bluetooth bluetooth = new Bluetooth();//deklaracja klasy Bluetooth
        String choosenFile = ""; //zmiena przechowujaca wybrany plik

        public FormBT()
        {
            InitializeComponent();

        }
        public void clearDevices() //wylaczenie sekcji urzadzen
        {
            allDevicesGroupBox.Enabled = false;
            actualDeviceGroupBox.Enabled = false;
            sendFileGroupBox.Enabled = false;
            deviceNameTextBox.Text = "";
            deviceMacTextBox.Text = "";
            choosenFileTextBox.Text = "";
            choosenFile = "";
        }
        private void searchAdaptersButton_Click(object sender, EventArgs e) //wyszukiwanie adapterow
        {
            currentAdapterGroup.Enabled = false; //wylaczanie sekcji info o adapterze i zerowanie labeli
            adapterNameBox.Text = "";
            adapterMacBox.Text = "";
            clearDevices(); //wywolanie sekcji czyszczenia urzadzen
            bluetooth.UpdateAdapters();//pobranie adapterow
            adaptersListBox.Items.Clear();//wyczyszczenie starych z listy
            if (bluetooth.bluetoothRadios.Length > 0) //jezeli adaptery znalezione
                foreach (var device in bluetooth.bluetoothRadios)//iterujemy po ich liscie
                    adaptersListBox.Items.Add(device.Name);//dodajemy je do listboxa
            MessageBox.Show("Wyszukiwanie adapterów zakończone");
        }

        private void chooseAdapterButton_Click(object sender, EventArgs e) //wybranie adaptera
        {
            if (adaptersListBox.SelectedIndex >= 0) //jesli wybrano cos z listy
            {
                currentAdapterGroup.Enabled = true; //odblokowanie sekcji aktualnego adaptera
                bluetooth.bluetoothRadio = bluetooth.bluetoothRadios[adaptersListBox.SelectedIndex]; //przypisanie konkretnego adaptera do obecnego
                adapterNameBox.Text = bluetooth.bluetoothRadio.Name;//wypisanie nazwy adaptera
                adapterMacBox.Text = bluetooth.bluetoothRadio.LocalAddress.ToString();//wypisanie maca
                allDevicesGroupBox.Enabled = true;//wlaczenie sekcji urzadzen
             
            }

        }

        private void searchDevicesButton_Click(object sender, EventArgs e)//wyszukiwanie urzadzen
        {
            devicesListBox.Items.Clear();//czyszczenie listy
            bluetooth.FindDevices(authenticateCheckBox.Checked, rememberedCheckBox.Checked, unknownCheckBox.Checked);
            foreach (var device in bluetooth.bluetoothDeviceInfo)//iterowanie listy
                devicesListBox.Items.Add(device.DeviceName);//dodanie elemtnwo do listy
            MessageBox.Show("Wyszukiwanie urządzeń zakończone");
        }

        private void selectDeviceButton_Click(object sender, EventArgs e)
        {
            if (devicesListBox.SelectedIndex >= 0) //jezeli ktos wybral urzadzenie z listy
            {
                if (authenticateCheckBox.Checked)
                    pairDeviceButton.Visible = false;
                else
                    pairDeviceButton.Visible = true;
                actualDeviceGroupBox.Enabled = true; //wlaczamy sekcje info i wysylania pliku
                sendFileGroupBox.Enabled = true;
                bluetooth.device = bluetooth.bluetoothDeviceInfo[devicesListBox.SelectedIndex];//przypisanie wybranego urzadzenia
               
                deviceNameTextBox.Text = bluetooth.device.DeviceName.ToString(); //zmiana labelek nazwy i adresu
                deviceMacTextBox.Text = bluetooth.device.DeviceAddress.ToString();
                MessageBox.Show("Pobieranie danych urządzenia zakończone.");
            }
        }

        private void chooseFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();//zadeklarowanie okna pliku
            openFile.ShowDialog();//otworzenie okna wyboru pliku
            if (openFile != null)//jezeli cos wybralismy
            {
                choosenFile = openFile.FileName;//pobieramy nazwe
                choosenFileTextBox.Text = choosenFile;//wpisujemy do checkboxa
                sendFileButton.Enabled = true;//wlaczamy przycisk wysylania
            }
        }

        private void sendFileButton_Click(object sender, EventArgs e) //wysylanie pliku 
        {
            if (choosenFile != null && bluetooth.device!=null)//jezeli urzadzenie i plik jest
            {
                bluetooth.SendFile(choosenFile);//przekaz plik do metody
            }
        }

        private void pairDeviceButton_Click(object sender, EventArgs e)//parowaie urzadzniea
        {
            bluetooth.ConnectToDevice();//metoda parowania
        }
    }
}
