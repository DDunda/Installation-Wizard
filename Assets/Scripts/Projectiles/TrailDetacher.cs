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
		trail.transform.parent = null;
		trail.autodestruct = true;
	}
}
