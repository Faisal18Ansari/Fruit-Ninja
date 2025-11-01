using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score;
    public RawImage flash;
    private Blade blade;
    private Spawner spawner;
    void Start()
    {
        // Find components first so NewGame() can safely use them
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
        NewGame();
    }

    private void NewGame()
    {
        Time.timeScale = 1f;
        score = 0;
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
        else
            Debug.LogWarning("GameManager: scoreText not set in inspector.");

        if (blade == null) blade = FindObjectOfType<Blade>();
        if (blade != null)
            blade.enabled = true;
        else
            Debug.LogWarning("GameManager: Blade not found in scene.");

        if (spawner == null) spawner = FindObjectOfType<Spawner>();
        if (spawner != null)
            spawner.enabled = true;
        else
            Debug.LogWarning("GameManager: Spawner not found in scene.");
        ClearScreen();
    }
    private void ClearScreen()
    {
        Fruit[] fruits = FindObjectsOfType<Fruit>();
        foreach (Fruit fruit in fruits)
        {
            Destroy(fruit.gameObject);
        }
        Bomb[] bombs = FindObjectsOfType<Bomb>();
        foreach (Bomb bomb in bombs)
        {
            Destroy(bomb.gameObject);
        }
    }
    public void IncreaseScore(int amount)
    {
        score += amount;
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
    }
    public void Explode()
    {
        if (blade != null) blade.enabled = false;
        if (spawner != null) spawner.enabled = false;
        if (scoreText != null) scoreText.text += "\nGame Over!";
        else Debug.LogWarning("GameManager: scoreText not set when Explode called.");

        StartCoroutine(ExplodeSequence());
    }
    private IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;
        float duration = 0.5f;
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            if (flash != null)
                flash.color = Color.Lerp(Color.clear, Color.white, t);
            Time.timeScale = 1f;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1f);
        NewGame();
        elapsed = 0f;
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            if (flash != null)
                flash.color = Color.Lerp(Color.white, Color.clear, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
