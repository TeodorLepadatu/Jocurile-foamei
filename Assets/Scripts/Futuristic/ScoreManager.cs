using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager instance;

	public TextMeshProUGUI scoreText;
	private int score = 0;

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
	}

	void UpdateScoreUI()
	{
		if (scoreText != null)
			scoreText.text = score.ToString();
	}
}
