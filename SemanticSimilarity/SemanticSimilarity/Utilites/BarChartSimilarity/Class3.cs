using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;

namespace SimilarityGraph
{
    public partial class MainWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public ChartValues<double> SimilarityValues { get; set; }
        public List<string> Labels { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            LoadSimilarityData();
            DataContext = this;
        }

        private void LoadSimilarityData()
        {
            // Sample data (replace with database values)
            var similarityScores = new List<double> { 0.2, 0.5, 0.75, 0.9, 0.6, 0.8, 0.95 };

            SimilarityValues = new ChartValues<double>(similarityScores);
            Labels = Enumerable.Range(1, similarityScores.Count).Select(i => $"Text {i}").ToList();
        }
    }
}
