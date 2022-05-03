using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingHandler
{
    public static float TotalScore = 100;

    public static float Bet;

    public static (string, float) Payout(int handResult)
    {
        switch (handResult)
        {
            case 0:
                float res = Bet / 2;
                TotalScore += res;
                return ("Player win!", res);
            case 1:
                res = Bet;
                TotalScore -= res;
                return ("Dealer win.", res);
            default:
                res = 0;
                return ("Draw", res);
        }
    }
}
