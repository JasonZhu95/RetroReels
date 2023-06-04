using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = transform.GetComponent<Animator>();
    }

    public void SetStartBoolToFalse()
    {
        anim.SetBool("start", false);
    }
}
