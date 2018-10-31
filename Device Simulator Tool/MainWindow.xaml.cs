using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Device_Simulator_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
  
        SerialPort serialPort = new SerialPort("COM1", 57600, Parity.None, 8, StopBits.One);
        string data;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Open_Port(object sender, RoutedEventArgs e)
        {
            if (!serialPort.IsOpen)
            {

                serialPort.PortName = SelectedPort.Text;
                serialPort.Handshake = Handshake.None;
                serialPort.BaudRate = 57600;
                serialPort.Handshake = Handshake.None;
                serialPort.Parity = Parity.None;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;
                serialPort.ReadTimeout = 200;
                serialPort.WriteTimeout = 50;
                serialPort.Open();
                var inistance = new SerialDataReceivedEventHandler(receive);
                serialPort.DataReceived += inistance;
            }
            if ((string)Port_Status.Content == "Open Port")
            {
                Port_Status.Content = "Close Port";
            }
            else if ((string)Port_Status.Content == "Close Port")
            {
                Port_Status.Content = "Open Port";
                serialPort.Close();
                
            }
        }
        private delegate void UpdateUiTextDelegate(string text);
        private void receive(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(300);
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
            }
            data = serialPort.ReadExisting();
            SendResponseBack(data);
            
            Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate(WriteData), data);
        }
        private void SendResponseBack(string data)
        {
            var Random = new Random();
            if (data != "")
            {
                if (data[0] == 36)
                {
                    data = data.Substring(1);
                }
                string ResponseString = "";
                for (int i = 0; i < data.Length - 1; i += 2)
                {
                    if (i == 0)
                    {
                        //Write Now Date
                        ResponseString += data[i] + "" + data[i + 1] + "'"+DateTime.Now.ToString() +"'";
                        
                    }
                    if (data[i + 2] == 60)
                    {
                        i += 3;
                    }
                    if (data[i] != 59 && i!=0)
                    {
                        ResponseString += data[i] + "" + data[i + 1] + Random.Next(10, 999);
                    }
                 
                }
                serialPort.Write("AA55Bs99AC<GQ344GR344GS55GT22.33.66.55.44.55.99GU12GV11GW112ER33>Au77AB<Au34Be32Cs234Ca'CallMeASAp'Bi33>Ai33");
            }
        }
        private void WriteData(string text)
        {
          
            TextArea.Content = text;

        }
    }
}
