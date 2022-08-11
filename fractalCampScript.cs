using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fractalCampScript : MonoBehaviour
{
    public Text dialogText;
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;
    public GameObject button5;
    public GameObject button6;
    public string[] words;

    public playerCanvas3 playerCanvas;
    // Start is called before the first frame update
    void Start()
    {
        words = new string[10];

        words[0] = "- Go away please.";
        words[1] = "- Thank you.";
        words[2] = "- ...Why don't you leave me alone.";
        words[3] = "Ok";
        words[4] = "I want to help you.";
        words[5] = "- I don't believe you.";
        words[6] = "Well I wont stay longer";
        words[7] = "No I really do, just tell me what you need.";
        words[8] = "- Ok there is a special item I need you to get me, and take these, they will help.";
        dialogText.text = words[0];
    }


    public void Accept0()
    {
        dialogText.text = words[1];

        button1.SetActive(false);
        button2.SetActive(false);

        //button3.SetActive(true);
        button4.SetActive(true);
    }
    public void Decline0()
    {
        dialogText.text = words[2];

        button1.SetActive(false);
        button2.SetActive(false);

        button3.SetActive(true);
        button4.SetActive(true);
    }
    public void Accept1()
    {
        dialogText.text = words[5];

        button3.SetActive(false);
        button4.SetActive(false);

        button5.SetActive(true);
        button6.SetActive(true);
    }
    public void Decline1()
    {
        dialogText.text = words[3];

        button3.SetActive(false);
        button4.SetActive(false);
    }
    public void Accept2()
    {
        dialogText.text = words[8];

        button5.SetActive(false);
        button6.SetActive(false);

        playerCanvas.FractalsDiscovered();
    }
    public void Decline2()
    {
        dialogText.text = "  ";

        button5.SetActive(false);
        button6.SetActive(false);
    }
}
