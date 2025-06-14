using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
	public string goalTag;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Ball")) return;

		if (goalTag == "PoartaPlayer") // Check if the goal is for the opponent
			ScoreManagerVolley.I.AddOpponentPoint();
		else if (goalTag == "PoartaOponent")
			ScoreManagerVolley.I.AddPlayerPoint(); // Check if the goal is for the player
		Debug.Log($"Goal Triggered: {goalTag} by {other.name}");

	}
}
