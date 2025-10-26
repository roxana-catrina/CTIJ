using UnityEngine;

public class ManegerLevel2 : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform startPoint;
    void Awake()
    {
        if (FindObjectOfType<PlayerMovement>() == null)
        {
            Instantiate(playerPrefab);
        }

        startPoint = GameObject.Find("StartPoint").transform;

    }
}
