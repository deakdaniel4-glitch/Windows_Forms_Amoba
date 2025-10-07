using Elte.Amoba.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elte.Amoba.Persistence
{
    public class FilePersistence : IPersistence
    {
        public async Task<(Player[,] table, int steps, int oTime, int xTime)> Load(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String line = await reader.ReadLineAsync() ?? String.Empty;
                    String[] numbers = line.Split(' ');
                    Int32 oTime = Int32.Parse(numbers[0]);
                    Int32 xTime = Int32.Parse(numbers[1]);
                    Int32 steps = Int32.Parse(numbers[2]);
                    Int32 tableSize = Int32.Parse(numbers[3]);
                    Player[,] table = new Player[tableSize, tableSize];

                    for (Int32 i = 0; i < tableSize; i++)
                    {
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        numbers = line.Split(' ');

                        for (Int32 j = 0; j < tableSize; j++)
                        {
                            table[i, j] = FromInt(Int32.Parse(numbers[j]));
                        }
                    }

                    return (table, steps, oTime, xTime);
                }
            }
            catch
            {
                throw new AmobaDataException();
            }
        }

        public async Task Save(string path, (Player[,] table, int steps, int oTime, int xTime) game)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    Int32 length = game.table.GetLength(0);
                    await writer.WriteLineAsync($"{game.oTime} {game.xTime} {game.steps} {game.table.GetLength(0)}");

                    for (Int32 i = 0; i < length; i++)
                    {
                        for (Int32 j = 0; j < length; j++)
                        {
                            await writer.WriteAsync(ToInt(game.table[i, j]) + " ");
                        }

                        await writer.WriteLineAsync();
                    }
                }
            }
            catch
            {
                throw new AmobaDataException();
            }
        }

        private Player FromInt(int i)
        {
            if (i == 1)
            {
                return Player.PlayerX;
            }
            else if (i == 2)
            {
                return Player.PlayerO;
            }
            else
            {
                return Player.NoPlayer;
            }
        }

        private int ToInt(Player player)
        {
            if (player == Player.PlayerX)
            {
                return 1;
            }
            else if (player == Player.PlayerO)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
    }
}
