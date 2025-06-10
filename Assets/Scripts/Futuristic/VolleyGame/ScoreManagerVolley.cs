using UnityEngine;
using TMPro;

public class ScoreManagerVolley : MonoBehaviour
{
	public static ScoreManagerVolley I { get; private set; }

	[Header("UI References (TextMeshPro)")]
	public TMP_Text playerScoreText;
	public TMP_Text opponentScoreText;

	int playerScore, opponentScore;

	void Awake()
	{
		if (I == null) I = this;
		else { Destroy(gameObject); return; }
	}

	public void AddPlayerPoint()
	{
		playerScore++;
		playerScoreText.text = playerScore.ToString();
	}

	public void AddOpponentPoint()
	{
		opponentScore++;
		opponentScoreText.text = opponentScore.ToString();
	}
}
