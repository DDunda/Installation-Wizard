using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeightedArray<T>
{
	[Serializable]
	public struct WeightedEntry
	{
		public float weight;
		public T item;

		public WeightedEntry(float weight, T item)
		{
			this.weight = weight;
			this.item = item;
		}
	}

	[SerializeField]
	private List<WeightedEntry> entries = new();
	private float _totalWeight = 0;
	private bool _totalWeightCalculated = false;
	private float TotalWeight {
		get
		{
			if (!_totalWeightCalculated) CalculateTotalWeight();
			return _totalWeight;
		}
	}

	public WeightedArray() {}

	public WeightedArray(T[] items)
	{
		foreach(var item in items)
		{
			entries.Add(new(1, item));
		}
	}

	public float CalculateTotalWeight()
	{
		_totalWeight = entries.Sum(e => e.weight);
		_totalWeightCalculated = true;
		return _totalWeight;
	}

	public void AddEntry(float weight, T item)
	{
		_totalWeight = TotalWeight + weight;
		entries.Add(new(weight, item));
	}

	public T GetRandom()
	{
		if (entries.Count == 0) return default(T);
		if (entries.Count == 1) return entries[0].item;

		float rWeight = UnityEngine.Random.value * TotalWeight;
		foreach(WeightedEntry e in entries)
		{
			if (rWeight <= e.weight) return e.item;

			rWeight -= e.weight;
		}
		return entries.Last().item;
	}
}