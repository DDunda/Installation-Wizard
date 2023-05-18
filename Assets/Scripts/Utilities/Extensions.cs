using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
	public static Vector2 Rad2Vec(float angle, float magnitude = 1) => new(Mathf.Cos(angle) * magnitude, Mathf.Sin(angle) * magnitude);
	public static Vector2 Deg2Vec(float angle, float magnitude = 1) => Rad2Vec(angle * Mathf.Deg2Rad, magnitude);

	public static float Angle(this Vector2 v) => Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;

	public static Vector2 RotateRad(this Vector2 v, float angle)
	{
		Vector2 v2 = Rad2Vec(angle);
		return new(v.x * v2.x - v.y * v2.y, v.x * v2.y + v.y * v2.x);
	}
	public static Vector2 RotateDeg(this Vector2 v, float angle) => RotateRad(v, angle * Mathf.Deg2Rad);

    public static Vector2 Clamp(this Vector2 v, float size)
    {
        if (v.magnitude > size) return v.normalized * size;
        else return v;
    }
    public static Vector3 Clamp(this Vector3 v, float size)
    {
        if (v.magnitude > size) return v.normalized * size;
        else return v;
    }

    public static void RegisterPause(this IPausable l, Pauser p = null)
    {
        if (p == null) p = Pauser.instance;
        p.RegisterListener(l);
	}
	public static void UnregisterPause(this IPausable l, Pauser p = null)
	{
		if (p == null) p = Pauser.instance;
		p.UnregisterListener(l);
	}
	public static T Random<T>(this T[] arr) => arr[UnityEngine.Random.Range(0,arr.Length)];

	public static Vector2 GetMouseWorldPosition(Camera cam)
	{
		Vector3 p = cam.ScreenToWorldPoint(Input.mousePosition);
		return new(p.x, p.y);
	}

	public static T[] Shuffled<T>(this T[] arr)
	{
		T[] _arr = (T[])arr.Clone();
		_arr.Shuffle();
		return _arr;
	}

	public static void Shuffle<T>(this T[] arr)
	{
		int n = arr.Length;
		while (n > 1)
		{
			int k = UnityEngine.Random.Range(0, n--);
			T temp = arr[n];
			arr[n] = arr[k];
			arr[k] = temp;
		}
	}

	public static void DrawGizmoSpiral1(Vector3 position, Vector3 scale, float segmentLength = 0.1f, int arms = 6, float a = 20f, int n = 10)
	{
		for (int j = 0; j < arms; j++)
		{
			Vector3 lp = position;
			for (int i = 0; i < n; i++)
			{
				Vector3 p = (Vector3)Deg2Vec(i * a + j * 360f / arms, segmentLength * (i + 1));
				p.Scale(scale);
				p += position;
				Gizmos.DrawLine(lp, p);
				lp = p;
			}
		}
	}

	public static void DrawGizmoSpiral2(Vector3 position, Vector3 scale, float segmentLength = 0.1735f, int arms = 6, float a = 20f, int n = 9)
	{
		for (int j = 0; j < arms; j++)
		{
			Vector3 lp = position;
			for (int i = 0; i < n; i++)
			{
				Vector3 p = (Vector3)Deg2Vec((i + 0.5f) * a + j * 360f / arms, segmentLength);
				p.Scale(scale);
				p += lp;
				Gizmos.DrawLine(lp, p);
				lp = p;
			}
		}
	}

	public static void DrawGizmoCircle(Vector3 position, Vector3 scale, float r = 1, float n = 45)
	{
		Vector3 lp = (Vector3)Deg2Vec(360f * (n - 1) / n, r) + position;
		lp.Scale(scale);
		for (int i = 0; i < n; i++)
		{
			Vector3 p = (Vector3)Deg2Vec(360f * i / n, r) + position;
			p.Scale(scale);
			Gizmos.DrawLine(lp, p);
			lp = p;
		}
	}
}