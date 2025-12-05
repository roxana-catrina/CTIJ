using UnityEngine;

public class PlayerLifecycleDebug : MonoBehaviour
{
    void Awake()
    {
        Debug.Log($"[LIFECYCLE] Player Awake: {gameObject.name} (InstanceID: {GetInstanceID()})");
    }

    void Start()
    {
        Debug.Log($"[LIFECYCLE] Player Start: {gameObject.name}");
    }

    void OnEnable()
    {
        Debug.Log($"[LIFECYCLE] Player OnEnable: {gameObject.name}");
    }

    void OnDisable()
    {
        Debug.Log($"[LIFECYCLE] Player OnDisable: {gameObject.name}");
        // Log stack trace to see who disabled it
        // Debug.Log(StackTraceUtility.ExtractStackTrace()); 
    }

    void OnDestroy()
    {
        Debug.Log($"[LIFECYCLE] Player OnDestroy: {gameObject.name}");
        Debug.Log("Stack Trace: " + System.Environment.StackTrace);
    }
}
