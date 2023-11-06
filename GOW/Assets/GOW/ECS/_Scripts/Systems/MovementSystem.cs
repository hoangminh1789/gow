using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

namespace GOW.ECS
{
    public class MovementSystem : ComponentSystem, IInitialize
    {
        public void Initialize()
        {
            _world.All<Move>().OnAdded.Bind(OnMove);
        }
        
        void OnMove(int entity)
        {
            _world.Get(entity, out Move move);
            _world.Get(move.entity, out Graphic graphic, out Speed speed, out Transform tran);

            if (graphic.IsAttacking == false)
            {
                Vector3 direction = move.direction;

                tran.position += (move.speedModifier * speed.value * Time.deltaTime) * direction;

                GameUtility.RotateToDirection(tran, direction);
                graphic.Run();
            }
        }
    }
}
