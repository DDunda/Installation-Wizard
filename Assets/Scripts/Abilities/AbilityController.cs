using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AbilityController : MonoBehaviour, IPausable
{
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
}
