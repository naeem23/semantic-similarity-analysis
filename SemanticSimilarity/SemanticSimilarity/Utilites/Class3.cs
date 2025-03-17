using System.Collections.ObjectModel;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;

public partial class MainWindow : Window
{
    public SeriesCollection SeriesCollection { get; set; }
    public ObservableCollection<double> SimilarityScores { get; set; } = new ObservableCollection<double>();

    public MainWindow()
    {
        InitializeComponent();
        SeriesCollection = new SeriesCollection
        {
            new LineSeries
            {
                Title = "Similarity Score",
                Values = new ChartValues<double>(SimilarityScores)
            }
        };
        DataContext = this;
    }

    private void UpdateGraph(double newScore)
    {
        SimilarityScores.Add(newScore);
    }
}
