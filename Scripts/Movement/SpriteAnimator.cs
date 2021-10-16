using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField]private Sprite[] frameArray;
    private int currentFrame;
    private float timer;
    private float frameRate;
    private SpriteRenderer spriteRenderer;
    private bool loop;
    private bool isPlaying = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isPlaying)
        {
            return;
        }
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer -= frameRate;
            if (frameArray.Length != 0)
            {
                currentFrame = (currentFrame + 1) % frameArray.Length;
                if (!loop && currentFrame == 0)
                {
                    StopPlaying();
                }
                else    //loop'taysa
                {
                    spriteRenderer.sprite = frameArray[currentFrame];
                }
            }
        }
    }

    public void StopPlaying()
    {
        isPlaying = false;
    }

    public void PlayAnimation(Sprite[] frameArray, float frameRate,bool loop)
    {
        this.loop = loop;
        this.frameArray = frameArray;
        this.frameRate = frameRate;
        currentFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = frameArray[currentFrame];
    }
}
