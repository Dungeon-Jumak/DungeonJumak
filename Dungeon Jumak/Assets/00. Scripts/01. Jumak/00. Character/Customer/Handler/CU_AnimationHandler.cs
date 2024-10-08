using System.Collections;
using UnityEngine;

public class CU_AnimationHandler
{
    private Animator m_animator;

    public CU_AnimationHandler(Animator _animator)
    {
        m_animator = _animator;
    }

    public void PlayMoveAnimation(bool _canMove)
    {
        if (_canMove)
            m_animator.SetBool("isWalk", true);
        else
            m_animator.SetBool("isWalk", false);
    }

    public void PlaySitAnimation(bool _isSit)
    {
        if (_isSit)
            m_animator.SetBool("isSit", true);
        else
            m_animator.SetBool("isSit", false);
    }

    public void PlayEatAnimation()
    {
        m_animator.SetBool("isEat", true);
    }

    public void FinishEatAnimation()
    {
        m_animator.SetBool("isEat", false);
    }
}



