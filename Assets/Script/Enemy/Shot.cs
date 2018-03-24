using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

    public float speed;
    public float damage;
    private Vector3 dir;

    private void Update()
    {
        if(dir != null)
        {
            transform.position += dir * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Contains("Player"))
        {
            collision.transform.GetComponent<PlayerManager>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    public void SetDirection(Vector3 direction)
    {
        dir = direction;
    }
}
