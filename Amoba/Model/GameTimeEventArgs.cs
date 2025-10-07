using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elte.Amoba.Model
{
    public class GameTimeEventArgs : EventArgs
    {
        public int PlayerXTime { get; } // PlayerX gondolkodási ideje
        public int PlayerOTime { get; } // PlayerO gondolkodási ideje

        public GameTimeEventArgs(int playerXTime, int playerOTime)
        {
            PlayerXTime = playerXTime;
            PlayerOTime = playerOTime;
        }
    }
}
