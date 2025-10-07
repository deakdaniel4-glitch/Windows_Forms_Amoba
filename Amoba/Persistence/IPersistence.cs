using Elte.Amoba.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elte.Amoba.Persistence
{
    public interface IPersistence
    {
        Task<(Player[,] table, int steps, int oTime, int xTime)> Load(String path);

        Task Save(String path, (Player[,] table, int steps, int oTime, int xTime) game);
    }
}
