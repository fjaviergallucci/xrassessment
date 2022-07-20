using TicTacToe;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public class WinnerEvent : UnityEvent<string>
{
}

public class TicTacToeManager : MonoBehaviour
{
    TicTacToeService _ticTacToeService;
    int _aiLevel;

    [SerializeField] private TicTacToeState currentPlayerState;

    private const TicTacToeState PlayerState = TicTacToeState.cross;
    private const TicTacToeState AIState = TicTacToeState.circle;

    private TileClickTrigger[,] _tileClickTriggers;

    [SerializeField] private Transform currentPlayerPoint;

    // [SerializeField] private TMP_Text currentPlayerText;
    private GameObject _currentPlayerObject;

    [SerializeField] private GameObject _xPrefab;
    [SerializeField] private GameObject _oPrefab;

    public UnityEvent onGameStarted;
    public UnityEvent onPlayerChanged;
    //Call This event with the player number to denote the winner
    public WinnerEvent onPlayerWin;
    
    [SerializeField] private GameObject startingPanel;
    [SerializeField] private GameObject gamePanel;

    public void StartAI(int aiLevel)
    {
        _aiLevel = aiLevel;
        StartGame();
    }

    private void StartGame()
    {
        _ticTacToeService = new TicTacToeService();
        _tileClickTriggers = new TileClickTrigger[3, 3];

        SelectRandomPlayer();
        onPlayerChanged.AddListener(ChangePlayer);

        onGameStarted.Invoke();
    }

    private void Awake()
    {
        if (onPlayerWin == null)
        {
            onPlayerWin = new WinnerEvent();
        }
    }

    public void RegisterTransform(int myCoordX, int myCoordY, TileClickTrigger tileClickTrigger)
    {
        _tileClickTriggers[myCoordX, myCoordY] = tileClickTrigger;
    }

    public bool TrySelect(int coordX, int coordY)
    {
        SetVisual(coordX, coordY);
        _ticTacToeService.TileSelected(coordX, coordY, currentPlayerState);
        CheckGameOver();
        onPlayerChanged.Invoke();
        return true;
    }

    private void SetVisual(int coordX, int coordY)
    {
        Instantiate(
            currentPlayerState == TicTacToeState.circle ? _oPrefab : _xPrefab,
            _tileClickTriggers[coordX, coordY].transform.position,
            Quaternion.identity
        );
    }

    private void CheckGameOver()
    {
        TicTacToeState winner = _ticTacToeService.GameOver();
        if (!winner.Equals(TicTacToeState.none))
        {
            onPlayerWin.Invoke(currentPlayerState == TicTacToeState.circle ? "Circle" : "Cross");
        }
    }

    private void ChangePlayer()
    {
        currentPlayerState =
            (currentPlayerState == TicTacToeState.circle ? TicTacToeState.cross : TicTacToeState.circle);
        SetCurrentPlayerVisual();
    }

    private void SetCurrentPlayerVisual()
    {
        // currentPlayerText.text = $"Current player: {(currentPlayerState == TicTacToeState.circle ? "Circle" : "Cross")}";
        Destroy(_currentPlayerObject);
        _currentPlayerObject = Instantiate(
            currentPlayerState == TicTacToeState.circle ? _oPrefab : _xPrefab,
            currentPlayerPoint.position,
            Quaternion.identity
        );
    }

    private void SelectRandomPlayer()
    {
        currentPlayerState = Random.value < 0.5f ? PlayerState : AIState;
        SetCurrentPlayerVisual();
    }
}