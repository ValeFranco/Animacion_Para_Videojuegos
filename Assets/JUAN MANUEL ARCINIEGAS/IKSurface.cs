using System;
using UnityEngine;
using System.Linq;

public class IKSurface : MonoBehaviour
{
    private const float REFRESH_DELAY = 0.5f;
    
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private Transform referencePoint;
    [SerializeField] private Transform raycastReference;
    
    /// <summary>
    /// Decides wether or not a surface is suitable for IK snap 
    /// </summary>
    private Collider[] Query()
    {
        return Physics.OverlapSphere(referencePoint.position, detectionRadius, detectionMask);
    }

    private bool GetNearestPositionForSnap(Collider[] nearColliders, out Vector3 nearestPoint)
    {
        try
        {
            var closestPoints = nearColliders.Select(collider => collider.ClosestPoint(referencePoint.position));
            Vector3 closestPoint = closestPoints.OrderBy(position => Vector3.Distance(referencePoint.position, position)).First();
            

            if (closestPoint == referencePoint.position)
            {
                Ray ray = new Ray(raycastReference.position, referencePoint.position - raycastReference.position);

                if (Physics.Raycast(ray, out RaycastHit hit, ray.direction.magnitude, detectionMask))
                {
                    nearestPoint = hit.point;
                    return true;
                }
                
            }

            else
            {
                nearestPoint= closestPoint;
                return true;
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
           
        }
        
        nearestPoint = transform.position;

        return false;
    }

    private void LateUpdate()
    {
        Vector3 nearestPosition;
        if (GetNearestPositionForSnap(Query(), out nearestPosition))
        {
            transform.position = nearestPosition;
            gameObject.SendMessage("OverrideIK",  true, SendMessageOptions.RequireReceiver);
        }

        else
        {
            gameObject.SendMessage("OverrideIK",  false, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(referencePoint.position, detectionRadius);
    }
}
