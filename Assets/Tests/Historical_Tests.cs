using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class Historical_Tests
{
    [Test]
    public void Player_InitialRadius_IsCorrect()
    {
        GameObject go = new GameObject();
        var player = go.AddComponent<PlayerController>();

        Assert.AreEqual(1f, player.pickupRadius);
    }
    
    [Test]
    public void Player_MaxHealth_IsCorrect()
    {
        GameObject go = new GameObject();
        var player = go.AddComponent<PlayerController>();

        Assert.AreEqual(10, player.maxHealth);
    }

    [Test]
    public void Player_InitialSpeed_IsCorrect()
    {
        GameObject go = new GameObject();
        var player = go.AddComponent<PlayerController>();

        Assert.AreEqual(3f, player.speed);
    }
}
