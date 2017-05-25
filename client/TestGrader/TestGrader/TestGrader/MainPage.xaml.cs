using Plugin.Media;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestGrader
{
    public partial class MainPage : ContentPage
    {
        public string GetMethodResponse;

        public MainPage()
        {
            InitializeComponent();

            showInfo.Clicked += async (sender, args) =>
            {
                DisplayAlert("Informacja", "1. Najlepszy rezultat otrzymamy, gdy znacznik arkusza(krzyżyk) będzie w lewym górnym rogu, a zdjecie obejmie wszystkie odpowiedzi.\n\n 2. Szybkość działania może zależeć od prędkości łącza internetowego. \n\n Autor: Mateusz Michalski", "OK");
            };

            takePhoto.Clicked += async (sender, args) =>
            {

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    DisplayAlert("Błąd aparatu", ":( Aparat nie jest dostępny.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    Directory = "Arkusze",
                    Name = "arusz.jpg"
                });


                if (file == null)
                    return;

                image.Source = ImageSource.FromStream(() =>
                {
                    return new MemoryStream();
                });

                var imageStream = ReadFully(file.GetStream());
                file.Dispose();

                DisplayAlert("Informacja", "Zdjęcie jest wysyłane...", "OK");
                await PostPictureAsByteArray(imageStream);
                await Task.Delay(3000);

                GetMethodResponse = await GetPictureResult();
                
                if (GetMethodResponse == null)
                {
                    DisplayAlert("Informacja", "Arkusz został źle zeskanowany. Spróbuj ponownie", "OK");
                }
                else
                {

                    image.Source = ImageSource.FromStream(() =>
                    {
                        var stream = new MemoryStream(Convert.FromBase64String(GetMethodResponse));
                        return stream;
                    });

                }
            };
        }

        public static byte[] ReadFully(System.IO.Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public async Task PostPictureAsByteArray(byte[] data)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            response = await client.PostAsync("http://217.182.64.159:8080/api/post", new ByteArrayContent(data));
            //response = await client.PostAsync("http://192.168.0.157:8080/api/post", new ByteArrayContent(data));

            if (response.IsSuccessStatusCode)
            {
                DisplayAlert("Informacja", "Proszę czekać na wynik...", "OK");

            }

        }

        public async Task<string> GetPictureResult()
        {
            HttpClient client = new HttpClient();

            var response = await client.GetAsync("http://217.182.64.159:8080/api/get");
            //var response = await client.GetAsync("http://192.168.0.157:8080/api/get");
            if (response.IsSuccessStatusCode)
            {
                return GetStringFromResult(response);
            }
            else
                return null;
        }

        public string GetStringFromResult(HttpResponseMessage result)
        {
            string content = result.Content.ReadAsStringAsync().Result;
            return content;
        }
    }
}
