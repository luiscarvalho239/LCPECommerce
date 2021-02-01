using LCPECommerce.Client.Services;
using LCPECommerce.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LCPECommerce.Client.Helpers
{
    public class FakeBackendHandler : HttpClientHandler
    {
        private ILocalStorageService _localStorageService;

        public FakeBackendHandler(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // array in local storage for registered Clients
            var ClientsKey = "ClientsXPTO";
            var Clients = await _localStorageService.GetItem<List<ClientRecord>>(ClientsKey) ?? new List<ClientRecord>();
            var method = request.Method;
            var path = request.RequestUri.AbsolutePath;

            return await handleRoute();

            async Task<HttpResponseMessage> handleRoute()
            {
                if (path == "/api/clients/authenticate" && method == HttpMethod.Post)
                    return await authenticate();
                if (path == "/api/clients/register" && method == HttpMethod.Post)
                    return await register();
                if (path == "/api/clients" && method == HttpMethod.Get)
                    return await getClients();
                if (Regex.Match(path, @"\/api/clients\/\d+$").Success && method == HttpMethod.Get)
                    return await getClientById();
                if (Regex.Match(path, @"\/api/clients\/\d+$").Success && method == HttpMethod.Put)
                    return await updateClient();
                if (Regex.Match(path, @"\/api/clients\/\d+$").Success && method == HttpMethod.Delete)
                    return await deleteClient();

                // pass through any requests not handled above
                return await base.SendAsync(request, cancellationToken);
            }

            // route functions

            async Task<HttpResponseMessage> authenticate()
            {
                var bodyJson = await request.Content.ReadAsStringAsync();
                var body = JsonSerializer.Deserialize<Clients>(bodyJson);
                var Client = Clients.FirstOrDefault(x => x.Email == body.Email && x.Password == body.Password);

                if (Client == null)
                    return await error("Email or password is incorrect");

                return await ok(new
                {
                    Id = Client.Id.ToString(),
                    Username = Client.Username,
                    Email = Client.Email,
                    FullName = Client.FullName,
                    BillingAddress = Client.BillingAddress,
                    ShippingAddress = Client.ShippingAddress,
                    Country = Client.Country,
                    Phone = Client.Phone,
                    Token = "fake-jwt-token"
                });
            }

            async Task<HttpResponseMessage> register()
            {
                var bodyJson = await request.Content.ReadAsStringAsync();
                var body = JsonSerializer.Deserialize<Clients>(bodyJson);

                if (Clients.Any(x => x.Email == body.Email))
                    return await error($"Client email '{body.Email}' is already taken");

                var Client = new ClientRecord
                {
                    Id = (Clients.Count() > 0) ? Clients.Max(x => x.Id) + 1 : 1,
                    Username = body.Username,
                    Email = body.Email,
                    Password = body.Password,
                    FullName = body.FullName,
                    BillingAddress = body.BillingAddress,
                    ShippingAddress = body.ShippingAddress,
                    Country = body.Country,
                    Phone = body.Phone
                };

                Clients.Add(Client);

                await _localStorageService.SetItem(ClientsKey, Clients);

                return await ok();
            }

            async Task<HttpResponseMessage> getClients()
            {
                if (!isLoggedIn()) return await unauthorized();
                return await ok(Clients.Select(x => basicDetails(x)));
            }

            async Task<HttpResponseMessage> getClientById()
            {
                if (!isLoggedIn()) return await unauthorized();

                var Client = Clients.FirstOrDefault(x => x.Id == idFromPath());
                return await ok(basicDetails(Client));
            }

            async Task<HttpResponseMessage> updateClient()
            {
                if (!isLoggedIn()) return await unauthorized();

                var bodyJson = await request.Content.ReadAsStringAsync();
                var body = JsonSerializer.Deserialize<Clients>(bodyJson);
                var Client = Clients.FirstOrDefault(x => x.Id == idFromPath());

                // if Clientname changed check it isn't already taken
                if (Client.Email != body.Email && Clients.Any(x => x.Email == body.Email))
                    return await error($"Client email '{body.Email}' is already taken");

                // only update password if entered
                if (!string.IsNullOrWhiteSpace(body.Password))
                    Client.Password = body.Password;

                // update and save Client
                Client.Email = body.Email;
                Client.Username = body.Username;
                Client.FullName = body.FullName;
                Client.BillingAddress = body.BillingAddress;
                Client.ShippingAddress = body.ShippingAddress;
                Client.Country = body.Country;
                Client.Phone = body.Phone;
                await _localStorageService.SetItem(ClientsKey, Clients);

                return await ok();
            }

            async Task<HttpResponseMessage> deleteClient()
            {
                if (!isLoggedIn()) return await unauthorized();

                Clients.RemoveAll(x => x.Id == idFromPath());
                await _localStorageService.SetItem(ClientsKey, Clients);

                return await ok();
            }

            // helper functions

            async Task<HttpResponseMessage> ok(object body = null)
            {
                return await jsonResponse(HttpStatusCode.OK, body ?? new { });
            }

            async Task<HttpResponseMessage> error(string message)
            {
                return await jsonResponse(HttpStatusCode.BadRequest, new { message });
            }

            async Task<HttpResponseMessage> unauthorized()
            {
                return await jsonResponse(HttpStatusCode.Unauthorized, new { message = "Unauthorized" });
            }

            async Task<HttpResponseMessage> jsonResponse(HttpStatusCode statusCode, object content)
            {
                var response = new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json")
                };

                // delay to simulate real api call
                await Task.Delay(500);

                return response;
            }

            bool isLoggedIn()
            {
                return request.Headers.Authorization?.Parameter == "fake-jwt-token";
            }

            int idFromPath()
            {
                return int.Parse(path.Split('/').Last());
            }

            dynamic basicDetails(ClientRecord Client)
            {
                return new
                {
                    Id = Client.Id.ToString(),
                    Username = Client.Username,
                    Email = Client.Email,
                    Password = Client.Password,
                    FullName = Client.FullName,
                    BillingAddress = Client.BillingAddress,
                    ShippingAddress = Client.ShippingAddress,
                    Country = Client.Country,
                    Phone = Client.Phone
                };
            }
        }
    }

    // class for Client records stored by fake backend

    public class ClientRecord
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
    }
}
