using System.Collections.Generic;
using System.Threading.Tasks;
using LCPECommerce.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace LCPECommerce.Client.Services
{
    public interface IAccountService
    {
        Clients Client { get; }
        Task Initialize();
        Task Login(Clients model);
        Task Logout();
        Task Register(Clients model);
        Task<IList<Clients>> GetAll();
        Task<Clients> GetById(int id);
        Task Update(int id, Clients model);
        Task Delete(int id);
    }

    public class AccountService : IAccountService
    {
        private IHttpService _httpService;
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;
        private string _ClientKey = "Client";

        public Clients Client { get; private set; }

        public AccountService(
            IHttpService httpService,
            NavigationManager navigationManager,
            ILocalStorageService localStorageService
        )
        {
            _httpService = httpService;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
        }

        public async Task Initialize()
        {
            Client = await _localStorageService.GetItem<Clients>(_ClientKey);
        }

        public async Task Login(Clients model)
        {
            Client = await _httpService.Post<Clients>("/api/clients/authenticate", model);
            await _localStorageService.SetItem(_ClientKey, Client);
        }

        public async Task Logout()
        {
            Client = null;
            await _localStorageService.RemoveItem(_ClientKey);
            _navigationManager.NavigateTo("account/login");
        }

        public async Task Register(Clients model)
        {
            await _httpService.Post("/api/clients/register", model);
        }

        public async Task<IList<Clients>> GetAll()
        {
            return await _httpService.Get<IList<Clients>>("/api/clients");
        }

        public async Task<Clients> GetById(int id)
        {
            return await _httpService.Get<Clients>($"/api/clients/{id}");
        }

        public async Task Update(int id, Clients model)
        {
            await _httpService.Put($"/api/clients/{id}", model);

            // update stored Client if the logged in Client updated their own record
            if (id == Client.Id)
            {
                // update local storage
                Client.Username = model.Username;
                Client.Email = model.Email;
                Client.Password = model.Password;
                Client.FullName = model.FullName;
                Client.BillingAddress = model.BillingAddress;
                Client.ShippingAddress = model.ShippingAddress;
                Client.Country = model.Country;
                Client.Phone = model.Phone;
                await _localStorageService.SetItem(_ClientKey, Client);
            }
        }

        public async Task Delete(int id)
        {
            await _httpService.Delete($"/api/clients/{id}");

            // auto logout if the logged in Client deleted their own record
            if (id == Client.Id)
                await Logout();
        }
    }
}
