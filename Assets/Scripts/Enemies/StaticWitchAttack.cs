using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticWitchAttack : MonoBehaviour
{
    [SerializeField] private float detonationTime;
    public SpriteRenderer spriteRenderer;
    public Sprite detonatedSprite;
    [SerializeField] private float duration;
    public bool detonated = false;
    public bool dealDamage = false;
    public bool damaging = false;

    protected iFrameHealth plrHealth;

    private void Start()
    {
        StartCoroutine(CountDown());
        plrHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<iFrameHealth>();
    }

    private void Update()
    {
        if (detonated && dealDamage && !damaging)
        {
            StartCoroutine(Damage());
        }
    }

    private IEnumerator CountDown()
    {
        var elapsedTime = 0f;

        while (elapsedTime < detonationTime) 
        { 
            if (spriteRenderer.color == Color.white)
            {
                spriteRenderer.color = Color.red;
            }
            else if (spriteRenderer.color == Color.red)
            {
                spriteRenderer.color = Color.white;
            }
            elapsedTime += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine(Detonate());
    }

    private IEnumerator Detonate()
    {
        detonated = true;
        spriteRenderer.sprite = detonatedSprite;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            dealDamage = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            dealDamage = false;
        }
    }

    private IEnumerator Damage()
    {
        damaging = true;
        plrHealth.ChangeHealth(-2);
        yield return new WaitForSeconds(0.5f);
        damaging = false;
    }
}
