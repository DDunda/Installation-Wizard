using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailDetacher : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer trail;

	// when this object is destroyed, detach its trail and have the trail destroy itself when it ends
	private void OnDestroy()
	{
		if (!this.gameObject.scene.isLoaded) { return; } // if the scene has unloaded, don't detach the trail, since it won't be cleaned up
		trail.transform.parent = null;
		trail.autodestruct = true;
	}
}
