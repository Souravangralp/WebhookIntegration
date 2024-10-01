using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using System.Net.Http.Headers;
using System.Text;
using WebhookIntegration.Models;

namespace WebhookIntegration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebhookController(HttpClient httpClient) : Controller
    {
        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        //const string endpointSecret = "whsec_ab5571878fe57d0b4ce2a08c311fd12db77c26cc3f9ee1a9bdea1719f51ea5c4";

        //This one is for live
        const string endpointSecret = "whsec_1WZ7fWNGxctuG6q6DHGBZBJ1aFK3pL4j";


        List<string> AcceptedEvents =
        [
            Events.PaymentIntentCreated,
            Events.PaymentIntentSucceeded,
            Events.PaymentIntentPaymentFailed,
            Events.PaymentIntentCanceled,
            Events.PaymentIntentProcessing,
            Events.PaymentIntentAmountCapturableUpdated,
            Events.PaymentIntentPartiallyFunded,
            Events.PaymentIntentRequiresAction
        ];

        private readonly HttpClient _httpClient = httpClient;

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                PaymentIntentResponse response = new();

                var stripeEvent = EventUtility.ConstructEvent(json,
                     Request.Headers["Stripe-Signature"], endpointSecret);

                if (!string.IsNullOrWhiteSpace(stripeEvent.Type) && AcceptedEvents.Contains(stripeEvent.Type))
                {;
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                   
                    if (paymentIntent != null)
                    {
                        try
                        {
                            if (paymentIntent != null)
                            {
                                response.IntentId = paymentIntent.Id; 
                                response.Status = paymentIntent.Status;

                                var knackResponse = await GetKnackRecordsAsync(response.IntentId ?? "");

                                if (knackResponse != null)
                                {
                                    await UpdateRecordAsync(knackResponse?.Records.FirstOrDefault()?.Id ?? "", new { field_875 = response.Status });
                                }
                            }
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine("Error deserializing PaymentIntent: " + ex.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("The payment intent data is null or not a valid");
                    }
                }

                return Ok("Event triggered successfully");
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("knack/records")]
        public async Task<IActionResult> GetRecords(string recordId)
        {
            try
            {
                var records = await GetKnackRecordsAsync(recordId);

                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest("Something wring with knack");
            }
          
        }

        private async Task<KnackModel> GetKnackRecordsAsync(string paymentIntentId)
        {
            var apiUrl = "https://api.knack.com/v1/objects/object_39/records/";
            var filters = new
            {
                rules = new[] {
                    new {
                        field = "field_876",
                        Operator = "is",
                        value = paymentIntentId
                    }
                }
            };

            var urlWithFilters = $"{apiUrl}?filters={{\"match\":\"and\",\"rules\":[{{\"field\":\"field_900\",\"operator\":\"is\",\"value\":\"{paymentIntentId}\"}}]}}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-knack-application-id", "5f96fae162159800189f7ed2");
            _httpClient.DefaultRequestHeaders.Add("x-knack-rest-api-key", "6ebdd691-22cf-4d68-91c7-745ffe6be2db");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.GetAsync(urlWithFilters);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<KnackModel>(responseBody);
        }

        private async Task UpdateRecordAsync(string recordId, object data)
        {
            var apiUrl = $"https://api.knack.com/v1/objects/object_39/records/{recordId}";
            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-knack-application-id", "5f96fae162159800189f7ed2");
            _httpClient.DefaultRequestHeaders.Add("x-knack-rest-api-key", "6ebdd691-22cf-4d68-91c7-745ffe6be2db");
            _httpClient.DefaultRequestHeaders.Add("cache-control", "no-cache");

            var response = await _httpClient.PutAsync(apiUrl, content);
            response.EnsureSuccessStatusCode(); // Throws an exception if the status code is not 2xx

            var responseBody = await response.Content.ReadAsStringAsync();
            // Handle response if needed
            // Console.WriteLine(responseBody);
        }
    }
}
