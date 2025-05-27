using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public enum Action
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Pickup,
        Drop,
        Shoot
    }

    private Dictionary<Action, KeyCode> keyBindings = new Dictionary<Action, KeyCode>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetDefaultBindings();
    }

    private void SetDefaultBindings()
    {
        keyBindings[Action.MoveUp] = KeyCode.W;
        keyBindings[Action.MoveDown] = KeyCode.S;
        keyBindings[Action.MoveLeft] = KeyCode.A;
        keyBindings[Action.MoveRight] = KeyCode.D;
        keyBindings[Action.Pickup] = KeyCode.E;
        keyBindings[Action.Drop] = KeyCode.F;
        keyBindings[Action.Shoot] = KeyCode.Q;
    }

    public void SetKey(Action action, KeyCode newKey)
    {
        keyBindings[action] = newKey;
    }

    public bool GetKey(Action action)
    {
        return Input.GetKey(keyBindings[action]);
    }

    public bool GetKeyDown(Action action)
    {
        return Input.GetKeyDown(keyBindings[action]);
    }

    public KeyCode GetBoundKey(Action action)
    {
        return keyBindings.ContainsKey(action) ? keyBindings[action] : KeyCode.None;
    }
}
