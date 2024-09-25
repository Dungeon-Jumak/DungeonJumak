using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DP_AnimationHandler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public DP_AnimationHandler(SpriteRenderer spriteRenderer = null, Animator animator = null)
    {
        this.spriteRenderer = spriteRenderer;
        this.animator = animator;
    }
}
