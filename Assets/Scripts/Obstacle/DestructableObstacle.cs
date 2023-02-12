using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthBase))]
public class DestructableObstacle : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public List<SetupSpriteByHealth> spriteSetups;
    public HealthBase health;

    void OnValidate()
    {
        health = GetComponent<HealthBase>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        health.ResetLife();
        health.OnDamage += OnDamage;
        OrderSpriteSetups();
    }

    private void OrderSpriteSetups()
    {
        spriteSetups.Sort((i, z) => z.healthPercentage - i.healthPercentage);
    }

    private void OnDamage(HealthBase hp)
    {
            Sprite sprite = spriteRenderer.sprite;
            foreach (var setup in spriteSetups)
            {
                if(hp.GetHealthPercentage() <= setup.healthPercentage)
                {
                    sprite = setup.sprite;
                }
            }
            spriteRenderer.sprite = sprite;
    }
}
