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

	bool matchOver = false;

	void Start()
	{
		
		resultCanvas.SetActive(false);
		StartCoroutine(MatchTimer());
	}

	IEnumerator MatchTimer()
	{
		float remaining = matchDuration;
		while (remaining > 0f)
		{
			int m = Mathf.FloorToInt(remaining / 60f);
			int s = Mathf.FloorToInt(remaining % 60f);
			timerText.text = $"{m:00}:{s:00}";
			yield return null;
			remaining -= Time.deltaTime;
		}

		timerText.text = "00:00";
		EndMatch();
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

	public void RestartMatch()
	{
		Time.timeScale = 1f;
		UnityEngine.SceneManagement.SceneManager.LoadScene(
			UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
	}
}
