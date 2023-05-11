using UnityEngine;

public class Ability : MonoBehaviour
{
    [SerializeField]
    private float cooldownMax = 3.0f; // the max cooldown of this ability
    private float cooldown = 0.0f; // the current cooldown of this ability

    // returns true if cooldown is still activate
    protected bool OnCooldown
	{
        get
		{
            if (cooldown > 0) { return true; }
            return false;
		}
	}

    // called when the ability is activated: returns true if the ability successfully changed state in some way (e.g. received an input)
    public virtual bool Activate() 
    {
        if (!OnCooldown)
		{
            Debug.Log("This is a test message for abilities!");
            RestartCooldown();
            return true;
		}

        return false;
    }

    // reduce the cooldown by 'time' seconds
    public void ReduceCooldown(float time)
	{
        cooldown -= time;
	}

    // restart the cooldown back to its maximum value
    protected void RestartCooldown()
	{
        cooldown = cooldownMax;
	}
}
