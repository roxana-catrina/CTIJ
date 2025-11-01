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
        if (CoinManager.instance.BuyItem1(1))
        {
            StartCoroutine(CoinManager.instance.ReassignUI());
            Debug.Log("Item 1 cumpărat!");


        }
        else
        {
            Debug.Log("Nu ai suficiente monede sau ai cumpărat deja acest item!");
        }
    }
     public void BuyItem2()
    {
        if (CoinManager.instance.BuyItem2(1))
        {
            StartCoroutine(CoinManager.instance.ReassignUI());
            Debug.Log("Item 1 cumpărat!");
           

        }
        else
        {
            Debug.Log("Nu ai suficiente monede sau ai cumpărat deja acest item!");
        }
    }

}

