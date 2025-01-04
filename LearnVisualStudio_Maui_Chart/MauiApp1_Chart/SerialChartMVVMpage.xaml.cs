using MauiApp1_Chart.ViewModels;
using System.Diagnostics;

namespace MauiApp1_Chart
{
    public partial class SerialChartMVVMpage : ContentPage
    {
        public SerialChartMVVMpage()
        {
            InitializeComponent();
            BindingContext = new SerialChartViewModel(); // Thiết lập BindingContext tại đây
            Debug.WriteLine("SerialChartMVVMpage initialized"); // Thêm dòng này để kiểm tra
        }
    }
}
