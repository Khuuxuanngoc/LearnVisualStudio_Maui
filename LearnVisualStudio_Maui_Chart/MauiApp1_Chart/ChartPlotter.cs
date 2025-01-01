using System.Collections.Generic;
using Microcharts;
using SkiaSharp;
using System.Linq;

public class ChartPlotter
{
    private LineChart _lineChart;

    public ChartPlotter()
    {
        _lineChart = new LineChart
        {
            BackgroundColor = SKColors.White
        };
    }

    public void UpdateChart(IEnumerable<(double, double)> data)
    {
        var entries = data.Select(d =>
            new ChartEntry((float)d.Item1)
            {
                Label = d.Item2.ToString(),
                ValueLabel = d.Item1.ToString(),
                Color = SKColors.Blue
            }).ToList();

        _lineChart.Entries = entries;
    }

    public LineChart GetChart()
    {
        return _lineChart;
    }
}
