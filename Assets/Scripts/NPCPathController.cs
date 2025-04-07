using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCPathController : MonoBehaviour
{    
    public Transform pathParent;
    Transform targetPoint;
    private NavMeshAgent agent;
    int index;
    Animator alpacaAnimator;
    [SerializeField]
    private Animator skeletonAnimator;
    string currentAnimationState;
    bool admiring = false;
    float rotationDuration = 0.5f;

    [SerializeField]
    private List<Material> skeletonTextures;
    [SerializeField]
    private List<Material> alpacaTextures;
    [SerializeField]
    private List<Material> ponchitoTextures;
    [SerializeField]
    private Renderer skeletonRenderer;
    [SerializeField]
    private Renderer alpacaRenderer;
    [SerializeField]
    private List<Renderer> ponchitoRenderer;


    void Start()
    {
        pathParent = GameObject.Find("NPCPath").transform;

        // Inicialitza l'�ndex en el primer punt del cam� i assigna el primer punt com a targetPoint
        index = 0;
        targetPoint = pathParent.GetChild(index);

        agent = GetComponent<NavMeshAgent>();
        alpacaAnimator = GetComponent<Animator>();

        // Asigna texturas aleatorias a los renderers
        skeletonRenderer.material = skeletonTextures[Random.Range(0, skeletonTextures.Count)];
        alpacaRenderer.material = alpacaTextures[Random.Range(0, alpacaTextures.Count)];
        foreach (var renderer in ponchitoRenderer)
        {
            renderer.material = ponchitoTextures[Random.Range(0, ponchitoTextures.Count)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!admiring)
        {
            // Mou l'objecte cap al punt objectiu
            agent.SetDestination(targetPoint.position);

            ChangeAnimationState("Walk");

            // Comprova si l'objecte ha arribat al punt objectiu i si �s aix�, canvia el punt objectiu al seg�ent
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.11f)
            {
                StartCoroutine(WaitAndChangeTarget());
            }
        }
    }

    private IEnumerator WaitAndChangeTarget()
    {
        if (pathParent.GetChild(index).name == "Exhibit")
        {
            admiring = true;
            ChangeAnimationState("IdleLookAround");

            Quaternion originalRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z);

            for (float t = 0; t < rotationDuration; t += Time.deltaTime)
            {
                transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, t / rotationDuration);
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(5, 16));

            admiring = false;
        }

        index++;
        index %= pathParent.childCount;
        targetPoint = pathParent.GetChild(index);
    }

    public void ChangeAnimationState(string newState)
    {
        // STOP THE SAME ANIMATION FROM INTERRUPTING WITH ITSELF //
        if (currentAnimationState == newState) return;

        // PLAY THE ANIMATION //
        currentAnimationState = newState;
        alpacaAnimator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
        skeletonAnimator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
    }

    // Dibuixa el cam� en l'editor per visualitzar-lo millor
    void OnDrawGizmos()
    {
        Vector3 from;
        Vector3 to;
        // Recorre tots els fills de pathParent i dibuixa l�nies entre ells
        for (int a = 0; a < pathParent.childCount; a++)
        {
            from = pathParent.GetChild(a).position;
            to = pathParent.GetChild((a + 1) %
            pathParent.childCount).position;
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawLine(from, to);
        }
    }
}
