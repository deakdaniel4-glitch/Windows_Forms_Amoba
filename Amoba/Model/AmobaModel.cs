using Elte.Amoba.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elte.Amoba.Model
{
    public class AmobaModel
    {

        #region Private fields

        private Player _currentPlayer;
        private Player[,] _gameTable = null!;
        private IPersistence _persistence = null!;
        private Int32 _gameTableSize = 10;
        private Int32 _stepNumber;
        private Int32 _playerXTime = 0; 
        private Int32 _playerOTime = 0;

        #endregion

        #region Properties

        public Int32 PlayerXTime { get { return _playerXTime; } }

        public Int32 PlayerOTime { get { return _playerOTime; } }
        public Int32 StepNumber { get { return _stepNumber; } }
        public int TableSize
        {
            get { return _gameTable.GetLength(0); }
            set { _gameTableSize = value; }
        }

        public Player CurrentPlayer { get { return _currentPlayer; } }

        public Player this[Int32 x, Int32 y]
        {
            get
            {
                if (x < 0 || x >= _gameTable.GetLength(0)) // ellenőrizzük a tartományt
                    throw new ArgumentException("Bad column index.", nameof(x));
                if (y < 0 || y >= _gameTable.GetLength(1))
                    throw new ArgumentException("Bad row index.", nameof(y));

                return _gameTable[x, y];
            }
        }

        #endregion

        #region Events

        public event EventHandler<GameOverEventArgs>? GameOver;


        public event EventHandler<FieldChangedEventArgs>? FieldChanged;


        public event EventHandler<GameTimeEventArgs>? GameTimeAdvanced;

        #endregion

        #region Constructors

        public AmobaModel(IPersistence persistence)
        {
            _persistence = persistence;

            NewGame();
        }

        #endregion

        #region Public methods

        public void NewGame()
        {
            _gameTable = new Player[this._gameTableSize, this._gameTableSize];
            for (Int32 i = 0; i < _gameTable.GetLength(0); i++) // végigmegyünk a mátrix elemein
                for (Int32 j = 0; j < _gameTable.GetLength(1); j++)
                {
                    _gameTable[i, j] = Player.NoPlayer; // a játékosok pozícióit töröljük
                }

            _currentPlayer = Player.PlayerX; // először az X lép

            _playerXTime = 0;
            _playerOTime = 0;
            _stepNumber = 0;
            OnGameTimeAdvanced();

        }

        public void AdvanceTime()
        {
            if (IsFilled())
            {
                return;
            }

            if (_currentPlayer == Player.PlayerX)
            {
                _playerXTime++;
            }
            else if (_currentPlayer == Player.PlayerO)
            {
                _playerOTime++;
            }
            OnGameTimeAdvanced();
        }

        public void StepGame(Int32 x, Int32 y)
        {
            if (x < 0 || x >= _gameTable.GetLength(0)) // ellenőrizzük a tartományt
                throw new ArgumentOutOfRangeException(nameof(x), "Bad column index.");
            if (y < 0 || y >= _gameTable.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "Bad row index.");
            if (_gameTable[0, y] != Player.NoPlayer)
                throw new InvalidOperationException("Column is full");
            if (_stepNumber >= _gameTable.GetLength(0) * _gameTable.GetLength(1)) // ellenőrizzük a lépésszámot
                 throw new InvalidOperationException("Game is over!");

            int i = _gameTable.GetLength(0) - 1;
            while (_gameTable[i, y] != Player.NoPlayer) i--;

            _gameTable[i, y] = _currentPlayer;
            OnFieldChanged(i, y, _currentPlayer);

            _stepNumber++;
            OnGameTimeAdvanced();

            if (_currentPlayer == Player.PlayerO)
            {
                _currentPlayer = Player.PlayerX;
            }
            else
            {
                _currentPlayer = Player.PlayerO;
            }

            CheckGame();
        }

        public bool IsColumnFilled(Int32 y)
        {
            return _gameTable[0, y] != Player.NoPlayer;
        }

        public async Task LoadGame(String path)
        {
            if (_persistence == null)
                throw new InvalidOperationException("No data access is provided.");

            // végrehajtjuk a betöltést
            (Player[,] players, int steps, int oTime, int xTime) = await _persistence.Load(path);

            _gameTable = players;

            if (_gameTable.Length == 0)
                throw new DataException("Error occurred during game loading.");

            _playerOTime = oTime;
            _playerXTime = xTime;
            _stepNumber = steps;
            _currentPlayer = _stepNumber % 2 == 0 ? Player.PlayerX : Player.PlayerO; // a lépésszámból kijön az aktuális játékos

            // beállítjuk az értékeket
            for (Int32 i = 0; i < _gameTable.GetLength(0); i++)
                for (Int32 j = 0; j < _gameTable.GetLength(1); j++)
                {
                    if (_gameTable[i, j] != Player.NoPlayer)
                    {
                        OnFieldChanged(i, j, _gameTable[i, j]);
                    }
                }

            CheckGame();
        }

        public async Task SaveGame(String path)
        {
            if (_persistence == null)
                throw new InvalidOperationException("No data access is provided.");

            // végrehajtjuk a mentést
            await _persistence.Save(path, (_gameTable, _stepNumber, _playerOTime, _playerXTime));
        }

        #endregion

        #region Private methods

        private void CheckGame()
        {
            Player won = Player.NoPlayer;
            List<(int x, int y)> winningCoords = [];

            for (int i = 0; i < _gameTable.GetLength(0); i++) 
                for (int j = 0; j < _gameTable.GetLength(1) - 3; j++)
                {
                    if (_gameTable[i, j] != Player.NoPlayer 
                        && _gameTable[i, j] == _gameTable[i, j + 1] 
                        && _gameTable[i, j] == _gameTable[i, j + 2] 
                        && _gameTable[i, j] == _gameTable[i, j + 3])
                    {
                        won = _gameTable[i, j];
                        winningCoords.Add((i, j));
                        winningCoords.Add((i, j + 1));
                        winningCoords.Add((i, j + 2));
                        winningCoords.Add((i, j + 3));
                    }
                }

            for (int i = 0; i < _gameTable.GetLength(0) - 3; ++i) // Jobbra-lefele átlók ellenőrzése
                for (int j = 0; j < _gameTable.GetLength(1) - 3; ++j)
                {
                    if (_gameTable[i, j] != Player.NoPlayer
                        && _gameTable[i, j] == _gameTable[i + 1, j + 1] 
                        && _gameTable[i, j] == _gameTable[i + 2, j + 2]
                        && _gameTable[i, j] == _gameTable[i + 3, j + 3])
                    {
                        won = _gameTable[i, j];
                        winningCoords.Add((i, j));
                        winningCoords.Add((i + 1, j + 1));
                        winningCoords.Add((i + 2, j + 2));
                        winningCoords.Add((i + 3, j + 3));
                    }
                }
            for (int i = 3; i < _gameTable.GetLength(0); ++i) // Jobbra-felfele átlók ellenőrzése
                for (int j = 0; j < _gameTable.GetLength(1) - 3; ++j)
                {
                    if (_gameTable[i, j] != Player.NoPlayer 
                        && _gameTable[i, j] == _gameTable[i - 1, j + 1] 
                        && _gameTable[i, j] == _gameTable[i - 2, j + 2]
                        && _gameTable[i, j] == _gameTable[i - 3, j + 3])
                    {
                        won = _gameTable[i, j];
                        winningCoords.Add((i, j));
                        winningCoords.Add((i - 1, j + 1));
                        winningCoords.Add((i - 2, j + 2));
                        winningCoords.Add((i - 3, j + 3));
                    }
                }

            if (won != Player.NoPlayer) // ha valaki győzött
            {
                OnGameOver(won, winningCoords); // esemény kiváltása
            }
            else if (_stepNumber == _gameTable.Length) // döntetlen játék
            {
                OnGameOver(Player.NoPlayer, []); // esemény kiváltása
            }

        }

        private bool IsFilled()
        {
            for (int i = 0; i < _gameTableSize; i++)
            {
                for (int j = 0; j < _gameTableSize; j++)
                {
                    if (_gameTable[i, j] == Player.NoPlayer)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        #endregion

        #region Event triggers


        private void OnGameOver(Player player, List<(int x, int y)> winningCoords)
        {
            GameOver?.Invoke(this, new GameOverEventArgs(player, winningCoords));
        }
       
        
        private void OnFieldChanged(Int32 x, Int32 y, Player player)
        {
            FieldChanged?.Invoke(this, new FieldChangedEventArgs(x, y, player));
        }

        private void OnGameTimeAdvanced()
        {
            GameTimeAdvanced?.Invoke(this, new GameTimeEventArgs(_playerXTime, _playerOTime));
        }

        #endregion
    }


}
