using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public Animator a;
    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<Animator>();
        Idle();
    }

    // Update is called once per frame
  private void Idle()
    {
        a.SetFloat("NPC",0.5f,0.1f,Time.deltaTime);
    }
}
