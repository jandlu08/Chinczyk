using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;
using System.Threading;
using System.Resources;

namespace Chinczyk
{
    public partial class GameTable : Form
    {
        public GameTable()
        {
            InitializeComponent();
        }

        Player _player1, _player2, _player3, _player4;

        PictureBox[] _table;

        Image[] _cubeImg;

        Button[] _buttonPlayer1, _buttonPlayer2, _buttonPlayer3, _buttonPlayer4;
        readonly Button[][] _xButton = new Button[4][];

        readonly Bitmap[] _cubeBmp =
        {
            Properties.Resources.cube_0, Properties.Resources.cube_1, Properties.Resources.cube_2,
            Properties.Resources.cube_3, Properties.Resources.cube_4, Properties.Resources.cube_5,
            Properties.Resources.cube_6
        };


        readonly Bitmap[][] _xBitmap = new Bitmap[16][];

        int _order, _playerNumber;
        bool _startStop = true;

        int locX0 = 27, locY0 = 30, locX1 = 195, locY1 = 30, locX2 = 27, locY2 = 195, locX3 = 195, locY3 = 195;

        readonly SoundPlayer _cubeRollSoundPlayer = new SoundPlayer();
        readonly SoundPlayer _moveSoundPlayer = new SoundPlayer();
        readonly SoundPlayer _killPawnSoundPlayer = new SoundPlayer();
        readonly SoundPlayer _parkSoundPlayer = new SoundPlayer();
        readonly SoundPlayer _insertPawnSoundPlayer = new SoundPlayer();
        readonly SoundPlayer _victorySoundPlayer = new SoundPlayer();

        readonly ResourceManager _locRm = LanguageSelect.LocRm;

