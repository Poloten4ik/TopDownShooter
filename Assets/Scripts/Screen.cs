using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Screen : MonoBehaviour
{
    public GameObject text;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ScreenBlackOut()
    {
        animator.SetTrigger("BlackOut");
        text.SetActive(true);
    }
}
