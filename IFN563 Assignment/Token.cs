using System;
using System.Collections.Generic;
using System.Text;

namespace IFN563_Assignment
{
    internal class Token
    {
        private int TokenNo { get; set; }
        private Player Player { get; set; }
        private bool Victory { get; set; }
        public Token(int no, Player player)
        {
            TokenNo = no;
            Player = player;
        }
        public int getTokenNo()
        {
            return TokenNo;
        }
        public Player getPlayer()
        {
            return Player;
        }
        public void setVictory(bool b)
        {
            Victory = b;
        }
        public bool isVictory()
        {
            return Victory;
        }
    }
}