        private void Tabla_Load(object sender, EventArgs e)
        {
            _player1 = new Player();
            _player2 = new Player();
            _player3 = new Player();
            _player4 = new Player();

            _playerNumber = ConfigureGame.NumOfPlayers;

            for (int i = 0; i <= 3; i++)
            {
                //players start positions 
                _player2.PlayerPosition[i] = 12;
                _player3.PlayerPosition[i] = 24;
                _player4.PlayerPosition[i] = 36;
            }

            // positions for each player from the beginning to the end of his movement on the board, including parking
            _player1.PositionOnGameTable = Enumerable.Range(0, 52).ToArray();
            _player2.PositionOnGameTable = Enumerable.Range(12, 36)
                .Concat(Enumerable.Range(0, 12).Concat(Enumerable.Range(52, 4))).ToArray();
            _player3.PositionOnGameTable = Enumerable.Range(24, 24)
                .Concat(Enumerable.Range(0, 24).Concat(Enumerable.Range(56, 4))).ToArray();
            _player4.PositionOnGameTable =
                Enumerable.Range(36, 12)
                    .Concat(Enumerable.Range(0, 36).Concat(Enumerable.Range(60, 4))).ToArray();

            _buttonPlayer1 = new[] {buttonR0, buttonR1, buttonR2, buttonR3};
            _buttonPlayer2 = new[] {buttonY0, buttonY1, buttonY2, buttonY3};
            _buttonPlayer3 = new[] {buttonB0, buttonB1, buttonB2, buttonB3};
            _buttonPlayer4 = new[] {buttonG0, buttonG1, buttonG2, buttonG3};


            // player buttons 4x4
            _xButton[0] = _buttonPlayer1;
            _xButton[1] = _buttonPlayer2;
            _xButton[2] = _buttonPlayer3;
            _xButton[3] = _buttonPlayer4;


            _cubeImg = new Image[7];

            for (int i = 0; i < _cubeImg.Length; i++)
                _cubeImg[i] = _cubeBmp[i];

            _table = new PictureBox[64];

            int x = 404, y = 12;

            for (int i = 0; i <= 47; i++)
            {
                _table[i] = new PictureBox();
                _table[i].Size = new Size(50, 50);
                _table[i].BackColor = Color.WhiteSmoke;


                if (i >= 1 && i <= 5 || i >= 11 && i <= 12 || i >= 18 && i <= 22)
                    y += 50 + 6;
                if (i >= 6 && i <= 10 || i >= 37 && i <= 41 || i >= 47)
                    x += 50 + 6;
                if (i >= 13 && i <= 17 || i >= 23 && i <= 24 || i >= 30 && i <= 34)
                    x -= 50 + 6;
                if (i >= 25 && i <= 29 || i >= 35 && i <= 36 || i >= 42 && i <= 46)
                    y -= 50 + 6;

                _table[i].Location = new Point(x, y);

                if (i == 0)
                {
                    _table[i].BackgroundImage = Properties.Resources.down;
                    _table[i].BackgroundImageLayout = ImageLayout.Stretch;
                }
                else if (i == 12)
                {
                    _table[i].BackgroundImage = Properties.Resources.left;
                    _table[i].BackgroundImageLayout = ImageLayout.Stretch;
                }
                else if (i == 24)
                {
                    _table[i].BackgroundImage = Properties.Resources.up;
                    _table[i].BackgroundImageLayout = ImageLayout.Stretch;
                }
                else if (i == 36)
                {
                    _table[i].BackgroundImage = Properties.Resources.right;
                    _table[i].BackgroundImageLayout = ImageLayout.Stretch;
                }

                this.Controls.Add(_table[i]);
            }

            _table[48] = pictureBox1;
            _table[49] = pictureBox2;
            _table[50] = pictureBox3;
            _table[51] = pictureBox4;

            _table[52] = pictureBox5;
            _table[53] = pictureBox6;
            _table[54] = pictureBox7;
            _table[55] = pictureBox8;

            _table[56] = pictureBox9;
            _table[57] = pictureBox10;
            _table[58] = pictureBox11;
            _table[59] = pictureBox12;

            _table[60] = pictureBox13;
            _table[61] = pictureBox14;
            _table[62] = pictureBox15;
            _table[63] = pictureBox16;

            SetPawn(ConfigureGame.Figure);

            _cubeRollSoundPlayer.Stream = Properties.Resources.rollCube;
            _moveSoundPlayer.Stream = Properties.Resources.tap;
            _killPawnSoundPlayer.Stream = Properties.Resources.insertPawn;
            _parkSoundPlayer.Stream = Properties.Resources.migmigParking;
            _insertPawnSoundPlayer.Stream = Properties.Resources.killPawn;
            _victorySoundPlayer.Stream = Properties.Resources.winSound;

            panelCubeEnabDisab.SendToBack();

            SetPlaerName(ConfigureGame.Red, labelImeR);
            SetPlaerName(ConfigureGame.Yellow, labelImeY);
            SetPlaerName(ConfigureGame.Blue, labelImeB);
            SetPlaerName(ConfigureGame.Green, labelImeG);


            toolTip.SetToolTip(buttonOrder0, _locRm.GetString("tTolTipB"));
            toolTip.SetToolTip(buttonOrder1, _locRm.GetString("tTolTipB"));
            toolTip.SetToolTip(buttonOrder2, _locRm.GetString("tTolTipB"));
            toolTip.SetToolTip(buttonOrder3, _locRm.GetString("tTolTipB"));

            toolTip.SetToolTip(labelPlayer0, _locRm.GetString("tTolTipL0"));
            toolTip.SetToolTip(labelPlayer1, _locRm.GetString("tTolTipL1"));
            toolTip.SetToolTip(labelPlayer2, _locRm.GetString("tTolTipL2"));
            toolTip.SetToolTip(labelPlayer3, _locRm.GetString("tTolTipL3"));

            this.Text = _locRm.GetString("pNazivForme");
        }

        public void CubeRoll()
        {
            if (_startStop)
            {
                _startStop = false;
                timerCubeRoll.Start();
                _cubeRollSoundPlayer.PlayLooping();
            }
            else
            {
                _startStop = true;
                timerCubeRoll.Stop();
                _cubeRollSoundPlayer.Stop();
            }
        }

