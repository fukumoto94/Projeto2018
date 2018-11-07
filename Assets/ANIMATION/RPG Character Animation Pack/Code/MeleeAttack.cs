using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour {

    public Transform enemy_weapon;

    void Start()
    {
      //  Physics.IgnoreCollision(this.GetComponent<Collider>(), enemy_weapon.GetComponent<Collider>());
    }
}
