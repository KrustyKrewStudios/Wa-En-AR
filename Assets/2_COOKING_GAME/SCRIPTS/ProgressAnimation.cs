/*
 * Author: Curtis Low
 * Date: 06/08/2024
 * Description: controls animation for progress bar in how to play panel
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressAnimation : MonoBehaviour
{
    private Animator anim;
    void OnEnable()
    {
        anim = GetComponent<Animator>();
    }
    public void StartAnimation()
    {
        anim.SetBool("Animation" ,true);
    }

    public void StopAnimation()
    {
        anim.SetBool("Animation", false);
    }


}
