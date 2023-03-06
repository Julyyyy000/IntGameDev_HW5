using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudManager : MonoBehaviour
{
    public float animationSpeed = 4f;

    private float animationTimer = 0f;
    private bool isTiming = false;

    public Animator animator;

    bool hasStartedPlaying = false;

    BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (isTiming)
        {
            animationTimer += Time.deltaTime;
            //Debug.Log(animationTimer);
            if (animationTimer >= animationSpeed)
            {
                animationTimer = 0f;
                isTiming = false;
                if (stateInfo.IsName("smallCloud"))
                {
                    animator.SetBool("growMid", true);
                    boxCollider.size = new Vector3(boxCollider.size.x * 1.5f, boxCollider.size.y);
                } else if (stateInfo.IsName("midCloud"))
                {
                    animator.SetBool("growBig", true);
                    boxCollider.size = new Vector3(boxCollider.size.x * 1.5f, boxCollider.size.y);
                } else if (stateInfo.IsName("bigCloud"))
                {
                    animator.SetBool("growHuge", true);
                    boxCollider.size = new Vector3(boxCollider.size.x * 1.5f, boxCollider.size.y);
                }
                else if (stateInfo.IsName("hugeCloud"))
                {
                    //Destroy(this.gameObject);
                }
            }
        }

        if (stateInfo.normalizedTime > 0 && stateInfo.normalizedTime < 1 && !hasStartedPlaying)
        {
            //when animation starts
            hasStartedPlaying = true;
        } else if (stateInfo.normalizedTime >= 1)
        {
            //when animation ends
            if (animator.GetBool("gone"))
            {
                Destroy(this.gameObject);
            }

            isTiming = true;
            hasStartedPlaying = false;
        }
    }

}
