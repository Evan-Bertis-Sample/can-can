using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FormulaBoy.Utility
{
    public static class MathUtility 
    {
        public static Vector2[] BoundsCorners(Bounds bounds)
        {
            Vector2[] points = new Vector2[4];
            points[0] = new Vector2(bounds.min.x, bounds.min.y);
            points[1] = new Vector2(bounds.min.x, bounds.max.y);
            points[2] = new Vector2(bounds.max.x, bounds.min.y);
            points[3] = new Vector2(bounds.max.x, bounds.max.y);
            return points;
        }
        public static void FlipXGlobal(Transform transform)
        {
            // Flip around the world Y-axis by adjusting rotation
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);

            // Then flip the local scale on the x-axis to mirror it (assuming the object's rotation aligns its local x-axis with the world y-axis)
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        public static void Flip(Transform transform, Vector3 v)
        {
            transform.localScale = new Vector3(Mathf.Sign(v.x) * Mathf.Abs(transform.localScale.x), Math.Abs(transform.localScale.y) * Mathf.Sign(v.y), Math.Abs(transform.localScale.z) * Mathf.Sign(v.z));
        
        }
        public static void FlipAll(List<Transform> transforms, float xScale)
        {
            foreach (Transform t in transforms)
            {
                Flip(t, new Vector3(xScale, 1, 1));
            }
        }

    }
}
