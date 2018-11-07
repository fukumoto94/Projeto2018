using UnityEngine;
using System.Collections;

public class PuzzleController : MonoBehaviour
{
    public GameObject[] points;
    public GameObject exit;
    private bool playOne;
    private AudioSource source;
    public GameObject doorEnd;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update ()
    {
        points = GameObject.FindGameObjectsWithTag("Fire");
        if(points.Length == 4)
        {
            if(!playOne)
            {
                source.Play();
                playOne = true;
            }
            doorEnd.GetComponent<CloseDoor>().close = true;
            exit.SetActive(true);
        }
        else
        {
            if(exit.GetComponent<SceneLoad>().isFaded)
            {
                exit.SetActive(false);

            }
        }
	}
}
