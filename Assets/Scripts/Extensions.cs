using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
	public static Vector2 Rad2Vec(float angle, float magnitude = 1) => new(Mathf.Cos(angle) * magnitude, Mathf.Sin(angle) * magnitude);
	public static Vector2 Deg2Vec(float angle, float magnitude = 1) => Rad2Vec(angle * Mathf.Deg2Rad, magnitude);

	public static Vector2 RotateRad(this Vector2 v, float angle)
	{
		Vector2 v2 = Rad2Vec(angle);
		return new(v.x * v2.x - v.y * v2.y, v.x * v2.y + v.y * v2.x);
	}
	public static Vector2 RotateDeg(this Vector2 v, float angle) => RotateRad(v, angle * Mathf.Deg2Rad);
}