using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DealCards : MonoBehaviour
{
    private HashSet<int> dealtCards;

    [SerializeField]
    private GameObject cardTemplate;

    [SerializeField]
    private GameObject playerHand;
    [SerializeField]
    private GameObject dealerHand;

    [SerializeField]
    private GameObject playerHandTemplate;

    [SerializeField]
    private GameObject playerHands;

    [SerializeField]
    private Text payoutText;

    private Texture2D[] hearts;
    private Texture2D[] clubs;
    private Texture2D[] spades;
    private Texture2D[] diamonds;

    private bool playerDone = false;

    private bool handDone = false;

    int activeHand = 0;
    // Start is called before the first frame update
    void Start()
    {
        payoutText.text = "";
        hearts = Resources.LoadAll<Texture2D>("Heart");
        clubs = Resources.LoadAll<Texture2D>("Club");
        spades = Resources.LoadAll<Texture2D>("Spade");
        diamonds = Resources.LoadAll<Texture2D>("Diamond");

        dealtCards = new HashSet<int>();

        GetActiveHand().SetActive(GetActiveHand().GetComponent<EvalHand>().active);

        DealInitialHands();

        //StartCoroutine(HandlePlayerHands());
    }

    private GameObject GetActiveHand()
    {
        return playerHands.transform.GetChild(activeHand).gameObject;
    }

    private void DealInitialHands()
    {
        for (int i = 0; i < 2; i++)
        {
            DealCard(playerHand, false);
            DealCard(dealerHand, true);
        }

        int firstCardVal = dealerHand.GetComponent<EvalHand>().cards[0].GetComponent<CardValue>().Value;
        
        if (firstCardVal == 0 || firstCardVal >= 9)
        {
            FlipUpsideDownCard();
        }

        if(EvalHand.GetHighestValidValue(dealerHand.GetComponent<EvalHand>().getHandValue()) == 21)
        {
            playerDone = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        DisplayActiveHand();
        if (!playerDone)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DealCard(GetActiveHand(), false);
                if (EvalHand.HandBusted(GetActiveHand().GetComponent<EvalHand>().getHandValue()).Item1)
                {
                    NextHand();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) && GetActiveHand().GetComponent<EvalHand>().canSplit)
            {
                SplitHand(GetActiveHand());
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                NextHand();
            }
        }
        else
        {
            if (!handDone)
            {
                HandleDealer();
            } else
            {
                activeHand = 0;

                GetActiveHand().GetComponent<EvalHand>().active = true;

                DisplayActiveHand();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetHand();
            }
        }
    }

    private void DisplayActiveHand()
    {
        foreach (Transform hand in playerHands.transform)
        {
            GameObject handObj = hand.gameObject;
            handObj.SetActive(handObj.GetComponent<EvalHand>().active);
            foreach (Transform card in hand)
            {
                GameObject cardObj = card.gameObject;
                cardObj.SetActive(handObj.GetComponent<EvalHand>().active);
            }
        }
    }

    private void NextHand()
    {
        GetActiveHand().GetComponent<EvalHand>().active = false;
        activeHand++;
        if (activeHand >= playerHands.transform.childCount) playerDone = true;
        else GetActiveHand().GetComponent<EvalHand>().active = true;
    }

    private void SplitHand(GameObject hand)
    {
        GameObject hand2 = Instantiate(playerHandTemplate, playerHands.transform);
        hand2.GetComponent<EvalHand>().active = false;

        GameObject card = hand.GetComponent<EvalHand>().cards[1];

        hand2.GetComponent<EvalHand>().cards.Add(card);
        card.transform.parent = hand2.transform;
        hand.GetComponent<EvalHand>().cards.Remove(card);
        card.SetActive(false);

        card.transform.position = new Vector3(hand2.transform.childCount + hand2.transform.position.x, hand2.transform.position.y);


        DealCard(hand, false);
        DealCard(hand2, false);
    }

    private void ResetHand()
    {
        SceneManager.LoadScene("Scenes/ResetScene");
    }

    private void HandleDealer()
    {
        bool allBusted = true;

        foreach (Transform hand in playerHands.transform)
        {
            if (!EvalHand.HandBusted(hand.gameObject.GetComponent<EvalHand>().getHandValue()).Item1)
            {
                allBusted = false;
                break;
            }
        }

        if (!allBusted)
        { 
            FlipUpsideDownCard();

            int dealerVal = EvalHand.GetHighestValidValue(dealerHand.GetComponent<EvalHand>().getHandValue());

            while (dealerVal <= 16)
            {
                DealCard(dealerHand, true);
                dealerVal = EvalHand.GetHighestValidValue(dealerHand.GetComponent<EvalHand>().getHandValue());
            }
        }

        handDone = true;

        activeHand = 0;

        string payoutStr = "";
        float finalPayout = 0;

        while(activeHand < playerHands.transform.childCount)
        {
            int result = EvalHand.CompareHand(GetActiveHand(), dealerHand);
            (string, float) payoutRes = BettingHandler.Payout(result);
            string resultStr = payoutRes.Item1;
            float payoutAmmount = payoutRes.Item2;
            
            payoutStr += "Hand " + (activeHand + 1) + ": " + resultStr;

            if (result == 1)
            {
                payoutStr += " | Lost " + payoutAmmount + "\n";
                finalPayout -= payoutAmmount;
            } else if(result == 0)
            {
                payoutStr += " | Won " + payoutAmmount + "\n";
                finalPayout += payoutAmmount;
            }
            activeHand++;
        }

        payoutStr += "Total payout: " + finalPayout;

        payoutText.text = payoutStr;
    }

    private void FlipUpsideDownCard()
    {
        GameObject upsideDownCard = dealerHand.GetComponent<EvalHand>().cards[1];
        int value = upsideDownCard.GetComponent<CardValue>().Value;
        int suit = upsideDownCard.GetComponent<CardValue>().Suit;
        Texture2D texture = ChooseTexture(suit, value);

        upsideDownCard.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 300);
    }

    private void DealCard(GameObject hand, bool dealer){
        int card;

        do {
            card = Random.Range(0, 51);
        } while(dealtCards.Contains(card));

        dealtCards.Add(card);


        CreateCard(card, hand, dealer);
    }

    private void CreateCard(int card, GameObject hand, bool dealer){
        int suit = card % 4;
        int value = card % 13;

        GameObject newCard = Instantiate(cardTemplate, hand.transform);

        newCard.GetComponent<CardValue>().Value = value;
        newCard.GetComponent<CardValue>().Suit = suit;
        newCard.SetActive(hand.GetComponent<EvalHand>().active);

        newCard.transform.position = new Vector3(hand.transform.childCount + hand.transform.position.x, hand.transform.position.y);

        Texture2D texture;

        if (dealer && hand.transform.childCount == 2)
        {
            texture = Resources.Load<Texture2D>("BackColor_Red");
        }
        else
        {
            texture = ChooseTexture(suit, value);
        }

        newCard.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 300);

        hand.GetComponent<EvalHand>().cards.Add(newCard);
    }

    private Texture2D ChooseTexture(int suit, int value)
    {

        switch (suit)
        {
            case 0:
                return hearts[value];
            case 1:
                return clubs[value];
            case 2:
                return diamonds[value];
            default:
                return spades[value];
        }
    }

    /*private IEnumerator HandlePlayerHands()
    {
        //HandlePlayer();
        yield return null;
        Debug.Log("Waiting");
    }*/
}
