using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

    public enum Emetteur
    {
        enemy,
        player
    }

    public float speed;
    public float damage;
    private Vector3 dir;
    private Emetteur emetteur;

    private void Update()
    {
        if (GameManager.pause)
        {
            return;
        }

        if(dir != null)
        {
            transform.position += dir * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (emetteur == Emetteur.enemy && collision.transform.tag.Contains("Player"))
        {
            collision.transform.GetComponent<PlayerController>().TakeDamage(damage);
        }else if (emetteur == Emetteur.player && collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<AbstractEnemy>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    public void SetDirection(Vector3 direction, Emetteur emet)
    {
        dir = direction;
        emetteur = emet;
    }
}
