## Ngoài StackLayout, bạn còn có thể sử dụng nhiều loại layout khác trong .NET MAUI để bố trí các phần tử giao diện trong `MultiLiveChartsPageNoXaml.cs`. Dưới đây là một số layout phổ biến và cách sử dụng chúng:

### Grid: Grid cho phép bạn sắp xếp các phần tử theo hàng và cột, tạo thành một lưới linh hoạt.

``` c#

var grid = new Grid();
grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

var chart = new CartesianChart
{
    Series = viewModel.SeriesCollection[0],
    XAxes = viewModel.XAxes,
    YAxes = viewModel.YAxes,
    ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X,
    ZoomingSpeed = 800
};
Grid.SetRow(chart, 0);
Grid.SetColumnSpan(chart, 2);
grid.Children.Add(chart);

var addButton = new Button
{
    Text = "Add Data",
    Command = viewModel.AddDataCommand
};
Grid.SetRow(addButton, 1);
Grid.SetColumn(addButton, 0);
grid.Children.Add(addButton);

var clearButton = new Button
{
    Text = "Clear Data",
    Command = viewModel.ClearDataCommand
};
Grid.SetRow(clearButton, 1);
Grid.SetColumn(clearButton, 1);
grid.Children.Add(clearButton);

var entry = new Entry
{
    Placeholder = "Enter new value",
    Keyboard = Keyboard.Numeric
};
entry.SetBinding(Entry.TextProperty, nameof(viewModel.NewValue));
Grid.SetRow(entry, 2);
Grid.SetColumnSpan(entry, 2);
grid.Children.Add(entry);

Content = grid;


```

### AbsoluteLayout: AbsoluteLayout cho phép bạn định vị các phần tử bằng tọa độ tuyệt đối hoặc phần trăm.

```c#

var absoluteLayout = new AbsoluteLayout();

var chart = new CartesianChart
{
    Series = viewModel.SeriesCollection[0],
    XAxes = viewModel.XAxes,
    YAxes = viewModel.YAxes,
    ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X,
    ZoomingSpeed = 800
};
AbsoluteLayout.SetLayoutBounds(chart, new Rect(0, 0, 1, 0.8));
AbsoluteLayout.SetLayoutFlags(chart, AbsoluteLayoutFlags.All);
absoluteLayout.Children.Add(chart);

var addButton = new Button
{
    Text = "Add Data",
    Command = viewModel.AddDataCommand
};
AbsoluteLayout.SetLayoutBounds(addButton, new Rect(0, 0.8, 0.5, 0.1));
AbsoluteLayout.SetLayoutFlags(addButton, AbsoluteLayoutFlags.All);
absoluteLayout.Children.Add(addButton);

var clearButton = new Button
{
    Text = "Clear Data",
    Command = viewModel.ClearDataCommand
};
AbsoluteLayout.SetLayoutBounds(clearButton, new Rect(0.5, 0.8, 0.5, 0.1));
AbsoluteLayout.SetLayoutFlags(clearButton, AbsoluteLayoutFlags.All);
absoluteLayout.Children.Add(clearButton);

var entry = new Entry
{
    Placeholder = "Enter new value",
    Keyboard = Keyboard.Numeric
};
entry.SetBinding(Entry.TextProperty, nameof(viewModel.NewValue));
AbsoluteLayout.SetLayoutBounds(entry, new Rect(0, 0.9, 1, 0.1));
AbsoluteLayout.SetLayoutFlags(entry, AbsoluteLayoutFlags.All);
absoluteLayout.Children.Add(entry);

Content = absoluteLayout;

```

### FlexLayout: FlexLayout cho phép bạn sắp xếp các phần tử theo cả chiều dọc và chiều ngang linh hoạt, tương tự như Flexbox trong CSS.

```c#

var flexLayout = new FlexLayout
{
    Direction = FlexDirection.Column,
    AlignItems = FlexAlignItems.Center
};

var chart = new CartesianChart
{
    Series = viewModel.SeriesCollection[0],
    XAxes = viewModel.XAxes,
    YAxes = viewModel.YAxes,
    ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X,
    ZoomingSpeed = 800,
    HeightRequest = 300
};
flexLayout.Children.Add(chart);

var addButton = new Button
{
    Text = "Add Data",
    Command = viewModel.AddDataCommand
};
flexLayout.Children.Add(addButton);

var clearButton = new Button
{
    Text = "Clear Data",
    Command = viewModel.ClearDataCommand
};
flexLayout.Children.Add(clearButton);

var entry = new Entry
{
    Placeholder = "Enter new value",
    Keyboard = Keyboard.Numeric
};
entry.SetBinding(Entry.TextProperty, nameof(viewModel.NewValue));
flexLayout.Children.Add(entry);

Content = flexLayout;

```

### VerticalStackLayout và HorizontalStackLayout: VerticalStackLayout sắp xếp các phần tử theo chiều dọc, trong khi HorizontalStackLayout sắp xếp theo chiều ngang.

```c#

var verticalStackLayout = new VerticalStackLayout();

var chart = new CartesianChart
{
    Series = viewModel.SeriesCollection[0],
    XAxes = viewModel.XAxes,
    YAxes = viewModel.YAxes,
    ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X,
    ZoomingSpeed = 800,
    HeightRequest = 300
};
verticalStackLayout.Children.Add(chart);

var horizontalStackLayout = new HorizontalStackLayout();

var addButton = new Button
{
    Text = "Add Data",
    Command = viewModel.AddDataCommand
};
horizontalStackLayout.Children.Add(addButton);

var clearButton = new Button
{
    Text = "Clear Data",
    Command = viewModel.ClearDataCommand
};
horizontalStackLayout.Children.Add(clearButton);

verticalStackLayout.Children.Add(horizontalStackLayout);

var entry = new Entry
{
    Placeholder = "Enter new value",
    Keyboard = Keyboard.Numeric
};
entry.SetBinding(Entry.TextProperty, nameof(viewModel.NewValue));
verticalStackLayout.Children.Add(entry);

Content = verticalStackLayout;


```