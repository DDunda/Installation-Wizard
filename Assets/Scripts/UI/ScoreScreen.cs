using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour
{
    public Text damageTakenText;
    public Text damageDealtText;
    public Text ememiesKilledText;
    public Text timesDiedText;
    public Text timerText;


    // Start is called before the first frame update
    void Start()
    {
        damageTakenText.text = "Damage Taken: " + GameObject.Find("GameManager").GetComponent<GlobalStatTransfer>().GetDamageTaken().ToString();
        damageDealtText.text = "Damage Dealt: " + GameObject.Find("GameManager").GetComponent<GlobalStatTransfer>().GetDamageDealt().ToString();
        ememiesKilledText.text = "Enemies Killed: " + GameObject.Find("GameManager").GetComponent<GlobalStatTransfer>().GetEmemiesKilled().ToString();
        timesDiedText.text = "Times Died: " + GameObject.Find("GameManager").GetComponent<GlobalStatTransfer>().GetTimesDied().ToString();
        DisplayTime(GameObject.Find("GameManager").GetComponent<GlobalStatTransfer>().GetTimerTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisplayTime(float displayTime)
    {
        float minutes = Mathf.FloorToInt(displayTime / 60);
        float seconds = Mathf.FloorToInt(displayTime % 60);
        timerText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        //timerText.text = "Time: " + (timerTime >= 60 ? Mathf.FloorToInt(timerTime / 60).ToString() + "m " : "") + $"{timerTime % 60:0.000}s";
    }
}
