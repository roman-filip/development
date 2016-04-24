using LazarusJokesDatabase.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace LazarusJokesDatabase.Controllers
{
    public class LazarusJokesController : Controller
    {
        public ActionResult Jokes(JokesViewModel model)
        {
            ViewBag.Message = "All jokes";

            model.NewJoke = new Joke
            {
                Author = Request.LogonUserIdentity.Name,
                Date = DateTime.Now.Date
            };
            model.Jokes = LoadJokes();

            return View(model);
        }

        // POST: LazarusJokes/AddJoke
        [HttpPost]
        public ActionResult AddJoke(JokesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var jokes = LoadJokes();
                jokes.Add(model.NewJoke);
                SaveJokes(jokes);
            }

            return RedirectToAction("Jokes");
        }

        private List<Joke> LoadJokes()
        {
            if (!System.IO.File.Exists(GetFilePath()))
            {
                return new List<Joke>();
            }

            var serializer = new XmlSerializer(typeof(List<Joke>));
            using (Stream reader = new FileStream(GetFilePath(), FileMode.Open))
            {
                var jokes = (List<Joke>)serializer.Deserialize(reader);
                return jokes;
            }
        }

        private void SaveJokes(IEnumerable<Joke> jokes)
        {
            var serializer = new XmlSerializer(jokes.GetType());
            using (TextWriter streamWriter = new StreamWriter(GetFilePath()))
            {
                serializer.Serialize(streamWriter, jokes);
            }
        }

        private string GetFilePath()
        {
            return Server.MapPath("~/App_Data/jokes.xml");
        }
    }
}
