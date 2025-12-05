using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDebug : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log("=== PLAYER DEBUG START ===");
        Debug.Log("Player GameObject: " + gameObject.name);
        Debug.Log("Player Tag: " + gameObject.tag);
        Debug.Log("Player Layer: " + LayerMask.LayerToName(gameObject.layer));
        Debug.Log("Player Position: " + transform.position);
        Debug.Log("Player Active: " + gameObject.activeInHierarchy);
        
        if (spriteRenderer != null)
        {
            Debug.Log("SpriteRenderer found: " + spriteRenderer.enabled);
            Debug.Log("Sprite: " + (spriteRenderer.sprite != null ? spriteRenderer.sprite.name : "NULL"));
            Debug.Log("Color: " + spriteRenderer.color);
            Debug.Log("Sorting Layer: " + spriteRenderer.sortingLayerName);
            Debug.Log("Order in Layer: " + spriteRenderer.sortingOrder);
        }
        else
        {
            Debug.LogError("NO SPRITERENDERER ON PLAYER!");
        }
        
        Debug.Log("=== END PLAYER DEBUG ===");
    }
    
    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Debug.Log("=== PLAYER STATUS (P key pressed) ===");
            Debug.Log("Position: " + transform.position);
            Debug.Log("Active: " + gameObject.activeInHierarchy);
            if (spriteRenderer != null)
            {
                Debug.Log("SpriteRenderer.enabled: " + spriteRenderer.enabled);
                Debug.Log("Sprite: " + (spriteRenderer.sprite != null ? spriteRenderer.sprite.name : "NULL"));
            }
            
            // Verifică camera
            Camera cam = Camera.main;
            if (cam != null)
            {
                Debug.Log("Camera Position: " + cam.transform.position);
                Debug.Log("Camera Culling Mask: " + cam.cullingMask);
                Debug.Log("Is player in camera view: " + IsVisibleFrom(spriteRenderer, cam));
            }
        }
    }
    
    bool IsVisibleFrom(Renderer renderer, Camera camera)
    {
        if (renderer == null || camera == null) return false;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
}
