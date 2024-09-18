using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TheAnh{
public class FakeShadow : MonoBehaviour
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
}
}
