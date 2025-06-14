using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PlayerControllerFuturistic player;
	bool done = false;
	// update camera position based on player position
	void Update()
    {   if(player.IsRunning())
		{
			transform.Translate(Vector3.right * player.speed * Time.deltaTime);
		}

		if(player.IsDead() && !done)
		{
			transform.Translate(new Vector3(-6, 0, 0));
			done = true;
		}
		
	}
}
