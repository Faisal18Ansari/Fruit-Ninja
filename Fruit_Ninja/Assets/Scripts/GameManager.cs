using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine;

// GameManager: the cranky puppet master of the game. Keeps score, restarts the circus,
// and occasionally loses its temper when you touch a bomb.
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score;
    public RawImage flash;
    private Blade blade;
    private Spawner spawner;
    void Start()
    {
        // Start: find the important people (objects) so we can boss them around.
        // If the universe is in a bad mood and can't find them, we'll warn you.
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
        NewGame();
    }

    private void NewGame()
    {
        // NewGame: reset time, score, and re-enable the people who make the fruit.
        // It's the 'refresh' button for life, but only for this tiny game world.
        Time.timeScale = 1f;
        score = 0;
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
        else
            Debug.LogWarning("GameManager: scoreText not set in inspector.");

        if (blade == null) blade = FindObjectOfType<Blade>();
        if (blade != null)
            blade.enabled = true; // let the blade do what it does best: slice
        else
            Debug.LogWarning("GameManager: Blade not found in scene.");

        if (spawner == null) spawner = FindObjectOfType<Spawner>();
        if (spawner != null)
            spawner.enabled = true; // resume spawning chaos
        else
            Debug.LogWarning("GameManager: Spawner not found in scene.");
        ClearScreen();
    }
    private void ClearScreen()
    {
        // ClearScreen: remove everything that isn't supposed to persist between rounds.
        // Think of it as the world's tidying-up montage, minus the uplifting music.
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
        // IncreaseScore: called when the player does something worthy of points.
        // We add the points, update the text, and pretend we didn't just get lucky.
        score += amount;
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
    }
    public void Explode()
    {
        // Explode: the moment someone touches a bomb and the game throws a tantrum.
        // We disable controls/spawning, show 'Game Over', and run a little sequence.
        if (blade != null) blade.enabled = false;
        if (spawner != null) spawner.enabled = false;
        if (scoreText != null) scoreText.text += "\nGame Over!";
        else Debug.LogWarning("GameManager: scoreText not set when Explode called.");

        StartCoroutine(ExplodeSequence());
    }
    private IEnumerator ExplodeSequence()
    {
        // ExplodeSequence: do a quick flash, wait, then reset. It's basically the game's dramatic
        // sigh followed by a broom-and-mop routine.
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