        private void timerCubeRoll_Tick(object sender, EventArgs e)
        {
            if (_order == 0)
                _player1.CubeRoll(pictureBoxCube, _cubeImg);
            else if (_order == 1)
                _player2.CubeRoll(pictureBoxCube, _cubeImg);
            else if (_order == 2)
                _player3.CubeRoll(pictureBoxCube, _cubeImg);
            else
                _player4.CubeRoll(pictureBoxCube, _cubeImg);
        }

        private void pictureBoxCube_Click(object sender, EventArgs e)
        {
            CubeRoll();

            if (_order == 0 && timerCubeRoll.Enabled == false)
                EnableDisableButtons(_player1, _buttonPlayer1, _buttonPlayer2, _buttonPlayer3, _buttonPlayer4, 'R');

            else if (_order == 1 && timerCubeRoll.Enabled == false)
                EnableDisableButtons(_player2, _buttonPlayer2, _buttonPlayer1, _buttonPlayer3, _buttonPlayer4, 'Y');

            else if (_order == 2 && timerCubeRoll.Enabled == false)
                EnableDisableButtons(_player3, _buttonPlayer3, _buttonPlayer1, _buttonPlayer2, _buttonPlayer4, 'B');

            else if (_order == 3 && timerCubeRoll.Enabled == false)
                EnableDisableButtons(_player4, _buttonPlayer4, _buttonPlayer1, _buttonPlayer2, _buttonPlayer3, 'G');
        }

        private void button_click(object sender, EventArgs e)
        {
            Button b = (Button) sender;

            if (_order == 0)
            {
                if (CheckPlayer(_player1, b))
                {
                    b.Parent.Controls.Remove(b);
                    this.Controls.Add(b);
                    b.BringToFront();
                }
            }
            else if (_order == 1)
            {
                if (CheckPlayer(_player2, b))
                {
                    b.Parent.Controls.Remove(b);
                    this.Controls.Add(b);
                    b.BringToFront();
                }
            }
            else if (_order == 2)
            {
                if (CheckPlayer(_player3, b))
                {
                    b.Parent.Controls.Remove(b);
                    this.Controls.Add(b);
                    b.BringToFront();
                }
            }
            else
            {
                if (CheckPlayer(_player4, b))
                {
                    b.Parent.Controls.Remove(b);
                    this.Controls.Add(b);
                    b.BringToFront();
                }
            }
        }

        private void buttonChangeOrder_Click(object sender, EventArgs e)
        {
            Button b = (Button) sender;
            char buttonName = b.Name[b.Name.Length - 1];

            _order++;
            if (_order == _playerNumber + 1)
                _order = 0;

            b.Visible = false;
            pictureBoxCube.Enabled = true;
            panelCubeEnabDisab.BackColor = Color.Green;
            pictureBoxCube_Click(null, null);

            if (_playerNumber == 1)
            {
                if (buttonName == '0')
                {
                    labelPlayer0.Visible = false;
                    labelPlayer1.Visible = true;
                }
                else if (buttonName == '1')
                {
                    labelPlayer1.Visible = false;
                    labelPlayer0.Visible = true;
                }
            }
            else if (_playerNumber == 2)
            {
                if (buttonName == '0')
                {
                    labelPlayer0.Visible = false;
                    labelPlayer1.Visible = true;
                }
                else if (buttonName == '1')
                {
                    labelPlayer1.Visible = false;
                    labelPlayer2.Visible = true;
                }
                else if (buttonName == '2')
                {
                    labelPlayer2.Visible = false;
                    labelPlayer0.Visible = true;
                }
            }
            else
            {
                if (buttonName == '0')
                {
                    labelPlayer0.Visible = false;
                    labelPlayer1.Visible = true;
                }
                else if (buttonName == '1')
                {
                    labelPlayer1.Visible = false;
                    labelPlayer2.Visible = true;
                }
                else if (buttonName == '2')
                {
                    labelPlayer2.Visible = false;
                    labelPlayer3.Visible = true;
                }
                else if (buttonName == '3')
                {
                    labelPlayer3.Visible = false;
                    labelPlayer0.Visible = true;
                }
            }
        }

