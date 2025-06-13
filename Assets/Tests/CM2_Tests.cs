using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class CM2_Tests
{
    [Test]
    public void Player_InitialHealth_IsCorrect()
    {
        GameObject go = new GameObject();
        var player = go.AddComponent<CM2_PlayerController>();

        Assert.AreEqual(5, player.currentHealth);
    }
    
    [Test]
    public void Player_InitialMoveSpeed_IsCorrect()
    {
        GameObject go = new GameObject();
        var player = go.AddComponent<CM2_PlayerController>();

        Assert.AreEqual(3f, player.moveSpeed);
    }

    [Test]
    public void Player_InitialScale_IsCorrect()
    {
        GameObject go = new GameObject();
        var player = go.AddComponent<CM2_PlayerController>();

        Assert.AreEqual(0.15f, player.getScale());
    }
}
