using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
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

        public bool Activate()
		{
            return ability.Activate();
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

    [SerializeField]
    private AbilityHolder[] abilities;

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < abilities.Length; i++)
		{
            var holder = abilities[i];
            holder.ReduceCooldown(Time.deltaTime); // reduce the cooldown

            // if this ability's button is currently being pressed and the cooldown is over,
            // use this ability and restart the cooldown
            if (holder.Button != null)
			{
                if (Input.GetButtonDown(holder.Button) && holder.Cooldown < 0)
				{
                    // activate the ability and, if successful, restart the cooldown
                    if (holder.Activate())
					{
                        holder.RestartCooldown();
					}
				}
			}
		}
    }

    // manually activate an ability
    public bool ActivateAbility(int abilityIndex)
	{
        if (abilityIndex > 0 && abilityIndex < abilities.Length)
		{
            return abilities[abilityIndex].Activate();
		}

        return false;
	}
}
