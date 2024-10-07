using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeoulStay
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Listing : ContentPage
    {
        public Listing()
        {
            InitializeComponent();
            LoadListings();

            Title = "Your Listings";
        }
        private async void LoadListings()
        {
            var listing = await GetListings();
            ListingsListView.ItemsSource = listing.OrderBy(l => l.AvailableDays);
        }

        private async Task<List<PropertyListing>> GetListings()
        {
            using (var client = new HttpClient())
            {
                var url = $"http://10.0.2.2:8055/api/Items";

                try
                {
                    var response = await client.GetStringAsync(url);
                    var items = JsonConvert.DeserializeObject<List<Item>>(response);

                    var listings = items.Select(item => new PropertyListing
                    {
                        Id = item.Id,
                        Title = item.Title,
                        LastDateOfPricing = item.ItemPrices.Any() ? item.ItemPrices.Max(ip => ip.Date) : DateTime.MinValue,
                        AvailableDays = item.ItemPrices.Count(ip => ip.Date >= DateTime.Today)
                    }).ToList();
                    return listings;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occured: {ex.Message}", "Ok");
                    return new List<PropertyListing>();
                }
            }
        }
        

        

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var listing = button.CommandParameter as PropertyListing;

            if (listing != null)
            {
                // Navigate to DetailsPage and pass the listing title
                Navigation.PushAsync(new Page1(listing));
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }

        public class PropertyListing
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public DateTime LastDateOfPricing { get; set; }
            public int AvailableDays { get; set; } 
        }

        public class Item
        {
            public long Id { get; set; }
            public Guid Guid { get; set; }
            public long UserId { get; set; }
            public long ItemTypeId { get; set; }
            public long AreaId { get; set; }
            public string Title { get; set; }
            public int Capacity { get; set; }
            public int NumberOfBeds { get; set; }
            public int NumberOfBedrooms { get; set; }
            public int NumberOfBathrooms { get;set; }
            public string ExactAddress { get; set; }
            public string ApproximateAddress { get; set; }
            public string Description { get; set; }
            public string HostRules { get; set; }
            public int MinimumNights { get; set; }
            public int MaximumNights { get; set; }
            public List<ItemPrice> ItemPrices { get; set; }

        }
        public class ItemPrice
        {
            public long Id { get; set; }
            public Guid Guid { get; set; }
            public long ItemId { get; set; }
            public DateTime Date { get; set; }
            public decimal Price { get; set; }
            public long CancellationPolicyId { get; set; }
            public virtual Item Item { get; set; } 
        }

        }
}