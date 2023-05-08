using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AbilityController : MonoBehaviour, IPausable
{
    /*
     * DELETE IF NOT BEING USED
    [Serializable]
    public class AbilityHolder
    {
        private float cooldown; // the current cooldown of this ability, in seconds
        [SerializeField]
        private Ability ability; // the ability assigned to this slot
        [SerializeField]
        private float cooldownMax; // the value the cooldown is set to when this ability is used
        [SerializeField]
        private string button; // name of virtual button used to activate this ability (if null, ability not activated w/ button)

        public float Cooldown
		{
            get { return cooldown; }
		}

        public string Button
		{
            get { return button; }
		}

        public bool Activate(Transform entity)
		{
            if (cooldown > 0) return false;
            if (!ability.Activate(entity)) return false;

            RestartCooldown();
            return true;
		}

        public void RestartCooldown()
		{
            cooldown = cooldownMax;
		}

        public void ReduceCooldown(float reduction)
		{
            cooldown -= reduction;
		}
    }
    */

    [SerializeField]
    private Ability[] abilities;

	private void Awake()
	{
        this.RegisterPause();
	}

	private void OnDestroy()
	{
		this.UnregisterPause();
	}

    public void OnPause()
    {
        enabled = false;
    }

    public void OnResume()
    {
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Ability abi;

        // reduce all ability cooldowns
        for (var i = 0; i < abilities.Length; i++)
        {
            abi = abilities[i];
            abi.ReduceCooldown(Time.deltaTime);
        }

        // try to activate each ability: only activate the first one that changes state
        for (var i = 0; i < abilities.Length; i++)
		{
            abi = abilities[i];
            if (abi.Activate())
			{
                //Debug.Log(string.Format("Ability {0} just activated on frame {1}!", i, Time.frameCount));
                break;
			}
		}
    }

    /*
    // manually activate an ability
    public bool ActivateAbility(int abilityIndex)
	{
        if (abilityIndex >= 0 && abilityIndex < abilities.Length)
		{
            return abilities[abilityIndex].Activate();
		}

        return false;
	}
    */
}
