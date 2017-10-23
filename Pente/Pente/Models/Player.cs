using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pente.Models
{
	/// <summary>
	/// Simple Class to represent a person as a player during the pente game
	/// Only has a name, and a boolean representing if they control white or black pieces
	/// </summary>
	[Serializable]
	public class Player
    {
        public String Name { get; set; }
        public bool IsWhitePlayer { get; }


		/// <summary>
		/// Constructor to create a user player
		/// </summary>
		/// <param name="name"></param>
		/// <param name="isWhitePlayer"></param>
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
