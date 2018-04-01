using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSkill : MonoBehaviour {

    public static List<Delegate> skills;

    private delegate void FixedView(DrawPath player);
    private delegate void Mortier(Vector3 position);
    private delegate void Mine(Vector3 position);
    private delegate void Perception(FieldOfView viewPlayer);
	private delegate void Shield();

    //for mortier
    public GameObject prefAmmoMortier;
    //for mine
    public GameObject prefMine;

    private void Awake()
    {
        FixedView fixedView = delegate (DrawPath player)
        {
            player.KillViewInMove();
        };

        Mortier mortier = delegate (Vector3 position)
        {
            var ammo = Instantiate(prefAmmoMortier, position.WithY(0), Quaternion.identity);
            Destroy(ammo, 0.5f);
        };

        Mine mine = delegate (Vector3 position)
        {
            Instantiate(prefMine, position.WithY(0.55f), Quaternion.identity);
        };

        Perception perception = delegate (FieldOfView viewPlayer)
        {

        };

		Shield shield = delegate () {
			
		};


        skills = new List<Delegate>();
        skills.Add(fixedView);
        skills.Add(mortier);
        skills.Add(mine);
        skills.Add(perception);
		skills.Add (shield);
    }
}
