using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvalHand : MonoBehaviour
{
    public List<GameObject> cards;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public List<int> getHandValue()
    {
        List<int> handVals = new List<int>();
        handVals.Add(0);

        foreach (GameObject card in cards)
        {
            int cardVal = card.GetComponent<CardValue>().Value;

            if (cardVal >= 10)
            {
                for(int i = 0; i < handVals.Count; i++)
                {
                    handVals[i] += 10;
                }
            }
            else if (cardVal == 0)
            {
                handVals.Add(handVals[0]);
                handVals[0] += 1;

                for (int i = 1; i < handVals.Count; i++)
                {
                    handVals[i] += 11;
                }
            }
            else
            {
                for (int i = 0; i < handVals.Count; i++)
                {
                    handVals[i] += cardVal + 1;
                }
            }
        }

        return handVals;
    }

    public static int CompareHand(GameObject playerHand, GameObject dealerHands)
    {
        List<int> playerHandVals = playerHand.GetComponent<EvalHand>().getHandValue();
        List<int> dealerHandVals = dealerHands.GetComponent<EvalHand>().getHandValue();

        (bool, int) playerBusted = HandBusted(playerHandVals);
        (bool, int) dealerBusted = HandBusted(dealerHandVals);

        if (playerBusted.Item1) return 1;
        if (dealerBusted.Item1) return 0;

        int playerHandVal = playerBusted.Item2;
        int dealerHandVal = dealerBusted.Item2;

        if (playerHandVal == dealerHandVal) return 2;

        return playerHandVal > dealerHandVal ? 0 : 1;
    }

    public static (bool, int) HandBusted(List<int> hand)
    {
        int value = GetHighestValidValue(hand);
        return (value > 21, value);
    }

    public static int GetHighestValidValue(List<int> vals)
    {
        int val = vals[0];
        for(int i = 1; i < vals.Count; i++)
        {
            val = vals[i] <= 21 ? Mathf.Max(val, vals[i]) : val;
        }

        return val;
    }
}
