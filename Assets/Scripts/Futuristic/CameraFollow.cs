using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PlayerControllerFuturistic player;
    // Update is called once per frame
    void Update()
    {   if(player.IsRunning())
		{
			transform.Translate(Vector3.right * player.speed * Time.deltaTime);
		}
		
	}
}
