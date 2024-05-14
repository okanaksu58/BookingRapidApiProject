using BookingRapidApiProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookingRapidApiProject.Controllers
{

    public class BookController : Controller
    {
        public async Task<string> GetId(string cityName)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://booking-com.p.rapidapi.com/v1/hotels/locations?name={cityName}&locale=tr"),
                Headers =
    {
        { "X-RapidAPI-Key", "c8de957553mshf1f6c4611cb197bp1849e8jsn10e0418ee477" },
        { "X-RapidAPI-Host", "booking-com.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var locations = JsonConvert.DeserializeObject<List<City>>(body);
                if (locations != null && locations.Count > 0)
                {
                    return locations[0].dest_id;
                }
                else
                {
                    return null;
                }

            }

        }
        public async Task<List<Otel.Result>> GetOtel(Search p)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://booking-com.p.rapidapi.com/v1/hotels/search?checkout_date=2024-09-15&order_by=popularity&filter_by_currency=AED&room_number=1&dest_id={p.dest_id}&dest_type=city&adults_number=2&checkin_date=2024-09-14&locale=en-gb&units=metric&include_adjacency=true&children_number=2&categories_filter_ids=class%3A%3A2%2Cclass%3A%3A4%2Cfree_cancellation%3A%3A1&page_number=0&children_ages=5%2C0"),
                Headers =
    {
        { "X-RapidAPI-Key", "c8de957553mshf1f6c4611cb197bp1849e8jsn10e0418ee477" },
        { "X-RapidAPI-Host", "booking-com.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Otel>(body);
                return result.result.ToList();
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(Search p)
        {
            var destId = await GetId(p.city_name);
            if (destId == null)
            {
                return RedirectToAction("Error404", "ErrorPage");
            }

            p.dest_id = destId;

            var hotels = await GetOtel(p);
            if (hotels == null)
            {
                return RedirectToAction("Error404", "ErrorPage");
            }

            return View("Index", hotels);

        }


    }
}