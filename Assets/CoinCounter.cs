using UnityEngine;
using TMPro;  // pentru TextMeshPro

public class CoinCounter : MonoBehaviour
{
    public TextMeshPro coinText;  // referință la textul din UI
    private int coinCount = 0;        // numărul de banuti

    void Start()
    {
        UpdateCoinText();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        coinText.text = "Coins: " + coinCount.ToString();
    }
}
