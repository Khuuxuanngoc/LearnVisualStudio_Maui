using Microsoft.Maui.Controls;
using LiveChartsCore.SkiaSharpView.Maui;
using MauiApp1_Chart.ViewModels;

namespace MauiApp1_Chart.kxnPage
{
    public class MultiLiveChartsPageNoXaml : ContentPage
    {
        public MultiLiveChartsPageNoXaml()
        {
            int numberOfCharts = 5; // Hoặc bất kỳ số lượng nào bạn muốn
            var viewModel = new MultiLiveChartsViewModel(numberOfCharts);
            BindingContext = viewModel;

            var scrollView = new ScrollView();
            var stackLayout = new VerticalStackLayout();

            foreach (var series in viewModel.SeriesCollection)
            {
                var chart = new CartesianChart
                {
                    Series = series,
                    XAxes = viewModel.XAxes,
                    YAxes = viewModel.YAxes,
                    ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X,
                    ZoomingSpeed = 800,
                    HeightRequest = 300, // Đặt chiều cao biểu đồ
                    WidthRequest = 300, // Đặt chiều rộng biểu đồ
                };
                stackLayout.Children.Add(chart);
            }

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

            scrollView.Content = stackLayout;
            Content = scrollView;
        }
    }
}
