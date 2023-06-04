using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ----------------------------------------------------------------------------
 * Class: AnimationEventHandler
 * Description: Allows a function to set an animation bool to false by using the
 * Unity Animation Events.
 * ---------------------------------------------------------------------------- */
public class AnimationEventHandler : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = transform.GetComponent<Animator>();
    }

    /* ------------------------------------------------------------------------
    * Function: SetStartBoolToFalse
    * Description: When called, set the animator bool named "start" to false
    * ---------------------------------------------------------------------- */
    public void SetStartBoolToFalse()
    {
        anim.SetBool("start", false);
    }
}
