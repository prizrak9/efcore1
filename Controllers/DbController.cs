using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using efcore1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

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

        [HttpPost]
        
        // [ValidateAntiForgeryToken]
        public ActionResult Update(int __index, string __entityType, IFormCollection collection)
        {
            try
            {
                Console.WriteLine("A");
                var type = Type.GetType(__entityType);

                var entities = SelectAll(type);

                var entity = entities.ElementAt(__index);

                var excludedNames = new[] { "__RequestVerificationToken", "__entityType", "__index" };

                var values = collection
                    .Where(n => !string.IsNullOrWhiteSpace(n.Value.FirstOrDefault()))
                    .Where(n => !excludedNames.Contains(n.Key))
                    .ToDictionary(n => n.Key, n => (object)n.Value.FirstOrDefault());

                var fields = type.GetProperties()
                    .Where(n => (n.PropertyType.IsValueType || n.PropertyType == typeof(string)));

                foreach (var field in fields)
                {
                    var validTypeValue = Convert.ChangeType(values[field.Name], field.PropertyType);

                    field.SetValue(entity, validTypeValue);
                }

                context.Update(entity);

                context.SaveChanges();

                // return RedirectToAction("Index");
                return ShowTable(__entityType);
            }
            catch(Exception e)
            {
                ViewData["Message"] = e.Message;

                return ShowTable(__entityType);
            }
        }

        [HttpPost]
        public ActionResult Delete(int __index, string __entityType)
        {
            try
            {
                var type = Type.GetType(__entityType);

                var entities = SelectAll(type);

                var entity = entities.ElementAt(__index);

                context.Remove(entity);

                context.SaveChanges();

                return ShowTable(__entityType);
            }
            catch(Exception e)
            {
                ViewData["Message"] = e.Message;

                return ShowTable(__entityType);
            }
        }
        private IEnumerable<object> SelectAll(Type type)
        {
            return (IEnumerable<object>) context.GetType().GetProperty(type.Name).GetValue(context);
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
