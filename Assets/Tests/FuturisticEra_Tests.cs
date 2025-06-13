using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class FuturisticEra_Tests
{
    [Test]
    public void Player_Acceleration_IsCorrect()
    {
        GameObject go = new GameObject();
        var player = go.AddComponent<PlayerControllerFuturistic>();

        Assert.AreEqual(0.000000000001f, player.acceleration);
    }
    
    [Test]
    public void Player_InitialMoveSpeed_IsCorrect()
    {
        GameObject go = new GameObject();
        var player = go.AddComponent<PlayerControllerFuturistic>();

        Assert.AreEqual(14f, player.jumpForce);
    }
}
