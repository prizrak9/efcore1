using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using efcore1.Models;
using Microsoft.EntityFrameworkCore;

namespace efcore1.Controllers
{
    public class DbController : Controller
    {
        providerContext context;

        public DbController(providerContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return PrintTable();

            // var bills = context.Bills;

            // var fields = typeof(Bills).GetProperties();

            // var model = new TableProvider{
            //     EntityType = typeof(Bills),
            //     FieldNames = fields.Select(n => n.Name),
            //     Values = bills.Select(n => fields.ToDictionary(m => m.Name, m => m.GetValue(n))),
            //     AvailableTables = context.GetType()
            //         .GetProperties()
            //         .Where(n => n.PropertyType.IsGenericType)
            //         .Select(n => n.Name)
            //         .Distinct(),
            // };


            // return View(model);
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
        
        public ActionResult ShowTable(string __entityType)
        {
            var type = Type.GetType(__entityType);

            return PrintTable(type);
        }

        public ActionResult PrintTable(Type type = null)
        {
            if (type is null)
            {
                type = context
                    .GetType()
                    .GetProperties()
                    .Where(n => n.PropertyType.IsGenericType)
                    .First()
                    .PropertyType
                    .GetGenericArguments()[0];
            }

            var entities = (IEnumerable<object>)context
                    .GetType()
                    .GetProperties()
                    .Where(n => n.PropertyType.IsGenericType)
                    .First(n => n.PropertyType.GetGenericArguments()[0] == type)
                    .GetGetMethod().Invoke(context, null);

            var fields = type.GetProperties()
                .Where(n => (n.PropertyType.IsValueType || n.PropertyType == typeof(string)));

            var model = new TableProvider{
                EntityType = type,
                FieldNames = fields.Select(n => n.Name),
                Values = entities.Select(n => fields.ToDictionary(m => m.Name, m => m.GetValue(n))),
                AvailableTables = context.GetType()
                    .GetProperties()
                    .Where(n => n.PropertyType.IsGenericType)
                    .Select(n => n.PropertyType.GetGenericArguments()[0])
                    .Distinct(),
            };

            return View("Index", model);
        }
    }
}
