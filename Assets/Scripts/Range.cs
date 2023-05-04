using System;
using UnityEngine;

[Serializable]
public struct Range<T>
{
	public T min, max;
	public Range(T min, T max)
	{
		this.min = min;
		this.max = max;
	}
}

public static class RangeExtension
{
	public static float Lerp(this Range<float> r, float t)
	{
		return Mathf.Lerp(r.min, r.max, t);
	}
	public static Vector2 Lerp(this Range<Vector2> r, float t)
	{
		return Vector2.Lerp(r.min, r.max, t);
	}
	public static Vector3 Lerp(this Range<Vector3> r, float t)
	{
		return Vector3.Lerp(r.min, r.max, t);
	}
	public static Color Lerp(this Range<Color> r, float t)
	{
		return Color.Lerp(r.min, r.max, t);
	}

	public static float Random(this Range<float> r)
	{
		return UnityEngine.Random.Range(r.min, r.max);
	}

	public static float InverseLerp(this Range<float> r, float value)
	{
		return Mathf.InverseLerp(r.min, r.max, value);
	}
	public static float Clamp(this Range<float> r, float value)
	{
		return Mathf.Clamp(value, r.min, r.max);
	}

	public static bool Contains<T>(this Range<T> r, T value) where T : struct, IComparable<T>
	{
		return value.CompareTo(r.min) >= 0 && value.CompareTo(r.max) <= 0;
	}

	public static bool Intersects<T>(this Range<T> r1, Range<T> r2) where T : struct, IComparable<T>
	{
		return r1.max.CompareTo(r2.min) >= 0 && r1.min.CompareTo(r2.max) <= 0;
	}

	public static Range<T>? Union<T>(this Range<T> r1, Range<T> r2) where T : struct, IComparable<T>
	{
		if (!r1.Intersects(r2))
		{
			return null;
		}

		return new(
			r1.min.CompareTo(r2.min) < 0 ? r1.min : r2.min,
			r1.max.CompareTo(r2.max) > 0 ? r1.max : r2.max
		);
	}
	public static Range<T>? Intersection<T>(this Range<T> r1, Range<T> r2) where T : struct, IComparable<T>
	{
		if (!r1.Intersects(r2))
		{
			return null;
		}

		return new(
			r1.min.CompareTo(r2.min) < 0 ? r2.min : r1.min,
			r1.max.CompareTo(r2.max) > 0 ? r2.max : r1.max
		);
	}
}