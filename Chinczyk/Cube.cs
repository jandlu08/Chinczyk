using System;
using System.Media;
using System.Windows.Forms;

namespace Chinczyk
{
    public class Cube
    {
        bool _startStop = true;
       public static readonly SoundPlayer CubeRollSoundPlayer = new SoundPlayer();
        public void CubeRoll(Timer timerCubeRoll)
        {
            if (_startStop)
            {
                _startStop = false;
                timerCubeRoll.Start();
                CubeRollSoundPlayer.PlayLooping();
            }
            else
            {
                _startStop = true;
                timerCubeRoll.Stop();
                CubeRollSoundPlayer.Stop();
            }
        }

      
    }
}