using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager instance;

	public TextMeshProUGUI scoreText;
	public GameObject winningScreen;

	private int score = 0;

	void Start()
	{
		if (winningScreen != null)
			winningScreen.SetActive(false);
	}


	void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	public void AddScore(int amount)
	{
		score += amount;
		UpdateScoreUI();

		if (score >= 25)
		{
			ShowWinningScreen();
		}
	}

	void UpdateScoreUI()
	{
		if (scoreText != null)
			scoreText.text = score.ToString();
	}

	void ShowWinningScreen()
	{
		if (winningScreen != null)
		{
			winningScreen.SetActive(true);
			Time.timeScale = 0f; 
		}
	}
}
