using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    internal static class Operations3D
    {
        public static GameObject GetClosestObjectInNearby(Transform currentObject, List<GameObject> nearbyObjects)
        {
            if (nearbyObjects == null || nearbyObjects.Count == 0)
            {
                return new GameObject();
            }

            if (nearbyObjects.Count == 1)
            {
                return nearbyObjects.First();
            }
            
            GameObject closestGrabableObject = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = currentObject.position;
            foreach (GameObject currentGrabableObject in nearbyObjects)
            {
                float dist = Vector3.Distance(currentGrabableObject.transform.position, currentPos);
                if (dist < minDist)
                {
                    closestGrabableObject = currentGrabableObject;
                    minDist = dist;
                }
            }

            return closestGrabableObject;
        }
    }
}
