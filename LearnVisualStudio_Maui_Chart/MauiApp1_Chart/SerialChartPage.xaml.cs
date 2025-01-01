using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MauiApp1_Chart
{
    public partial class SerialChartPage : ContentPage
    {
        private ArduinoCommunicator _arduinoCommunicator;
        private FormattedString _logFormattedString;
        private StringBuilder _logBuilder = new StringBuilder();
        private Span _logSpan = new Span();

        public SerialChartPage()
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
            var ports = _arduinoCommunicator.GetAvailablePorts();
            if (ports.Length > 0)
            {
                PortsPicker_Cbbox.ItemsSource = ports;
                PortsPicker_Cbbox.SelectedIndex = 0; // Chọn item đầu tiên
                DisplayAlert("Scan COM", string.Join(", ", ports), "OK");
            }
            else
            {
                PortsPicker_Cbbox.ItemsSource = null;
                DisplayAlert("Scan COM", "No COM ports found", "OK");
            }
        }

        private void OnScanPortBtnClicked(object sender, EventArgs e)
        {
            LoadAvailablePorts();
        }

        private void OnConnectToggleClicked(object sender, EventArgs e)
        {
            if (ConnectButton.Text == "Connect")
            {
                if (PortsPicker_Cbbox.SelectedItem != null && int.TryParse(BaudRateEntry.Text, out int baudRate))
                {
                    string portName = PortsPicker_Cbbox.SelectedItem.ToString();
                    _arduinoCommunicator.SetupPort(portName, baudRate);
                    _arduinoCommunicator.Connect();

                    ConnectButton.Text = "Disconnect";
                    ConnectionStatusBox.Color = Colors.Green;
                }
                else
                {
                    DisplayAlert("Error", "Please select a port and enter a valid baud rate.", "OK");
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
            _logBuilder.Clear();
        }

        private void OnSendDataClicked(object sender, EventArgs e)
        {
            try
            {
                string data = SendDataEntry.Text;
                _arduinoCommunicator.SendData(data);
                LogData($"Sent: {data}", Colors.Red);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to send data: {ex.Message}", "OK");
            }
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

        private async void LogData(string message, Color color)
        {
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
                await LogScrollView.ScrollToAsync(0, LogLabel.Height, false);
            }
        }

        private void OnTestButtonClicked(object sender, EventArgs e)
        {
            TestPortsPicker();
        }

        private void TestPortsPicker()
        {
            var testPorts = new List<string> { "COM1", "COM2", "COM3" };
            PortsPicker_Cbbox.ItemsSource = testPorts;
            PortsPicker_Cbbox.SelectedIndex = 0; // Chọn item đầu tiên
            DisplayAlert("Test COM Ports", string.Join(", ", testPorts), "OK");
        }
    }
}
