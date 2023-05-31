using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitFlash : MonoBehaviour
{
    [SerializeField] private Image img; // the image/panel whos color is being changed
    [SerializeField] private Gradient hitGradient; // how the color changes over time (from a given color to fully transparent)
    [SerializeField] private float hitLength = 0.1f; // how long the flash lasts, in seconds
    private float hitTime; // the current point in the animation

    // Start is called before the first frame update
    void Start()
    {
        hitTime = hitLength;
    }

    // Update is called once per frame
    void Update()
    {
        // pick a color on the gradient based on the y value of the curve given the x value of the time passed
        // this script would likely be more efficient as a coroutine, but this works fine
        hitTime += Time.deltaTime;
        img.color = hitGradient.Evaluate(Mathf.InverseLerp(0, hitLength, hitTime)); 
    }

    // restart the flash
    public void StartFlash()
	{
        hitTime = 0f;
	}
}
