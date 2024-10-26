using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessNumber.Model
{
    public class Player
    {
        public string Name { get; set; }
        public int Attempt { get; set; }

        public Player(string name)
        {
            Name = name;
            Attempt = 0;
        }
        
    }
}
