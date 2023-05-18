using UnityEngine;

public class EnumMask : PropertyAttribute {}

public class EntityTeams : MonoBehaviour, ITeams
{
	[SerializeField,EnumMask]
	private Team _team;

	public Team team { get => _team; set => _team = value; }
}
