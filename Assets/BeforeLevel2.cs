using UnityEngine;
using UnityEngine.SceneManagement;

public class BeforeLevel2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    public void StartLevel2()
    {
        SceneManager.LoadScene("Level 2");
        Debug.Log("Monede colectate: " + CoinManager.instance.coinsCollected);
    }

    public void BuyItem1()
    {
        if (CoinManager.instance.BuyItem1(5))
        {
            StartCoroutine(CoinManager.instance.ReassignUI());
            Debug.Log("Item 1 cumpărat cu succes! Total: " + CoinManager.instance.item1 + "/2");
        }
        else
        {
            if (CoinManager.instance.item1 >= 2)
            {
                Debug.Log("Ai cumpărat deja Item 1 de 2 ori (maxim)!");
            }
            else
            {
                Debug.Log("Nu ai suficiente monede pentru Item 1!");
            }
        }
    }
    
    public void BuyItem2()
    {
        if (CoinManager.instance.BuyItem2(5))
        {
            StartCoroutine(CoinManager.instance.ReassignUI());
            Debug.Log("Item 2 cumpărat cu succes! Total: " + CoinManager.instance.item2 + "/2");
        }
        else
        {
            if (CoinManager.instance.item2 >= 2)
            {
                Debug.Log("Ai cumpărat deja Item 2 de 2 ori (maxim)!");
            }
            else
            {
                Debug.Log("Nu ai suficiente monede pentru Item 2!");
            }
        }
    }

}

