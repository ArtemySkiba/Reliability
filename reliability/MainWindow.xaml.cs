using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;

namespace reliability
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal partial class MainWindow : Window
    {
        public MainWindow(MainWindowVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            //double[] pointsArray = { 1, 2,4,2,4,3,1,2 };
            //double[] pointsArray1 = { 1, 2,3,31,34,3,32 };

            //ResultChart.ChartAreas.Add(new ChartArea("a"));
            //ResultChart.Palette = ChartColorPalette.SeaGreen;
            //ResultChart.Titles.Add("Pets");
            //ResultChart.Series.Add(new Series());
            //ResultChart.Series[0].Points.Add(pointsArray1);
            //ResultChart.Series[0].ChartType = SeriesChartType.StackedArea;
=======
            double[] pointsArray = { 1, 2,4,2,4,3,1,2 };
            double[] pointsArray1 = { 1, 2,3,31,34,3,32 };

            ResultChart.ChartAreas.Add(new ChartArea("a"));
            ResultChart.Palette = ChartColorPalette.SeaGreen;
            ResultChart.Titles.Add("Pets");
            ResultChart.Series.Add(new Series());
            ResultChart.Series[0].Points.Add(pointsArray1);
            ResultChart.Series[0].ChartType = SeriesChartType.StackedArea;
>>>>>>> 487169efa03527261078c061938f5605ca22562a
        }
    }
}
