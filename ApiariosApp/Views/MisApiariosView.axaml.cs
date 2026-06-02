using Avalonia.Controls;
using Avalonia.Media;
using System.Collections.ObjectModel;
using System.Linq;

namespace ApiariosApp.Views
{
    public partial class MisApiariosView : UserControl
    {
        public ObservableCollection<ColmenaItem> ColmenasList { get; set; }

        public MisApiariosView()
        {
            InitializeComponent();
            DataContext = this;

            ColmenasList = new ObservableCollection<ColmenaItem>();
            LoadMockData();
        }

        private void LoadMockData()
        {
            int[] observacionIds = { 2, 3, 8, 15 };
            int[] alertaIds = { 6, 13, 23, 25, 31 };

            string checkIcon = "M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z";
            string infoIcon = "M11 7h2v2h-2zm0 4h2v6h-2zm1-9C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8z";
            string warnIcon = "M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z";

            for (int i = 1; i <= 32; i++)
            {
                var colmena = new ColmenaItem { Id = $"#{i:000}" };

                if (alertaIds.Contains(i))
                {
                    colmena.BgColor = SolidColorBrush.Parse("#FEF2F2");
                    colmena.BorderColor = SolidColorBrush.Parse("#FCA5A5");
                    colmena.IconColor = SolidColorBrush.Parse("#DC2626");
                    colmena.LogoColor = SolidColorBrush.Parse("#B91C1C");
                    colmena.TextColor = SolidColorBrush.Parse("#991B1B");
                    colmena.StatusIconData = warnIcon;
                }
                else if (observacionIds.Contains(i))
                {
                    colmena.BgColor = SolidColorBrush.Parse("#FEF9C3");
                    colmena.BorderColor = SolidColorBrush.Parse("#FDE047");
                    colmena.IconColor = SolidColorBrush.Parse("#D97706");
                    colmena.LogoColor = SolidColorBrush.Parse("#9C6026");
                    colmena.TextColor = SolidColorBrush.Parse("#5C4033");
                    colmena.StatusIconData = infoIcon;
                }
                else
                {
                    colmena.BgColor = SolidColorBrush.Parse("#FAFAFA");
                    colmena.BorderColor = SolidColorBrush.Parse("#E2E8F0");
                    colmena.IconColor = SolidColorBrush.Parse("#059669");
                    colmena.LogoColor = SolidColorBrush.Parse("#A0AEC0");
                    colmena.TextColor = SolidColorBrush.Parse("#4A5568");
                    colmena.StatusIconData = checkIcon;
                }

                ColmenasList.Add(colmena);
            }
        }
    }

    public class ColmenaItem
    {
        public string Id { get; set; } = string.Empty;
        public IBrush BgColor { get; set; } = Brushes.Transparent;
        public IBrush BorderColor { get; set; } = Brushes.Transparent;
        public IBrush IconColor { get; set; } = Brushes.Transparent;
        public IBrush LogoColor { get; set; } = Brushes.Transparent;
        public IBrush TextColor { get; set; } = Brushes.Black;
        public string StatusIconData { get; set; } = string.Empty;
    }
}
