using UnityEngine;
using UnityEngine.UI;

public class CM2_UIManager : MonoBehaviour
{
    public static CM2_UIManager Instance;
    public Image[] hearts;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateHearts(int health)
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].enabled = i < health;
    }
}
