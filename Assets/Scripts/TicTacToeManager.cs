using System;
using TicTacToe;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[System.Serializable]
public class WinnerEvent : UnityEvent<TicTacToeState>
{
}

public class TicTacToeManager : MonoBehaviour
{
    private TicTacToeService _ticTacToeService;
    public int aiLevel;
    public bool isPlayerTurn;

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
        this.aiLevel = aiLevel;
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
        
        /* If Game finished */
        if (IsGameOver()) return true;
        
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

    private bool IsGameOver()
    {
        /*ToDo: Refactor */ 
        TicTacToeState winner = _ticTacToeService.GameOver();
        if (winner.Equals(TicTacToeState.none)) return false;

        onPlayerWin.Invoke(winner);
        return true;
    }

    private void ChangePlayer()
    {
        /*ToDo: Refactor with random player method */
        //By default player is always active
        isPlayerTurn = true;
        var recentPlayer = currentPlayerState;
        currentPlayerState =
            (currentPlayerState == TicTacToeState.circle ? TicTacToeState.cross : TicTacToeState.circle);
        SetCurrentPlayerVisual();

        /* If AI Turn */
        if (PlayerState.Equals(recentPlayer))
        {
            isPlayerTurn = false;
            StartCoroutine(WaitAIToMove());
        }
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
        //By default player is always active
        isPlayerTurn = true;
        currentPlayerState = Random.value < 0.5f ? PlayerState : AIState;
        SetCurrentPlayerVisual();

        /* If AI Turn */
        if (AIState.Equals(currentPlayerState))
        {
            isPlayerTurn = false;
            StartCoroutine(WaitAIToMove());
        }
    }

    IEnumerator WaitAIToMove()
    {
        yield return new WaitForSeconds(2);
        AiMove();
    }

    private void AiMove()
    {
        /*ToDo: Implement difficulty level */
        (int, int) coords;
        switch (aiLevel)
        {
            case 1:
                coords = RandomMove();
                break;

            case 2:
                coords = RandomMove();
                break;

            default:
                coords = RandomMove();
                break;
        }

        _tileClickTriggers[coords.Item1, coords.Item2].TileSelected();
    }

    private (int, int) RandomMove()
    {
        int coordX, coordY;
        do
        {
            coordX = Random.Range(0, 3);
            coordY = Random.Range(0, 3);
        } while (_ticTacToeService.IsSelected(coordX, coordY));

        return (coordX, coordY);
    }
}