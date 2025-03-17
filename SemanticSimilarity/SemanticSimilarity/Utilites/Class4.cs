using System.Collections.ObjectModel;
using System.Windows;

public partial class MainWindow : Window
{
    public ObservableCollection<ObservableCollection<double>> SimilarityMatrix { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        SimilarityMatrix = ComputeSimilarityMatrix();
        DataContext = this;
    }

    private ObservableCollection<ObservableCollection<double>> ComputeSimilarityMatrix()
    {
        List<string> texts = new List<string> { "AI is smart", "Machine learning is AI", "AI is powerful" };
        var matrix = new ObservableCollection<ObservableCollection<double>>();

        foreach (var text1 in texts)
        {
            var row = new ObservableCollection<double>();
            foreach (var text2 in texts)
            {
                row.Add(TFIDFHelper.ComputeTFIDFSimilarity(text1, text2, texts));
            }
            matrix.Add(row);
        }

        return matrix;
    }
}
