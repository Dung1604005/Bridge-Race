using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NavMeshPlayerController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private Ray ray;

    [SerializeField] private RaycastHit rayCastHit;

    [SerializeField] private Camera cam;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out rayCastHit))
            {
                agent.SetDestination(rayCastHit.point);
            }
        }
    }
}
