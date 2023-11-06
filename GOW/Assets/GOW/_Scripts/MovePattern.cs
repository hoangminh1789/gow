using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOW
{
    public class MovePattern : MonoBehaviour
    {
        [SerializeField] float  _range      = 4;
        [SerializeField] int    _count      = 6;
        [SerializeField] float  _idleTime   = 1.0f;
        [SerializeField] bool   _showDebug  = false;
        
        List<Vector3>   _positions      = new List<Vector3>();
        int             _index          = 0;
        float           _lastTime       = 0;
        
        void Start()
        {
            Vector3 pos = transform.position;
            for (int i = 0; i < _count; ++i)
            {
                Vector2 rand = Random.insideUnitCircle * _range;
                _positions.Add(new Vector3(pos.x + rand.x, pos.y, pos.z + rand.y));
            }
        }

        public bool IsReady => (_positions.Count > 0 && Time.realtimeSinceStartup - _lastTime > _idleTime);
        public Vector3 TargetPosition => _positions[_index];

        public void MoveNext()
        {
            _index      = (_index + 1) % _positions.Count;
            _lastTime   = Time.realtimeSinceStartup;
        }

        void OnDrawGizmos()
        {
            if (_showDebug)
            {
                EditorUtility.GizmosDrawCircle(transform.position, _range, Color.gray);
                
                Color color = Gizmos.color;
                Gizmos.color = Color.gray;
                
                for (int i = 0; i < _positions.Count - 1; ++i)
                {
                    Gizmos.DrawLine( _positions[i], _positions[i + 1] );
                }

                Gizmos.color = color;
            }
        }
    }
}
