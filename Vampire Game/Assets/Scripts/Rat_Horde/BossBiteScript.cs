using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBiteScript : MonoBehaviour
{
    private Animator bossAnimator;
    private PolygonCollider2D biteHitbox;
    private float cooldown;
    private float time = 0f;
    private Rat_Horde_AI bossAI;

    // Start is called before the first frame update
    void Start()
    {
        bossAI = GetComponentInParent<Rat_Horde_AI>();
        bossAnimator = GetComponent<Animator>();
        biteHitbox = GetComponentInChildren<PolygonCollider2D>();
        biteHitbox.enabled = false;
        cooldown = bossAI.getBiteCooldown();
    }

    // Update is called once per frame
    void Update()
    {
        if(time >= 0)
        {
            time -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(time <= 0)
        {
            time = cooldown;
            if(collision.CompareTag("Player"))
            {
                bossAnimator.SetInteger("StateInt", 3);
            }
        }
    }

    public void biteActivate()
    {
        biteHitbox.enabled = true;
    }

    public void biteDeactivate()
    {
        biteHitbox.enabled = false;
        bossAnimator.SetInteger("StateInt", 0);
    }
}
