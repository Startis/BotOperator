using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    //public PlayerController playerController { get; set; }

    public Slider sliderHp;

    //comportement
    public Button pacifism, inView, agressif;
    public int currentIdPlayer { get; set; }

	public Button r1, r2, r3;

    private void Awake()
    {
        pacifism.onClick.AddListener(delegate
        {
            pacifism.image.color = Color.red;
            inView.image.color = Color.white;
            agressif.image.color = Color.white;
            PlayerManager.Instance.players[currentIdPlayer].agressiveState = PlayerController.AgressiveState.pacifism;
        });

        inView.onClick.AddListener(delegate
        {
            inView.image.color = Color.red;
            pacifism.image.color = Color.white;
            agressif.image.color = Color.white;
            PlayerManager.Instance.players[currentIdPlayer].agressiveState = PlayerController.AgressiveState.inView;
        });

        agressif.onClick.AddListener(delegate
        {
            agressif.image.color = Color.red;
            pacifism.image.color = Color.white;
            inView.image.color = Color.white;
            PlayerManager.Instance.players[currentIdPlayer].agressiveState = PlayerController.AgressiveState.agressif;
        });

		r1.onClick.AddListener (delegate {
			r1.image.color = Color.red;
			r2.image.color = Color.white;
			r3.image.color = Color.white;
			GameObject.Find("PlayerTest").GetComponent<PlayerController>().ChooseRobot();
		});

		r2.onClick.AddListener (delegate {
			r2.image.color = Color.red;
			r1.image.color = Color.white;
			r3.image.color = Color.white;
			GameObject.Find("PlayerTest2").GetComponent<PlayerController>().ChooseRobot();
		});

		r3.onClick.AddListener (delegate {
			r3.image.color = Color.red;
			r1.image.color = Color.white;
			r2.image.color = Color.white;
			GameObject.Find("PlayerTest3").GetComponent<PlayerController>().ChooseRobot();
		});
    }

	public void SelectRobot(int id){
		switch(id){
			case 0:
			r1.image.color = Color.red;
			r2.image.color = Color.white;
			r3.image.color = Color.white;
				break;
			case 1:
			r2.image.color = Color.red;
			r1.image.color = Color.white;
			r3.image.color = Color.white;
				break;
			case 2:
			r3.image.color = Color.red;
			r1.image.color = Color.white;
			r2.image.color = Color.white;
				break;
		}
	}

    public void ChangeRobotState(int id, PlayerController.AgressiveState agressive)
    {
        currentIdPlayer = id;
        switch (agressive)
        {
            case PlayerController.AgressiveState.pacifism:
                pacifism.image.color = Color.red;
                inView.image.color = Color.white;
                agressif.image.color = Color.white;
                break;
            case PlayerController.AgressiveState.inView:
                inView.image.color = Color.red;
                pacifism.image.color = Color.white;
                agressif.image.color = Color.white;
                break;
            case PlayerController.AgressiveState.agressif:
                agressif.image.color = Color.red;
                pacifism.image.color = Color.white;
                inView.image.color = Color.white;
                break;
        }
    }
}
