using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChessBot.Models
{
    public class GameState
    {
        public bool blackTurn { get; set; }
        public char[,] currentBoard { get; set; }
        public List<string> lastTenMoves { get; set; }

        public string ToBoardString()
        {
            string returnText = "";

            for (int i = 0; i < currentBoard.GetLength(0); i++)
            {
                for (int j = 0; j < currentBoard.GetLength(1); j++)
                {
                    returnText += (Char.IsUpper(currentBoard[i, j]) && currentBoard[i, j] != ChessBoard.eSC) ? String.Format("**{0}** ", currentBoard[i, j]) : currentBoard[i, j] + " ";
                }
                returnText += Environment.NewLine;
            }

            return returnText;
        }

        public string ToListString()
        {
            string returnText = String.Empty;

            for (int i = 0; i < lastTenMoves.Count; i++)
            {
                returnText += String.Format("{0}. {1}\n", i + 1, lastTenMoves[i]);
            }

            return returnText.Length != 0 ? returnText : "No moves have been made yet.";
        }

        override public string ToString()
        {
            string returnText = "";

            for (int i = 0; i < currentBoard.GetLength(0); i++)
            {
                for (int j = 0; j < currentBoard.GetLength(1); j++)
                {
                    returnText += (Char.IsUpper(currentBoard[i, j]) && currentBoard[i, j] != ChessBoard.eSC) ? String.Format("**{0}** ", currentBoard[i, j]) : currentBoard[i, j] + " ";
                }
                returnText += System.Environment.NewLine; //returnText += "\n\r";
            }

            for (int i = 0; i < lastTenMoves.Count; i++)
            {
                returnText += String.Format("{0}. {1}\n", i + 1, lastTenMoves[i]);
            }

            return returnText;
        }
    }
}