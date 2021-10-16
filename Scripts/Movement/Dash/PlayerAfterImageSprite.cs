using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{

    private float activeTime = .1f; //bu go kaç sn aktif olcak
    private float timeActivated; //kaç sndir aktif
    private float alpha;
    private float alphaSet = .8f;
    private float alphaMultiplier = .85f;


    private Transform player;

    private SpriteRenderer spriteRenderer; //after image go'i için
    private SpriteRenderer playerSpriteRenderer; //current sprite için

    private Color color;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        spriteRenderer.sprite = playerSpriteRenderer.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;

    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        spriteRenderer.color = color;
        if (Time.time >= (timeActivated + activeTime)) //Pool'a geri ekleme
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }

}
