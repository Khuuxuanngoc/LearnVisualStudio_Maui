using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1_Chart.ViewModels
{
    public partial class MultiLiveChartsViewModel : ObservableObject
    {
        public MultiLiveChartsViewModel() : this(5)
        {
        }

        public MultiLiveChartsViewModel(int chartCount)
        {
            SeriesCollection = new ObservableCollection<ObservableCollection<ISeries>>();
            XAxes = new ObservableCollection<Axis> { new Axis { Name = "X Axis" } };
            YAxes = new ObservableCollection<Axis> { new Axis { Name = "Y Axis" } };

            for (int i = 0; i < chartCount; i++)
            {
                AddChart();
            }

            AddDataCommand = new RelayCommand(AddData);
            ClearDataCommand = new RelayCommand(ClearData);
            StartRealTimeData();
        }

        public ObservableCollection<ObservableCollection<ISeries>> SeriesCollection { get; }
        public ObservableCollection<Axis> XAxes { get; }
        public ObservableCollection<Axis> YAxes { get; }

        [ObservableProperty]
        private double newValue;

        public ICommand AddDataCommand { get; }
        public ICommand ClearDataCommand { get; }

        private void AddData()
        {
            foreach (var series in SeriesCollection)
            {
                if (series[0].Values is ObservableCollection<double> values1)
                {
                    values1.Add(NewValue);
                    EnsureLastNValues(values1, 20);
                }
            }
        }

        private void ClearData()
        {
            foreach (var series in SeriesCollection)
            {
                foreach (var s in series)
                {
                    if (s.Values is ObservableCollection<double> values)
                    {
                        values.Clear();
                    }
                }
            }
        }

        private void AddChart()
        {
            var series = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = new ObservableCollection<double> { 2, 4, 6, 8, 10 },
                    Name = "Series 1"
                },
                new LineSeries<double>
                {
                    Values = new ObservableCollection<double> { 1, 3, 5, 7, 9 },
                    Name = "Series 2"
                }
            };

            SeriesCollection.Add(series);
        }

        private void EnsureLastNValues(ObservableCollection<double> values, int n)
        {
            while (values.Count > n)
            {
                values.RemoveAt(0);
            }
        }

        private async void StartRealTimeData()
        {
            await Task.Run(() => UpdateData());
        }

        private void UpdateData()
        {
            // Giả lập dữ liệu thời gian thực
            while (true)
            {
                Task.Delay(1000).Wait();
                NewValue = new Random().NextDouble() * 100;
                AddData();
            }
        }
    }
}