        private bool CheckPlayer(Player player, Button button)
        {
            int index = Convert.ToInt32(button.Name[button.Name.Length - 1].ToString());
            char c = button.Name[button.Name.Length - 2];

            // if it is a 6 and it is not on the board, put it in the starting position
            if (index == 0 && player.CubeNumber == 6 && player.GameStart[index])
            {
                player.GameStart[index] = false;
                player.PlayerNumber[index] = 0;
                button.Location = new Point(_table[player.PlayerPosition[index]].Location.X,
                    _table[player.PlayerPosition[index]].Location.Y);
                _insertPawnSoundPlayer.Play();

                KillPawn(_player1, _player2, _player3, _player4, c, index);

                DisableButtonsPlayer(c);
                DisableEnableButtonOrder(player, c);
                return true;
            }

            if (index == 1 && player.CubeNumber == 6 && player.GameStart[index])
            {
                player.GameStart[index] = false;
                player.PlayerNumber[index] = 0;
                button.Location = new Point(_table[player.PlayerPosition[index]].Location.X,
                    _table[player.PlayerPosition[index]].Location.Y);
                _insertPawnSoundPlayer.Play();

                KillPawn(_player1, _player2, _player3, _player4, c, index);

                DisableButtonsPlayer(c);
                DisableEnableButtonOrder(player, c);
                return true;
            }

            if (index == 2 && player.CubeNumber == 6 && player.GameStart[index])
            {
                player.GameStart[index] = false;
                player.PlayerNumber[index] = 0;
                button.Location = new Point(_table[player.PlayerPosition[index]].Location.X,
                    _table[player.PlayerPosition[index]].Location.Y);
                _insertPawnSoundPlayer.Play();

                KillPawn(_player1, _player2, _player3, _player4, c, index);

                DisableButtonsPlayer(c);
                DisableEnableButtonOrder(player, c);
                return true;
            }

            if (index == 3 && player.CubeNumber == 6 && player.GameStart[index])
            {
                player.GameStart[index] = false;
                player.PlayerNumber[index] = 0;
                button.Location = new Point(_table[player.PlayerPosition[index]].Location.X,
                    _table[player.PlayerPosition[index]].Location.Y);
                _insertPawnSoundPlayer.Play();

                KillPawn(_player1, _player2, _player3, _player4, c, index);

                DisableButtonsPlayer(c);
                DisableEnableButtonOrder(player, c);
                return true;
            }

            int[] copy = (int[]) player.PlayerNumber.Clone();

            Array.Sort(copy);

            int comShift = player.PlayerNumber[index] + player.CubeNumber;
            int comp = copy[Array.IndexOf(copy, player.PlayerNumber[index]) + 1];

            if (CheckSpacing(player, copy) && player.CubeNumber == 6 &&
                Array.Exists(player.GameStart, element => element))
            {
                return false;
            }

            if (comShift >= comp)
            {
                return false;
            }

            int tempMove = player.PlayerNumber[index];
            player.PlayerNumber[index] += player.CubeNumber;

            player.PlayerPosition[index] = player.PositionOnGameTable[player.PlayerNumber[index]];

            DisableButtonsPlayer(c);

            #region Player move simulation on table

            for (int i = tempMove + 1; i <= player.PlayerNumber[index]; i++)
            {
                button.Location = new Point(_table[player.PositionOnGameTable[i]].Location.X,
                    _table[player.PositionOnGameTable[i]].Location.Y);

                if (i >= 48)
                    _parkSoundPlayer.Play();
                else
                    _moveSoundPlayer.Play();

                Application.DoEvents();
                Thread.Sleep(200);
            }

            #endregion

            KillPawn(_player1, _player2, _player3, _player4, c, index);

            DisableEnableButtonOrder(player, c);

            //check if the player won the move
            int[] copyForVictory = (int[]) player.PlayerNumber.Clone();
            Array.Sort(copyForVictory);
            ChcekWinner(copyForVictory, c);

            return true;
        }

