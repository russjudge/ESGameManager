using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESGameManagerLibrary
{
    public class NewGameListEventArgs : EventArgs
    {
        public NewGameListEventArgs(GameList gameList)
        {
            GameList = gameList;
        }

        public GameList GameList { get; private set; }
    }
}
