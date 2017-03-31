using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vegetable : MonoBehaviour
{
    private const float GRAVITY = 2f;
    public bool isActive { get; set; }
    
    private float vervicalVelocity;
    private float speed;
    private bool isSliced = false;
    public SpriteRenderer sRenderer;

    public Sprite[] sprites;
    private int spriteIndex;
    private float lastSpriteUpdate;
    private float lasSpriteUpdateDelta = 0.1f;

    public void LaunchVegetable(float vervicalVelocity, float xSpeed, float xStart, float rand)
    {
        isActive = true;
        speed = xSpeed;
        this.vervicalVelocity = vervicalVelocity;
        transform.position = new Vector3(xStart, 0, 0);
        isSliced = false;
        spriteIndex = 0;
        sRenderer.sprite = sprites[spriteIndex];
        sRenderer.flipX = (rand > 0.5) ? true : false;
    }


    private void Update()
    {
        if (!isActive)
            return;

        vervicalVelocity -= GRAVITY * Time.deltaTime;
        transform.position += new Vector3(speed, vervicalVelocity, 0) * Time.deltaTime;

        if (isSliced)
        {
            if (spriteIndex != (sprites.Length -1) && Time.time - lastSpriteUpdate > lasSpriteUpdateDelta)
            {
                lastSpriteUpdate = Time.time;
                spriteIndex++;
                sRenderer.sprite = sprites[spriteIndex];
            }
        }

        if (transform.position.y < -1)
        {
            isActive = false;
            if (!isSliced)
               GameController.Instance.LoseLifePoint();
        }
    }

    public int Slice()
    {
        if (isSliced)
            return 0;

        if (vervicalVelocity < 0.5f)
            vervicalVelocity = 0.5f;

        speed = speed * 0.5f;
        isSliced = true;

        GameController.Instance.IncrementScore(1);
        return 1;
    }

}
