using System;
using System.Collections.Generic;

public class DataQueue
{
    private Queue<(string, double)> _dataQueue = new Queue<(string, double)>();

    public event Action<string, double> DataAdded;

    public void EnqueueData(string text, double number)
    {
        _dataQueue.Enqueue((text, number));
        DataAdded?.Invoke(text, number);
    }

    public void EnqueueDataFromString(string dataFromSerial)
    {
        var parts = dataFromSerial.Split(' ');
        if (parts.Length == 2 && double.TryParse(parts[1], out double number))
        {
            EnqueueData(parts[0], number);
        }
    }

    public void ClearQueue()
    {
        _dataQueue.Clear();
    }

    public Queue<(string, double)> GetDataQueue()
    {
        return new Queue<(string, double)>(_dataQueue);
    }
}
