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
		if (instance == null) // Singleton pattern to ensure only one instance exists
			instance = this;
		else
			Destroy(gameObject);
	}

	public void AddScore(int amount)
	{
		score += amount;
		UpdateScoreUI(); // Update the score display

		if (score >= 20) // Check if the score has reached the winning condition
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
			winningScreen.GetComponent<WinningScreen>().ShowAndProceed();
		}
	}
}
