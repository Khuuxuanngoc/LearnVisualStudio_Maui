using System.Collections.Generic;
using System.Linq;

namespace MauiApp1_Chart.Models
{
    public static class DataService_Model
    {
        private static List<DataPoint_Model> _dataPoints = new List<DataPoint_Model>();

        public static void AddData(DataPoint_Model dataPoint)
        {
            _dataPoints.Add(dataPoint);
        }

        public static List<DataPoint_Model> GetListData()
        {
            return _dataPoints.ToList();
        }

        public static void ClearData()
        {
            _dataPoints.Clear();
        }

        public static void SetData(IEnumerable<DataPoint_Model> dataPoints)
        {
            _dataPoints = dataPoints.ToList();
        }

        public static IEnumerable<DataPoint_Model> GetSampleData()
        {
            return new List<DataPoint_Model>
            {
                new DataPoint_Model { Value = 10, Time = 1 },
                new DataPoint_Model { Value = 20, Time = 2 },
                new DataPoint_Model { Value = 30, Time = 3 }
            };
        }
    }
}
