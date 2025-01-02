# LearnVisualStudio_Maui

* Vẽ đồ thị với LiveChart2 (tham khảo, chuẩn bị học)
https://livecharts.dev/docs/Maui/2.0.0-rc4/CartesianChart.Cartesian%20chart%20control

# Video tham khảo

* Visualize Your Data with Charts in .NET MAUI
https://www.youtube.com/watch?v=yMG8oPIuMig

* Adding Charts to Your .NET MAUI app with DevExpress Controls (4 cell phone)
https://www.youtube.com/watch?v=uvcMy2WP0_M&t=436s

* Start Your New .NET MAUI App with These Amazing Templates!
https://www.youtube.com/watch?v=waq1ITSqA84
Nên tham khảo để viết app cho Android,...

* Save Files With Just 1 Line of Code with .NET MAUI FileSaver! (Chưa test)
https://www.youtube.com/watch?v=Q9T-dRYq3Ps 

* .NET MAUI for Desktop: Window Size & Position, Tooltips, Context Menus
https://www.youtube.com/watch?v=o35BEuIC-uA 

* Local Push Notifications with .NET MAUI, Easy With This Plugin!
https://www.youtube.com/watch?v=dWdXXGa1_hI

* Material Design & Free Controls for .NET MAUI with UraniumUI
https://www.youtube.com/watch?v=7SxdgdbOHBc

* Create a .NET MAUI Windows MSIX to Sideload Or Publish to the Microsoft Store
https://www.youtube.com/watch?v=FNwv_W3TtSU

* Release Your .NET MAUI iOS App to the Apple App Store
https://www.youtube.com/watch?v=kpZi5xAvpZA

* Create a Signed and Publishable .NET MAUI Android App in VS2022
https://www.youtube.com/watch?v=jfSVb_RR7X0

* Connect Any Client, Anywhere to localhost with Visual Studio Dev Tunnels!
https://www.youtube.com/watch?v=azuC8SFHWp8

* Save an Image from .NET MAUI Community Toolkit DrawingView
https://www.youtube.com/watch?v=OB65n17bR98
    - #if WINDOWS
        #elif ANDROID
        #elif IOS || MACCATALYST

* Generate QR Codes and Other Barcodes with Camera.MAUI in .NET MAUI
https://www.youtube.com/watch?v=ERZfz_NX_Wc

* Sort, Filter & Show Data with this Free DataGrid Control for .NET MAUI
https://www.youtube.com/watch?v=ERQMKw26zrs

* Use .NET MAUI FilePicker to Pick PDFs, Images, Videos and More!
https://www.youtube.com/watch?v=Wg1fhr3iwKY

* How to Generate PDFs in .NET Apps (2024)
https://www.youtube.com/watch?v=JOJRzVqTmBY&list=PLfbOp004UaYWu-meDkRN6_Y1verl96npI&index=2

* Getting Started with the .NET MAUI Scheduler Control
https://www.youtube.com/watch?v=Io2ElO8ORSQ&list=PLDzXQPWT8wEDfRchprRQInF16RHyQGxRh&index=114

# video ứng dụng:

* Build a Complete Restaurant POS Desktop App with .Net MAUI + XAML + SQLite - .Net 8 by Abhay Prince
https://www.youtube.com/watch?v=yU6wsIR37Gs

## Cách build app:

1. Mở Command Prompt hoặc Terminal.

Điều hướng đến thư mục dự án:

`cd D:\git\github\khuuxuanngoc\Learn_CSharp_Maui\LearnVisualStudio_Maui\LearnVisualStudio_Maui_Chart\MauiApp1_Chart`

2. Làm sạch dự án:

`dotnet clean`

3. Build lại dự án ở chế độ Release:

`dotnet build -c Release`

4. Tạo gói phân phối:

  * Đối với Android: `dotnet publish -c Release -f net9.0-android -o ./publish`

  * Đối với iOS: `dotnet publish -c Release -f net9.0-ios -o ./publish`

  * Đối với Mac Catalyst: `dotnet publish -c Release -f net9.0-maccatalyst -o ./publish`

  * Đối với Windows: `dotnet publish -c Release -f net9.0-windows10.0.19041.0 -o ./publish`

  ## Fix lỗi:

  * Lỗi build release

        - https://github.com/mono/SkiaSharp/issues/2444#issuecomment-1564090245