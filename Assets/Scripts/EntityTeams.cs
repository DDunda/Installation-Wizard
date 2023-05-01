using UnityEngine;

public class EnumMask : PropertyAttribute {}

public class EntityTeams : MonoBehaviour
{
	[SerializeField]
	[EnumMask]
	private Team teams;

	public Team Teams { get => teams; }
}
