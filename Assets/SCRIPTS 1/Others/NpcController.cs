using UnityEngine;
using System.Collections;

public class NpcController : MonoBehaviour
{
    public GameObject[] npcs;
    private bool firstNpc = false;
    private int aux = 0;
	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	    for(int i = 0; i < npcs.Length; i++)
        {
            if(npcs[i].GetComponent<NPC_dialogue>().startDialogue)
            {
                aux = i;
                firstNpc = true;
            }
       
            if (firstNpc)
            {
                npcs[i].SetActive(npcs[i].GetComponent<NPC_dialogue>().startDialogue);

                if (!npcs[aux].GetComponent<NPC_dialogue>().startDialogue)
                {
                    firstNpc = false;

                }

            }
            else
            {
                npcs[i].SetActive(true);

            }



        }
    }
}
