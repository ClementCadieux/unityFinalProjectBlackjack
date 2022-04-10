using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNewCard : MonoBehaviour
{
    public GameObject card;
    public GameObject hand;

    private GameObject inGameHand;
    private int cardCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        inGameHand = Instantiate(hand, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)){
            GameObject newCard = Instantiate(card, new Vector3(-8 + cardCount * 1.5f, 0, 0), Quaternion.identity);
            newCard.transform.parent = inGameHand.transform;
            cardCount++;
        }

        if(Input.GetKeyDown(KeyCode.D)){
            Destroy(inGameHand);
            inGameHand = Instantiate(hand, new Vector3(0, 0, 0), Quaternion.identity);
            cardCount = 0;
        }
    }
}
