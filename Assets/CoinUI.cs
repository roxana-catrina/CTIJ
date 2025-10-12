using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    void Update()
    {
        coinText.text = "Coins: " + 
            CoinManager.instance.coinsCollected;
    }
}