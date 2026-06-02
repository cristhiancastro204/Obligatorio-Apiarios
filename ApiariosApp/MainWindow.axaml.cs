using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace ApiariosApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Set default view
            MainContent.Content = new Views.PrincipalView();
        }

        public void NavigateToPrincipal(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.PrincipalView();
            UpdateSidebarSelection(BtnDashboard);
        }

        public void NavigateToProduccion(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.ProduccionYCosechaView();
            UpdateSidebarSelection(BtnProduccion);
        }

        public void NavigateToMisApiarios(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.MisApiariosView();
            UpdateSidebarSelection(BtnMisApiarios);
        }

        private void UpdateSidebarSelection(Button activeBtn)
        {
            // Reset all
            BtnDashboard.Background = Brushes.Transparent;
            if (BtnMisApiarios != null) BtnMisApiarios.Background = Brushes.Transparent;
            BtnProduccion.Background = Brushes.Transparent;
            // Setting a background on transparent Avalonia buttons requires slightly different handling,
            // but we can just use SolidColorBrush.
            
            // Set active
            if (activeBtn != null)
            {
                activeBtn.Background = SolidColorBrush.Parse("#F4F2EC");
            }
        }
    }
}