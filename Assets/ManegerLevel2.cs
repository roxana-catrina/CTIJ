using UnityEngine;

public class ManegerLevel2 : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform startPoint;
    public GameManager spear;
    void Awake()
    {
        if (FindFirstObjectByType<PlayerMovement>()== null)
        {
            Instantiate(playerPrefab);
        }

        startPoint = GameObject.Find("StartPoint").transform;
        spear.SetActive(false);

    }
}
