using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NPC_dialogue : MonoBehaviour
{
    public bool startDialogue = false;
    private OrbitCamera oc;
    private RPGCharacterControllerFREE player;
    public int numNpc;
    public bool stopNpc = true;
    public bool isCollide = false;
    public bool giveItem;
    public GameObject item;


    private bool playOne;

    //new text
    public GameObject textBox;
    public Text theText;
    public TextAsset textFile;
    public string[] textLines;

    private int currentLine = 0;
    private int endAtLine;

    private bool isTyping = false;
    private bool cancelTyping = false;
    public float typeSpeed;
    private bool end  = true;
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        oc = GameObject.FindGameObjectWithTag("OrbitCamera").GetComponent<OrbitCamera>() as OrbitCamera;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>() as RPGCharacterControllerFREE;

    }

    // Use this for initialization
    void Start ()
    {
        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }
        endAtLine = textLines.Length;
       // StartCoroutine(TextScroll(textLines[currentLine]));

    }

    // Update is called once per frame
    void Update ()
    {
        if (startDialogue)
        {
            if(giveItem)
            {
                item.SetActive(oc.isPick);
            }

            oc.auxNpc = numNpc;
            if (Input.GetKeyDown(KeyCode.Space) && !player.givingItem)
            {
                if (currentLine >= endAtLine)
                {
                    textBox.SetActive(false);

                    if (giveItem && item!=null && !oc.isPick)
                    {
                        if(!playOne)
                        {
                            source.Play();
                            playOne = true;

                        }
                        oc.isPick = true;
                    }
                    else
                    {
                        oc.isPick = false;
                    }


                    oc.isNpc = false;
                    currentLine = 0;
                }
                else
                {

  
                    textBox.SetActive(true);
                    if (!isTyping)
                    {

                        
                        StartCoroutine(TextScroll(textLines[currentLine]));


                    }
                    else if (isTyping && !cancelTyping)
                    {
                        cancelTyping = true;
                    }
                    transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                    oc.isNpc = true;


                }

            }
        //    theText.text = textLines[currentLine];

        }
        else
        {
            textBox.SetActive(false);

            oc.isNpc = false;
            currentLine = 0;
        }


    }

    private IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0;
        theText.text = "";
        isTyping = true;
        cancelTyping = false;
        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            theText.text += lineOfText[letter];
            letter += 1;

            yield return new WaitForSeconds(typeSpeed);
        }
        currentLine++;
        theText.text = lineOfText;
        isTyping = false;
        cancelTyping = false;
    }
}
