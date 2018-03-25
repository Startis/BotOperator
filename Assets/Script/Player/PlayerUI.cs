using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public PlayerController playerController { get; set; }

    public Slider sliderHp;

    //comportement
    public Button pacifism, inView, agressif;

    private void Awake()
    {
        pacifism.onClick.AddListener(delegate
        {
            pacifism.image.color = Color.red;
            inView.image.color = Color.white;
            agressif.image.color = Color.white;
            playerController.agressiveState = PlayerController.AgressiveState.pacifism;
        });

        inView.onClick.AddListener(delegate
        {
            inView.image.color = Color.red;
            pacifism.image.color = Color.white;
            agressif.image.color = Color.white;
            playerController.agressiveState = PlayerController.AgressiveState.inView;
        });

        agressif.onClick.AddListener(delegate
        {
            agressif.image.color = Color.red;
            pacifism.image.color = Color.white;
            inView.image.color = Color.white;
            playerController.agressiveState = PlayerController.AgressiveState.agressif;
        });
    }
}
