using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class DropItens : MonoBehaviour
{
    public Transform hpPrefab;
    public Transform arrowPrefab;
    private float timerDestroy = 2f;
    public GameObject particle;
    public GameObject mesh;

    public int percentege;
    int count = 0;
    System.Random randNum = new System.Random();
    List<int> lista = new List<int>();
    int numAux = -1;
    bool colide = false;
    public bool onlyHp;
    public bool onlyArrow;

    // Update is called once per frame
    void Update ()
    {
        DropPercentege();
        DropItem();
	}

    void DropPercentege()
    {
        while (count < percentege)
        {
            int num = randNum.Next(100);

            if (lista.Contains(num) == false)
            {
                lista.Add(num);
               // Debug.Log(lista[count]);
                count++;
            }
        }
    }

    void DropItem()
    {
        if(timerDestroy < 0)
        {
            Destroy(this.gameObject);
        }
        particle.SetActive(colide);
        mesh.SetActive(!colide);


        if (onlyHp)
        {
            if (lista.Contains(numAux) == true)
            {
                GameObject hp = Instantiate(hpPrefab.gameObject, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation) as GameObject;
                numAux = -1;
                Destroy(this.gameObject);
            }
            else
            {
                if (colide)
                {
                    timerDestroy -= Time.deltaTime;
                }
            }
        }
        else if(onlyArrow)
        {
            if (lista.Contains(numAux) == true)
            {
                GameObject arrow = Instantiate(arrowPrefab.gameObject, transform.position, transform.rotation) as GameObject;
                numAux = -1;
                Destroy(this.gameObject);
            }
            else
            {
                if (colide)
                {
                    timerDestroy -= Time.deltaTime;

                }
            }
        }
        else
        {
            if (lista.Contains(numAux) == true)
            {
                if (numAux < 50)
                {
                    GameObject arrow = Instantiate(arrowPrefab.gameObject, transform.position, transform.rotation) as GameObject;
                    Debug.Log("Arrow");
                }
                else
                {
                    GameObject hp = Instantiate(hpPrefab.gameObject, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation) as GameObject;
                    Debug.Log("Hp");

                }
                numAux = -1;
                Destroy(this.gameObject);
            }
            else
            {
                if (colide)
                {
                    timerDestroy -= Time.deltaTime;

                }
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon" && !colide)
        {
            System.Random randNum2 = new System.Random();
            numAux = randNum2.Next(100);
            Debug.Log(numAux);
            colide = true;
        }
    }
}
