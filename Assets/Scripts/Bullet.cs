using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int _damage;
    private void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(_damage);
        }
        Destroy(gameObject);
    }
    
}
