//using MauiApp1_Chart;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

////namespace MauiApp1_Chart
////{
////    internal class DataService
////    {
////    }
////}

using System.Collections.Generic;

public static class DataService
{
    public static List<DataPoint> GetSampleData()
    {
        return new List<DataPoint>
        {
            new DataPoint { Value = 10, Time = 1 },
            new DataPoint { Value = 20, Time = 2 },
            new DataPoint { Value = 30, Time = 3 },
            // Thêm dữ liệu mẫu khác nếu cần
        };
    }

    private static List<DataPoint> _dataPoints = new List<DataPoint>
    {
        new DataPoint { Value = 40, Time = 1 },
        new DataPoint { Value = 50, Time = 2 },
        new DataPoint { Value = 60, Time = 3 }
    };

    public static List<DataPoint> GetListData()
    {
        return new List<DataPoint>(_dataPoints);
    }
    public static void AddData(DataPoint dataPoint)
    {
        _dataPoints.Add(dataPoint);
    }

    public static void ClearData()
    {
        _dataPoints.Clear();
    }

    public static void SetData(List<DataPoint> dataPoints)
    {
        _dataPoints = dataPoints;
    }
}

//public struct DataPoint
//{
//    public double Value { get; set; }
//    public double Time { get; set; }
//}


