using Microcharts;
using SkiaSharp;

// https://www.youtube.com/watch?v=yMG8oPIuMig


namespace MauiApp1_Chart
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

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
        public MainPage()
        {
            InitializeComponent();

            foreach (var point in points)
            {
                entries.Add(new ChartEntry((float)point.Value)
                {
                    Label = point.Time.ToString(),
                    ValueLabel = point.Value.ToString(),
                    Color = SKColor.Parse("#FF0000") // Màu đỏ
                });
            }

            

            chartView.Chart = new BarChart
            {
                Entries = entries
            };

            chartView1.Chart = new LineChart
            {
                Entries = entries
            };

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
    }

}
