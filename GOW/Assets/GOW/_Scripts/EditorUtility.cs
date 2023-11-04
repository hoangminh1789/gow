using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{
    public class EditorUtility
    {
        static Dictionary<int, List<Vector3>> _sPointBySegment = new Dictionary<int, List<Vector3>>();
        
        public static void GizmosDrawCircle(Vector3 position, float radius, Color color, int segments = 10)
        {
            List<Vector3> points = null;
            if (_sPointBySegment.TryGetValue(segments, out points) == false)
            {
                points = new List<Vector3>();
                _sPointBySegment[segments] = points;
                
                float theta_scale = 2 * Mathf.PI / segments;
                
                for(float theta = 0; theta < 2 * Mathf.PI; theta += theta_scale) {
                    float x = Mathf.Cos(theta);
                    float y = Mathf.Sin(theta);
                    Vector3 pos = new Vector3(x, 0, y);
                    points.Add(pos);
                }
                
                points.Add(points[0]);
            }

            Color backupColor = Gizmos.color;
            Gizmos.color = color;
            for (int i = 0; i < points.Count - 1; ++i)
            {
                Gizmos.DrawLine(position + points[i] * radius, position + points[i + 1] * radius);
            }

            Gizmos.color = backupColor;
        }
    }
}
