using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1_Chart.Models
{
    public class ArduinoCommunicator_Model
    {
        private SerialPort _serialPort;
        public event Action<string> DataReceived;
        public event Action ConnectionLost;

        public string[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        public void SetupPort(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.DataReceived += OnDataReceived;
            _serialPort.ErrorReceived += OnErrorReceived;
        }

        public void Connect()
        {
            if (_serialPort != null && !_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
        }

        public void Disconnect()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        public void SendData(string data)
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.WriteLine(data);
            }
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                var data = _serialPort.ReadLine();
                DataReceived?.Invoke(data);
            }
        }

        private void OnErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            ConnectionLost?.Invoke();
        }
    }
}
