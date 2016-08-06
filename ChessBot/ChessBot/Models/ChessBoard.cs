using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ChessBot.Models
{
    public class ChessBoard
    {
        private bool blackTurn = false;

        public static char eSC = 'O';

        private static char[,] initialBoard = new char[8, 8] {
            {'R', 'N', 'B', 'Q', 'K', 'B', 'N', 'R'},
            {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P'},
            {eSC, eSC, eSC, eSC, eSC, eSC, eSC, eSC},
            {eSC, eSC, eSC, eSC, eSC, eSC, eSC, eSC},
            {eSC, eSC, eSC, eSC, eSC, eSC, eSC, eSC},
            {eSC, eSC, eSC, eSC, eSC, eSC, eSC, eSC},
            {'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p'},
            {'r', 'n', 'b', 'q', 'k', 'b', 'n', 'r'}
        };

        public static char[,] InitialBoard {
            get {
                return initialBoard;
            }
        }

        private List<string> lastTenMoves = new List<string>();

        public List<string> LastTenMoves {
            get
            {
                return lastTenMoves;
            }
        }

        public void makeMove(string moveFromLocation, string moveToLocation, GameState state)
        {
            int[] moveFromLoc = convertAlgebraicToPoint(moveFromLocation);
            int[] moveToLoc = convertAlgebraicToPoint(moveToLocation);
            bool takingPiece = state.currentBoard[moveToLoc[0], moveToLoc[1]] != eSC;
            state.currentBoard[moveToLoc[0], moveToLoc[1]] = state.currentBoard[moveFromLoc[0], moveFromLoc[1]];
            state.currentBoard[moveFromLoc[0], moveFromLoc[1]] = eSC;
            state.lastTenMoves.Add(Char.ToUpper(state.currentBoard[moveToLoc[0], moveToLoc[1]]) != 'P' ? Char.ToUpper(state.currentBoard[moveToLoc[0], moveToLoc[1]]) + moveToLocation : moveToLocation);
            blackTurn = !blackTurn;
            state.blackTurn = !state.blackTurn;
        }

        public static bool isOccupiedLocation(int[] loc, GameState state)
        {
            var pieceInLoc = state.currentBoard[loc[0], loc[1]];
            return state.currentBoard[loc[0], loc[1]] != eSC;
        }

        public static int[] convertAlgebraicToPoint(string location)
        {
            if (isValidChessLocation(location))
            {
                char[] locationArray = location.ToCharArray();
                int[] point = new int[2];
                point[0] = 7 - (Char.ToLower(locationArray[1]) - '1');
                point[1] = Char.ToLower(locationArray[0]) - 'a';
                return point;
            }
            throw new InvalidCastException("location must be a valid chess location");
        }

        public static string convertPointToAlgebraic(int[] loc)
        {
            string location = $"{(char)('a' + loc[1])}{(char)('1' + (7 - loc[0]))}";

            if (loc[0] >= 0 && loc[0] < 8 && loc[1] >= 0 && loc[1] < 8)
                return location;

            throw new InvalidCastException("Both coordinates in location must be between 0 and 8.");
        }

        public static List<int[]> getPathHorizontalOrVerticalOrDiagonal(int[] moveFromLoc, int[] moveToLoc)
        {
            int diffX = moveToLoc[0] - moveFromLoc[0];
            int diffY = moveToLoc[1] - moveFromLoc[1];
            int maxDistance = Math.Max(Math.Abs(diffX), Math.Abs(diffY));
            List<int[]> returnList = new List<int[]>();

            for (int i = 1; i <= maxDistance - 1; i++)
            {
                returnList.Add(new int[] { moveFromLoc[0] + i * diffX / maxDistance, moveFromLoc[1] + i * diffY / maxDistance });
            }

            return returnList;
        }

        public static bool isPathClear(List<int[]> path, GameState state)
        {
            for (int i = 0; i < path.Count; i++)
            {
                if (state.currentBoard[path[i][0], path[i][1]] != eSC)
                    return false;
            }
            return true;
        }

        public static bool isValidChessLocation(string location)
        {
            Regex r = new Regex(@"[a-h][1-8]", RegexOptions.IgnoreCase);
            char[] locationArray = location.ToCharArray();
            if(location.Length != 2)
            {
                return false;
            }
            else if(!r.Match(location).Success)
            {
                return false;
            }
            return true;
        }

        public static bool isMoveDiagonal(string location1, string location2, GameState state)
        {
            int[] moveFromLoc = convertAlgebraicToPoint(location1);
            int[] moveToLoc = convertAlgebraicToPoint(location2);
            return Math.Abs(moveFromLoc[0] - moveToLoc[0]) == Math.Abs(moveFromLoc[1] - moveToLoc[1]) && (moveFromLoc[0] - moveToLoc[0]) != 0;
        }

        public static bool isMoveVerticalOrHorizontal(string location1, string location2, GameState state)
        {
            int[] moveFromLoc = convertAlgebraicToPoint(location1);
            int[] moveToLoc = convertAlgebraicToPoint(location2);
            int diffX = moveFromLoc[0] - moveToLoc[0];
            int diffY = moveFromLoc[1] - moveToLoc[1];
            return diffX != 0 && diffY == 0 || diffX == 0 && diffY != 0;
        }

        public static bool isMoveLShaped(string location1, string location2, GameState state)
        {
            int[] moveFromLoc = convertAlgebraicToPoint(location1);
            int[] moveToLoc = convertAlgebraicToPoint(location2);
            int absDiffX = Math.Abs(moveFromLoc[0] - moveToLoc[0]);
            int absDiffY = Math.Abs(moveFromLoc[1] - moveToLoc[1]);
            return absDiffX == 1 && absDiffY == 2 || absDiffX == 2 && absDiffY == 1;
        }

        public static bool isMoveEnPassant(string location1, string location2, GameState state)
        {
            int[] moveFromLoc = convertAlgebraicToPoint(location1);
            int[] moveToLoc = convertAlgebraicToPoint(location2);
            return Char.ToLower(state.currentBoard[moveFromLoc[0], moveFromLoc[1]]) == 'p' && Char.ToLower(state.currentBoard[moveFromLoc[0], moveToLoc[1]]) == 'p' && Math.Abs(moveToLoc[1] - moveFromLoc[1]) == 1;
        }

        public static bool isMoveOneAway(string location1, string location2, GameState state)
        {
            int[] moveFromLoc = convertAlgebraicToPoint(location1);
            int[] moveToLoc = convertAlgebraicToPoint(location2);

            return Math.Max(Math.Abs(moveFromLoc[0] - moveToLoc[0]), Math.Abs(moveFromLoc[1] - moveFromLoc[1])) == 1;
        }

        public static bool isMoveLegal(string location1, string location2, GameState state)
        {
            int[] moveFromLoc = convertAlgebraicToPoint(location1);
            int[] moveToLoc = convertAlgebraicToPoint(location2);

            List<int[]> path = getPathHorizontalOrVerticalOrDiagonal(moveFromLoc, moveToLoc);

            if(state.currentBoard[moveFromLoc[0], moveFromLoc[1]] == eSC)
            {
                return false;
            }
            if (state.currentBoard[moveToLoc[0], moveToLoc[1]] != eSC && (Char.IsLower(state.currentBoard[moveFromLoc[0], moveFromLoc[1]]) == Char.IsLower(state.currentBoard[moveToLoc[0], moveToLoc[1]])))
            {
                return false;
            }

            char pieceChar = state.currentBoard[moveFromLoc[0], moveFromLoc[1]], pieceCharUpper = Char.ToUpper(state.currentBoard[moveFromLoc[0], moveFromLoc[1]]);
            
            if (pieceCharUpper == PieceCharacters.Pawn) {
                int diffX = moveFromLoc[0] - moveToLoc[0];
                int diffY = moveFromLoc[1] - moveToLoc[1];
                bool currentlyValid = diffY == 0 && (Math.Abs(diffX) == 1 || Math.Abs(diffX) == 2 && (Char.IsUpper(state.currentBoard[moveFromLoc[0], moveFromLoc[1]]) && moveToLoc[0] == 3) || (Char.IsLower(state.currentBoard[moveFromLoc[0], moveFromLoc[1]]) && moveToLoc[0] == 4)) || isMoveEnPassant(location1, location2, state);
                currentlyValid = currentlyValid && (diffX > 0 && Char.IsLower(pieceChar) || diffX < 0 && Char.IsUpper(pieceChar));

                currentlyValid = currentlyValid && (path.Count == 0 || isPathClear(path, state));

                return currentlyValid && !checkForSelfCheck();
            } else if (pieceCharUpper == PieceCharacters.Rook) {
                bool currentlyValid = isMoveVerticalOrHorizontal(location1, location2, state);

                currentlyValid = currentlyValid && (path.Count == 0 || isPathClear(path, state));

                return currentlyValid && !checkForSelfCheck();
            } else if (pieceCharUpper == PieceCharacters.Knight)
            {
                bool currentlyValid = isMoveLShaped(location1, location2, state);

                return currentlyValid && !checkForSelfCheck();
            } else if (pieceChar == PieceCharacters.Bishop)
            {
                bool currentlyValid = isMoveDiagonal(location1, location2, state);

                currentlyValid = currentlyValid && (path.Count == 0 || isPathClear(path, state));

                return currentlyValid && !checkForSelfCheck();
            } else if (pieceCharUpper == PieceCharacters.Queen)
            {
                bool currentlyValid = isMoveVerticalOrHorizontal(location1, location2, state) || isMoveDiagonal(location1, location2, state);

                currentlyValid = currentlyValid && (path.Count == 0 || isPathClear(path, state));

                return currentlyValid && !checkForSelfCheck();
            }
            else if (pieceCharUpper == PieceCharacters.King)
            {
                bool currentlyValid = isMoveOneAway(location1, location2, state);

                return currentlyValid && !checkForSelfCheck();
            }
            return true;
        }

        public static bool isMoveYours(string location1, GameState state)
        {
            int[] moveFromLoc = convertAlgebraicToPoint(location1);
            return Char.IsUpper(state.currentBoard[moveFromLoc[0], moveFromLoc[1]]) && state.blackTurn || Char.IsLower(state.currentBoard[moveFromLoc[0], moveFromLoc[1]]) && !state.blackTurn;
        }

        public static bool checkForSelfCheck()
        {
            return false;
        }

        public static bool isInCheckBlack(GameState state)
        {
            return isInCheck(state, 'K');
        }

        public static bool isInCheckWhite(GameState state)
        {
            return isInCheck(state, 'k');
        }

        private static bool isInCheck(GameState state, char kingPiece)
        {
            int i, j;
            bool found = false;
            for (i = 0; i < state.currentBoard.GetLength(0); i++)
            {
                for (j = 0; j < state.currentBoard.GetLength(1); j++)
                {
                    if (state.currentBoard[i,j] == kingPiece)
                    {
                        found = true; 
                        break;
                    }
                    if (found)
                        break;
                }
            }

            bool check = false;

            for (i = 0; i < state.currentBoard.GetLength(0); i++)
            {
                for (j = 0; j < state.currentBoard.GetLength(1); j++)
                {
                    if (state.currentBoard[i, j] != eSC && Char.IsLower(state.currentBoard[i, j]) != Char.IsLower(kingPiece) && isInCheckBy(state, convertPointToAlgebraic(new int[] { i, j }), kingPiece))
                    {
                        check = true;
                        break;
                    }
                    if (check)
                        break;
                }
            }

            return check;
        }

        public static bool isBlackInCheckBy(GameState state, string byLocation)
        {
            return isInCheckBy(state, byLocation, 'K');
        }

        public static bool isWhiteInCheckBy(GameState state, string byLocation)
        {
            return isInCheckBy(state, byLocation, 'k');
        }

        private static bool isInCheckBy(GameState state, string byLocation, char kingPiece)
        {
            int[] byLoc = convertAlgebraicToPoint(byLocation);
            char byPiece = state.currentBoard[byLoc[0], byLoc[1]];

            if (byPiece == eSC)
                return false;

            int i, j = 0;
            bool found = false;
            for (i = 0; i < state.currentBoard.GetLength(0); i++)
            {
                for (j = 0; j < state.currentBoard.GetLength(1); j++)
                {
                    if (state.currentBoard[i, j] == kingPiece)
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
            }

            if (Char.IsLower(byPiece) == Char.IsLower(kingPiece))
                return false;

            string kingLocation = convertPointToAlgebraic(new int[] { i, j });

            return isMoveLegal(byLocation, kingLocation, state);
        }
    }
}