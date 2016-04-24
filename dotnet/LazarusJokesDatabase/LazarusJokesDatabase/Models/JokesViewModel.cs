using System.Collections.Generic;

namespace LazarusJokesDatabase.Models
{
    public class JokesViewModel
    {
        public Joke NewJoke { get; set; }

        public IEnumerable<Joke> Jokes { get; set; }
    }
}