#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {


        SceneManager.LoadScene("Story");

    }
    
     public void QuitGame()
    {
        Debug.Log("Quit button pressed!"); // doar pentru test în editor
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}