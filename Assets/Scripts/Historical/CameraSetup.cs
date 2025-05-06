using UnityEngine;
using UnityEngine.SceneManagement;
//using Cinemachine;
using Unity.Cinemachine;

public class GlobalCameraManager : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindWithTag("Player");
        CinemachineCamera vcam = GameObject.FindWithTag("VirtualCamera")?.GetComponent<CinemachineCamera>();

        if (vcam != null && player != null)
        {
            vcam.Follow = player.transform;
            vcam.LookAt = player.transform;
        }
        if(vcam == null)
            Debug.Log("VirtualCamera not found in scene!");
        if (player == null)
            Debug.Log("Player not found in scene!");
    }
}
