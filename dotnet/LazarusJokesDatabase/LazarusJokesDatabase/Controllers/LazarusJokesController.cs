﻿using LazarusJokesDatabase.Models;
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
                model.NewJoke.Id = jokes.Max(joke => joke.Id) + 1;
                jokes.Add(model.NewJoke);
                SaveJokes(jokes);
            }

            return RedirectToAction("Jokes");
        }

        public ActionResult VoteForJoke(long jokeId, int userVote)
        {
            var user = Request.LogonUserIdentity.Name;

            var jokes = LoadJokes();

            var actualJoke = jokes.Single(joke => joke.Id == jokeId);
            var givenUserVote = actualJoke.UserVotes.SingleOrDefault(vote => vote.UserName == user);
            if (givenUserVote == null)
            {
                givenUserVote = new UserVote { UserName = user };
                actualJoke.UserVotes.Add(givenUserVote);
            }
            givenUserVote.Vote = userVote;

            SaveJokes(jokes);

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

                var user = Request.LogonUserIdentity.Name;
                jokes.ForEach(joke => joke.VotesOfCurrentUser = joke.UserVotes.Where(vote => vote.UserName == user).ToList());

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
