using System;
using Microsoft.Maui.Controls;
using System.Text;
using System.Text.RegularExpressions;

namespace MauiApp1_Chart
{
    public partial class SerialPage : ContentPage
    {
        private ArduinoCommunicator _arduinoCommunicator;
        private FormattedString _logFormattedString;

        private StringBuilder _logBuilder = new StringBuilder();
        private Span _logSpan = new Span();

        private DataQueue _dataQueue = new DataQueue();

        private int _dataCount = 0;

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
                LogData($"Received {_dataCount}: {data}", Colors.Green);
                _dataCount++;
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


        private void OnDataToChart(string data)
        {
            // Biểu thức chính quy để tìm các số trong chuỗi
            var regex = new Regex(@"\d+");

            // Tìm tất cả các số trong chuỗi
            var matches = regex.Matches(data);

            if (matches.Count >= 2 && double.TryParse(matches[0].Value, out double number1) && double.TryParse(matches[1].Value, out double number2))
            {
                // Sử dụng số đầu tiên và số thứ hai
                SharedDataService.AddData(number1.ToString(), number2);
            }
        }

        private void LogData(string message, Color color)
        {
            //_dataQueue.EnqueueDataFromString(message);
            OnDataToChart(message);

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
