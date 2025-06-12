using UnityEngine;
using UnityEngine.SceneManagement;

public class MountainClick : MonoBehaviour
{
	public string sceneToLoad;

	void OnMouseDown()
	{
		SceneManager.LoadScene(sceneToLoad);
	}
}
