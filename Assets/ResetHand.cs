using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetHand : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        slider.minValue = 1;
        slider.maxValue = BettingHandler.TotalScore;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Total score: " + BettingHandler.TotalScore + "\nBet: " + slider.value;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BettingHandler.Bet = slider.value;
            SceneManager.LoadScene("Scenes/SampleScene");

        }
    }

    public static string betAmount = "" + BettingHandler.Bet;
    public static string playerScoreAmount = "" + BettingHandler.TotalScore;

    public void BetAmount()
    {
        text.text = "Total score: " + BettingHandler.TotalScore + "\nBet: " + slider.value;
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
            BettingHandler.Bet = slider.value;
            SceneManager.LoadScene("Scenes/SampleScene");
        //}
    }
}