using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshButton : MonoBehaviour
{
    [SerializeField] private string soundName;
	private AudioSource abilitySound;

    private void Awake()
	{
        abilitySound = GameObject.Find(soundName).GetComponent<AudioSource>();
	}

    // Start is called before the first frame update
    void Start()
    {
        abilitySound.Play();
    }

}
