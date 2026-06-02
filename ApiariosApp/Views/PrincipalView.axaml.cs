using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiariosApp.Views
{
    public partial class PrincipalView : UserControl, INotifyPropertyChanged
    {
        private string _weatherTemp = "Cargando...";
        public string WeatherTemp 
        { 
            get => _weatherTemp; 
            set { _weatherTemp = value; OnPropertyChanged(); } 
        }

        private string _weatherDesc = "Obteniendo datos...";
        public string WeatherDesc 
        { 
            get => _weatherDesc; 
            set { _weatherDesc = value; OnPropertyChanged(); } 
        }

        private string _weatherHumidity = "--";
        public string WeatherHumidity 
        { 
            get => _weatherHumidity; 
            set { _weatherHumidity = value; OnPropertyChanged(); } 
        }

        private string _weatherWind = "--";
        public string WeatherWind 
        { 
            get => _weatherWind; 
            set { _weatherWind = value; OnPropertyChanged(); } 
        }

        private string _seasonMessage = string.Empty;
        public string SeasonMessage 
        { 
            get => _seasonMessage; 
            set { _seasonMessage = value; OnPropertyChanged(); } 
        }

        public int AlertasCriticasCount { get; set; }
        public ObservableCollection<AlertaItem> AlertasList { get; set; }
        public ObservableCollection<TareaItem> TareasList { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public PrincipalView()
        {
            InitializeComponent();
            DataContext = this;

            AlertasList = new ObservableCollection<AlertaItem>
            {
                new AlertaItem { Titulo = "Segunda Dosis - Varroa", Subtitulo = "Apiario \"El Tejón\" - 45 Colmenas", Tiempo = "Vence hoy" },
                new AlertaItem { Titulo = "Segunda Dosis - Varroa", Subtitulo = "Apiario \"Las Flores\" - 32 Colmenas", Tiempo = "Vence mañana" },
                new AlertaItem { Titulo = "Revisión Alimentación", Subtitulo = "Apiario \"Norte\" - Núcleos nuevos", Tiempo = "En 2 días" }
            };
            AlertasCriticasCount = AlertasList.Count;

            TareasList = new ObservableCollection<TareaItem>
            {
                new TareaItem { Titulo = "Inspección de reinas - El Tejón", Detalles = "09:00 AM", DetallesColor = SolidColorBrush.Parse("#718096"), Completada = false },
                new TareaItem { Titulo = "Aplicar tratamiento Varroa", Detalles = "Urgente - Las Flores", DetallesColor = SolidColorBrush.Parse("#DC2626"), Completada = false },
                new TareaItem { Titulo = "Preparar material para núcleos", Detalles = "Taller", DetallesColor = SolidColorBrush.Parse("#718096"), Completada = false }
            };

            CalculateSeason();
            
            // Iniciar llamada asíncrona a la API del clima
            _ = LoadRealWeatherAsync();
        }

        private void CalculateSeason()
        {
            var date = DateTime.Now;
            string season = "";
            string seasonText = "";
            
            int m = date.Month;
            int d = date.Day;

            // Hemisferio Sur
            if ((m == 12 && d >= 21) || m == 1 || m == 2 || (m == 3 && d < 21)) 
            {
                season = "Verano";
                seasonText = "Fase de mantenimiento. Vigilar acceso a agua y floración tardía.";
            } 
            else if ((m == 3 && d >= 21) || m == 4 || m == 5 || (m == 6 && d < 21)) 
            {
                season = "Otoño";
                seasonText = "Preparación para el invierno. Tratamientos sanitarios clave.";
            } 
            else if ((m == 6 && d >= 21) || m == 7 || m == 8 || (m == 9 && d < 21)) 
            {
                season = "Invierno";
                seasonText = "Fase de invernada. Revisar reservas de alimento y humedad.";
            } 
            else 
            {
                season = "Primavera";
                seasonText = "Fase de alto flujo de néctar. Preparar alzas para cosecha inminente.";
            }

            SeasonMessage = $"Condiciones actuales de {season}. {seasonText}";
        }

        private async Task LoadRealWeatherAsync()
        {
            try
            {
                using var client = new HttpClient();
                // Coordenadas de Rivera, Uruguay (Aprox: -30.9053, -55.5508)
                string url = "https://api.open-meteo.com/v1/forecast?latitude=-30.9053&longitude=-55.5508&current=temperature_2m,relative_humidity_2m,wind_speed_10m,weather_code";
                
                // Configurar un timeout para no colgar la UI si no hay internet
                client.Timeout = TimeSpan.FromSeconds(10);
                
                string json = await client.GetStringAsync(url);

                using JsonDocument doc = JsonDocument.Parse(json);
                var current = doc.RootElement.GetProperty("current");

                double temp = current.GetProperty("temperature_2m").GetDouble();
                int humidity = current.GetProperty("relative_humidity_2m").GetInt32();
                double wind = current.GetProperty("wind_speed_10m").GetDouble();
                int code = current.GetProperty("weather_code").GetInt32();

                string desc = GetWeatherDescription(code);

                // Actualizar las propiedades en el hilo de UI
                Dispatcher.UIThread.Post(() => 
                {
                    WeatherTemp = $"{Math.Round(temp)}°C";
                    WeatherHumidity = $"{humidity}%";
                    WeatherWind = $"{Math.Round(wind)} km/h";
                    WeatherDesc = desc;
                });
            }
            catch (Exception)
            {
                Dispatcher.UIThread.Post(() => 
                {
                    WeatherTemp = "--°C";
                    WeatherDesc = "Sin conexión a internet";
                    WeatherHumidity = "--";
                    WeatherWind = "--";
                });
            }
        }

        private string GetWeatherDescription(int code)
        {
            return code switch
            {
                0 => "Despejado",
                1 => "Mayormente despejado",
                2 => "Parcialmente nublado",
                3 => "Nublado",
                45 or 48 => "Niebla",
                51 or 53 or 55 => "Llovizna",
                61 or 63 or 65 => "Lluvia",
                71 or 73 or 75 => "Nieve",
                95 or 96 or 99 => "Tormenta",
                _ => "Clima variable"
            };
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AlertaItem
    {
        public string Titulo { get; set; } = string.Empty;
        public string Subtitulo { get; set; } = string.Empty;
        public string Tiempo { get; set; } = string.Empty;
    }

    public class TareaItem
    {
        public string Titulo { get; set; } = string.Empty;
        public string Detalles { get; set; } = string.Empty;
        public IBrush DetallesColor { get; set; } = Brushes.Gray;
        public bool Completada { get; set; }
    }
}
