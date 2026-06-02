using Avalonia.Controls;
using System.Collections.ObjectModel;
using Avalonia.Media;

namespace ApiariosApp.Views
{
    public partial class ProduccionYCosechaView : UserControl
    {
        public ObservableCollection<CosechaItem> HistorialCosechas { get; set; }

        public ProduccionYCosechaView()
        {
            InitializeComponent();

            HistorialCosechas = new ObservableCollection<CosechaItem>();

            DataContext = this;
        }
    }

    public class CosechaItem
    {
        public string LoteId { get; set; } = string.Empty;
        public string Fecha { get; set; } = string.Empty;
        public string Colmena { get; set; } = string.Empty;
        public string PesoNeto { get; set; } = string.Empty;
        public string PerfilBotanico { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public IBrush StatusColor { get; set; } = Brushes.Black;
        public IBrush PesoColor { get; set; } = Brushes.Black;
        public IBrush EstadoBgColor { get; set; } = Brushes.White;
        public IBrush EstadoTextColor { get; set; } = Brushes.Black;
    }
}
