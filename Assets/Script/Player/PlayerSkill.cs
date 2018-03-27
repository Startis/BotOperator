using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSkill : MonoBehaviour {

    public static List<Delegate> skills;

    private delegate void FixedView(Vector3 dir, DrawPath player);
    private delegate void Mortier(Vector3 position);
    private delegate void Mine(Vector3 position);
    private delegate void Perception(FieldOfView viewPlayer);

    //for mortier
    public GameObject prefAmmoMortier;

    //for mine
    public GameObject prefMine;

    private void Awake()
    {
        FixedView fixedView = delegate (Vector3 dir, DrawPath player)
        {
            player.KillViewInMove();
            player.transform.forward = dir;
        };

        Mortier mortier = delegate (Vector3 position)
        {
            var ammo = Instantiate(prefAmmoMortier, position, Quaternion.identity);
            Destroy(ammo);
        };

        Mine mine = delegate (Vector3 position)
        {
            Instantiate(prefMine, position.WithY(0.55f), Quaternion.identity);
        };

        Perception perception = delegate (FieldOfView viewPlayer)
        {

        };


        skills = new List<Delegate>();
        skills.Add(fixedView);
        skills.Add(mortier);
        skills.Add(mine);
        skills.Add(perception);
    }
}
