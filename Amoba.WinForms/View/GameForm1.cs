using Elte.Amoba.Model;
using Elte.Amoba.Persistence;

namespace Elte.Amoba.View
{
    public partial class GameForm1 : Form
    {
        #region Fields

        private AmobaModel _model;
        private Button[,] _buttonGrid = null!;
        private System.Windows.Forms.Timer _timer;

        #endregion

        #region Constructors

        public GameForm1()
        {
            InitializeComponent();

            _game10x10button.Click += Game10x10_Click;
            _game20x20Button.Click += Game20x20_Click;
            _game30x30Button.Click += Game30x30_Click;

            _pauseButton.Click += Pause_Click;
            _menuFileLoadGame.Click += MenuFileLoadGame_Click;
            _menuFileSaveGame.Click += MenuFileSaveGame_Click;



            IPersistence _persistence = new FilePersistence();
            _model = new AmobaModel(_persistence);
            _model.FieldChanged += Game_FieldChanged;
            _model.GameOver += Game_GameOver;
            _model.GameTimeAdvanced += Game_GameTimeAdvanced;

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
            _timer.Start();

            GenerateTable();
        }

        #endregion

        #region Private Methods

        private void GenerateTable()
        {
            if (_buttonGrid != null)
            {
                foreach (var f in _buttonGrid)
                {
                    field.Controls.Remove(f);
                }
            }


            _buttonGrid = new Button[_model.TableSize, _model.TableSize];
            for (Int32 i = 0; i < _model.TableSize; i++)
            {
                for (Int32 j = 0; j < _model.TableSize; j++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(3 + 30 * j, 30 * i); // elhelyezkedés
                    _buttonGrid[i, j].Size = new Size(30, 30); // méret
                    _buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold); // betűtípus
                    _buttonGrid[i, j].Enabled = true; // kikapcsolt állapot
                    _buttonGrid[i, j].TabIndex = 100 + i * _model.TableSize + j; // index eltárolása
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    _buttonGrid[i, j].MouseClick += ButtonGrid_MouseClick; // közös eseménykezelő hozzárendelése minden gombhoz
                    _buttonGrid[i, j].TextAlign = ContentAlignment.MiddleCenter; // szöveg igazítása

                    Player player = _model[i, j];

                    if (player == Player.PlayerX)
                    {
                        _buttonGrid[i, j].Text = "X";
                    }
                    else if (player == Player.PlayerO)
                    {
                        _buttonGrid[i, j].Text = "O";
                    }

                    field.Controls.Add(_buttonGrid[i, j]);

                }
            }


        }

        private void ResetGame()
        {
            _model.NewGame();
            GenerateTable();
            _timer.Start();
            _actualPlayer.Text = "X";
        }

        #endregion

        #region Timer Event Handlers

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _model.AdvanceTime();
        }

        #endregion
       
        #region Grid event Handlers

        private void ButtonGrid_MouseClick(object? sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                // a TabIndex-ből megkapjuk a sort és oszlopot
                Int32 x = (button.TabIndex - 100) / _model.TableSize;
                Int32 y = (button.TabIndex - 100) % _model.TableSize;

                _model.StepGame(x, y); // lépés a játékban

                if (_model.IsColumnFilled(y)) // oszlop lezárása
                {
                    for (int cX = 0; cX < _model.TableSize; cX++)
                    {
                        _buttonGrid[cX, y].Enabled = false;
                    }
                }
            }
        }

        #endregion

        #region Game Event Handlers

        private void Game_FieldChanged(object? sender, FieldChangedEventArgs e)
        {
            if (e.Player == Player.PlayerX)
            {
                _buttonGrid[e.X, e.Y].Text = "X";
                _actualPlayer.Text = "O";
            }
            else if (e.Player == Player.PlayerO)
            {
                _buttonGrid[e.X, e.Y].Text = "O";
                _actualPlayer.Text = "X";
            }
        }

        private void Game_GameOver(object? sender, GameOverEventArgs e)
        {

            _timer.Stop();
            List<(int x, int y)> coords = e.WinningCoords;

            foreach ((int x, int y) in coords)
            {
                _buttonGrid[x, y].BackColor = Color.Chartreuse;
            }

            if (e.Player == Player.PlayerX)
            {
                MessageBox.Show("Nyert: x játékos", "Játék vége", MessageBoxButtons.OK);
            }

            else if (e.Player == Player.PlayerO)
            {
                MessageBox.Show("Nyert: O játékos", "Játék vége", MessageBoxButtons.OK);
            }

            else
            {
                MessageBox.Show("Nem nyert senki! ", "Játék vége", MessageBoxButtons.OK);
            }

            ResetGame();
        }


        private void Game_GameTimeAdvanced(object? sender, GameTimeEventArgs e)
        {
            _xTime.Text = TimeSpan.FromSeconds(e.PlayerXTime).ToString("g");
            _OTime.Text = TimeSpan.FromSeconds(e.PlayerOTime).ToString("g");
        }

        #endregion

        #region Menu event Handlers

        private async void MenuFileLoadGame_Click(object? sender, EventArgs e)
        {
            Boolean restartTimer = _timer.Enabled;
            _timer.Stop();

            if (_openFileDialog.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék betöltése
                    await _model.LoadGame(_openFileDialog.FileName);
                    _menuFileSaveGame.Enabled = true;
                }
                catch (AmobaDataException)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    _model.NewGame();
                    _menuFileSaveGame.Enabled = true;
                }

                if (_model.CurrentPlayer == Player.PlayerX)
                {                  
                    _actualPlayer.Text = "X";
                }
                else if (_model.CurrentPlayer == Player.PlayerO)
                {                   
                    _actualPlayer.Text = "O";
                }
                GenerateTable();
            }

            if (restartTimer)
                _timer.Start();
        }

        private async void MenuFileSaveGame_Click(object? sender, EventArgs e)
        {
            Boolean restartTimer = _timer.Enabled;
            _timer.Stop();

            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játé mentése
                    await _model.SaveGame(_saveFileDialog.FileName);
                }
                catch (AmobaDataException)
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (restartTimer)
                _timer.Start();
        }

        private void Pause_Click(object? sender, EventArgs e)
        {
            _timer.Stop();
            if (MessageBox.Show("A folytatáshoz kattints az OK gombra!", "Játék megállítva", MessageBoxButtons.OK) == DialogResult.OK)
            {
                _timer.Start();
            } 
        }

        private void Game10x10_Click(object? sender, EventArgs e)
        {
            _model.TableSize = 10;
            _model.NewGame();
            GenerateTable();
        }

        private void Game20x20_Click(object? sender, EventArgs e)
        {
            _model.TableSize = 20;
            _model.NewGame();
            GenerateTable();
        }

        private void Game30x30_Click(object? sender, EventArgs e)
        {
            _model.TableSize = 30;
            _model.NewGame();
            GenerateTable();
        }

        #endregion
    }
}