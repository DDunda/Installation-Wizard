using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStatTransfer : MonoBehaviour
{

    //static
    public static float damageTaken;
    public static float damageDealt;
    public static float timeTaken;
    public static int ememiesKilled;
    public static int abilitiesUsed;

    //local
    public float localDamageTaken;
    public float localDamageDealt;
    public float localTimeTaken;
    public int localEmemiesKilled;
    public int localAbilitiesUsed;

    public void UpdateStats()
    {
        damageTaken += localDamageTaken;
        damageDealt += localDamageDealt;
        timeTaken += localTimeTaken;
        ememiesKilled += localEmemiesKilled;
        abilitiesUsed += localAbilitiesUsed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
