using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentIA : MonoBehaviour
{
    [Header("Adversaire Movement")]
    public float movementspeed = 1f;
    public float rotationspped = 10f;
    public Animator animator;
    public CharacterController characterController;

    [Header("Adversaire Fight")]
    public float attackCooldown = 0.5f;
    public int attackDamages = 2;
    public string[] attackAnimations = { "Attack1Animation", "Attack2Animation", "Attack3Animation", "Attack4Animation" };
    public float dodgedistance = 2f;
    public int attackcount = 0;
    public int randomnumber;
    public float rayonattack= 2.2f;
    public fightingcontroller[] fightingcontrollers;
    public Transform[] players;
    public bool istakingDamage;

    private float lastAttackTime;

    [Header("effects et songs")]
    public ParticleSystem attack1effect;
    public ParticleSystem attack2effect;
    public ParticleSystem attack3effect;
    public ParticleSystem attack4effect;


    public AudioClip[] hitsounds;
    public AudioClip[] attacksounds;

    [Header("Health")]
    public int maxhealth = 100;
    public int currenthealth;
    public HealthBar healthBar;


    private bool isFirstAttackReady = false;
    private bool hasWon = false;

    public AudioClip salutationSound;
    public AudioClip walkingSound;

    void Start()
    {
        currenthealth = maxhealth;
        healthBar.GiveFullHEALTH(currenthealth);
        createRandomNumber();
    }
    void Update()
    {
        /*if( attackcount == randomnumber)
        {
            attackcount = 0;
            createRandomNumber();
        }*/
        for(int i=0; i< fightingcontrollers.Length; i++)
        {
            if (hasWon) break; // Si l'adversaire a gagné, arrêter la boucle

            // Vérifie si l'adversaire a gagné
            if (fightingcontrollers[i].isDead && currenthealth > 0)
            {
                PlayVictoryAnimation();
                break;
            }
            if (players[i].gameObject.activeSelf &&  !fightingcontrollers[i].isDead && currenthealth >0 && Vector3.Distance(transform.position, players[i].position) <= rayonattack)
            {
                animator.SetBool("Walking", false);
                
                if (!isFirstAttackReady) // Vérifie si l'attente de 2 secondes a eu lieu
                {
                    animator.Play("salutation");
                    StartCoroutine(PrepareFirstAttack(i, 2f)); // Attendre 2 secondes avant la première attaque
                }

                else if (Time.time - lastAttackTime > attackCooldown)
                {
                    int randomAttackindex = Random.Range(0, attackAnimations.Length);
                    if (!istakingDamage)
                    {
                        PerformAttack(randomAttackindex);
                    }
                    fightingcontrollers[i].StartCoroutine(fightingcontrollers[i].PlayHitDamageAnimation(attackDamages));
                }
               
            }
            else
            {
                if (players[i].gameObject.activeSelf && !fightingcontrollers[i].isDead  && currenthealth >0)
                {
                    Vector3 direction = (players[i].position - transform.position).normalized;
                    characterController.Move(direction * movementspeed * Time.deltaTime);
                    Quaternion targetrotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, rotationspped * Time.deltaTime);

                    animator.SetBool("Walking", true);
                }
            }
        }
    }

    void PerformAttack(int attackIndex)
    {
       
        animator.Play(attackAnimations[attackIndex]);
        int damage = attackDamages;
        Debug.Log("performed attack " + (attackIndex + 1) + " dealing" + damage + "damage");
        lastAttackTime = Time.time;
        if (attacksounds != null && attackIndex < attacksounds.Length && attacksounds[attackIndex] != null)
        {
            AudioSource.PlayClipAtPoint(attacksounds[attackIndex], transform.position);
            Debug.Log("Played attack sound for attack " + (attackIndex + 1));
        }
    }
    IEnumerator PrepareFirstAttack(int playerIndex, float delay)
    {
        yield return new WaitForSeconds(delay);  // Attend 2 secondes
        isFirstAttackReady = true;  // Permet la première attaque après le délai

       
    }
    public void PlaySalutationSound()
    {
        if (salutationSound != null)
        {
            AudioSource.PlayClipAtPoint(salutationSound, transform.position);
        }
    }
    public void PlayWalkingSound()
    {
        if (walkingSound != null)
        {
            AudioSource.PlayClipAtPoint(walkingSound, transform.position);
        }
    }
    void performDodgeFront()
    {
         animator.Play("DodgeFrontAnimation");
         Vector3 dodgedirection = -transform.forward * dodgedistance;
        characterController.SimpleMove(dodgedirection);
    }

    void createRandomNumber()
    {
        randomnumber = Random.Range(1, 5);
    }

    public IEnumerator PlayHitDamageAnimation(int takedamage)
    {
        yield return new WaitForSeconds(0.5f);
        if (hitsounds != null && hitsounds.Length > 0)
        {
            int randomindex = Random.Range(0, hitsounds.Length);
            AudioSource.PlayClipAtPoint(hitsounds[randomindex], transform.position);
        }
        currenthealth -= takedamage;
        healthBar.SetHealth(currenthealth);
        if (currenthealth <= 0)
        {
            Die();
        }
        animator.Play("HitDamageAnimation");
    }
    void Die()
    {
        
        animator.Play("death_animation");
        characterController.enabled = false; 
        movementspeed = 0f; 


        Debug.Log("Player died");
    }

    void PlayVictoryAnimation()
    {
        animator.SetBool("Walking", false);
        animator.Play("gagner");
        hasWon = true; 
        Debug.Log("Opponent has won the fight!");
    }
    public void Attack1effect()
    {
        attack1effect.Play();
    }
    public void Attack2effect()
    {
        attack2effect.Play();
    }
    public void Attack3effect()
    {
        attack3effect.Play();
    }
    public void Attack4effect()
    {
        attack4effect.Play();
    }
}
