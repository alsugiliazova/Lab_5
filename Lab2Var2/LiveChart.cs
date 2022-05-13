using System;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace Lab2Var2
{
    public class LiveChart
    {
        public SeriesCollection Series { get; set; }
        public Func<double, string> Formatter { get; set; }

        public LiveChart()
        {
            Series = new SeriesCollection();

            Formatter = value => value.ToString("F4");
        }

        public void AddToSeries(double[] points, double[] values, string title, int mode)
        {
            ChartValues<ObservablePoint> Values = new ChartValues<ObservablePoint>();
            for (int i = 0; i < values.Length; i++)
            {
                Values.Add(new(points[i], values[i]));
            }

            if(mode == 0)
            {
                Series.Add(new ScatterSeries
                {
                    Title = title,
                    Values = Values,
                    Fill = Brushes.Blue,
                    MinPointShapeDiameter = 5,
                    MaxPointShapeDiameter = 5
                });
            }
            else if(mode == 1)
            {
                Series.Add(new LineSeries
                {
                    Title = title,
                    Values = Values,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Red,
                    PointGeometry = null,
                    LineSmoothness = 0
                });
            }
        }
    }
}
