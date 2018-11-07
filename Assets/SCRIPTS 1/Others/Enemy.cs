using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Material mat;
    float timer;
    public Transform weapon_transform;
    float weapon_timer;
    float weapon_zPos;
    bool weapon_attack;
    RPGCharacterControllerFREE player_script;
	// Use this for initialization
	void Awake ()
    {
        weapon_zPos = weapon_transform.position.z - 1.2f;
        player_script = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer -= Time.deltaTime;
        Attack();
        if (timer > 0 && player_script.attack_time > 0)
        {
            mat.color = Color.red;
        }
        else
        {
            mat.color = Color.white;
        }
    }
    void Attack()
    {
        weapon_timer -= Time.deltaTime;

        if(weapon_timer > 0 && weapon_timer <1f)
        {
            weapon_attack = true;
        }
        else if(weapon_timer < -1f)
        {
            weapon_attack = false;
            weapon_timer = 2f;
        }


        if (weapon_attack)
        {
            weapon_transform.position = Vector3.MoveTowards(weapon_transform.position, new Vector3(weapon_transform.position.x, weapon_transform.position.y, weapon_zPos), 1f);
        }
        else
        {
            weapon_transform.position = Vector3.MoveTowards(weapon_transform.position, transform.position, 1f);
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Weapon")
        {
            timer = 0.1f;
        }
    }
}
