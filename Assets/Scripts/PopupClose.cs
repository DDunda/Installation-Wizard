using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupClose : MonoBehaviour
{
    [SerializeField] private float _unitsDown = -5.0f; // how far this entity should move down when closing
    [SerializeField] private float _animationLength = 0.3f; // how long the animation takes
    [SerializeField] private float _animationTime; // the current point in the animation
    [SerializeField] private SpriteRenderer _sprite; // the sprite of the entity
    [SerializeField] private BaseEnemy _enemyScript; // the script that controls this enemy's attacks/behaviour
    private bool _active = false; // whether the object is currently closing

    private Color _color;
    private Vector3 _startPos;
    private Vector3 _startScale;

    // Update is called once per frame
    void Update()
    {
        // fade, shrink and move down over time
        if (_active)
		{
            _animationTime += Time.deltaTime;
            float progress = Mathf.InverseLerp(0, _animationLength, _animationTime); // 0 - 1
            Debug.Log(progress);

            // if animation is complete, destroy this object
            if (progress == 1)
			{
                Object.Destroy(this.gameObject);
            }

            this.transform.position = _startPos + new Vector3(0, _unitsDown * progress, 0);
            this.transform.localScale = _startScale * (1 - progress);
            _sprite.color = new Color(_color.r, _color.g, _color.b, 1 - progress);
		}
    }

    public void Activate()
	{
        Object.Destroy(_enemyScript); // destroying this script stops most enemies from being able to attack during the animation
        _active = true;
        _color = _sprite.color;
        _startPos = this.transform.position;
        _startScale = this.transform.localScale;
        _animationTime = 0;
    }
}
