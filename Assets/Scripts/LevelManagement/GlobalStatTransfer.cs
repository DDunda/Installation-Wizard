using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStatTransfer : MonoBehaviour
{

    //static
    private static float damageTaken;
    private static float damageDealt;
    private static int ememiesKilled;
    //private static int abilitiesUsed;
    private static int timesDied;
    private static float totalTimerTime;

    //local
    public float localDamageTaken;
    public float localDamageDealt;
    
    public int localEmemiesKilled;
    //public int localAbilitiesUsed;
    public int localTimesDied;
    private float timerTime;
    public float localTimerTime;

    

    public void UpdateStats()
    {
        damageTaken += localDamageTaken;
        damageDealt += localDamageDealt;
        ememiesKilled += localEmemiesKilled;
        //abilitiesUsed += localAbilitiesUsed;
        timesDied += localTimesDied;
        totalTimerTime += localTimerTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        localDamageTaken = damageTaken;
        localDamageDealt = damageDealt;
        localEmemiesKilled = ememiesKilled;
        localTimesDied = timesDied;
        localTimerTime = totalTimerTime;
    }

    // Update is called once per frame
    void Update()
    {
        localTimerTime += Time.deltaTime;
    }

    public float GetDamageTaken()
    {
        return damageTaken;
    }

    public float GetDamageDealt()
    {
        return damageDealt;
    }

    public float GetEmemiesKilled()
    {
        return ememiesKilled;
    }

    public float GetTimesDied()
    {
        return timesDied;
    }

    public float GetTimerTime()
    {
        return totalTimerTime;
    }
}
