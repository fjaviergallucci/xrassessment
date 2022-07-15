using UnityEngine;
using UnityEngine.Events;

public enum TicTacToeState { none, cross, circle }

[System.Serializable]
public class WinnerEvent : UnityEvent<int>
{
}

public class TicTacToeAI : MonoBehaviour
{

    TicTacToeService _ticTacToeService;

    int _aiLevel;

    TicTacToeState[,] boardState;

    [SerializeField]
    private bool _isPlayerTurn;

    [SerializeField]
    private int _gridSize = 3;

    [SerializeField]
    private TicTacToeState playerState = TicTacToeState.cross;
    TicTacToeState aiState = TicTacToeState.circle;

    [SerializeField]
    private GameObject _xPrefab;

    [SerializeField]
    private GameObject _oPrefab;

    public UnityEvent onGameStarted;

    //Call This event with the player number to denote the winner
    public WinnerEvent onPlayerWin;

    ClickTrigger[,] _triggers;

    private void Awake()
    {
        if (onPlayerWin == null)
        {
            onPlayerWin = new WinnerEvent();
        }
    }

    public void StartAI(int AILevel)
    {
        _aiLevel = AILevel;
        StartGame();
    }

    public void RegisterTransform(int myCoordX, int myCoordY, ClickTrigger clickTrigger)
    {
        _triggers[myCoordX, myCoordY] = clickTrigger;
    }

    private void StartGame()
    {
        _ticTacToeService = new TicTacToeService();
        _triggers = new ClickTrigger[3, 3];
        onGameStarted.Invoke();
    }

    public void PlayerSelects(int coordX, int coordY)
    {
        trySelect(coordX, coordY, playerState);
    }

    public void AiSelects(int coordX, int coordY)
    {
        trySelect(coordX, coordY, aiState);
    }

    private bool trySelect(int coordX, int coordY, TicTacToeState targetState)
    {
        if (!_ticTacToeService.isSelected(coordX, coordY))
        {
            this._ticTacToeService.TileSelected(coordX, coordY, targetState);
            SetVisual(coordX, coordY, targetState);
            checkGameOver();
            return true;
        }
        return false;
    }

    private void SetVisual(int coordX, int coordY, TicTacToeState targetState)
    {
        Instantiate(
            targetState == TicTacToeState.circle ? _oPrefab : _xPrefab,
            _triggers[coordX, coordY].transform.position,
            Quaternion.identity
        );
    }

    private void checkGameOver()
    {
        TicTacToeState Winner = this._ticTacToeService.gameOver();
        if (!Winner.Equals(TicTacToeState.none))
        {
            onPlayerWin = null;
            this.Awake();
        }
    }
}
