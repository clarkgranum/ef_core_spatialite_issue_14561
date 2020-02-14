using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpatialiteCrash.Database;
using SpatialiteCrash.Web.Models;

namespace SpatialiteCrash.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("query-database")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult QueryDatabase()
        {
            using (var db = new SpatialiteCrashDbContext())
            {
                var orderCount = db.Orders.Count();

                var randomOrderNumber = new Random().Next(0, orderCount);
                var numToTake = new Random().Next(0, 10);

                var orders = db.Orders
                    .Skip(randomOrderNumber)
                    .Take(numToTake)
                    .ToList(); //intentional materialize

                //intentional lazy load of order items
                var orderItems = orders
                    .SelectMany(o => o.OrderItems)
                    .ToList(); //intentional materialize

                //just transforming so json doesnt try to lazy load nav props
                var ordersTransformed = orders.Select(o => new
                {
                    o.Id,
                    o.OrderNumber,
                    o.OrderDate
                })
                .ToList();

                var orderItemsTransformed = orderItems.Select(i => new
                {
                    i.Id,
                    i.Name,
                    i.OrderId
                })
                .ToList();

                return Json(new
                {
                    Orders = ordersTransformed,
                    orderItems = orderItemsTransformed
                });
            }
        }
    }
}