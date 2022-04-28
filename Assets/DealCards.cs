using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DealCards : MonoBehaviour
{
    private HashSet<int> dealtCards;

    [SerializeField]
    private GameObject cardTemplate;

    [SerializeField]
    private GameObject playerHand;
    [SerializeField]
    private GameObject dealerHand;
    private Texture2D[] hearts;
    private Texture2D[] clubs;
    private Texture2D[] spades;
    private Texture2D[] diamonds;

    private bool playerDone = false;

    private bool playerBusted = false;

    private bool handDone = false;

    // Start is called before the first frame update
    void Start()
    {
        hearts = Resources.LoadAll<Texture2D>("Heart");
        clubs = Resources.LoadAll<Texture2D>("Club");
        spades = Resources.LoadAll<Texture2D>("Spade");
        diamonds = Resources.LoadAll<Texture2D>("Diamond");

        dealtCards = new HashSet<int>();



        DealInitialHands();

        //StartCoroutine(HandlePlayerHands());
    }

    /*private void HandlePlayer()
    {
        int activeHand = 0;

        while(activeHand < playerHand.transform.childCount)
        {
            HandleHand(activeHand);
            activeHand++;
        }
    }*/

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
        if (!playerDone)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DealCard(playerHand, false);
                if (EvalHand.HandBusted(playerHand.GetComponent<EvalHand>().getHandValue()).Item1)
                {
                    playerDone = true;
                    playerBusted = true;
                    //payout dealer, end hand;
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                playerDone = true;
            }
        }
        else
        {
            if (!handDone)
            {
                HandleDealer();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetHand();
            }
        }
    }

    /*private void SplitHand()
    {
        GameObject hand = playerHand.transform.GetChild(0).gameObject;
        GameObject hand2 = Instantiate(playerHandTemplate, playerHand.transform);

        hand.transform.position = new Vector3(-3, hand.transform.position.y);

        hand2.transform.position = new Vector3(3, hand.transform.position.y);

        hand2.GetComponent<EvalHand>().cards.Add(hand.GetComponent<EvalHand>().cards[1]);
        hand.GetComponent<EvalHand>().cards[1].transform.parent = hand2.transform;
        hand.GetComponent<EvalHand>().cards.Remove(hand.GetComponent<EvalHand>().cards[1]);

        DealCard(hand, false);
        DealCard(hand2, false);
    }*/

    /*private void HandleHand(int activeHand)
    {
        bool handDone = false;
        GameObject hand = playerHand.transform.GetChild(activeHand).gameObject;
        while (!handDone)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DealCard(hand, false);
                if (EvalHand.HandBusted(hand.GetComponent<EvalHand>().getHandValue()).Item1)
                {
                    playerDone = true;
                    playerBusted = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                handDone = true;
            }
        }
    }*/

    private void ResetHand()
    {
        /*playerDone = false;
        playerBusted = false;
        handDone = false;

        foreach (Transform card in playerHand.transform)
        {
            Destroy(card.gameObject);
        }

        foreach (Transform card in dealerHand.transform)
        {
            Destroy(card.gameObject);
        }

        
        Destroy(playerHand);
        playerHand = Instantiate(playerHand);

        Destroy(dealerHand);
        dealerHand = Instantiate(dealerHand);
        

        playerHand.GetComponent<EvalHand>().cards.Clear();
        dealerHand.GetComponent<EvalHand>().cards.Clear();*/
        
        SceneManager.LoadScene("Scenes/ResetScene");
    }

    private void HandleDealer()
    {
        if (playerBusted)
        {
            Debug.Log(EvalHand.CompareHand(playerHand, dealerHand));
            handDone = true;
            return;
        }

        FlipUpsideDownCard();

        int dealerVal = EvalHand.GetHighestValidValue(dealerHand.GetComponent<EvalHand>().getHandValue());

        while (dealerVal <= 16)
        {
            DealCard(dealerHand, true);
            dealerVal = EvalHand.GetHighestValidValue(dealerHand.GetComponent<EvalHand>().getHandValue());
        }

        handDone = true;

        Debug.Log(EvalHand.CompareHand(playerHand, dealerHand));
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
