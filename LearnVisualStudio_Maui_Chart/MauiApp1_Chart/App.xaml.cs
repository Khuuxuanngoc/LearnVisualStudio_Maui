namespace MauiApp1_Chart
{
    //public partial class App : Application
    //{
    //    public App()
    //    {
    //        InitializeComponent();
    //    }

    //    protected override Window CreateWindow(IActivationState? activationState)
    //    {
    //        return new Window(new AppShell());
    //    }
    //}

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            // Thêm xử lý ngoại lệ toàn cầu
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var ex = args.ExceptionObject as Exception;
                if (ex != null)
                {
                    Console.WriteLine($"Unhandled exception: {ex.Message}");
                }
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                var ex = args.Exception;
                Console.WriteLine($"Unobserved task exception: {ex.Message}");
                args.SetObserved();
            };
        }
    }

}

