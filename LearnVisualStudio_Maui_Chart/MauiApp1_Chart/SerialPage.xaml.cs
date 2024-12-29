using System;
using Microsoft.Maui.Controls;
using System.Text;

namespace MauiApp1_Chart
{
    public partial class SerialPage : ContentPage
    {
        private ArduinoCommunicator _arduinoCommunicator;
        private FormattedString _logFormattedString;

        private StringBuilder _logBuilder = new StringBuilder();
        private Span _logSpan = new Span();

        private DataQueue _dataQueue = new DataQueue();

        public SerialPage()
        {
            InitializeComponent();
            _arduinoCommunicator = new ArduinoCommunicator();
            _arduinoCommunicator.DataReceived += OnDataReceived;
            _arduinoCommunicator.ConnectionLost += OnConnectionLost;

            _logFormattedString = new FormattedString();
            LogLabel.FormattedText = _logFormattedString;

            LoadAvailablePorts();
        }

        private void LoadAvailablePorts()
        {
            PortsPicker.ItemsSource = _arduinoCommunicator.GetAvailablePorts();
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {
            LoadAvailablePorts();
        }

        private void OnConnectToggleClicked(object sender, EventArgs e)
        {
            if (ConnectButton.Text == "Connect")
            {
                if (PortsPicker.SelectedItem != null && int.TryParse(BaudRateEntry.Text, out int baudRate))
                {
                    string portName = PortsPicker.SelectedItem.ToString();
                    _arduinoCommunicator.SetupPort(portName, baudRate);
                    _arduinoCommunicator.Connect();

                    ConnectButton.Text = "Disconnect";
                    ConnectionStatusBox.Color = Colors.Green;
                }
            }
            else
            {
                _arduinoCommunicator.Disconnect();
                ConnectButton.Text = "Connect";
                ConnectionStatusBox.Color = Colors.Red;
            }
        }

        private void OnClearLogClicked(object sender, EventArgs e)
        {
            _logFormattedString.Spans.Clear();
        }

        private void OnSendDataClicked(object sender, EventArgs e)
        {
            string data = SendDataEntry.Text;
            _arduinoCommunicator.SendData(data);
            LogData($"Sent: {data}", Colors.Red);
        }

        private void OnDataReceived(string data)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                LogData($"Received: {data}", Colors.Green);
            });
        }

        private void OnConnectionLost()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ConnectButton.Text = "Connect";
                ConnectionStatusBox.Color = Colors.Red;
                LogData("Connection lost.", Colors.Red);
            });
        }

        //private void LogData(string message, Color color)
        //{
        //    var span = new Span { Text = message + "\n", TextColor = color };
        //    if(_logFormattedString.Spans.Count > 20)
        //    {
        //        //_logFormattedString.Spans.Clear();
        //        _logFormattedString.Spans.RemoveAt(0);
        //    }    

        //    _logFormattedString.Spans.Add(span);

        //    if (AutoScrollCheckBox.IsChecked)
        //    {
        //        // Ensure that ScrollView scrolls to the bottom
        //        LogScrollView.ScrollToAsync(0, LogLabel.Height, false);
        //    }
        //}

        private void LogData(string message, Color color)
        {
            _dataQueue.EnqueueDataFromString(message);

            // Giới hạn số lượng dòng log
            if (_logBuilder.Length > 2000) // Giới hạn dựa trên độ dài chuỗi
            {
                _logBuilder.Remove(0, _logBuilder.ToString().IndexOf('\n') + 1);
            }

            // Thêm thông điệp mới vào StringBuilder
            _logBuilder.AppendLine(message);

            // Cập nhật Span duy nhất
            _logSpan.Text = _logBuilder.ToString();
            _logSpan.TextColor = color;

            // Cập nhật FormattedString
            _logFormattedString.Spans.Clear();
            _logFormattedString.Spans.Add(_logSpan);

            if (AutoScrollCheckBox.IsChecked)
            {
                // Ensure that ScrollView scrolls to the bottom
                LogScrollView.ScrollToAsync(0, LogLabel.Height, false);
            }
        }
    }
}
