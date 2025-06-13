using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class CM1_Tests
{
    [Test]
    public void Player_InitialHealth_IsCorrect()
    {
        GameObject go = new GameObject();
        var player = go.AddComponent<PlayerContemporary>();

        Assert.AreEqual(5, player.getHearts());
    }
    
    [Test]
    public void Player_InitialMoveSpeed_IsCorrect()
    {
        GameObject go = new GameObject();
        var player = go.AddComponent<PlayerContemporary>();

        Assert.AreEqual(5f, player.getMoveSpeed());
    }

    [Test]
    public void Player_InitialScale_IsCorrect()
    {
        GameObject go = new GameObject();
        var player = go.AddComponent<PlayerContemporary>();

        Assert.AreEqual(0.11f, player.getPlayerScale());
    }
}
