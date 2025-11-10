using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StoryController : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] paragraphs;           // fiecare element e un paragraf din poveste
    public TMP_Text storyText;            // referință la textul de pe ecran
    public float typingSpeed = 0.04f;     // viteza efectului de scriere
    public float delayAfterParagraph = 2f; // timp de așteptare după finalul unui paragraf

    private int currentParagraph = 0;
    private bool isTyping = false;
    private bool skipPressed = false;

    void Start()
    {
        // Verifică dacă storyText este setat în Inspector
        if (storyText == null)
        {
            Debug.LogError("StoryText nu este setat! Te rog să atașezi componenta TMP_Text în Inspector.");
            return;
        }

        // Verifică dacă există paragrafe de afișat
        if (paragraphs == null || paragraphs.Length == 0)
        {
            Debug.LogError("Nu există paragrafe de afișat! Adaugă text în Inspector.");
            return;
        }

        storyText.text = "";
        StartCoroutine(PlayStory());
    }


    public void SkipParagraph()
    {
        // Dacă jucătorul apasă SKIP:
        if (isTyping)
        {
            // afișează instant tot paragraful
            skipPressed = true;
        }
        else
        {
            // trece direct la următorul paragraf
            currentParagraph++;
            if (currentParagraph < paragraphs.Length)
            {
                StopAllCoroutines();
                StartCoroutine(PlayStory());
            }
            else
            {
                LoadNextScene();
            }
        }
    }

    IEnumerator PlayStory()
    {
        storyText.text = "";
        string paragraph = paragraphs[currentParagraph];
        isTyping = true;
        skipPressed = false;

        // efectul de scriere
        foreach (char c in paragraph)
        {
            if (skipPressed)
            {
                storyText.text = paragraph; // afișează totul instant
                break;
            }
            storyText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        // dacă jucătorul nu apasă skip, așteaptă puțin și trece automat mai departe
        yield return new WaitForSeconds(delayAfterParagraph);

        currentParagraph++;
        if (currentParagraph < paragraphs.Length)
        {
            StartCoroutine(PlayStory());
        }
        else
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("BeforeLevel1"); // ← schimbă cu numele scenei următoare
    }
}
