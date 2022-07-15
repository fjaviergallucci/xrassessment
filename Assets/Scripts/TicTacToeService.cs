public class TicTacToeService
{
    private TicTacToeState[,] board;
    private short movements;

    public TicTacToeService()
    {
        board = new TicTacToeState[3, 3] { { TicTacToeState.none, TicTacToeState.none, TicTacToeState.none }, { TicTacToeState.none, TicTacToeState.none, TicTacToeState.none }, { TicTacToeState.none, TicTacToeState.none, TicTacToeState.none } };
        this.movements = 0;
    }

    public void TileSelected(int coordX, int coordY, TicTacToeState playerState)
    {
        if (this.board[coordX, coordY].Equals(TicTacToeState.none))
        {
            this.board[coordX, coordY] = playerState;
            this.movements++;
        }
    }

    public bool isSelected(int coordX, int coordY)
    {
        return !this.board[coordX, coordY].Equals(TicTacToeState.none);
    }

    public TicTacToeState gameOver()
    {
        /* Minimum 5 movements to check for winner */
        if (this.movements < 5)
            return TicTacToeState.none;

        TicTacToeState Winner = checkHorizontal();

        if (!Winner.Equals(TicTacToeState.none))
            return Winner;

        Winner = checkVertical();
        if (!Winner.Equals(TicTacToeState.none))
            return Winner;

        Winner = checkDiagonal();
        if (!Winner.Equals(TicTacToeState.none))
            return Winner;

        /* If max movements and no winners, then is a tie */
        if (this.movements >= 9)
            return TicTacToeState.tie;

        return TicTacToeState.none;
    }

    private TicTacToeState checkHorizontal()
    {
        for (int row = 0; row < 3; row++)
        {
            if (this.board[row, 0].Equals(this.board[row, 1]) && this.board[row, 0].Equals(this.board[row, 2]))
            {
                return this.board[row, 0];
            }
        }
        return TicTacToeState.none;
    }

    private TicTacToeState checkVertical()
    {
        for (int column = 0; column < 3; column++)
        {
            if (this.board[0, column].Equals(this.board[1, column]) && this.board[0, column].Equals(this.board[2, column]))
            {
                return this.board[0, column];
            }
        }
        return TicTacToeState.none;
    }

    private TicTacToeState checkDiagonal()
    {
        if (this.board[0, 0].Equals(this.board[1, 1]) && this.board[0, 0].Equals(this.board[2, 2]))
        {
            return this.board[0, 0];
        }
        if (this.board[0, 2].Equals(this.board[1, 1]) && this.board[0, 2].Equals(this.board[2, 0]))
        {
            return this.board[0, 2];
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