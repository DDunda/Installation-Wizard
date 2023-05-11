using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Teams", menuName = "Team Relationship")]
public class TeamRelationship : ScriptableObject
{
	[System.Serializable]
	public struct Relationship
	{
		public Relationship(Team teamMask, bool value)
		{
			this.teamMask = teamMask;
			this.value = value;
		}
		[EnumMask]
		public Team teamMask;
		public bool value;
	}

	[SerializeField]
	public bool defaultRelationship = false;
	[SerializeField]
	protected List<Relationship> relationships = new();

	public bool GetRelationship(Team mask)
	{
		if (mask == 0) return defaultRelationship;

		foreach (var r in relationships)
		{
			if ((r.teamMask & mask) == 0) continue;

			return r.value;
		}

		return defaultRelationship;
	}
}