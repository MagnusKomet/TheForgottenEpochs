using System;
using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack.Interface;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerSpace
{
    public class PlayerController : MonoBehaviour
    {

        // ---------- //
        //   General  //
        // ---------- //

        #region General

        PlayerInput playerInput;
        PlayerInput.OnFootActions input;

        [SerializeField]
        GameObject shortParticleOnHit;
        [SerializeField]
        GameObject longParticleOnHit;

        [SerializeField]
        GameObject ComboHud;
        [SerializeField]
        GameObject Earth;
        [SerializeField]
        GameObject Fire;
        [SerializeField]
        GameObject Air;
        [SerializeField]
        GameObject Water;

        [SerializeField]
        ElementalCrystalsController CrystalsController;

        CharacterController controller;
        Animator animator;
        AudioSource audioSource;

        [Header("Controller")]
        public float moveSpeed = 5;
        public float gravity = -9.8f;
        public float jumpHeight = 1.2f;

        Vector3 _PlayerVelocity;

        bool isGrounded;

        [Header("Camera")]
        public Camera cam;
        public float sensitivity;

        float xRotation = 0f;

        private static string combo = "";

        InventoryDataController inventory;

        public float playerReach = 3f;
        Interactable currentInteractable;

        void Awake()
        {
            
            ComboHud = GameObject.Find("ComboHUD");
            controller = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            audioSource = GetComponent<AudioSource>();

            playerInput = new PlayerInput();
            input = playerInput.OnFoot;
            AssignInputs();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;            
        }

        private void Start()
        {
            inventory = InventoryVisualManager.Instance.inventoryData;
        }

        void Update()
        {
            isGrounded = controller.isGrounded;

            CheckInteraction();

            if (input.Interact.WasPressedThisFrame() && currentInteractable != null)
            {
                currentInteractable.Interact();
            }


            if (input.Earth.WasPressedThisFrame())
            {
                AddToCombo('E', Earth);
            }
            else if (input.Air.WasPressedThisFrame())
            {
                AddToCombo('A', Air);
            }
            else if (input.Fire.WasPressedThisFrame())
            {
                AddToCombo('F', Fire);
            }
            else if (input.Water.WasPressedThisFrame())
            {
                AddToCombo('W', Water);
            }

            SetAnimations();

        }

        private void CheckInteraction()
        {
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if(Physics.Raycast(ray, out hit, playerReach))
            {
                if(hit.collider.tag == "Interactable")
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    
                    if(currentInteractable != null && interactable != currentInteractable)
                    {
                        currentInteractable.DisableOutline();
                    }

                    if (interactable.enabled)
                    {
                        SetCurrentInteractable(interactable);
                    }
                    else
                    {
                        DisableCurrentInteractable();
                    }
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }

        }

        private void SetCurrentInteractable(Interactable interactable)
        {
            currentInteractable = interactable;
            currentInteractable.EnableOutline();
            InventoryVisualManager.Instance.EnableInteractionText(interactable.message);
        }

        private void DisableCurrentInteractable()
        {            
            if (currentInteractable != null)
            {
                InventoryVisualManager.Instance.DisableInteractionText();
                currentInteractable.DisableOutline();
                currentInteractable = null;
            }
        }

        void AddToCombo(char element, GameObject elementObject)
        {
            if (!InventoryVisualManager.Instance.menuActivated)
            {
                if (combo.Length >= 15)
                {
                    DestroyCombo();
                }
                else
                {
                    combo += element;
                    CrystalsController.ReduceCrystalMana(element);
                    Instantiate(elementObject).transform.SetParent(ComboHud.transform);
                }
            }            
        }

        void FixedUpdate()
        { MoveInput(input.Movement.ReadValue<Vector2>()); }

        void LateUpdate()
        { LookInput(input.Look.ReadValue<Vector2>()); }

        void MoveInput(Vector2 input)
        {
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = input.x;
            moveDirection.z = input.y;

            controller.Move(transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
            _PlayerVelocity.y += gravity * Time.deltaTime;
            if (isGrounded && _PlayerVelocity.y < 0)
                _PlayerVelocity.y = -2f;
            controller.Move(_PlayerVelocity * Time.deltaTime);
        }

        void LookInput(Vector3 input)
        {
            float mouseX = input.x;
            float mouseY = input.y;

            xRotation -= (mouseY * Time.deltaTime * sensitivity);
            xRotation = Mathf.Clamp(xRotation, -80, 80);

            cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            transform.Rotate(Vector3.up * (mouseX * Time.deltaTime * sensitivity));
        }

        void OnEnable()
        { input.Enable(); }

        void OnDisable()
        { input.Disable(); }

        void Jump()
        {
            // Adds force to the player rigidbody to jump
            if (isGrounded && !InventoryVisualManager.Instance.menuActivated)
                _PlayerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        void AssignInputs()
        {
            input.Jump.performed += ctx => Jump();
            input.Attack.performed += ctx => Attack();
        }

        

        #endregion

        // ---------- //
        // ANIMATIONS //
        // ---------- //

        #region Animations
        public const string IDLE = "Idle";
        public const string WALK = "Walk";
        public const string ATTACK1 = "Attack 1";
        public const string ATTACK2 = "Attack 2";
        public const string ATTACKORB = "Attack Orb";

        string currentAnimationState;

        public void ChangeAnimationState(string newState)
        {
            // STOP THE SAME ANIMATION FROM INTERRUPTING WITH ITSELF //
            if (currentAnimationState == newState) return;

            // PLAY THE ANIMATION //
            currentAnimationState = newState;
            animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
        }

        void SetAnimations()
        {
            // If player is not attacking
            if (!attacking)
            {
                if (_PlayerVelocity.x == 0 && _PlayerVelocity.z == 0)
                { ChangeAnimationState(IDLE); }
                else
                { ChangeAnimationState(WALK); }
            }
        }

        #endregion

        // ------------------- //
        // ATTACKING BEHAVIOUR //
        // ------------------- //

        #region Attacks

        [Header("Attacking")]
        private float attackSpeed = 0.8f;


        public AudioClip swordSwing;
        public AudioClip hitSound;

        bool attacking = false;
        bool readyToAttack = true;
        int attackCount;


        public void Attack()
        {
            if (!InventoryVisualManager.Instance.menuActivated)
            {
                if(combo.Length > 0)
                {
                    if (!readyToAttack || attacking) return;

                    readyToAttack = false;
                    attacking = true;

                    Invoke(nameof(ResetAttack), attackSpeed);

                    ShootSpell();

                    audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    audioSource.PlayOneShot(swordSwing);

                    if (attackCount == 0)
                    {
                        ChangeAnimationState(ATTACK1);
                        attackCount++;
                    }
                    else if (attackCount == 1)
                    {
                        ChangeAnimationState(ATTACKORB);
                        attackCount++;
                    }
                    else
                    {
                        ChangeAnimationState(ATTACK2);
                        attackCount = 0;
                    }
                }
                else
                {
                    //TODO: Add bad spell animation
                }
                

            }
        }

        void ResetAttack()
        {
            attacking = false;
            readyToAttack = true;
        }



        void DestroyCombo()
        {
            foreach (Transform child in ComboHud.transform)
            {
                Destroy(child.gameObject);
            }
            combo = "";
        }

        #endregion

        #region Spells

        // tmp
        HashSet<string> unlockedSpells = new HashSet<string> { "FFA", "F", "AAE" };


        [SerializeField]
        private GameObject fireball;
        [SerializeField]
        private GameObject secondFireball;
        [SerializeField]
        private GameObject windBlade;

        [SerializeField]
        private Transform SpellsSpawnPoint;

        void ShootSpell()
        {

            if (unlockedSpells.Contains(combo))
            {
                switch (combo)
                {
                    case "F":
                        ShootFireball(fireball);
                        break;

                    case "FFA":
                        ShootFireball(secondFireball);
                        break;

                    case "AAE":
                        ShootWindBlade();
                        break;
                }
            }

            DestroyCombo();
        }

        void ShootFireball(GameObject tmpFireball)
        {
            var spell = Instantiate(tmpFireball, SpellsSpawnPoint.position, SpellsSpawnPoint.rotation);
            Rigidbody rb = spell.GetComponent<Rigidbody>();
            FireballController controller = spell.GetComponent<FireballController>();
            if (controller != null)
            {
                float damageMultiplier = CrystalsController.GetDamageMultiplier(combo);
                int damage = (int)(15 * damageMultiplier);
                controller.damage = damage;
            }

            if (rb != null)
            {
                rb.velocity = cam.transform.forward * 50f;
            }
        }

        void ShootWindBlade()
        {
            var spell = Instantiate(windBlade, SpellsSpawnPoint.position, SpellsSpawnPoint.rotation);
            Rigidbody rb = spell.GetComponent<Rigidbody>();
            WindBladeController controller = spell.GetComponent<WindBladeController>();
            if (controller != null)
            {
                float damageMultiplier = CrystalsController.GetDamageMultiplier(combo);
                int damage = (int)(50 * damageMultiplier);
                controller.damage = damage;
            }

            if (rb != null)
            {
                rb.velocity = cam.transform.forward * 50f;
            }
            
        }

        #endregion

        // ------------------- //
        //      COLLISIONS     //
        // ------------------- //

        #region Collisions

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Portal")
            {
                if (other.gameObject.name == "PortalMuseo")
                {
                    SceneManager.LoadScene("MuseoScene");
                }
                else if (other.gameObject.name == "PortalBosque")
                {
                    SceneManager.LoadScene("BosqueScene");
                }
            }
        }
           


        #endregion

    }
}
