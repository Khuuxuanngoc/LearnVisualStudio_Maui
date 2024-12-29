using System;
using System.IO.Ports;
using System.Collections.Generic;

public class DataReceiver
{
    private SerialPort _serialPort;
    private Queue<(double, double)> _dataQueue = new Queue<(double, double)>();

    public event Action<double, double> DataReceived;

    public DataReceiver()
    {
        
    }

    //public DataReceiver(string portName, int baudRate)
    //{
    //    _serialPort = new SerialPort(portName, baudRate)
    //    {
    //        ReadTimeout = 500,
    //        WriteTimeout = 500
    //    };
    //    _serialPort.DataReceived += OnDataReceived;
    //    _serialPort.Open();
    //}

    private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        try
        {
            string data = _serialPort.ReadLine();
            var numbers = data.Split(' ');
            if (numbers.Length == 2 &&
                double.TryParse(numbers[0], out double value1) &&
                double.TryParse(numbers[1], out double value2))
            {
                _dataQueue.Enqueue((value1, value2));
                DataReceived?.Invoke(value1, value2);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error receiving data: {ex.Message}");
        }
    }

    public void PareData(String data)
    {
        var numbers = data.Split(' ');
        if (numbers.Length == 2 &&
            double.TryParse(numbers[0], out double value1) &&
            double.TryParse(numbers[1], out double value2))
        {
            _dataQueue.Enqueue((value1, value2));
            DataReceived?.Invoke(value1, value2);
        }
    }

    public void Close()
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.Close();
        }
    }

    public Queue<(double, double)> GetDataQueue()
    {
        return _dataQueue;
    }
}
