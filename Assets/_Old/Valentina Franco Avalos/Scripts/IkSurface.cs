using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IkSurface : MonoBehaviour
{
    //const float REFRESH_DELAY = 0.5f; 
    //[SerializeField] private float detectionRadius;
    //[SerializeField] private LayerMask detectionMask;
    //[SerializeField] private Transform referencePoint;

    ////1. Encontrar objetos idóneos

    ///// <summary>
    ///// Returns all suitable objects for IK snap
    ///// </summary>
    //private Collider[] Query()
    //{
    //    return Physics.OverlapSphere(referencePoint.position, detectionRadius, detectionMask);
    //}

    ////2. Encontrar al objeto cuyo punto más cercano de referencia es el más cercano

    //private Vector3 GetNearestPositionForSnap(Collider[] nearColliders)
    //{
    //    try
    //    {
    //        //encontrar punto más cercano a la superficie
    //        //Creo una lista de los puntos cercanos a cada uno de los colliders
    //        var closestPoints = nearColliders.Select(collider => collider.ClosestPoint(referencePoint.position));

    //        //Encuentro el punto mas cercano al collider más cercano
    //        Vector3 closestPoint = closestPoints.OrderBy(position => Vector3.Distance(referencePoint.position, position)).First();

    //        //Evaluo si el punto de referencia esta dentro del collider
    //        if(closestPoint == referencePoint.position)
    //        {
    //            //Estoy dentro
    //            //debo castear el rayo desde la mano hacia el punto de referencia
    //            Ray ray = new Ray(transform.position, referencePoint.position - transform.position);

    //            if (Physics.Raycast(ray, out RaycastHit hit, ray.direction.magnitude, detectionMask))
    //            {
    //                //devolvemos punto de intersección
    //                return hit.point;
    //            }

    //        }
    //        else
    //        {
    //            //Estoy fuera
    //        }
    //        return closestPoint;
    //    }
    //    catch
    //    {
    //       //ignore
    //    }
    //    //si no se detecta nada retornamos la posición misma de la mano
    //    return transform.position;
    //}

    //private void LateUpdate()
    //{
    //    Vector3 positionToSnap = GetNearestPositionForSnap(Query());
    //    Debug.DrawLine(transform.position, positionToSnap);
    //    transform.position = positionToSnap;
    //}
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(referencePoint.position, detectionRadius);
    //}

    private const float REFRESH_DELAY = 0.5f;

    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private Transform referencePoint;
    [SerializeField] private Transform raycastReference;

    //Encontrar objetos idoneos

    /// <summary>
    /// Returns all suitable objects for IK Snap
    /// </summary>
    private Collider[] Query()
    {
        return Physics.OverlapSphere(referencePoint.position, detectionRadius, detectionMask);
    }

    //Encontrar punto de referencia más cercano
    private bool GetNearestPositionForSnap(Collider[] nearColliders, out Vector3 nearestPoint)
    {
        try
        {
            //Encontrar punto mas cercano de la superficie

            //Se crea una lista de los puntos más cercanos a los colliders
            var closestPoints = nearColliders.Select(collider => collider.ClosestPoint(referencePoint.position));

            //Se encuentra el punto más cercano, del collider más cercano
            Vector3 closestPoint = closestPoints.OrderBy(position => Vector3.Distance(referencePoint.position, position))
                .First();

            //Evaluar si el punto de referencia está dentro del collider
            if (closestPoint == referencePoint.position)
            {
                //Estoy dentro
                //Toca castear el rayo de la mano al punto de referencia
                Ray ray = new Ray(raycastReference.position, referencePoint.position - raycastReference.position);
                if (Physics.Raycast(ray, out RaycastHit hit, ray.direction.magnitude, detectionMask))
                {
                    //Devolver punto de interseccion
                    nearestPoint = hit.point;
                    return true;
                }
            }
            else
            {
                //Estoy fuera
                nearestPoint = closestPoint;
                return true;
            }

        }
        catch (Exception e)
        {
            //ignore
        }
        //Si no se detecta nada, se retorna la misma mano
        nearestPoint = transform.position;
        return false;
    }

    private void LateUpdate()
    {
        if (GetNearestPositionForSnap(Query(), out Vector3 nearestPosition))
        {
            //Superficie con posición valida cerca
            transform.position = nearestPosition;
            //Mandar la señal
            gameObject.SendMessage("OverrideIK", true, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            gameObject.SendMessage("OverrideIK", false, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(referencePoint.position, detectionRadius);
    }

}
