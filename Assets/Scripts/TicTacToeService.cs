using UnityEngine.Events;

namespace TicTacToe
{
    public enum TicTacToeState { none, cross, circle, tie }
    
    public class TicTacToeService
    {
        private TicTacToeState[,] _board;
        private short _movements;

        public TicTacToeService()
        {
            _board = new TicTacToeState[3, 3]
            {
                { TicTacToeState.none, TicTacToeState.none, TicTacToeState.none },
                { TicTacToeState.none, TicTacToeState.none, TicTacToeState.none },
                { TicTacToeState.none, TicTacToeState.none, TicTacToeState.none }
            };
            this._movements = 0;
        }

        public void TileSelected(int coordX, int coordY, TicTacToeState playerState)
        {
            // if (IsSelected(coordX, coordY)) return false;
            this._board[coordX, coordY] = playerState;
            this._movements++;
        }

        // private bool IsSelected(int coordX, int coordY)
        // {
        //     return !this._board[coordX, coordY].Equals(TicTacToeState.none);
        // }

        public TicTacToeState GameOver()
        {
            /* Minimum 5 movements to check for winner */
            if (this._movements < 5)
                return TicTacToeState.none;

            TicTacToeState winner = CheckHorizontal();

            if (!winner.Equals(TicTacToeState.none))
                return winner;

            winner = CheckVertical();
            if (!winner.Equals(TicTacToeState.none))
                return winner;

            winner = CheckDiagonal();
            if (!winner.Equals(TicTacToeState.none))
                return winner;

            /* If max movements and no winners, then is a tie */
            if (this._movements >= 9)
                return TicTacToeState.tie;

            return TicTacToeState.none;
        }

        private TicTacToeState CheckHorizontal()
        {
            for (int row = 0; row < 3; row++)
            {
                if (!this._board[row, 0].Equals(TicTacToeState.none) && this._board[row, 0].Equals(this._board[row, 1]) && this._board[row, 0].Equals(this._board[row, 2]))
                {
                    return this._board[row, 0];
                }
            }

            return TicTacToeState.none;
        }

        private TicTacToeState CheckVertical()
        {
            for (int column = 0; column < 3; column++)
            {
                if (!this._board[0, column].Equals(TicTacToeState.none) && this._board[0, column].Equals(this._board[1, column]) &&
                    this._board[0, column].Equals(this._board[2, column]))
                {
                    return this._board[0, column];
                }
            }

            return TicTacToeState.none;
        }

        private TicTacToeState CheckDiagonal()
        {
            if (!this._board[0, 0].Equals(TicTacToeState.none) && this._board[0, 0].Equals(this._board[1, 1]) && this._board[0, 0].Equals(this._board[2, 2]))
            {
                return this._board[0, 0];
            }

            if (!this._board[0, 2].Equals(TicTacToeState.none) && this._board[0, 2].Equals(this._board[1, 1]) && this._board[0, 2].Equals(this._board[2, 0]))
            {
                return this._board[0, 2];
            }

            return TicTacToeState.none;
        }
    }

    /**
     * selecciona una casilla
     * verifica horizontal, vertical diagonal
     * Horizontal: x-1 y x+1 hasta que x = 0 y x=2
     * vertical: y-1 y y+1 hasta que y = 0 y y=2
     * diagonal: x+1&&y+1 hasta x y y igual 2... x-1&&y-1 hasta x y y igual 0... 
     * 
     * 
     */
}