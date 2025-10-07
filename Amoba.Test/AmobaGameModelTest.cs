using Elte.Amoba.Model;
using Elte.Amoba.Persistence;
using Moq;

namespace Amoba.Test
{
    [TestClass]
    public class AmobaGameModelTest
    {

        private AmobaModel _model = null!; // a tesztelendő modell
        private Player[,] _mockedTable = null!; // mockolt játéktábla
        private Mock<IPersistence> _mock = null!; // az adatelérés mock-ja

        [TestInitialize]
        public void Initialize()
        {
            _mockedTable = new Player[10, 10];

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _mockedTable[i, j] = Player.NoPlayer;
                }
            }

            _mockedTable[9, 0] = Player.PlayerX;
            _mockedTable[9, 1] = Player.PlayerO;

            _mock = new Mock<IPersistence>();
            _mock.Setup(mock => mock.Load(It.IsAny<String>()))
                .Returns(() => Task.FromResult((_mockedTable, 2, 1, 3)));
            // a mock a LoadAsync műveletben bármilyen paraméterre az előre beállított játéktáblát fogja visszaadni

            _model = new AmobaModel(_mock.Object);
            // példányosítjuk a modellt a mock objektummal

        }


        [TestMethod]
        public void AmobaConstructorTest() // egységteszt művelet
        {
            _model.NewGame();
            Assert.AreEqual(10, _model.TableSize);
            for (Int32 i = 0; i < 10; i++)
                for (Int32 j = 0; j < 10; j++)
                    Assert.AreEqual(Player.NoPlayer, _model[i, j]); 
        }

        [TestMethod]
        public void AmobaStepGameTest()
        {
            _model.NewGame();
            _model.StepGame(9, 0);

            Assert.AreEqual(Player.PlayerX, _model[9, 0]); // átváltott-e az érték X-re

            for (Int32 i = 0; i < 10; i++)
                for (Int32 j = 0; j < 10; j++)
                {
                    if (i != 9 || j != 0)
                        Assert.AreEqual(Player.NoPlayer, _model[i, j]); // valamennyi további mező üres
                }

            _model.StepGame(9, 1);

            Assert.AreEqual(Player.PlayerO, _model[9, 1]); // átváltott-e az érték O-ra



            try
            {
                for (Int32 i = 0; i < 10; i++)
                {
                    _model.StepGame(9, 1);
                }
                Assert.Fail(); // ha nem váltódik ki kivétel, akkor hibás a működés
            }
            catch (InvalidOperationException) { }

            Assert.AreEqual(Player.PlayerO, _model[9, 1]); // továbbá nem szabad, hogy az érték megváltozzon
        }

        [TestMethod]
        public void AmobaStepNumberTest()
        {
            _model.NewGame();
            Assert.AreEqual(_model.StepNumber, 0);

            Int32 k = 0;
            for (Int32 i = 0; i < 10; i++)
                for (Int32 j = 0; j < 10; j++)
                {
                    _model.StepGame(i, j);
                    k++;
                    Assert.AreEqual(k, _model.StepNumber); // lépésszám megfelelő-e
                }
        }

        [TestMethod]
        [DataRow(9, 0)]
        [DataRow(9, 1)]
        [DataRow(8, 0)]
        [DataRow(8, 1)]
        [DataRow(9, 2)]
        public void AmobaIndexerValidTest(int x, int y)
        {
            _model.NewGame();
            Assert.AreEqual(Player.NoPlayer, _model[9, y]);
            _model.StepGame(x, y);
            Assert.AreEqual(Player.PlayerX, _model[9, y]);
        }

        [TestMethod]
        [DataRow(-9, 0)]
        [DataRow(11, 1)]
        [DataRow(1, -8)]
        [DataRow(9, 13)]
        [ExpectedException(typeof(ArgumentException))]
        public void AmobaIndexerInvalidTest(int x, int y)
        {
            _model.NewGame();
            Player _ = _model[x, y];
        }

        [TestMethod]
        public void AmobaGameWonTest()
        {
            bool eventRaised = false;
            _model.GameOver += (sender, e) =>
            {
                eventRaised = true;
                Assert.IsTrue(e.Player == Player.PlayerX); // a megfelelő játékos győzött-e
            };

            _model.NewGame();
            _model.StepGame(9, 0);
            _model.StepGame(9, 4);
            _model.StepGame(9, 1);
            _model.StepGame(9, 5);
            _model.StepGame(9, 2);
            _model.StepGame(9, 6);
            _model.StepGame(9, 3);

            Assert.IsTrue(eventRaised); // kiváltottuk-e az eseményt
        }

        [TestMethod]
        public async Task AmobaGameLoadTest()
        {
            // kezdünk egy új játékot és végrehajtunk pár lépést
            _model.NewGame();
            _model.StepGame(0, 0);
            _model.StepGame(0, 1);
            _model.StepGame(1, 0);
            _model.StepGame(1, 1);

            // majd betöltünk egy játékot
            await _model.LoadGame(String.Empty);

            Int32 stepNumber = 0;
            for (Int32 i = 0; i < 10; i++)
                for (Int32 j = 0; j < 10; j++)
                {
                    
                    stepNumber += (_model[i, j] != Player.NoPlayer) ? 1 : 0;
                }

            // ellenőrizzük a lépésszámot, és a következ ő játékost
            Assert.AreEqual(stepNumber, _model.StepNumber);
            Assert.AreEqual(stepNumber % 2 == 0 ? Player.PlayerX : Player.PlayerO, _model.CurrentPlayer);
            Assert.AreEqual(3, _model.PlayerXTime);
            Assert.AreEqual(1, _model.PlayerOTime);

            // ellenőrizzük, hogy meghívták-e a Load műveletet a megadott paraméterrel
            _mock.Verify(dataAccess => dataAccess.Load(String.Empty), Times.Once());
        }
    }
}