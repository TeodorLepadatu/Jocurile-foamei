using System.Collections.Generic;
using UnityEngine;
using System;

public static class Controls
{
    public enum Action
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Pickup,
        Drop,
        Shoot,
        InteractWithNPC
    }

    private static Dictionary<Action, KeyCode> keyBindings;

    static Controls()
    {
        keyBindings = new Dictionary<Action, KeyCode>();

        foreach (Action action in Enum.GetValues(typeof(Action)))
        {
            string savedKey = PlayerPrefs.GetString(action.ToString(), "");
            if (Enum.TryParse(savedKey, out KeyCode key))
                keyBindings[action] = key;
            else
                keyBindings[action] = GetDefaultKey(action);
        }
    }

    private static KeyCode GetDefaultKey(Action action)
    {
        switch (action)
        {
            case Action.MoveUp: return KeyCode.W;
            case Action.MoveDown: return KeyCode.S;
            case Action.MoveLeft: return KeyCode.A;
            case Action.MoveRight: return KeyCode.D;
            case Action.Pickup: return KeyCode.E;
            case Action.Drop: return KeyCode.F;
            case Action.Shoot: return KeyCode.Space;
            case Action.InteractWithNPC: return KeyCode.H;
            default: return KeyCode.None;
        }
    }

    public static void SetKey(Action action, KeyCode newKey)
    {
        keyBindings[action] = newKey;
    }

    public static bool GetKey(Action action)
    {
        return Input.GetKey(keyBindings[action]);
    }

    public static bool GetKeyDown(Action action)
    {
        return Input.GetKeyDown(keyBindings[action]);
    }

    public static KeyCode GetBoundKey(Action action)
    {
        return keyBindings.TryGetValue(action, out var key) ? key : KeyCode.None;
    }
}
