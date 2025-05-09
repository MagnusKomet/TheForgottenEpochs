using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


namespace MimicSpace
{
    public class MimicController : EnemyController
    {
        [Header("Movimiento")]
        [Tooltip("Altura del cuerpo respecto al suelo")]
        [Range(0.5f, 5f)]
        public float height = 1.6f;
        public float speed = 5f;
        private Vector3 velocity = Vector3.zero;
        public float velocityLerpCoef = 4f;
        private Mimic myMimic;

        public AudioClip deathSound;

        public override void Awake()
        {
            myMimic = GetComponent<Mimic>();
            if (GameObject.Find("Player"))
            {
                player = GameObject.Find("Player").transform;
            }
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            MovimientoMimico();
            IAMovement();
        }
                
        public void MovimientoMimico()
        {
            myMimic.velocity = velocity;

            transform.position += velocity * Time.deltaTime;
            RaycastHit hit;
            Vector3 destHeight = transform.position;
            if (Physics.Raycast(transform.position + Vector3.up * 5f, -Vector3.up, out hit))
                destHeight = new Vector3(transform.position.x, hit.point.y + height, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, destHeight, velocityLerpCoef * Time.deltaTime);
        }


        private void OnDestroy()
        {
            if (SceneManager.GetActiveScene().isLoaded)
            {
                BasicSpellController.PlayDeathSound(deathSound, transform.position);
            }
        }
    }
}
