using System;
using System.Collections.Generic;
using TicTacToe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class TileClickTrigger : MonoBehaviour
{
    private TicTacToeManager _gameManager;

    [SerializeField] private int myCoordX = 0;
    [SerializeField] private int myCoordY = 0;

    [SerializeField] private bool canClick;

    public UnityEvent _selectTile;

    private void Awake()
    {
        _gameManager = FindObjectOfType<TicTacToeManager>();
    }

    private void Start()
    {
        _gameManager.onGameStarted.AddListener(AddReference);
        _gameManager.onGameStarted.AddListener(() => SetInputEndabled(true));
        _gameManager.onPlayerWin.AddListener((win) => SetInputEndabled(false));
        _selectTile.AddListener(OnMouseDown);
    }

    private void SetInputEndabled(bool val)
    {
        canClick = val;
    }

    private void AddReference()
    {
        _gameManager.RegisterTransform(myCoordX, myCoordY, this);
        canClick = true;
    }

    private void OnMouseDown()
    {
        /*If tile selected or is not player turn, return*/
        if (!canClick || !_gameManager.isPlayerTurn) return;
        TileSelected();
    }

    public void TileSelected()
    {
        if (_gameManager.TrySelect(myCoordX, myCoordY))
        {
            this.canClick = false;
        }
    }
}