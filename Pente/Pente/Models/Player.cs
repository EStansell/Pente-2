using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pente.Models
{
	[Serializable]
	public class Player
    {
        public String Name { get; set; }
        public bool IsWhitePlayer { get; }

        public Player(String name, bool isWhitePlayer)
        {
            if (String.IsNullOrEmpty(name))
            {
                name = "[Unknown]";
            }
            Name = name;
            IsWhitePlayer = isWhitePlayer;
        }
    }
}
