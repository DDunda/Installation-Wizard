using UnityEngine;

public class TemplateAbility : Ability
{
    public override bool Activate(Transform entity)
	{
		Debug.Log("This is when an ability would activate!");
		return true;
	}
}
