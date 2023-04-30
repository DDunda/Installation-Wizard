using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public abstract bool Activate(); // called when the ability is activated: returns whether the ability successfully activated
}
