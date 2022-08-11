using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassicsCampScript : MonoBehaviour
{
    public Text dialogText;
    public GameObject button1;
    public GameObject button2;
    public string[] words;

    public playerCanvas3 playerCanvas;
    // Start is called before the first frame update
    void Start()
    {
        words = new string[10];

        words[0] = "Greetings stranger, you may consinder joining our knights training program. We will teach you how to use sword and face powerfull foes, the decision is all yours.";
        words[1] = "Time for your first mission! Pick that sword up and kill some monsters,and don't worry of what it is made of, it is very effective!";
        words[2] = "As you wish have a nice day stranger.";

        dialogText.text = words[0];
    }


    public void Accept0()
    {
        dialogText.text = words[1];

        button1.SetActive(false);
        button2.SetActive(false);

        playerCanvas.ClassicsDiscovered();
    }
    public void Decline0()
    {
        dialogText.text = words[2];

        button1.SetActive(false);
        button2.SetActive(false);
    }
}
