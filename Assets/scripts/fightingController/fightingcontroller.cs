using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using System.IO;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Security.Cryptography;

public class fightingcontroller : MonoBehaviour
{
    [Header("Initial Movement")]
    public float autoMoveDistance = 5f;
    public float autoMoveSpeed = 3f;
    private bool isAutoMoving = true;
    private Vector3 initialPosition;

    [Header("player Movement")]
    public float movementspeed = 1f;
    public float rotationspped = 10f;
    private Animator animator;
    private CharacterController characterController;

    [Header("player Fight")]
    public float attackCooldown = 0.5f;
    public int attackDamages = 5;
    public string[] attackAnimations = { "Attack1Animation", "Attack2Animation", "Attack3Animation", "Attack4Animation", "Attack5Animation" };
    public float dodgedistance = 2f;

    public float gravity = 9.81f; // Valeur de la gravité terrestre standard

    public float attackrayon = 2.2f;
    public Transform[] opponents;
    private float lastAttackTime;

    [Header("effects et songs")]
    public ParticleSystem attack1effect;
    public ParticleSystem attack2effect;
    public ParticleSystem attack3effect;
    public ParticleSystem attack4effect;
    public ParticleSystem attack5effect;
    public ParticleSystem protectioneffects;
    public ParticleSystem healthbaraugmente;



    public AudioClip[] hitsounds;
    public AudioClip[] attacksounds;

    [Header("Health")]
    public int maxhealth = 100;
    public int currenthealth;
    public HealthBar healthBar;

    public bool isDead = false;
    public bool hasWon = false;

    public AudioClip salutationSound;
    public AudioClip walkingSound;
    public AudioClip protectionSound;
    public AudioClip healingSound;
    public AudioClip voicewinner;
    public AudioClip perdusound;

    [Header("Protection Settings")]
    public float protectionDuration = 10f; // Durée de la protection
    private bool isProtected = false; // Indique si la protection est active
    private float protectionEndTime = 0f; // Heure de fin de la protection

    [Header("UI Buttons")]
    public Button attackButton1;
    public Button attackButton2;
    public Button attackButton3;
 
    public Button healButton;
    public Button protectionButton;
    public Button jumpButton;

    public Button moveUpButton;
    public Button moveDownButton;
    public Button moveLeftButton;
    public Button moveRightButton;

   


    private float doubleClickTime = 0.3f; // Temps maximum entre deux clics pour être considéré comme un double clic
    private float lastClickTime1 = 0f; // Pour attackButton1
    private float lastClickTime2 = 0f; // Pour attackButton2
    private float lastClickTime3 = 0f; // Pour attackButton3


    private Vector3 moveDirection;
    private bool isMoving = false;



    [Header("Joystick")]
    public VirtualJoystickFloating virtualJoystick;





    void Start()
    {
        // Test de sauvegarde et de chargement
       
        currenthealth = maxhealth;
        healthBar.GiveFullHEALTH(currenthealth);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        initialPosition = transform.position; // Enregistre la position de départ

        animator.SetBool("Walking", true);// Démarre l'animation de marche

        // Assigner les boutons à leurs fonctions
        attackButton1.onClick.AddListener(() => HandleAttackButton1());
        attackButton2.onClick.AddListener(() => HandleAttackButton2());
        attackButton3.onClick.AddListener(() => HandleAttackButton3());
        jumpButton.onClick.AddListener(performDodgeFront);

        healButton.onClick.AddListener(HealPlayer);
        protectionButton.onClick.AddListener(ActivateProtection);
       


        // Associer les boutons de déplacement
        moveUpButton.onClick.AddListener(MoveUpButton);
        moveDownButton.onClick.AddListener(MoveDownButton);
        moveLeftButton.onClick.AddListener(MoveLeftButton);
        moveRightButton.onClick.AddListener(MoveRightButton);

        // Si tu veux désactiver le mouvement lorsque les boutons sont relâchés, tu peux gérer cela ici :
        moveUpButton.onClick.AddListener(() => StopMoving());
        moveDownButton.onClick.AddListener(() => StopMoving());
        moveLeftButton.onClick.AddListener(() => StopMoving());
        moveRightButton.onClick.AddListener(() => StopMoving());


        if (protectionSound != null)
        {
            AudioSource.PlayClipAtPoint(protectionSound, transform.position);
            Debug.Log("Healing sound played.");
        }

    }
    void HandleAttackButton1()
    {
        float currentTime = Time.time;
        if (currentTime - lastClickTime1 <= doubleClickTime)
        {
            // Double clic détecté : PerformAttack(4)
            PerformAttack(4);
        }
        else
        {
            // Simple clic : PerformAttack(1)
            PerformAttack(1);
        }
        lastClickTime1 = currentTime;
    }

