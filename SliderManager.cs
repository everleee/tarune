using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script is controlling the sliding UI panels

public class SliderManager : MonoBehaviour
{
    public GameObject panel;
    public Sprite[] arrows;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void ShowHidePanel()
    {
        if(panel != null)
        {
            Animator animator = panel.GetComponent<Animator>();
            if(animator != null)
            {
                bool isOpen = animator.GetBool("show");
                Debug.Log(gameObject.name + "show is: " + isOpen);
                animator.SetBool("show", !isOpen);
                if (!isOpen) image.sprite = arrows[1];
                else image.sprite = arrows[0];
            }
        }

    }
}
