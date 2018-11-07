using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHit : MonoBehaviour
{
    public bool head;
    private BossNovo boss;
    private RPGCharacterControllerFREE player;
    public int hand;
    private float lessCd;
    public Transform hitObject;
    public float distHit;
    private bool hit;
    private float timer;

	void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossNovo>() as BossNovo;
        lessCd = boss.cdAttack / 4;
	}

    void Update()
    {
        if(!head)
        {
            if ((hitObject.position - player.transform.position).magnitude < distHit && !hit)
            {
                player.GetDamage(5);
                player.GetHit();
                timer = 2f;
                hit = true;
            }
          //  Debug.Log("hit:: " + (hitObject.position - player.transform.position).magnitude);

            if (timer < 0)
            {
                hit = false;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Arrow")
        {
            if(head && boss.gemHit[0] && boss.gemHit[1])
            {
    
                boss.headHit = true;
                
            }
            else
            {
                if(!head)
                {
                    if (!boss.gemHit[hand])
                    {
                        boss.knockback = true;
                        boss.gemHit[hand] = true;
                    }
                }
             
            }

        }
        if (other.gameObject.tag == "Weapon" && boss.firstHit)
        {
            boss.lifes++;
            boss.cdAttack -= lessCd;
            boss.firstHit = false;
        }
    }

}