    void HandleAttackButton2()
    {
        float currentTime = Time.time;
        if (currentTime - lastClickTime2 <= doubleClickTime)
        {
            // Double clic détecté : PerformAttack(2)
            PerformAttack(2);
        }
        else
        {
            // Simple clic : PerformAttack(0)
            PerformAttack(0);
        }
        lastClickTime2 = currentTime;
    }

    void HandleAttackButton3()
    {
        float currentTime = Time.time;
        if (currentTime - lastClickTime3 <= doubleClickTime)
        {
            // Double clic détecté : Tu peux ajouter une autre action ici si nécessaire
            Debug.Log("Double clic sur attackButton3, aucune action spécifique.");
        }
        else
        {
            // Simple clic : PerformAttack(3)
            PerformAttack(3);
        }
        lastClickTime3 = currentTime;
    }
    void Update()
    {
       
        if (isAutoMoving)
        {
            PerformAutoMove();
        }
        else if (!isDead || !hasWon)
        {
            if (virtualJoystick != null)
            {
                float horizontal = virtualJoystick.Horizontal();
                float vertical = virtualJoystick.Vertical();
                moveDirection = new Vector3(horizontal, 0, vertical).normalized;

                if (moveDirection.magnitude > 0.1f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationspped * Time.deltaTime);
                    characterController.Move(moveDirection * movementspeed * Time.deltaTime);
                    animator.SetBool("Walking", true);
                }
                else
                {
                    animator.SetBool("Walking", false);
                }
            }



            

            // Activer l'effet de protection avec la touche H
            if (Input.GetKeyDown(KeyCode.H) && !isProtected)
            {
                ActivateProtection();
            }

            // Désactiver la protection lorsque la durée est écoulée
            if (isProtected && Time.time >= protectionEndTime)
            {
                DeactivateProtection();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                HealPlayer();
              
            }


            // Gérer les attaques
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PerformAttack(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PerformAttack(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PerformAttack(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                PerformAttack(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                PerformAttack(4);
            }
           
        }
    }
   // Fonction de mouvement vers le haut
public void MoveUpButton()
{
    moveDirection = transform.forward;  // Direction avant
    isMoving = true;
}

// Fonction de mouvement vers le bas
public void MoveDownButton()
{
    moveDirection = -transform.forward; // Direction arrière
    isMoving = true;
}

// Fonction de mouvement vers la gauche
public void MoveLeftButton()
{
    moveDirection = -transform.right;  // Direction gauche
    isMoving = true;
}

// Fonction de mouvement vers la droite
public void MoveRightButton()
{
    moveDirection = transform.right;   // Direction droite
    isMoving = true;
}

// Méthode pour arrêter le mouvement
public void StopMoving()
{
    isMoving = false;  // Arrêter le mouvement
}


    void ActivateProtection()
    {
        if (isProtected)
        {
            Debug.Log("Protection is already active.");
            return; // Empêche d'activer la protection plusieurs fois
        }

        isProtected = true;
        protectionEndTime = Time.time + protectionDuration;

        // Déclenche l'effet de protection
        if (protectioneffects != null)
        {
            protectioneffects.transform.position = transform.position; // Place l'effet sur le joueur
            protectioneffects.Play(); // Joue l'effet de protection
            Debug.Log("Protection effect triggered.");
        }

        if (protectionSound != null)
        {
            AudioSource.PlayClipAtPoint(protectionSound, transform.position);
            Debug.Log("Protection sound played.");
        }

        Debug.Log("Protection activated for " + protectionDuration + " seconds.");
    }



    void DeactivateProtection()
    {
        isProtected = false;

        // Arrête l'effet de protection
        if (protectioneffects != null && protectioneffects.isPlaying)
        {
            protectioneffects.Stop();
            Debug.Log("Protection effect stopped.");
        }

        Debug.Log("Protection deactivated.");
    }




    void PerformAutoMove()
    {
        // Calcule la direction vers l'avant
        Vector3 moveDirection = transform.forward;

        // Vérifie si la distance cible est atteinte
        if (Vector3.Distance(initialPosition, transform.position) < autoMoveDistance)
        {
            // Déplace le joueur
            characterController.Move(moveDirection * autoMoveSpeed * Time.deltaTime);
        }
        else
        {
            // Arrête le mouvement automatique
            isAutoMoving = false;
            animator.SetBool("Walking", false);
            animator.Play("salutation");// Arrête l'animation de marche
            Debug.Log("Auto move complete, player control enabled.");
        }
    }

   /*void performmovement()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(-VerticalInput, 0f, HorizontalInput);
        if (movement != Vector3.zero)
        {
            Quaternion targetrotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, rotationspped * Time.deltaTime);
            if (HorizontalInput > 0)
            {
                animator.SetBool("Walking", true);
            }
            else if (HorizontalInput < 0)
            {
                animator.SetBool("Walking", true);
            }
            else if (VerticalInput != 0)
            {
                animator.SetBool("Walking", true);
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }
        characterController.Move(movement * movementspeed * Time.deltaTime);
    }*/
    void PerformAttack(int attackIndex)
    {
        if (hasWon)
        {
            if (voicewinner != null)
            {
                AudioSource.PlayClipAtPoint(voicewinner, transform.position);
                Debug.Log("voice winner played.");
            }
            Debug.Log("Player won the fight!");
            Debug.Log("Cannot perform attack, player has already won.");
          
            return; // Empêche toute attaque si le joueur a gagné
        }
        if (isDead)
        {
            if (perdusound != null)
            {
                AudioSource.PlayClipAtPoint(perdusound, transform.position);
                Debug.Log("voice perdu played.");
            }
            Debug.Log("Cannot perform attack, player has already died game Over.");
            return; // Empêche toute attaque si le joueur est mort
        }
        if (Time.time - lastAttackTime > attackCooldown)
        {
            animator.Play(attackAnimations[attackIndex]);
            int damage = attackDamages;
            Debug.Log("Performed attack " + (attackIndex + 1) + " dealing " + damage + " damage.");
            lastAttackTime = Time.time;

            // Lecture du son d'attaque correspondant
            if (attacksounds != null && attackIndex < attacksounds.Length && attacksounds[attackIndex] != null)
            {
                AudioSource.PlayClipAtPoint(attacksounds[attackIndex], transform.position);
                Debug.Log("Played attack sound for attack " + (attackIndex + 1));
            }

            // Appliquer les dommages aux adversaires dans le rayon
            foreach (Transform opponent in opponents)
            {
                if (Vector3.Distance(transform.position, opponent.position) <= attackrayon)
                {
                    opponent.GetComponent<OpponentIA>().StartCoroutine(opponent.GetComponent<OpponentIA>().PlayHitDamageAnimation(attackDamages));
                    if (opponent.GetComponent<OpponentIA>().currenthealth <= 0 && currenthealth > 0)
                    {
                        hasWon = true;
                        animator.Play("gagner");
                        

                    }
                }
            }
        }
        else
        {
            Debug.Log("Cannot perform attack yet. Cooldown time remaining.");
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

        Vector3 dodgeDirection = transform.forward * dodgedistance;
        dodgeDirection.y -= gravity * Time.deltaTime;  // Appliquer la gravité

        characterController.Move(dodgeDirection * Time.deltaTime);
    }

    public IEnumerator PlayHitDamageAnimation(int takedamage)
    {
        if (isProtected)
        {
            Debug.Log("Player is protected, no damage taken.");
            yield break; // Sortie immédiate si protégé
        }

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
        isDead = true;
        animator.Play("death_animation");
        characterController.enabled = false;
        movementspeed = 0f;
        Debug.Log("Player died");
    }
    public void PlaySalutationSound()
    {
        if (salutationSound != null)
        {
            AudioSource.PlayClipAtPoint(salutationSound, transform.position);
        }
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
    public void Attack5effect()
    {
        attack5effect.Play();
    }
    public void protectioneffect()
    {
        protectioneffects.Play();
        
    }
    public void Healthbaraugmente()
    {
        healthbaraugmente.Play();
       
    }

    public void HealPlayer()
    {
        if (currenthealth < maxhealth)
        {
            StartCoroutine(PerformHealingWithEffect());
        }
        else
        {
            Debug.Log("Player is already at max health.");
        }
    }

    private IEnumerator PerformHealingWithEffect()
    {
        // Déclenchement de l'effet de guérison
        if (healthbaraugmente != null)
        {
            healthbaraugmente.transform.position = transform.position; // Place l'effet sur le joueur
            Healthbaraugmente();
            if (healingSound != null)
            {
                AudioSource.PlayClipAtPoint(healingSound, transform.position);
                Debug.Log("Healing sound played.");
            }
            Debug.Log("Starting healing effect...");
        }

        yield return new WaitForSeconds(1f); // Temps de l'effet de guérison

        // Augmentation des points de santé
        currenthealth = Mathf.Min(currenthealth + 30, maxhealth);
        healthBar.SetHealth(currenthealth);

        Debug.Log("Player healed by 30 points. Current health: " + currenthealth);

        // Arrêter l'effet après un délai
        yield return new WaitForSeconds(1f);
        if (healthbaraugmente != null && healthbaraugmente.isPlaying)
        {
            healthbaraugmente.Stop();
            Debug.Log("Healing effect stopped.");
        }
    }

    
    
    


}