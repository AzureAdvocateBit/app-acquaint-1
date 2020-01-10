using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyContacts.Interfaces;
using MyContacts.Shared.Models;
using MyContacts.Util;
using Xamarin.Essentials;

namespace MyContacts.Services
{
    public class AzureDataStore : IDataSource<Contact>
    {
        HttpClient client;
        IEnumerable<Contact> contacts;

        public static string BackendUrl = "https://mycontactsapi20200107080452.azurewebsites.net";

        //public static string BackendUrl = 
        //    DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";


        public AzureDataStore()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"{BackendUrl}/");

            contacts = new List<Contact>();
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
        public async Task<IEnumerable<Contact>> GetItems()
        {
            if (IsConnected)
            {
                var json = await client.GetStringAsync($"api/Contacts");
                contacts = await Task.Run(() => JsonSerializer.Deserialize<IEnumerable<Contact>>(json));
            }


            Settings.LastUpdate = DateTime.UtcNow;

            return contacts;
        }

        public async Task<Contact> GetItem(string id)
        {
            if (id != null && IsConnected)
            {
                var json = await client.GetStringAsync($"api/Contacts/{id}");
                return await Task.Run(() => JsonSerializer.Deserialize<Contact>(json));
            }


            Settings.LastUpdate = DateTime.UtcNow;

            return null;
        }

        public async Task<bool> AddItem(Contact Contact)
        {
            if (Contact == null || !IsConnected)
                return false;

            var serializedContact = JsonSerializer.Serialize(Contact);

            var response = await client.PostAsync($"api/Contacts", new StringContent(serializedContact, Encoding.UTF8, "application/json"));


            Settings.LastUpdate = DateTime.UtcNow;

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateItem(Contact Contact)
        {
            if (Contact == null || Contact.Id == null || !IsConnected)
                return false;

            var serializedContact = JsonSerializer.Serialize(Contact);
            var buffer = Encoding.UTF8.GetBytes(serializedContact);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(new Uri($"api/Contacts/{Contact.Id}"), byteContent);


            Settings.LastUpdate = DateTime.UtcNow;

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveItem(Contact contact)
        {
            var id = contact?.Id;

            if (string.IsNullOrEmpty(id) && !IsConnected)
                return false;

            var response = await client.DeleteAsync($"api/Contacts/{id}");


            Settings.LastUpdate = DateTime.UtcNow;

            return response.IsSuccessStatusCode;
        }
    }
}
