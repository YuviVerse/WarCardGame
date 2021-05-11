using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject cardsPlaceholder;

    public Queue<Card> deck = new Queue<Card>();
    private GameObject newCard;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public Card DrawACard(bool war)
    {
        CardDisplay cardDisplayScript = cardPrefab.GetComponent<CardDisplay>();
        cardDisplayScript.card = deck.Dequeue();
        cardDisplayScript.LoadData(war);
        newCard = Instantiate(cardPrefab,cardsPlaceholder.transform);
        if(gameObject.tag == "Player")
        {
            newCard.GetComponent<Animator>().Play("PlayerCardDraw");
        }
        else
        {
            newCard.GetComponent<Animator>().Play("AICardDraw");
        }
        return cardDisplayScript.card;
    }
    
    public void OutOfCards()
    {
        Image image = GetComponent<Image>();
        Color tempColor = image.color;
        tempColor.a = 0f;
        image.color = tempColor;
    }

    public void ActiveButtonInteraction()
    {
        StartCoroutine("GetReady");
    }

    IEnumerator GetReady()
    {
        yield return new WaitForSeconds(3);
        button.interactable = true;
    }
}
