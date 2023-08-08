using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public Animator anim;
    AudioSource audio;
    public float cooldown = 2f;
    public AudioClip AttackSound;
    private bool isAttacking = false;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAttacking && Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Choose a random attack method or implement your attack selection logic
            int randomAttack = Random.Range(1, 5);
            StartCoroutine(PerformAttack(randomAttack));
        }
    }

    private IEnumerator PerformAttack(int attackIndex)
    {
        isAttacking = true;
        switch (attackIndex)
        {
            case 1:
                Attack1();
                break;
            case 2:
                Attack2();
                break;
            case 3:
                Attack3();
                break;
            case 4:
                Attack4();
                break;
        }

        yield return new WaitForSeconds(cooldown);
        isAttacking = false;
    }

    public void Attack1()
    {
        audio.PlayOneShot(AttackSound);
        anim.SetFloat("BOSSATTACK", 0.1f);
    }

    private void Attack2()
    {
        audio.PlayOneShot(AttackSound);
        anim.SetFloat("BOSSATTACK", 0.5f);
    }

    private void Attack3()
    {
        audio.PlayOneShot(AttackSound);
        anim.SetFloat("BOSSATTACK", 0.7f);
    }

    private void Attack4()
    {
        audio.PlayOneShot(AttackSound);
        anim.SetFloat("BOSSATTACK", 1f);
    }
}
