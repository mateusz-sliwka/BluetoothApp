using System;
using System.Windows.Forms;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
namespace BT_UP
{
    class Bluetooth
    {
        public int parseNumber = 5;
        public BluetoothRadio[] bluetoothRadios;
        public BluetoothClient bluetoothClient;
        public BluetoothDeviceInfo[] bluetoothDeviceInfo;
        public BluetoothDeviceInfo device;
        private BluetoothEndPoint bluetoothEndPoint;
        public BluetoothRadio bluetoothRadio;
        public string password = "123456";

        public Bluetooth() //konsturktor
        {
            bluetoothRadio = null;
        }
        public void UpdateAdapters() //pobranie listy adatperow
        {
            bluetoothRadios = BluetoothRadio.AllRadios;
        }
        public void FindDevices(bool Authenticate, bool Remembered, bool Unknown) //szukanie urzadzen
        {
            bluetoothEndPoint = new BluetoothEndPoint(bluetoothRadio.LocalAddress, BluetoothService.SerialPort);
            bluetoothClient = new BluetoothClient(bluetoothEndPoint);
            bluetoothDeviceInfo = bluetoothClient.DiscoverDevices(50, Authenticate, Remembered, Unknown);

        }
        public void ConnectToDevice() //parowanie urzadzenia
        {
            device.Update();
            device.Refresh();
            device.SetServiceState(BluetoothService.ObexObjectPush, true);
            BluetoothSecurity.PairRequest(device.DeviceAddress, password);
        }
        public void SendFile(string fileName)//wysylanie pliku
        {
            int iteration = 0;
            while(iteration<parseNumber)
            {
                try
                {
                    var requestUri = new Uri("obex://" + device.DeviceAddress + "/" + fileName);
                    ObexWebRequest request = new ObexWebRequest(requestUri);
                    request.ReadFile(fileName);
                    ObexWebResponse response = (ObexWebResponse)request.GetResponse();
                    response.Close();
                    iteration = parseNumber;
                    MessageBox.Show("Transfer udany.");
                }
                catch
                {
                    iteration++; //iterowanie po zmiennej, trzymanie sie ustlaonej ilosci prob
                    if (iteration == parseNumber - 1)
                        MessageBox.Show("Błąd transferu.");
                }

            }
        }
    }
}