        private void KillPawn(Player player0, Player player1, Player player2, Player player3, char color, int index)
        {
            int indexXy = 0, pointX = 0, pointY = 0, playerNumber = 0;
            bool killed = false;

            if (color == 'R')
            {
                for (int i = 0; i <= 3; i++)
                {
                    // if the position of the player's figure is equal to that of the other player's figure
                    // and this number in the field remembers its index and prepares the figure to be removed from the field
                    if (player0.PlayerPosition[index] == player1.PlayerPosition[i] && player1.GameStart[i] == false)
                    {
                        indexXy = i;
                        playerNumber = 1;
                        killed = true;

                        player1.PlayerNumber[i] = 52;
                        player1.PlayerPosition[i] = 12;
                        player1.GameStart[i] = true;
                        player1.ParkingHelp[i] = true;
                        break;
                    }

                    if (player0.PlayerPosition[index] == player2.PlayerPosition[i] && player2.GameStart[i] == false)
                    {
                        indexXy = i;
                        playerNumber = 2;
                        killed = true;

                        player2.PlayerNumber[i] = 52;
                        player2.PlayerPosition[i] = 24;
                        player2.GameStart[i] = true;
                        player2.ParkingHelp[i] = true;
                        break;
                    }

                    if (player0.PlayerPosition[index] == player3.PlayerPosition[i] && player3.GameStart[i] == false)
                    {
                        indexXy = i;
                        playerNumber = 3;
                        killed = true;

                        player3.PlayerNumber[i] = 52;
                        player3.PlayerPosition[i] = 36;
                        player3.GameStart[i] = true;
                        player3.ParkingHelp[i] = true;
                        break;
                    }
                }
            }
            else if (color == 'Y')
            {
                for (int i = 0; i <= 3; i++)
                {
                    if (player1.PlayerPosition[index] == player0.PlayerPosition[i] && player0.GameStart[i] == false)
                    {
                        indexXy = i;
                        playerNumber = 0;
                        killed = true;

                        player0.PlayerNumber[i] = 52;
                        player0.PlayerPosition[i] = 0;
                        player0.GameStart[i] = true;
                        player0.ParkingHelp[i] = true;
                        break;
                    }

                    if (player1.PlayerPosition[index] == player2.PlayerPosition[i] && player2.GameStart[i] == false)
                    {
                        indexXy = i;
                        playerNumber = 2;
                        killed = true;

                        player2.PlayerNumber[i] = 52;
                        player2.PlayerPosition[i] = 24;
                        player2.GameStart[i] = true;
                        player2.ParkingHelp[i] = true;
                        break;
                    }

                    if (player1.PlayerPosition[index] == player3.PlayerPosition[i] && player3.GameStart[i] == false)
                    {
                        indexXy = i;
                        playerNumber = 3;
                        killed = true;

                        player3.PlayerNumber[i] = 52;
                        player3.PlayerPosition[i] = 36;
                        player3.GameStart[i] = true;
                        player3.ParkingHelp[i] = true;
                        break;
                    }
                }
            }
            else if (color == 'B')
            {
                for (int i = 0; i <= 3; i++)
                {
                    if (player2.PlayerPosition[index] == player0.PlayerPosition[i] && player0.GameStart[i] == false)
                    {
                        indexXy = i;
                        playerNumber = 0;
                        killed = true;

                        player0.PlayerNumber[i] = 52;
                        player0.PlayerPosition[i] = 0;
                        player0.GameStart[i] = true;
                        player0.ParkingHelp[i] = true;
                        break;
                    }

                    if (player2.PlayerPosition[index] == player1.PlayerPosition[i] && player1.GameStart[i] == false)
                    {
                        indexXy = i;
                        playerNumber = 1;
                        killed = true;

                        player1.PlayerNumber[i] = 52;
                        player1.PlayerPosition[i] = 12;
                        player1.GameStart[i] = true;
                        player1.ParkingHelp[i] = true;
                        break;
                    }

                    if (player2.PlayerPosition[index] == player3.PlayerPosition[i] && player3.GameStart[i] == false)
                    {
                        indexXy = i;
                        playerNumber = 3;
                        killed = true;

                        player3.PlayerNumber[i] = 52;
                        player3.PlayerPosition[i] = 36;
                        player3.GameStart[i] = true;
                        player3.ParkingHelp[i] = true;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i <= 3; i++)
                {
                    if (player3.PlayerPosition[index] == player0.PlayerPosition[i] && player0.GameStart[i] == false)
                    {
                        indexXy = i;
                        playerNumber = 0;
                        killed = true;

                        player0.PlayerNumber[i] = 52;
                        player0.PlayerPosition[i] = 0;
                        player0.GameStart[i] = true;
                        player0.ParkingHelp[i] = true;
                        break;
                    }

                    if (player3.PlayerPosition[index] == player1.PlayerPosition[i] && player1.GameStart[i] == false)
                    {
                        indexXy = i;
                        playerNumber = 1;
                        killed = true;

                        player1.PlayerNumber[i] = 52;
                        player1.PlayerPosition[i] = 12;
                        player1.GameStart[i] = true;
                        player1.ParkingHelp[i] = true;
                        break;
                    }

                    if (player3.PlayerPosition[index] == player2.PlayerPosition[i] && player2.GameStart[i] == false)
                    {
                        indexXy = i;
                        playerNumber = 2;
                        killed = true;

                        player2.PlayerNumber[i] = 52;
                        player2.PlayerPosition[i] = 24;
                        player2.GameStart[i] = true;
                        player2.ParkingHelp[i] = true;
                        break;
                    }
                }
            }

            // the condition under which the figure is kicked out of the game
            if (killed)
            {
                // set the correct position of a player's figure when it is removed from the field or when it is kicked out of play
                if (indexXy == 0)
                {
                    pointX = locX0;
                    pointY = locY0;
                }
                else if (indexXy == 1)
                {
                    pointX = locX1;
                    pointY = locY1;
                }
                else if (indexXy == 2)
                {
                    pointX = locX2;
                    pointY = locY2;
                }
                else if (indexXy == 3)
                {
                    pointX = locX3;
                    pointY = locY3;
                }

                if (playerNumber == 0) // the case where one figure was rolled by player 0
                {
                    _buttonPlayer1[indexXy].Location = new Point(pointX, pointY);

                    this.Controls.Remove(_buttonPlayer1[indexXy]);
                    panel0.Controls.Add(_buttonPlayer1[indexXy]);
                    _buttonPlayer1[indexXy].BringToFront();

                    _killPawnSoundPlayer.Play();
                }
                else if (playerNumber == 1)
                {
                    _buttonPlayer2[indexXy].Location = new Point(pointX, pointY);

                    this.Controls.Remove(_buttonPlayer2[indexXy]);
                    panel1.Controls.Add(_buttonPlayer2[indexXy]);
                    _buttonPlayer2[indexXy].BringToFront();

                    _killPawnSoundPlayer.Play();
                }
                else if (playerNumber == 2) 
                {
                    _buttonPlayer3[indexXy].Location = new Point(pointX, pointY);

                    this.Controls.Remove(_buttonPlayer3[indexXy]);
                    panel2.Controls.Add(_buttonPlayer3[indexXy]);
                    _buttonPlayer3[indexXy].BringToFront();

                    _killPawnSoundPlayer.Play();
                }
                else 
                {
                    _buttonPlayer4[indexXy].Location = new Point(pointX, pointY);

                    this.Controls.Remove(_buttonPlayer4[indexXy]);
                    panel3.Controls.Add(_buttonPlayer4[indexXy]);
                    _buttonPlayer4[indexXy].BringToFront();

                    _killPawnSoundPlayer.Play();
                }
            }
        }

        private void ChcekWinner(int[] niz, char color)
        {
            if (niz[0] == 48 && niz[1] == 49 && niz[2] == 50 && niz[3] == 51)
            {
                LanguageSelect start = new LanguageSelect();

                if (color == 'R')
                {
                    DisableEndGame();
                    VictoryCelebration(_player1.PositionOnGameTable, _buttonPlayer1);

                    ConfigureGame.NumOfPlayers = 0;
                    this.Hide();
                    start.Show();
                }
                else if (color == 'Y')
                {
                    DisableEndGame();
                    VictoryCelebration(_player2.PositionOnGameTable, _buttonPlayer2);

                    ConfigureGame.NumOfPlayers = 0;
                    this.Hide();
                    start.Show();
                }
                else if (color == 'B')
                {
                    DisableEndGame();
                    VictoryCelebration(_player3.PositionOnGameTable, _buttonPlayer3);

                    ConfigureGame.NumOfPlayers = 0;
                    this.Hide();
                    start.Show();
                }
                else
                {
                    DisableEndGame();
                    VictoryCelebration(_player4.PositionOnGameTable, _buttonPlayer4);

                    ConfigureGame.NumOfPlayers = 0;
                    this.Hide();
                    start.Show();
                }
            }
        }

        private void DisableEndGame()
        {
            buttonOrder0.Visible = false;
            buttonOrder1.Visible = false;
            buttonOrder2.Visible = false;
            buttonOrder3.Visible = false;

            pictureBoxCube.Enabled = false;
        }


        private void VictoryCelebration(int[] niz, Button[] b)
        {
            _victorySoundPlayer.Play();

            int poz0, poz1 = 0, poz2 = 0, poz3;

            for (int i = 0; i < niz.Length; i++)
            {
                poz0 = i;
                b[0].BringToFront();
                b[0].Location = new Point(_table[niz[poz0]].Location.X, _table[niz[poz0]].Location.Y);
                b[0].BringToFront();
                if (i >= 1)
                {
                    poz1 = poz0 - 1;
                    b[1].BringToFront();
                    b[1].Location = new Point(_table[niz[poz1]].Location.X, _table[niz[poz1]].Location.Y);
                }

                if (i >= 2)
                {
                    poz2 = poz1 - 1;
                    b[2].BringToFront();
                    b[2].Location = new Point(_table[niz[poz2]].Location.X, _table[niz[poz2]].Location.Y);
                }

                if (i >= 3)
                {
                    poz3 = poz2 - 1;
                    b[3].BringToFront();
                    b[3].Location = new Point(_table[niz[poz3]].Location.X, _table[niz[poz3]].Location.Y);
                }

                Thread.Sleep(800);
            }
        }

        private void DisableButtonsPlayer(char boja)
        {
            if (boja == 'R')
                DisableButtonPlayers(_buttonPlayer1);
            if (boja == 'Y')
                DisableButtonPlayers(_buttonPlayer2);
            if (boja == 'B')
                DisableButtonPlayers(_buttonPlayer3);
            if (boja == 'G')
                DisableButtonPlayers(_buttonPlayer4);
        }

        private void DisableButtonPlayers(Button[] buttonDisable)
        {
            for (int i = 0; i <= 3; i++)
                buttonDisable[i].Enabled = false;
        }

        private void DisableEnableButtonOrder(Player ig, char boja)
        {
            if (ig.CubeNumber != 6) // if it's not 6, let the row be carried over to the next player
                DisableEnableOrder(boja);
            else
            {
                pictureBoxCube.Enabled =
                    true; // otherwise leave it to the same player to turn again because it was 6 points
                panelCubeEnabDisab.BackColor = Color.Green;
            }
        }

        private void DisableEnableOrder(char color)
        {
            if (color == 'R')
            {
                buttonOrder0.Visible = true;
                buttonOrder1.Visible = false;
                buttonOrder2.Visible = false;
                buttonOrder3.Visible = false;
            }
            else if (color == 'Y')
            {
                buttonOrder0.Visible = false;
                buttonOrder1.Visible = true;
                buttonOrder2.Visible = false;
                buttonOrder3.Visible = false;
            }
            else if (color == 'B')
            {
                buttonOrder0.Visible = false;
                buttonOrder1.Visible = false;
                buttonOrder2.Visible = true;
                buttonOrder3.Visible = false;
            }
            else if (color == 'G')
            {
                buttonOrder0.Visible = false;
                buttonOrder1.Visible = false;
                buttonOrder2.Visible = false;
                buttonOrder3.Visible = true;
            }
        }

        private void EnableDisableButtons(Player player, Button[] buttonEnable, Button[] buttonDis1, Button[] buttonDis2,
            Button[] buttonDis3, char color)
        {
            int[] copy = (int[]) player.PlayerNumber.Clone();
            Array.Sort(copy);

            for (int i = 0; i <= 3; i++)
            {
                if (player.CubeNumber == 6)
                {
                    if (CheckSpacing(player, copy) && CheckAllOnTheField(player, player.GameStart))
                    {
                        DisableButtonsPlayer(color);
                        pictureBoxCube.Enabled = true;
                        panelCubeEnabDisab.BackColor = Color.Green;
                        goto Kraj;
                    }
                    else if (Array.Exists(player.PlayerNumber, element => element == 0))
                    {
                        if (player.GameStart[i] == false)// if it's 6 digits and taken, only allow those in the field
                            buttonEnable[i].Enabled = true;
                    }
                    else if (!Array.Exists(player.PlayerNumber, element => element == 0))
                        buttonEnable[i].Enabled = true; // if it's 6 and the house is unoccupied let everyone
                }
                else if (player.CubeNumber != 6)
                {
                    if (CheckSpacing(player, copy))
                    {
                        DisableButtonsPlayer(color);
                        DisableEnableOrder(color);
                        break;
                    }
                    else if (player.GameStart[i] == false) // if not 6 let only those in the field
                        buttonEnable[i].Enabled = true;
                    else if (player.GameStart[0] && player.GameStart[1] && player.GameStart[2] &&
                             player.GameStart[3])
                    {
                        DisableEnableOrder(color); // if it's not a 6 and everyone in the box forbids everyone the player who plays
                        break;
                    }
                }
            }

            pictureBoxCube.Enabled = false;
            panelCubeEnabDisab.BackColor = Color.Red;

            Kraj: ;
        }

        private bool CheckSpacing(Player ig, int[] niz)
        {
            // checks the distance between players to detect the possibility of movement, if the players' figures cannot move returns true
            for (int i = 0; i <= 3; i++)
                if (niz[i] + ig.CubeNumber < niz[i + 1])
                    return false;

            return true;
        }

        private bool CheckAllOnTheField(Player player, bool[] niz)
        {
            // checks to see if all of a player's pieces are on the court, if so he returns true
            for (int i = 0; i <= 3; i++)
                if (niz[i])
                    return false;

            return true;
        }

        private void SetPlaerName(string name, Label nameLabel)
        {
            for (int i = 0; i <= name.Length - 1; i++)
            {
                if (i == name.Length - 1)
                    nameLabel.Text += name[i].ToString();
                else
                    nameLabel.Text += name[i] + "\n";
            }
        }

        private void SetPawn(int[] pawns)
        {
            int position = 0;
            for (int i = 0; i < pawns.Length; i++)
            {
                if (i == 0)
                    position = 0;
                else if (i == 1)
                    position = 4;
                else if (i == 2)
                    position = 8;
                else if (i == 3)
                    position = 12;

                if (pawns[i] != -1)
                    for (int j = 0; j < _xButton.Length; j++)
                        _xButton[i][j].BackgroundImage = _xBitmap[pawns[i] + position][j];
            }
        }

        private void GameTable_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cubeRollSoundPlayer.Stop();
            Dispose();
        }
    }
}