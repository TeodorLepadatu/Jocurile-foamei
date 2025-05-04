using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform[] path;
    public Transform startPoint;
    private void Awake()
    {
        main = this;
    }
}
