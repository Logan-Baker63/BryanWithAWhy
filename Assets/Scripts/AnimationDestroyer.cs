using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDestroyer : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!AnimatorIsPlaying())
        {
            Destroy(gameObject);
        }
    }

    bool AnimatorIsPlaying()
    {
        AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);

        return asi.length * asi.speed > asi.normalizedTime;
    }
}
