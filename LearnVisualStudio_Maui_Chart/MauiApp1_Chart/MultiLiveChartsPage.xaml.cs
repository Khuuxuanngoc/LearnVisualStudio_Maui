using Microsoft.Maui.Controls;
using LiveChartsCore.SkiaSharpView.Maui;
using MauiApp1_Chart.ViewModels;
using LiveChartsCore.Measure;

namespace MauiApp1_Chart
{
    public partial class MultiLiveChartsPage : ContentPage
    {
        public MultiLiveChartsPage()
        {
            InitializeComponent();
            int numberOfCharts = 5; // Hoặc bất kỳ số lượng nào bạn muốn
            var viewModel = new MultiLiveChartsViewModel(numberOfCharts);
            BindingContext = viewModel;

            var stackLayout = new StackLayout();

            //foreach (var series in viewModel.SeriesCollection)
            //{
            //    var chart = new CartesianChart
            //    {
            //        Series = series,
            //        XAxes = viewModel.XAxes,
            //        YAxes = viewModel.YAxes,
            //        ZoomMode = ZoomAndPanMode.X,
            //        ZoomingSpeed = 800
            //    };
            //    stackLayout.Children.Add(chart);
            //}

            stackLayout.Children.Add(new Button
            {
                Text = "Add Data",
                Command = viewModel.AddDataCommand
            });

            stackLayout.Children.Add(new Button
            {
                Text = "Clear Data",
                Command = viewModel.ClearDataCommand
            });

            var entry = new Entry
            {
                Placeholder = "Enter new value",
                Keyboard = Keyboard.Numeric
            };
            entry.SetBinding(Entry.TextProperty, nameof(viewModel.NewValue));
            stackLayout.Children.Add(entry);

            Content = stackLayout;
        }
    }
}
