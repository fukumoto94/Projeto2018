using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    public Slider camera_speed;
    public Slider camera_bow;
    public float timer;
    public Text txt;
    public bool enemyClose;
    private GameObject[] gos;
    public ArrayList closestList;

    // Use this for initialization
    void Awake ()
    {
        gos = GameObject.FindGameObjectsWithTag("EnemyM");
        closestList = new ArrayList(gos.Length);
    }
	
	// Update is called once per frame
	void Update ()
    {
        



      //  Debug.Log("Game:  " + enemyClose);
    }

}
