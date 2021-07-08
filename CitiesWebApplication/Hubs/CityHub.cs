using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CitiesWebApplication.Hubs
{
    public class CityHub : Hub
    {
        public async Task CityDelete(string id)
        {
            await Clients.All.SendAsync("CityDeleted", id);
        }
    }
}
