using System;
using System.Collections.Generic;

public static class SharedDataService
{
    private static Queue<(string text, double number)> _dataQueue = new Queue<(string text, double number)>();

    public static event Action<string, double> DataAdded;

    public static void AddData(string text, double number)
    {
        _dataQueue.Enqueue((text, number));
        DataAdded?.Invoke(text, number);
    }

    public static Queue<(string text, double number)> GetDataQueue()
    {
        return new Queue<(string text, double number)>(_dataQueue);
    }

    public static void ClearData()
    {
        _dataQueue.Clear();
    }
}


/*
 SharedDataService là một lớp tĩnh thông thường với mục đích rõ ràng là để chia sẻ dữ liệu giữa các phần khác nhau của ứng dụng. 
Nó sử dụng các thuộc tính tĩnh để cho phép truy cập và thay đổi dữ liệu từ bất kỳ đâu trong ứng dụng. 
Mặc dù cấu trúc của nó không khác gì so với một lớp C# bình thường, nhưng cách sử dụng của nó giúp quản lý dữ liệu tiện lợi và hiệu quả hơn.

So sánh với lớp bình thường:
    Lớp bình thường:

        Thường tạo đối tượng mới bằng từ khóa new và mỗi đối tượng có trạng thái riêng biệt.

        Dữ liệu không chia sẻ giữa các đối tượng trừ khi được thiết kế đặc biệt.

    Lớp tĩnh (như SharedDataService):

        Không thể tạo đối tượng mới. Các thành viên tĩnh có thể truy cập trực tiếp thông qua tên lớp.

        Dữ liệu chia sẻ giữa tất cả các phần của ứng dụng mà không cần phải truyền qua các phương thức hay đối tượng.

 */