using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
//using Microcharts;
//using SkiaSharp;
using Microcharts;
using Microcharts.Maui;
using SkiaSharp;
using System.Timers;
using System.Threading;

namespace MauiApp1_Chart
{
    public partial class SerialChartPage : ContentPage
    {
        private ArduinoCommunicator _arduinoCommunicator;
        private FormattedString _logFormattedString;
        private StringBuilder _logBuilder = new StringBuilder();
        private Span _logSpan = new Span();


        // Đoạn code cho Chart

        private readonly FilePickerFileType CsvFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.iOS, new[] { "public.comma-separated-values-text" } }, // hoặc 'public.csv'
            { DevicePlatform.Android, new[] { "text/csv" } },
            { DevicePlatform.UWP, new[] { ".csv" } },
        });


        List<DataPoint> points = new List<DataPoint>()
        {
            new DataPoint
            {
                Value = 10,
                Time = 0
            },
            new DataPoint
            {
                Value = 20,
                Time = 2
            }
        };

        //ChartEntry[] entries = new []
        List<ChartEntry> entries = new List<ChartEntry>
        {
            new ChartEntry(212)
            {
                Label = "Windows",
                ValueLabel = "112",
                Color = SKColor.Parse("#2c3e50")
            },
            new ChartEntry(248)
            {
                Label = "Android",
                ValueLabel = "648",
                Color = SKColor.Parse("#77d065")
            },
            new ChartEntry(128)
            {
                Label = "IOS",
                ValueLabel = "428",
                Color = SKColor.Parse("#b455b6")
            },
            //new ChartEntry(150)
            //{
            //    Label = "kxn",
            //    ValueLabel = points[0].Value.ToString()


            //}
        };

        private ChartPlotter _chartPlotter;

        //private Timer _updateTimer; // lỗi vì chart cũng có Timer
        private System.Timers.Timer _updateTimer; // Sử dụng đầy đủ không gian tên

        private bool _hasNewData;

        // Doan code cho chuc năng tiếp theo

        public SerialChartPage()
        {
            InitializeComponent();
            _arduinoCommunicator = new ArduinoCommunicator();
            _arduinoCommunicator.DataReceived += OnDataReceived;
            _arduinoCommunicator.ConnectionLost += OnConnectionLost;

            _logFormattedString = new FormattedString();
            LogLabel.FormattedText = _logFormattedString;

            setupChart();
            LoadAvailablePorts();
        }

        private void setupChart()
        {
            _chartPlotter = new ChartPlotter();

            var dataPointsFromDataService = DataService.GetSampleData();

            //SharedDataService.DataAdded += OnDataAdded;

            _hasNewData = false;

            // Thiết lập Timer để cập nhật biểu đồ mỗi giây
            _updateTimer = new System.Timers.Timer(1000); // 1000ms = 1 giây
            _updateTimer.Elapsed += OnUpdateTimerElapsed;
            _updateTimer.Start();

            foreach (var point in dataPointsFromDataService)
            {
                entries.Add(new ChartEntry((float)point.Value)
                {
                    Label = point.Time.ToString(),
                    ValueLabel = point.Value.ToString(),
                    Color = SKColor.Parse("#FF0000") // Màu đỏ
                });
            }

            foreach (var point in points)
            {
                entries.Add(new ChartEntry((float)point.Value)
                {
                    Label = point.Time.ToString(),
                    ValueLabel = point.Value.ToString(),
                    Color = SKColor.Parse("#FF0000") // Màu đỏ
                });
            }

            chartView1.Chart = new LineChart
            {
                Entries = entries
            };
        }

        //private void OnUpdateTimerElapsed(object sender, ElapsedEventArgs e)
        //{
        //    if (_hasNewData == true)
        //    {
        //        MainThread.BeginInvokeOnMainThread(() =>
        //        {
        //            var dataQueue = SharedDataService.GetDataQueue();
        //            _chartPlotter.UpdateChart(dataQueue.Select(d => (d.number, d.number)));
        //            chartView1.Chart = _chartPlotter.GetChart();
        //            _hasNewData = false;
        //        });
        //    }
        //}

        //private void OnUpdateTimerElapsed(object sender, ElapsedEventArgs e)
        //{
        //    if (_hasNewData == true)
        //    {
        //        MainThread.BeginInvokeOnMainThread(() =>
        //        {
        //            var dataQueue = SharedDataService.GetDataQueue();

        //            // Lấy 10 giá trị cuối cùng
        //            var lastTenDataPoints = dataQueue.Skip(Math.Max(0, dataQueue.Count() - 10));

        //            _chartPlotter.UpdateChart(lastTenDataPoints.Select(d => (d.number, d.number)));
        //            chartView1.Chart = _chartPlotter.GetChart();
        //            _hasNewData = false;
        //        });
        //    }
        //}

        private void OnUpdateTimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdateChart();
            _hasNewData = false;

            if (_hasNewData)
            {
                _hasNewData = false;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        //var dataQueue = SharedDataService.GetDataQueue();

                        //// Lấy 10 giá trị cuối cùng
                        //var lastTenDataPoints = dataQueue.Skip(Math.Max(0, dataQueue.Count() - 10));

                        //_chartPlotter.UpdateChart(lastTenDataPoints.Select(d => (d.number, d.number)));
                        //chartView1.Chart = _chartPlotter.GetChart();

                        UpdateChart();
                    }
                    catch (Exception ex)
                    {
                        // Ghi lại lỗi nếu xảy ra
                        Console.WriteLine($"Error updating chart: {ex.Message}");
                    }
                });
            }
        }



        //private void UpdateChart()
        //{
        //    var dataPointsFromDataService = DataService.GetListData();
        //    entries.Clear();


        //    if (dataPointsFromDataService.Count == 0)
        //    {
        //        entries.Add(new ChartEntry(0)
        //        {
        //            Label = "No Data",
        //            ValueLabel = "0",
        //            Color = SKColor.Parse("#FF0000") // Màu đỏ
        //        });
        //    }
        //    else
        //    {
        //        foreach (var point in dataPointsFromDataService)
        //        {
        //            entries.Add(new ChartEntry((float)point.Value)
        //            {
        //                Label = point.Time.ToString(),
        //                ValueLabel = point.Value.ToString(),
        //                Color = SKColor.Parse("#FF0000") // Màu đỏ
        //            });
        //        }
        //    }

        //    chartView1.Chart = new LineChart
        //    {
        //        Entries = entries
        //    };
        //}

        private void UpdateChart()
        {
            var dataPointsFromDataService = DataService.GetListData();

            // Lấy 10 giá trị cuối cùng
            var lastTenDataPoints = dataPointsFromDataService.Skip(Math.Max(0, dataPointsFromDataService.Count - 30));

            // Nếu danh sách entries có hơn 10 phần tử, bỏ bớt 10 phần tử đầu tiên
            if (entries.Count > 30)
            {
                entries.RemoveRange(0, entries.Count - 10);
            }

            if (!lastTenDataPoints.Any())
            {
                entries.Add(new ChartEntry(0)
                {
                    Label = "No Data",
                    ValueLabel = "0",
                    Color = SKColor.Parse("#FF0000") // Màu đỏ
                });
            }
            else
            {
                foreach (var point in lastTenDataPoints)
                {
                    entries.Add(new ChartEntry((float)point.Value)
                    {
                        Label = point.Time.ToString(),
                        ValueLabel = point.Value.ToString(),
                        Color = SKColor.Parse("#FF0000") // Màu đỏ
                    });
                }
            }

            chartView1.Chart = new LineChart
            {
                Entries = entries,
                LineMode = LineMode.Straight, // Giảm hiệu ứng bằng cách sử dụng đường thẳng
                PointMode = PointMode.None, // Không hiển thị điểm
                LineSize = 8,
                PointSize = 0, // Không hiển thị điểm để giảm hiệu ứng
                BackgroundColor = SKColors.White,
                LabelTextSize = 30, // Tăng kích thước chữ để dễ đọc hơn
            };
        }




        private void OnDataAdded(string text, double number)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                //var dataQueue = SharedDataService.GetDataQueue();
                //_chartPlotter.UpdateChart(dataQueue.Select(d => (d.number, d.number)));
                //chartView1.Chart = _chartPlotter.GetChart();

                ////DataService.AddData(new DataPoint { Value = value, Time = time });
                ////UpdateChart();
                _hasNewData = true;
            });
        }

        private void OnAddDataClicked(object sender, EventArgs e)
        {
            if (double.TryParse(ValueEntry.Text, out double value) && double.TryParse(TimeEntry.Text, out double time))
            {
                DataService.AddData(new DataPoint { Value = value, Time = time });
                UpdateChart();
                //ValueEntry.Text = string.Empty;
                //TimeEntry.Text = string.Empty;
            }
        }

        private void OnClearDataClicked(object sender, EventArgs e)
        {
            DataService.ClearData();
            UpdateChart();
        }

        private async void OnSaveCSVClicked(object sender, EventArgs e)
        {
            var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "testVisualStudioSaveCSV");

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, "data.csv");

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Value,Time");
                foreach (var point in DataService.GetListData())
                {
                    writer.WriteLine($"{point.Value},{point.Time}");
                }
            }

            await DisplayAlert("Success", $"Data saved to {filePath}", "OK");
        }


        private async void OnImportCSVClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Please select a CSV file",
                FileTypes = CsvFileType
            });

            if (result != null)
            {
                using (var reader = new StreamReader(result.FullPath))
                {
                    var header = reader.ReadLine(); // Read header
                    string line;
                    var dataPoints = new List<DataPoint>();

                    while ((line = reader.ReadLine()) != null)
                    {
                        var values = line.Split(',');

                        if (values.Length == 2 && double.TryParse(values[0], out double value) && double.TryParse(values[1], out double time))
                        {
                            dataPoints.Add(new DataPoint { Value = value, Time = time });
                        }
                    }

                    DataService.SetData(dataPoints); // Clear and set new data
                    UpdateChart();
                }
            }
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
                //SharedDataService.AddData(number1.ToString(), number2);

                DataService.AddData(new DataPoint { Value = number1, Time = number2 });
                //UpdateChart();
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
            //TestPortsPicker();
            _hasNewData = true;
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
