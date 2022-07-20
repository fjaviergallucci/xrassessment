using System;
using System.Collections;
using System.Collections.Generic;
using TicTacToe;
using TMPro;
using UnityEngine;

public class EndMessage : MonoBehaviour
{

	[SerializeField]
	private TMP_Text _playerMessage = null;

	public void OnGameEnded(TicTacToeState winner)
	{
		string winnerString;
		switch (winner)
		{
			case TicTacToeState.circle:
				winnerString = "Circle wins";
                
				break;
			case TicTacToeState.cross:
				winnerString = "Cross wins";
				break;
            
			default:
				winnerString = "Tie";
				break;
		}
		_playerMessage.text = $"{winnerString}";
	}
}
