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
    public class WebhookController : Controller
    {
        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        //whsec_ab5571878fe57d0b4ce2a08c311fd12db77c26cc3f9ee1a9bdea1719f51ea5c4

        //This one is for live
        const string endpointSecret = "whsec_cOlQb0vwh869AMDmbJ8gBvD3i2I9eZAB";
        

        List<string> AcceptedEvents =
        [
            StripeEvents.ChargeCaptured,
            //StripeEvents.SourceChargeable,
            StripeEvents.ChargeFailed,
            StripeEvents.ChargePending,
            StripeEvents.ChargeRefunded,
            StripeEvents.ChargeSucceeded,
            StripeEvents.ChargeUpdated,
            StripeEvents.ChargeDisputeClosed,
            StripeEvents.ChargeDisputeCreated,
            StripeEvents.ChargeDisputeFundsReinstated,
            StripeEvents.ChargeDisputeFundsWithdrawn,
            StripeEvents.ChargeDisputeUpdated,
            StripeEvents.ChargeRefundUpdated
        ];

        private readonly HttpClient _httpClient;

        public WebhookController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                ChargeResponse response = new();

                var stripeEvent = StripeEventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                if (!string.IsNullOrWhiteSpace(stripeEvent.Type) && AcceptedEvents.Contains(stripeEvent.Type))
                {
                    var chargeData = JsonConvert.DeserializeObject<StripeChargeCustom>(stripeEvent.Data.Object.ToString());

                    if (chargeData != null)
                    {
                        response.ChargeId = chargeData?.Id;
                        //response.ChargeId = "py_1Pt2yeHxOzOtBmAaVDjREuav";
                        response.ChargeStatus = stripeEvent.Type;
                    }

                    var knackResponse = await GetKnackRecordsAsync(response.ChargeId ?? "");

                    if (knackResponse != null)
                    {
                        await UpdateRecordAsync(knackResponse?.Records.FirstOrDefault()?.Id ?? "", new { field_875 = response.ChargeStatus });
                    }
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        [HttpGet("knack/records")]
        public async Task<IActionResult> GetRecords(string recordId)
        {
            var records = await GetKnackRecordsAsync(recordId);

            return Ok(records);
        }

        private async Task<KnackModel> GetKnackRecordsAsync(string chargeId)
        {
            var apiUrl = "https://api.knack.com/v1/objects/object_39/records/";
            var filters = new
            {
                rules = new[] {
                    new {
                        field = "field_876",
                        Operator = "is",
                        value = chargeId
                    }
                }
            };

            var urlWithFilters = $"{apiUrl}?filters={{\"match\":\"and\",\"rules\":[{{\"field\":\"field_876\",\"operator\":\"is\",\"value\":\"{chargeId}\"}}]}}";

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
