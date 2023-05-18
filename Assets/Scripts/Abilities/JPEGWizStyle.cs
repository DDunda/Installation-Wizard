using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


[CreateAssetMenu(fileName = "Style", menuName = "Wizard.JPEG/Art Style")]
public class JPEGWizStyle : ScriptableObject
{
	[Serializable]
	public class StyleSprite
	{
		public Sprite sprite;
		public Gradient tint;
		public Range<float> scale = 1;
		public float colliderRadius = 0.5f;
	}
	[SerializeField]
	private WeightedArray<StyleSprite> randomSprites = new();
	public Gradient tint;
	public Range<float> randomScale = 1;

	public bool AddSprite(GameObject obj, CircleCollider2D c, string layerName)
	{
		if (obj == null) return false;

		randomSprites.CalculateTotalWeight();

		StyleSprite s = randomSprites.GetRandom();
		SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
		sr.sprite = s.sprite;
		sr.color = tint.Evaluate(UnityEngine.Random.value) * s.tint.Evaluate(UnityEngine.Random.value);
		sr.sortingLayerName = layerName;
		obj.transform.localScale *= randomScale.Random() * s.scale.Random();
		if(c != null) c.radius = s.colliderRadius;

		return true;
	}
}