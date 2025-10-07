using Elte.Amoba.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elte.Amoba.Model
{
    public class GameOverEventArgs : EventArgs
    {
       
        public Player Player { get; private set; }
        public List<(int x, int y)> WinningCoords { get; private set; }

        public GameOverEventArgs(Player player, List<(int x, int y)> winningCoords)
        {
            Player = player;
            WinningCoords = winningCoords;
        }
    }
}
