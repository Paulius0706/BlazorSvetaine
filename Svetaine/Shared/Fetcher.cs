using Svetaine.Shared.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Svetaine.Shared
{
    public class Fetcher
    {
        public Fetcher() { }
        
        public async Task<string> PutRequest<EntityT>(HttpClient Http, EntityT entity) where EntityT : class
        {
            if (entity == null)
            {
                return "Enitity somehow is null";
            }
            HttpResponseMessage response = await Http.PutAsJsonAsync<EntityT>("api/" + typeof(EntityT).Name +"/" + typeof(EntityT).GetProperty("Id").GetValue(entity), entity);
            if (!response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            bool success = await response.Content.ReadFromJsonAsync<bool>();
            if (!success)
            {
                return "Enitity was not updated";
            }
            return "";
        }
        public async Task<(string,int)> PostRequest<EntityT>(HttpClient Http, EntityT entity) where EntityT : class
        {
            if (entity == null)
            {
                return ("Entity somehow is null",-1);
            }
            HttpResponseMessage response = await Http.PostAsJsonAsync<EntityT>("api/" + typeof(EntityT).Name, entity);
            if (!response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadAsStringAsync(),-1);
            }
            int Id = await response.Content.ReadFromJsonAsync<int>();
            if (Id == -1)
            {
                return ("Entity was not added",Id);
            }
            return ("",Id);
        }
        public async Task<(string, int)> PostRequest<EntityT1, EntityT2>(HttpClient Http, EntityT1 entity1, EntityT2 entity2) where EntityT1 : class where EntityT2 : class
        {
            if (entity1 == null)
            {
                return ("Entity somehow is null", -1);
            }
            typeof(EntityT2).GetProperty(typeof(EntityT1).Name).SetValue(entity2, entity1);
            HttpResponseMessage response = await Http.PostAsJsonAsync<EntityT2>(
                "api/" + typeof(EntityT1).Name +"/"+ typeof(EntityT1).GetProperty("Id").GetValue(entity1) + "/" + typeof(EntityT2).Name, 
                entity2, 
                new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.General)
            );
            if (!response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadAsStringAsync(), -1);
            }
            int Id = await response.Content.ReadFromJsonAsync<int>();
            if (Id == -1)
            {
                return ("Entity was not added", Id);
            }
            return ("", Id);
        }
        public async Task<string> DeleteRequest<EntityT>(HttpClient Http, EntityT entity) where EntityT : class
        {

            if (entity == null)
            {
                return "Entity somehow is null";
            }
            HttpResponseMessage response = await Http.DeleteAsync("api/User/" + typeof(EntityT).GetProperty("Id").GetValue(entity));
            if (!response.IsSuccessStatusCode)
            {
                return "Entity was not found";
            }
            return "";
        }
    }
}
