using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Mapa : MonoBehaviour
{
    private RPGCharacterControllerFREE player;
    public Text position;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;
    }

    void Update()
    {
        position.text = "Posição atual\n" +
            "  X: " + player.transform.position.x.ToString("0.00") +
            "  Y: " + player.transform.position.y.ToString("0.00") +
            "  Z: " + player.transform.position.z.ToString("0.00");
    }
}
