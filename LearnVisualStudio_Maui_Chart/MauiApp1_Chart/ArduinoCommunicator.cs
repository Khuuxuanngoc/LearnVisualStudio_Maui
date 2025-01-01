using System;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;

public class ArduinoCommunicator
{
    private SerialPort _serialPort;
    private bool _isCheckingConnection;

    public event Action<string> DataReceived;
    public event Action ConnectionLost;

    //public string[] GetAvailablePorts()
    //{
    //    return SerialPort.GetPortNames();
    //}
    public string[] GetAvailablePorts()
    {
        return System.IO.Ports.SerialPort.GetPortNames();
    }


    public void SetupPort(string portName, int baudRate)
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.Close();
            _serialPort.Dispose();
        }

        _serialPort = new SerialPort(portName, baudRate)
        {
            ReadTimeout = 500,
            WriteTimeout = 500
        };

        _serialPort.DataReceived += OnDataReceived;
    }

    public void Connect()
    {
        if (_serialPort != null && !_serialPort.IsOpen)
        {
            try
            {
                _serialPort.Open();
                _isCheckingConnection = true;
                Task.Run(CheckConnectionStatus);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }

    public void Disconnect()
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.Close();
            _isCheckingConnection = false;
            _serialPort.DataReceived -= OnDataReceived;
        }
    }

    public void SendData(string data)
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            try
            {
                _serialPort.WriteLine(data);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }

    private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            try
            {
                string data = _serialPort.ReadLine();
                DataReceived?.Invoke(data);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }

    private async Task CheckConnectionStatus()
    {
        while (_isCheckingConnection)
        {
            await Task.Delay(1000); // Check every second
            try
            {
                if (_serialPort != null && !_serialPort.IsOpen)
                {
                    _isCheckingConnection = false;
                    ConnectionLost?.Invoke();
                }
                else if (_serialPort != null && !SerialPort.GetPortNames().Contains(_serialPort.PortName))
                {
                    _isCheckingConnection = false;
                    _serialPort.Close();
                    ConnectionLost?.Invoke();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }

    private void HandleException(Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        _isCheckingConnection = false;
        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.Close();
        }
        ConnectionLost?.Invoke();
    }
}
