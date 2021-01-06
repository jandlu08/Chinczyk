using System;
using System.Windows.Forms;
using System.Drawing;

namespace Chinczyk
{
    class Player
    {
        int _cubeNumber;
        int[] _playerNumber, _playerPosition, _positionOnGameTable;
        bool[] _gameStart;
        bool[] _parkingHelp;
        readonly Random _random;

        public Player()
        {
            _playerNumber = new[] { 52, 52, 52, 52, 52 };
            _playerPosition = new[] { 0, 0, 0, 0 };

            _gameStart = new[] { true, true, true, true };
            _parkingHelp = new[] { true, true, true, true };
            _random = new Random();
        }

        public int CubeNumber
        {
            set { _cubeNumber = value; }
            get { return _cubeNumber; }
        }

        public bool[] GameStart
        {
            set { _gameStart = value; }
            get { return _gameStart; }
        }

        public bool[] ParkingHelp
        {
            set { _parkingHelp = value; }
            get { return _parkingHelp; }
        }

        public int[] PlayerNumber
        {
            set { _playerNumber = value; }
            get { return _playerNumber; }
        }

        public int[] PlayerPosition
        {
            set { _playerPosition = value; }
            get { return _playerPosition; }
        }

        public int[] PositionOnGameTable
        {
            set { _positionOnGameTable = value; }
            get { return _positionOnGameTable; }
        }

        public void CubeRoll(PictureBox pictBox, Image[] image6)
        {
            _cubeNumber = _random.Next(1, 7);
            pictBox.Image = image6[_cubeNumber];
        }
    }
}
