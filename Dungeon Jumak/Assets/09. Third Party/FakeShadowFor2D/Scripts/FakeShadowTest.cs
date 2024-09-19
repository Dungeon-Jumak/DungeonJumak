using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace TheAnh{

public class FakeShadowTest : MonoBehaviour
{
   [SerializeField] Vector2 Offset;
    protected SpriteRenderer render;
    MaterialPropertyBlock block;
    bool isDisplayed;
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();

        block = new MaterialPropertyBlock();
        render.GetPropertyBlock(block);
        block.SetVector("_SpriteSizeRel", render.bounds.size);
        block.SetVector("_Offset", Offset);
        block.SetVector("_SpritePivot", render.sprite.pivot / render.sprite.textureRect.size);
        block.SetVector("_SpriteSize", render.sprite.textureRect.size);
        render.SetPropertyBlock(block);


    }

    private void OnBecameVisible()
    {
        isDisplayed = true;
    }
    private void OnBecameInvisible()
    {
        isDisplayed = false;
    }

    float GetFootPos(SpriteRenderer renderer)
    {
        return renderer.transform.position.y - renderer.sprite.pivot.y / renderer.sprite.textureRect.size.y * renderer.sprite.bounds.max.y * 2 * renderer.transform.localScale.y;
    }
    bool InRange(SpriteRenderer target, float pos)
    {
        float x = target.transform.position.x;
        float p = target.sprite.pivot.x / target.sprite.textureRect.size.x;
        float w = 2 * target.sprite.bounds.max.x * target.transform.localScale.x;
        return pos >= x - p * w && pos <= x + p * w;
    }
    private void Update()
    {
        if (isDisplayed)
        {
            Material mat = render.sharedMaterial;
            float dir = mat.GetFloat("_Direction");

            FakeShadowTest[] shadows = FindObjectsOfType<FakeShadowTest>().Where(obj => GetFootPos(obj.render) - GetFootPos(render) < 3 && InRange(obj.render, transform.position.x + dir)).ToArray();
            float deltaY = 10;
            Transform s = transform;
            foreach (FakeShadowTest shadow in shadows)
            {
                if (shadow == this) continue;
                float dY = GetFootPos(shadow.render) - GetFootPos(render);
                if (dY < deltaY && dY > 0)
                {
                    deltaY = dY;
                    s = shadow.transform;
                }
            }

            render.GetPropertyBlock(block);
            if (s != transform)
            {
                block.SetFloat("_DeltaY", deltaY);
                // Debug.Log(deltaY);
            }

            render.SetPropertyBlock(block);

            shadows = null;
        }
    }
}
}

