//using Foundation;
using Microcharts;
using Microcharts.Maui;
using SkiaSharp;
using System.Timers;

// https://www.youtube.com/watch?v=yMG8oPIuMig Visualize Your Data with Charts in .NET MAUI
// 


namespace MauiApp1_Chart
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

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

        public MainPage()
        {
            InitializeComponent();

            SharedDataService.DataAdded += OnDataAdded;

            _chartPlotter = new ChartPlotter();

            var dataPointsFromDataService = DataService.GetSampleData();

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

            

            //chartView.Chart = new BarChart
            //{
            //    Entries = entries
            //};

            chartView1.Chart = new LineChart
            {
                Entries = entries
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

        private void OnUpdateTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_hasNewData == true)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var dataQueue = SharedDataService.GetDataQueue();
                    _chartPlotter.UpdateChart(dataQueue.Select(d => (d.number, d.number)));
                    chartView1.Chart = _chartPlotter.GetChart();
                    _hasNewData = false;
                });
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            SharedDataService.DataAdded -= OnDataAdded;
            _updateTimer.Stop();
            _updateTimer.Dispose();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void UpdateChart()
        {
            var dataPointsFromDataService = DataService.GetListData();
            entries.Clear();


            if (dataPointsFromDataService.Count == 0)
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
                foreach (var point in dataPointsFromDataService)
                {
                    entries.Add(new ChartEntry((float)point.Value)
                    {
                        Label = point.Time.ToString(),
                        ValueLabel = point.Value.ToString(),
                        Color = SKColor.Parse("#FF0000") // Màu đỏ
                    });
                }
            }

            //chartView.Chart = new BarChart
            //{
            //    Entries = entries
            //};

            chartView1.Chart = new LineChart
            {
                Entries = entries
            };
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

        //private async void OnSaveCSVClicked(object sender, EventArgs e)
        //{
        //    string filePath = Path.Combine(FileSystem.CacheDirectory, "data.csv");

        //    using (var writer = new StreamWriter(filePath))
        //    {
        //        writer.WriteLine("Value,Time");
        //        foreach (var point in DataService.GetListData())
        //        {
        //            writer.WriteLine($"{point.Value},{point.Time}");
        //        }
        //    }

        //    await DisplayAlert("Success", $"Data saved to {filePath}", "OK");
        //}

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

        //private async void OnImportCSVClicked(object sender, EventArgs e)
        //{
        //    var result = await FilePicker.PickAsync(new PickOptions
        //    {
        //        PickerTitle = "Please select a CSV file",
        //        FileTypes = FilePickerFileType.Custom,
        //        FileTypeFilter = new List<string> { ".csv" }
        //    });

        //    if (result != null)
        //    {
        //        using (var reader = new StreamReader(result.FullPath))
        //        {
        //            var header = reader.ReadLine(); // Read header
        //            string line;
        //            var dataPoints = new List<DataPoint>();

        //            while ((line = reader.ReadLine()) != null)
        //            {
        //                var values = line.Split(',');

        //                if (values.Length == 2 && double.TryParse(values[0], out double value) && double.TryParse(values[1], out double time))
        //                {
        //                    dataPoints.Add(new DataPoint { Value = value, Time = time });
        //                }
        //            }

        //            DataService.SetData(dataPoints); // Clear and set new data
        //            UpdateChart();
        //        }
        //    }
        //}

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

    }

}
