using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetBetAndPlayerScore : MonoBehaviour
{
    [SerializeField]
    private Text betAmountText;

    [SerializeField]
    private Text PlayerScoreText;

    private void Start()
    {
        betAmountText.text = "" + BettingHandler.Bet;
        PlayerScoreText.text = "" + BettingHandler.TotalScore;
    }
    public void getBetAmount()
    {
        betAmountText.text = ResetHand.betAmount;
    }
    public void getPlayerScore()
    {
        PlayerScoreText.text = ResetHand.playerScoreAmount;
    }

}
