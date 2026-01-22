using System.Windows;
using Tudormobile.Wpf.Controls;

namespace WpfControlsGallery
{
    /// <summary>
    /// Interaction logic for ChartTestWindow.xaml
    /// </summary>
    public partial class ChartTestWindow : Window
    {
        public ChartSeries DonutChartData { get; set; }
        public ChartSeries PieChartData { get; set; }
        public ChartSeries BarChartData { get; set; }
        public ChartTestWindow()
        {
            InitializeComponent();
            DataContext = this;

            DonutChartData = new ChartSeries()
            {
                Name = "Donut Data",
                DataPoints = [234.56, 789.12, 3456.78, 912.34, 1234.56]
            };

            PieChartData = new LabelledSeries
            {
                Name = "Series 1",
                DataPoints = [123.45, 483.72, 4847.21, 98.36, 928.34],
                Labels = ["Category A", "Category B", "Category C", "Category D", "Category E"]
            };

            BarChartData = new ChartSeries
            {
                Name = "Series 1",
                DataPoints = []
            };

            BarChartData.DataPoints.Clear();
            var balance = 123.45;
            var random = new Random();
            for (int i = 0; i < 500; i++)
            {
                BarChartData.DataPoints.Add(balance);
                balance += random.NextDouble() * 500;
            }


        }
    }
}
