using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeightedArray<T> : ISerializationCallbackReceiver
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
	private List<WeightedEntry> entries;
	private float _totalWeight = 0;

	public WeightedEntry[] Entries { get => entries.ToArray(); }

	public WeightedEntry this[int key]
	{
		get => entries[key];
		set
		{
			_totalWeight += value.weight - entries[key].weight;
			entries[key] = value;
		}
	}

	private float TotalWeight {
		get
		{
			return _totalWeight;
		}
	}

	public WeightedArray() {
		entries = new();
		_totalWeight = 0;
	}

	public WeightedArray(T item)
	{
		entries =  new(1) { new(1f, item) };
		_totalWeight = 1;
	}

	public WeightedArray(WeightedEntry entry)
	{
		entries = new(1) { entry };
		_totalWeight = entry.weight;
	}

	public WeightedArray(T[] items)
	{
		entries = new(from i in items select new WeightedEntry(1, i));
		_totalWeight = items.Length;
	}

	public WeightedArray(IEnumerable<WeightedEntry> entries)
	{
		this.entries = new(entries);
		CalculateTotalWeight();
	}

	public static implicit operator WeightedArray<T>(T item) => new(item);
	public static implicit operator WeightedArray<T>(T[] items) => new(items);

	public float CalculateTotalWeight()
	{
		_totalWeight = entries.Sum(e => e.weight);
		return _totalWeight;
	}

	public void AddEntry(float weight, T item)
	{
		if (weight <= 0) return;
		_totalWeight = TotalWeight + weight;
		entries.Add(new(weight, item));
	}

	public void AddEntries(IEnumerable<WeightedEntry> entries)
	{
		_totalWeight += entries.Sum(e => e.weight);
		this.entries.AddRange(entries);
	}

	public T GetRandom()
	{
		int i = GetRandomIndex();
		if (i == -1) return default;
		else return entries[i].item;
	}

	public int GetRandomIndex()
	{
		if (entries.Count == 0) return -1;
		if (entries.Count == 1) return 0;

		float rWeight = UnityEngine.Random.value * TotalWeight;
		int i = 0;
		foreach(WeightedEntry e in entries)
		{
			if (rWeight <= e.weight) return i;

			rWeight -= e.weight;
			i++;
		}
		return entries.Count - 1;
	}

	public T[] GetRandomN(uint n, bool unique = false)
	{
		if (n == 0 || entries.Count == 0) return new T[] {};
		if (unique && entries.Count <= n) return (from t in Entries.Shuffled() select t.item).ToArray();
		if (n == 1) return new T[] { GetRandom() };

		List<T> outArr = new();

		if(unique)
		{
			WeightedArray<T> copyArr = new(entries);
			for(int i = 0; i < n; i++)
			{
				int index = copyArr.GetRandomIndex();
				outArr.Add(copyArr[index].item);
				copyArr.RemoveAt(index);
			}
		}
		else
		{
			for(int i = 0; i < n; i++) outArr.Add(entries[i].item);
		}
		return outArr.ToArray();
	}

	public void RemoveAt(int index)
	{
		if (index < 0 || index >= entries.Count) return;

		_totalWeight -= entries[index].weight;
		entries.RemoveAt(index);
	}

	void ISerializationCallbackReceiver.OnBeforeSerialize() => CalculateTotalWeight();
	void ISerializationCallbackReceiver.OnAfterDeserialize() { }
}