using System;
using UnityEngine;
using System.Linq;

public class IKSurface : MonoBehaviour
{
    private const float REFRESH_DELAY = 0.5f;
    
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private Transform referencePoint;
    
    /// <summary>
    /// Decides wether or not a surface is suitable for IK snap 
    /// </summary>
    private Collider[] Query()
    {
        return Physics.OverlapSphere(referencePoint.position, detectionRadius, detectionMask);
    }

    private Vector3 GetNearestPositionForSnap(Collider[] nearColliders)
    {
        try
        {
            var closestPoints = nearColliders.Select(collider => collider.ClosestPoint(referencePoint.position));
            Vector3 closestPoint = closestPoints.OrderBy(position => Vector3.Distance(referencePoint.position, position)).First();
            

            if (closestPoint == referencePoint.position)
            {
                Ray ray = new Ray(transform.position, referencePoint.position - transform.position);

                if (Physics.Raycast(ray, out RaycastHit hit, ray.direction.magnitude, detectionMask))
                {
                    return hit.point;
                }
                
            }

            else
            {
                return closestPoint;
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
           
        }

        return transform.position;
    }

    private void LateUpdate()
    {
        Vector3 positionToSnap = GetNearestPositionForSnap(Query());
        Debug.DrawLine(transform.position, positionToSnap);
        transform.position = positionToSnap;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(referencePoint.position, detectionRadius);
    }
}
