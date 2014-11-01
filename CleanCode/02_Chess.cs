using System.Linq;
using NUnit.Framework;

namespace CleanCode
{
    public class Chess
    {
        private readonly Board board;

        public Chess(Board board)
        {
            this.board = board;
        }

        public string GetWhiteStatus()
        {
            var whiteUnderAttack = IsCheckForWhite();
            var canAvoidCheck = CanAvoidCheck();
            return GetGameState(whiteUnderAttack, canAvoidCheck);
        }

        private static string GetGameState(bool whiteUnderAttack, bool canAvoidCheck)
        {
            if (whiteUnderAttack)
                return canAvoidCheck ? "check" : "mate";
            return canAvoidCheck ? "ok" : "stalemate";
        }

        private bool CanAvoidCheck()
        {
            return board.Figures(Cell.White)
                .Any(cell => board.Get(cell).Figure.Moves(cell, board).Any(move => !UnderAttack(cell, move)));
        }

        private bool UnderAttack(Loc cell, Loc move)
        {
            var isUnderAttack = false;
            var oldLocation = board.PerformMove(cell, move);
            if (IsCheckForWhite())
                isUnderAttack = true;
            board.PerformUndoMove(cell, move, oldLocation);
            return isUnderAttack;
        }

        private bool IsCheckForWhite()
        {
            return board.Figures(Cell.Black)
                .Any(
                    location =>
                        board.Get(location).Figure.Moves(location, board).Any(move => board.Get(move).IsWhiteKing));
        }
    }
}