using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public Image artwork;
    public int value;
    public GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void LoadData(bool war) 
    {
        if (!war)
        {
            artwork.sprite = card.artwork;
        }
        else
        {
            artwork.sprite = card.cardBack;
        }
        value = card.value;
    }

    public void checkBoard()
    {
        gameManager.checkForWinner();
    }


    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }

}
