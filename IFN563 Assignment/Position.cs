using System;
using System.Collections.Generic;
using System.Text;

namespace IFN563_Assignment
{
    internal class Position
    {
        private int Index { get; set; }
        private Token Token { get; set; }
        private bool IsOccupied { get; set; }
        public Position(int index)
        {
            Index = index;
        }
        public int getIndex()
        {
            return Index;
        }
        public bool isOccupied()
        {
            return IsOccupied;
        }
        public Token getToken()
        {
            return Token;
        }
        public void setToken(Token token)
        {
            Token = token;
            IsOccupied = true;
        }
        public void removeToken()
        {
            Token = null;
            IsOccupied = false;
        }
    }
}
