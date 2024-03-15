using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarTalk : MonoBehaviour
{
    Animator animator;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update() {
        animator.SetBool("parla", GETAudio.isPlaying);
    }
}
