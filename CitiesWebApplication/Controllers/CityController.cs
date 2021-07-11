using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitiesWebApplication.Hubs;

namespace CitiesWebApplication.Controllers
{
    public class CityController : Controller
    {
        private IHubContext<CityHub> _hubContext;
        private ApiContext _context;

        public CityController(ApiContext context, IHubContext<CityHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var cities = _context.Cities.ToList();
            return View(cities);
        }

        [HttpPost]
        public IActionResult Add(Models.City city)
        {
            var newID = _context.Cities.Select(x => x.Id).Max() + 1;
            city.Id = newID;

            _context.Cities.Add(city);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var city = _context.Cities.Find(id);
            if (city != null)
            {
                ViewData["city"] = city;
                return View();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Models.City city)
        {
            if (CheckCity(city) && _context.Cities.Any(x => x.Id == city.Id))
            {
                city.Name = MakeFirstLetterCapital(city.Name);
                _context.Cities.Update(city);
                _context.SaveChanges();
                await _hubContext.Clients.All.SendAsync("CityEdited", city.Id, city.Name, city.Date.Date.ToString("dd.MM.yyyy"), city.Population);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var city = _context.Cities.Find(id);
            if (city != null)
            {
                _context.Cities.Remove(city);
                _context.SaveChanges();
                await _hubContext.Clients.All.SendAsync("CityDeleted", id);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var city = new Models.City();
            ViewData["city"] = city;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.City city)
        {
            if (CheckCity(city))
            {
                var newID = _context.Cities.Any() ? _context.Cities.Select(x => x.Id).Max() + 1 : 1;
                city.Id = newID;
                city.Name = MakeFirstLetterCapital(city.Name);
                _context.Cities.Add(city);
                _context.SaveChanges();
                await _hubContext.Clients.All.SendAsync("CityCreated", city.Id, city.Name, city.Date.Date.ToString("dd.MM.yyyy"), city.Population);
            }
            return RedirectToAction("Index");
        }

        private bool CheckCity(Models.City city)
        {
            if (city == null ||
                string.IsNullOrEmpty(city.Name) ||
                city.Name.Length > 3000 ||
                !Regex.IsMatch(city.Name, @"^[A-Za-zА-Яа-я][a-zA-ZА-Яа-я'\s-]*$") ||
                city.Population < 0 ||
                city.Population > 7000000000 ||
                city.Date.Date > System.DateTime.Now.Date)
                return false;
            return true;
        }

        private string MakeFirstLetterCapital(string name)
        {
            return char.ToUpper(name[0]) + name[1..];
        }
    }
}
