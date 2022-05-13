using System.Collections.ObjectModel;
using LogicLibrary;

namespace Lab2Var2
{
    public class ViewData
    {
        public Input Params { get; set; }
        public SplinesData Data { get; set; }
        public LiveChart Plot { get; set; }
        public ObservableCollection<string> ListOutput { get; set; }
        public TextOutput TextBlocks { get; set; }

        public ViewData()
        {
            Params = new();
            Data = new(new(Params), new(Params));
            Plot = new();
            ListOutput = new();
            TextBlocks = new();
        }

        public void UpdateData()
        {
            Data.Measures.Updater(Params);
            Data.Params.Updater(Params);
        }

        public double Interpolate()
        {
            return Data.Interpolate();
        }

        public double Integrate()
        {
            return Data.Integrate();
        }
    }
}
