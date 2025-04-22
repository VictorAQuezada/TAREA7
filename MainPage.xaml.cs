using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace VehiculosApp
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public ObservableCollection<Vehicle> Vehicles { get; set; } = new ObservableCollection<Vehicle>();
        private List<Vehicle> _allVehicles = new List<Vehicle>();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            Task task = LoadVehiclesData();
        }

        private async Task LoadVehiclesData()
        {
            try
            {
                var url = "https://raw.githubusercontent.com/JUANCITOPENA/ARCHIVO_JSON_FOTOS_AUTOS_CLASE/refs/heads/main/vehiculos.json";
                var response = await _httpClient.GetStringAsync(url);
                _allVehicles = JsonConvert.DeserializeObject<List<Vehicle>>(response);
                foreach (var vehicle in _allVehicles)
                    Vehicles.Add(vehicle);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No se pudo cargar la información: " + ex.Message, "OK");
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = e.NewTextValue.ToLower();
            var filtered = _allVehicles
                .Where(v => v.Marca.ToLower().Contains(searchText) ||
                            v.Tipo.ToLower().Contains(searchText))
                .ToList();
            Vehicles.Clear();
            foreach (var vehicle in filtered)
                Vehicles.Add(vehicle);
        }

        public class Vehicle
        {
            public string Codigo { get; set; }
            public string Marca { get; set; }
            public string Logo { get; set; }
            public string Tipo { get; set; }
            public string Modelo { get; set; }
            public string Caracteristicas { get; set; }
            public string Imagen { get; set; }
            public int NumeroPuertas { get; set; }
            public string Motor { get; set; }
            public string Combustible { get; set; }
            public string Categoria { get; set; }
            public double Precio_Compra { get; set; }
            public double Precio_Venta { get; set; }
            public int Existencia { get; set; }
        }
    }
}