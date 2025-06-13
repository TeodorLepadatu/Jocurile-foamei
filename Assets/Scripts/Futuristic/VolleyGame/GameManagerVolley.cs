using UnityEngine;
using TMPro;
using System.Collections;

public class GameManagerVolley : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;
    public GameObject resultCanvas;
    public TMP_Text resultText;

    [Header("Game Settings")]
    public float matchDuration = 120f;
    public int winningScore = 10; // Score needed to win

    bool matchOver = false;

    void Start()
    {
        resultCanvas.SetActive(false);
        StartCoroutine(MatchTimer());
    }

    IEnumerator MatchTimer()
    {
        float remaining = matchDuration;
        while (remaining > 0f && !matchOver)
        {
            // Check for score win condition every frame
            if (ScoreManagerVolley.I.PlayerScore >= winningScore || ScoreManagerVolley.I.OpponentScore >= winningScore)
            {
                EndMatchEarly();
                yield break;
            }

            int m = Mathf.FloorToInt(remaining / 60f);
            int s = Mathf.FloorToInt(remaining % 60f);
            timerText.text = $"{m:00}:{s:00}";
            yield return null;
            remaining -= Time.deltaTime;
        }

        if (!matchOver)
        {
            timerText.text = "00:00";
            EndMatch();
        }
    }

    void EndMatch()
    {
        if (matchOver) return;
        matchOver = true;

        Time.timeScale = 0f;

        int p = ScoreManagerVolley.I.PlayerScore;
        int o = ScoreManagerVolley.I.OpponentScore;
        resultText.text = p > o ? "You Won!" : "You Lost";

        resultCanvas.SetActive(true);
    }

    // Call this method when a player reaches the winning score
    public void EndMatchEarly()
    {
        if (matchOver) return;
        StopAllCoroutines();
        timerText.text = "00:00";
        EndMatch();
    }

    public void RestartMatch()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
