using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pente.Models
{
    public class Player
    {
        public String name { get; set; }
        public bool isWhitePlayer { get; }

        public Player(String name, bool isWhitePlayer)
        {
            if (String.IsNullOrEmpty(name))
            {
                name = "[Unknown]";
            }
            this.name = name;
            this.isWhitePlayer = isWhitePlayer;
        }
    }
}
