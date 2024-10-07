using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SeoulStay.Listing;

namespace SeoulStay
{
    public partial class Page1 : ContentPage
    {
        private readonly PropertyListing listings;
        private readonly HttpClient httpClient;
        public Page1(PropertyListing listing)
        {

            InitializeComponent();
            listings =listing;
            httpClient = new HttpClient();
            Title = $"{listings.Title} Prices";
            LoadPrices();
            
        }
        private async void LoadPrices()
        {
            var prices = await GetPrices();
            PricesListView.ItemsSource = prices;
        }

        private async Task<List<ItemPrice>> GetPrices()
        {
            var url = $"http://10.0.2.2:8055/api/itemprices?itemId={listings.Id}";

            try
            {
                var response = await httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<List<ItemPrice>>(response);
            } 
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occured: {ex.Message}", "OK");
                return new List<ItemPrice>();
            }
        }
    }
}