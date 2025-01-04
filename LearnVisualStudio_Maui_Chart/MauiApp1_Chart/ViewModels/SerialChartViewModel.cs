using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using MauiApp1_Chart.Models;

namespace MauiApp1_Chart.ViewModels
{
    public partial class SerialChartViewModel : ObservableObject
    {
        private readonly ArduinoCommunicator_Model _arduinoCommunicator;
        private bool _isConnected;

        public SerialChartViewModel()
        {
            try
            {
                Debug.WriteLine("SerialChartViewModel initialized");
                _arduinoCommunicator = new ArduinoCommunicator_Model();
                _arduinoCommunicator.DataReceived += OnDataReceived;
                _arduinoCommunicator.ConnectionLost += OnConnectionLost;

                AvailablePorts = new ObservableCollection<string>(_arduinoCommunicator.GetAvailablePorts());
                ConnectToggleCommand = new Command(OnConnectToggle);
                ClearLogCommand = new Command(OnClearLog);
                ScanPortCommand = new Command(OnScanPort);
                SendDataCommand = new Command(OnSendData);
                AddDataCommand = new Command(OnAddData);
                ClearDataCommand = new Command(OnClearData);
                SaveCSVCommand = new Command(OnSaveCSV);
                ImportCSVCommand = new Command(OnImportCSV);

                ChartEntries = new ObservableCollection<ChartEntry>();
                UpdateChart();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing ViewModel: {ex.Message}");
            }
        }

        public ObservableCollection<string> AvailablePorts { get; set; }
        public string SelectedPort { get; set; }

        [ObservableProperty]
        private string baudRate = "9600";

        [ObservableProperty]
        private string sendDataText;

        [ObservableProperty]
        private string logData;

        [ObservableProperty]
        private bool autoScroll = true;

        [ObservableProperty]
        private string value;

        [ObservableProperty]
        private string time;

        public ICommand ConnectToggleCommand { get; }
        public ICommand ClearLogCommand { get; }
        public ICommand ScanPortCommand { get; }
        public ICommand SendDataCommand { get; }
        public ICommand AddDataCommand { get; }
        public ICommand ClearDataCommand { get; }
        public ICommand SaveCSVCommand { get; }
        public ICommand ImportCSVCommand { get; }

        private ObservableCollection<ChartEntry> ChartEntries { get; }

        public Chart Chart => new LineChart
        {
            Entries = ChartEntries,
            LineMode = LineMode.Straight,
            PointMode = PointMode.None,
            LineSize = 8,
            PointSize = 0,
            BackgroundColor = SKColors.White,
            LabelTextSize = 30
        };

        private void OnScanPort()
        {
            Debug.WriteLine("SerialChartViewModel scan port");
            AvailablePorts.Clear();
            foreach (var port in _arduinoCommunicator.GetAvailablePorts())
            {
                AvailablePorts.Add(port);
            }
        }

        private void OnConnectToggle()
        {
            if (_isConnected)
            {
                _arduinoCommunicator.Disconnect();
                _isConnected = false;
            }
            else
            {
                if (SelectedPort != null && int.TryParse(BaudRate, out int baudRate))
                {
                    _arduinoCommunicator.SetupPort(SelectedPort, baudRate);
                    _arduinoCommunicator.Connect();
                    _isConnected = true;
                }
            }
            OnPropertyChanged(nameof(ConnectionStatusColor));
        }

        private void OnClearLog()
        {
            LogData = string.Empty;
            OnPropertyChanged(nameof(LogData));
        }

        private void OnSendData()
        {
            _arduinoCommunicator.SendData(SendDataText);
            Debug.WriteLine("SerialChartViewModel SendData" + SendDataText);
        }

        private void OnAddData()
        {
            if (double.TryParse(Value, out double value) && double.TryParse(Time, out double time))
            {
                DataService_Model.AddData(new DataPoint_Model { Value = value, Time = time });
                UpdateChart();
            }
        }

        private void OnClearData()
        {
            DataService_Model.ClearData();
            UpdateChart();
        }

        private async void OnSaveCSV()
        {
            // Lưu dữ liệu vào CSV, code này cần phải điều chỉnh theo môi trường cụ thể của bạn
        }

        private async void OnImportCSV()
        {
            // Nhập dữ liệu từ CSV, code này cần phải điều chỉnh theo môi trường cụ thể của bạn
        }

        private void OnDataReceived(string data)
        {
            LogData += $"{data}\n";
            OnPropertyChanged(nameof(LogData));

            if (AutoScroll)
            {
                // Cuộn đến cuối log
            }

            // Cập nhật biểu đồ
            var regex = new System.Text.RegularExpressions.Regex(@"\d+");
            var matches = regex.Matches(data);

            if (matches.Count >= 2 && double.TryParse(matches[0].Value, out double number1) && double.TryParse(matches[1].Value, out double number2))
            {
                var chartEntry = new ChartEntry((float)number2)
                {
                    Label = number1.ToString(),
                    ValueLabel = number2.ToString(),
                    Color = SKColor.Parse("#00BFFF")
                };
                ChartEntries.Add(chartEntry);

                if (ChartEntries.Count > 10)
                {
                    ChartEntries.RemoveAt(0); // Giữ lại 10 mục cuối cùng
                }

                OnPropertyChanged(nameof(Chart));
            }
        }

        private void OnConnectionLost()
        {
            _isConnected = false;
            OnPropertyChanged(nameof(ConnectionStatusColor));
        }

        private void UpdateChart()
        {
            ChartEntries.Clear();
            var dataPoints = DataService_Model.GetListData().Skip(Math.Max(0, DataService_Model.GetListData().Count - 10));
            foreach (var point in dataPoints)
            {
                ChartEntries.Add(new ChartEntry((float)point.Value)
                {
                    Label = point.Time.ToString(),
                    ValueLabel = point.Value.ToString(),
                    Color = SKColor.Parse("#FF0000")
                });
            }
            OnPropertyChanged(nameof(Chart));
        }

        public Color ConnectionStatusColor => _isConnected ? Colors.Green : Colors.Red;

        public string ConnectButtonText => _isConnected ? "Disconnect" : "Connect";
    }
}
