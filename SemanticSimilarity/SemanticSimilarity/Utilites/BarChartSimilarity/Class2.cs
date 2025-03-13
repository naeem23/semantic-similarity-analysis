< Window x: Class = "SimilarityGraph.MainWindow"
        xmlns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns: x = "http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns: lvc = "clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf"
        Title = "Similarity Trends" Height = "450" Width = "800" >
    < Grid >
        < lvc:CartesianChart Name = "SimilarityChart" >
            < lvc:CartesianChart.Series >
                < lvc:LineSeries Title = "Similarity Score" Values="{Binding SimilarityValues}" PointGeometrySize="10"/>
            </lvc:CartesianChart.Series >
            < lvc:CartesianChart.AxisX >
                < lvc:Axis Title = "Comparison Index" Labels="{Binding Labels}"/>
            </lvc:CartesianChart.AxisX >
            < lvc:CartesianChart.AxisY >
                < lvc:Axis Title = "Similarity Score" MinValue="0" MaxValue="1"/>
            </lvc:CartesianChart.AxisY >
        </ lvc:CartesianChart >
    </ Grid >
</ Window >
