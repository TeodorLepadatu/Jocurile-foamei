using System.Collections.Generic;
using UnityEngine;

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
        SetDefaultBindings();
    }

    private static void SetDefaultBindings()
    {
        keyBindings = new Dictionary<Action, KeyCode>
        {
            [Action.MoveUp] = KeyCode.W,
            [Action.MoveDown] = KeyCode.S,
            [Action.MoveLeft] = KeyCode.A,
            [Action.MoveRight] = KeyCode.D,
            [Action.Pickup] = KeyCode.E,
            [Action.Drop] = KeyCode.F,
            [Action.Shoot] = KeyCode.Space,
            [Action.InteractWithNPC] = KeyCode.H
        };
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
