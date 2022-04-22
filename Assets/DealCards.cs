using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        hearts = Resources.LoadAll<Texture2D>("Heart");
        clubs = Resources.LoadAll<Texture2D>("Club");
        spades = Resources.LoadAll<Texture2D>("Spade");
        diamonds = Resources.LoadAll<Texture2D>("Diamond");

        dealtCards = new HashSet<int>();


        for(int i = 0; i < 2; i++)
        {
            DealCard(playerHand);
            DealCard(dealerHand);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void DealCard(GameObject hand){
        int card;

        do {
            card = Random.Range(0, 51);
        } while(dealtCards.Contains(card));

        dealtCards.Add(card);


        CreateCard(card, hand);
    }

    private void CreateCard(int card, GameObject hand){
        int suit = card % 4;
        int value = card % 13;

        GameObject newCard = Instantiate(cardTemplate, this.transform);

        newCard.transform.parent = hand.transform;

        if(hand.transform.childCount == 2)
        {
            newCard.transform.position = new Vector3(5, hand.transform.position.y);
        } else
        {
            newCard.transform.position = new Vector3(-5, hand.transform.position.y);
        }

        Texture2D texture = ChooseTexture(suit, value);

        newCard.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 300);
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
}
