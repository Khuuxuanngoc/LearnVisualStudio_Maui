using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1_Chart.ViewModels
{
    public partial class LiveChartsViewModel : ObservableObject
    {
        public LiveChartsViewModel()
        {
            Series = new ObservableCollection<ISeries>
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

            AddDataCommand = new RelayCommand(AddData);
            ClearDataCommand = new RelayCommand(ClearData);
            StartRealTimeData();
        }

        public ObservableCollection<ISeries> Series { get; }

        [ObservableProperty]
        private double newValue;

        public ICommand AddDataCommand { get; }
        public ICommand ClearDataCommand { get; }

        private void AddData()
        {
            if (Series[0].Values is ObservableCollection<double> values1)
            {
                values1.Add(NewValue);
                EnsureLastNValues(values1, 20);
            }

            if (Series[1].Values is ObservableCollection<double> values2)
            {
                values2.Add(NewValue + 1); // Giả định giá trị của Series 2
                EnsureLastNValues(values2, 20);
            }
        }

        private void ClearData()
        {
            foreach (var series in Series)
            {
                if (series.Values is ObservableCollection<double> values)
                {
                    values.Clear();
                }
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

        private void EnsureLastNValues(ObservableCollection<double> values, int n)
        {
            while (values.Count > n)
            {
                values.RemoveAt(0);
            }
        }
    }
}
