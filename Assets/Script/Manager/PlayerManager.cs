using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager Instance;

    public PlayerController[] players;

    private void Awake()
    {
        Instance = this;
    }
}
