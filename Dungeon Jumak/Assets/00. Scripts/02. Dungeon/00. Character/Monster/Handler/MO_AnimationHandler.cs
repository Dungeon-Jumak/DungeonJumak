using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mo_AnimationHandler
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public Mo_AnimationHandler(SpriteRenderer spriteRenderer = null, Animator animator = null)
    {
        this.spriteRenderer = spriteRenderer;
        this.animator = animator;
    }
}
