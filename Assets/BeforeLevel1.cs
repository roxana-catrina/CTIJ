using UnityEngine;
using UnityEngine.SceneManagement;

public class BeforeLevel1 : MonoBehaviour
{
    public void StartLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
