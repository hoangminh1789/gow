using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

namespace GOW.ECS
{
    public class CullingSystem : ComponentSystem, IInitialize, IUpdate
    {
        Configuration   _config     = null;
        BattleECS       _battle     = null;
        bool            _culling    = false;
        
        public void Initialize()
        {
            _culling    = _config.culling;
        }
        
        public void Update()
        {
            Camera camera = _battle.MainCamera;
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            
            if (_culling)
            {
                var chars = _world.All<CharacterECS>();
                
                foreach (var entity in chars)
                {
                    _world.Get(entity, out Renderer renderer, out Graphic graphic);
                    
                    bool isInCamera = GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
                    
                    graphic.SetActive(isInCamera);
                }
            }
        }
    }
}