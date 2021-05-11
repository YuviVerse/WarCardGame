using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    [SerializeField] private Card[] cards = new Card[54];
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Deck[] playersDecks = new Deck[2];
    [SerializeField] private GameObject playerPlaceholder;
    [SerializeField] private GameObject AIPlaceholder;
    [SerializeField] private Animator UiAnimator;
    [SerializeField] private GameObject gameEndingScreen;

    private Stack<Card>[] onBoardDecks = new Stack<Card>[2];
    private bool war = false;
    

    void Start()
    {
        gameEndingScreen.SetActive(false);
        ShuffleDeck(cards);
        DivideCards();
        CreateOnBoardDecks();
    }

    void ShuffleDeck(Card[] cards)
    {
        Card tempCard;
        for (int i = 0; i < cards.Length; i++)
        {
            int rnd = Random.Range(0, cards.Length);
            tempCard = cards[rnd];
            cards[rnd] = cards[i];
            cards[i] = tempCard;
        }
    }

    void DivideCards()
    {
        //Player is 0, AI is 1
        for (int i = 0; i < cards.Length; i++)
        {
            playersDecks[i % playersDecks.Length].deck.Enqueue(cards[i]);
        }
    }

    public void OnPlayerDraw()
    {
        Card newCard;

        //Player is 0, AI is 1
        for (int i = 0; i < playersDecks.Length; i++)
        {
            newCard = playersDecks[i % playersDecks.Length].DrawACard(war);
            onBoardDecks[i % playersDecks.Length].Push(newCard);
        }
    }

    //Getting called after every card draw animation
    public void checkForWinner()
    {
        int playerCardValue = onBoardDecks[0].Peek().value;
        int AICardValue = onBoardDecks[1].Peek().value;
        if (!war)
        {
            if (playerCardValue == AICardValue)
            {
                UiAnimator.Play("WAR");
                war = true;
            }
            else
            {
                foreach (var deck in onBoardDecks)
                {
                    while (deck.Count > 0)
                    {
                        if (playerCardValue > AICardValue)
                        {
                            playersDecks[0].deck.Enqueue(deck.Pop());
                            UiAnimator.Play("PlayerWon"); //Call CleanBoard at the end
                            CleanBoard("Player");
                        }
                        else
                        {
                            playersDecks[1].deck.Enqueue(deck.Pop());
                            UiAnimator.Play("PlayerLost"); //Call CleanBoard at the end
                            CleanBoard("AI");
                        }
                    } 
                }
            }
        }
        else
        {
            war = false;
        }


        if (playersDecks[0].deck.Count == 0)
        {
            playersDecks[0].OutOfCards();
            gameEndingScreen.GetComponentInChildren<Text>().text = "You Won";
            UiAnimator.Play("GameFinished");

        }
        else if (playersDecks[1].deck.Count == 0)
        {
            playersDecks[1].OutOfCards();
            gameEndingScreen.GetComponentInChildren<Text>().text = "You Lose";
            UiAnimator.Play("GameFinished");
        }

    }

    public void CleanBoard(string winner)
    {
        foreach (Transform child in playerPlaceholder.transform)
        {
            if (winner == "Player")
            {
                child.gameObject.GetComponent<Animator>().Play("PlayerTakeCards");
            }
            else
            {
                child.gameObject.GetComponent<Animator>().Play("AITakeCards");
            }
        }
        foreach (Transform child in AIPlaceholder.transform)
        {
            if (winner == "Player")
            {
                child.gameObject.GetComponent<Animator>().Play("PlayerTakeCards");
            }
            else
            {
                child.gameObject.GetComponent<Animator>().Play("AITakeCards");
            }
        }
    }

    void CreateOnBoardDecks()
    {
        for (int i = 0; i < onBoardDecks.Length; i++)
        {
            onBoardDecks[i] = new Stack<Card>();
        }
    }
}
