using UnityEngine;

public class HealthbarController : MonoBehaviour
{
	public EntityHealth health;

	public SpriteRenderer healthbarSprite;
    public SpriteRenderer backgroundSprite;

    public Vector2 minSize = new(1,1);
    private Vector2 maxSize;

    public bool showIfFull = false;
    public bool showIfDead = false;

	private void Awake()
	{
        maxSize = healthbarSprite.size;
	}

	void Update()
    {
        if((!showIfDead && health.Health == 0) || (!showIfFull && health.Health == health.HealthMax))
        {
            healthbarSprite.enabled = false;
            backgroundSprite.enabled = false;
            return;
        }


        healthbarSprite.enabled = true;
        backgroundSprite.enabled = true;

        Range<Vector2> size = new(minSize, maxSize);

        healthbarSprite.size = size.Lerp(Mathf.InverseLerp(0, health.HealthMax, health.Health));
	}
}
