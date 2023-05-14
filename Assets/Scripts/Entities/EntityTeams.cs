using UnityEngine;

public class EnumMask : PropertyAttribute {}

public class EntityTeams : MonoBehaviour
{
	[SerializeField]
	[EnumMask]
	public Team teams;
}
