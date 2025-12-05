using UnityEngine;
using Unity.Cinemachine;

public class CameraFollowFix : MonoBehaviour
{
    public CinemachineCamera virtualCamera;
    private bool isConnected = false;
    private float retryTimer = 0f;

    void Start()
    {
        TryConnect();
    }

    void Update()
    {
        // Dacă nu e conectat, încearcă din nou la fiecare 0.5 secunde
        if (!isConnected)
        {
            retryTimer += Time.deltaTime;
            if (retryTimer > 0.5f)
            {
                TryConnect();
                retryTimer = 0f;
            }
        }
        
        // Verificare suplimentară: dacă am pierdut referința la player
        if (isConnected && virtualCamera != null && virtualCamera.Follow == null)
        {
            isConnected = false;
            Debug.LogWarning("CameraFollowFix: Lost player reference, retrying...");
        }
    }

    void TryConnect()
    {
        if (virtualCamera == null)
        {
            // 1. Încearcă să găsești camera în scenă (Unity 6 / Cinemachine 3.x)
            virtualCamera = FindAnyObjectByType<CinemachineCamera>();
            
            // 2. Dacă nu găsești, caută după tag-ul "MainCamera" și vezi dacă are componenta
            if (virtualCamera == null)
            {
                GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
                if (mainCam != null)
                {
                    virtualCamera = mainCam.GetComponent<CinemachineCamera>();
                }
            }
            
            // 3. Caută orice obiect care conține "CM" sau "vcam" în nume
            if (virtualCamera == null)
            {
                CinemachineCamera[] allCams = FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);
                if (allCams.Length > 0)
                {
                    virtualCamera = allCams[0];
                }
            }

            // 4. Încearcă să găsești și obiecte inactive
            if (virtualCamera == null)
            {
                CinemachineCamera[] allCamsInactive = Resources.FindObjectsOfTypeAll<CinemachineCamera>();
                if (allCamsInactive.Length > 0)
                {
                    foreach (var cam in allCamsInactive)
                    {
                        if (cam.gameObject.scene.rootCount != 0)
                        {
                            virtualCamera = cam;
                            Debug.LogWarning("CameraFollowFix: Found INACTIVE CinemachineCamera: " + cam.gameObject.name + ". Activating it...");
                            cam.gameObject.SetActive(true);
                            break;
                        }
                    }
                }
            }

            // 5. ULTIMA SPERANȚĂ: Caută componenta veche CinemachineVirtualCamera (pentru compatibilitate)
            // Uneori Unity face upgrade automat dar scripturile vechi rămân
            if (virtualCamera == null)
            {
                // Folosim Reflection pentru a nu avea erori de compilare dacă clasa nu există
                var type = System.Type.GetType("Cinemachine.CinemachineVirtualCamera, Unity.Cinemachine");
                if (type == null) type = System.Type.GetType("Cinemachine.CinemachineVirtualCamera, Cinemachine");
                
                if (type != null)
                {
                    var oldCam = FindFirstObjectByType(type) as MonoBehaviour;
                    if (oldCam != null)
                    {
                        Debug.LogWarning("CameraFollowFix: Found OLD CinemachineVirtualCamera on " + oldCam.name);
                        // Aici nu putem asigna la virtualCamera (care e de tip nou), dar putem seta Follow prin reflection
                        GameObject player = GameObject.FindGameObjectWithTag("Player");
                        if (player != null)
                        {
                            var followProp = type.GetProperty("Follow");
                            if (followProp != null)
                            {
                                followProp.SetValue(oldCam, player.transform);
                                isConnected = true;
                                Debug.Log("CameraFollowFix: Connected OLD camera via Reflection!");
                                return;
                            }
                        }
                    }
                }
            }
            
            // 6. SPERANȚA SUPREMĂ: Caută componenta CinemachineBrain și vezi dacă are o cameră activă
            if (virtualCamera == null)
            {
                CinemachineBrain brain = FindAnyObjectByType<CinemachineBrain>();
                if (brain != null)
                {
                    Debug.Log("CameraFollowFix: Found CinemachineBrain on " + brain.gameObject.name);
                    // Dacă brain-ul este pe același obiect cu camera virtuală (ceea ce e ciudat, dar posibil în logul tău)
                    virtualCamera = brain.GetComponent<CinemachineCamera>();
                    if (virtualCamera == null)
                    {
                        // Poate e pe un copil?
                        virtualCamera = brain.GetComponentInChildren<CinemachineCamera>();
                    }
                }
            }
        }

        if (virtualCamera != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            if (player == null)
            {
                PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
                if (pm != null) player = pm.gameObject;
            }

            if (player != null)
            {
                virtualCamera.Follow = player.transform;
                // virtualCamera.LookAt = player.transform; // Decomentează dacă e necesar
                
                isConnected = true;
                Debug.Log("CameraFollowFix: Camera SUCCESSFULLY connected to Player: " + player.name);
            }
            else
            {
                Debug.LogWarning("CameraFollowFix: Player not found yet...");
            }
        }
        else
        {
            Debug.LogWarning("CameraFollowFix: CinemachineCamera not found yet... Searching...");
            
            // Debugging extrem: Listează toate obiectele din scenă care au "Camera" în nume
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.scene.rootCount != 0 && (obj.name.Contains("Camera") || obj.name.Contains("CM") || obj.name.Contains("vcam")))
                {
                    Debug.Log($"[DEBUG] Found potential camera object: {obj.name} (Active: {obj.activeInHierarchy})");
                    // Listează componentele
                    Component[] components = obj.GetComponents<Component>();
                    foreach (Component c in components)
                    {
                        if (c != null) Debug.Log($"   - Component: {c.GetType().Name}");
                    }
                }
            }
        }
    }
}