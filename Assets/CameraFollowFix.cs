using UnityEngine;
using Unity.Cinemachine;
public class CameraFollowFix : MonoBehaviour
{
    [System.Obsolete]
    public CinemachineCamera virtualCamera;

    [System.Obsolete]
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt = player.transform; // optional
        }
    }
}