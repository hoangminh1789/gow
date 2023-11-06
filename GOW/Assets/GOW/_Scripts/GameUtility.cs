using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{
    public class GameUtility
    {
        public static Wave GetNextWave(float time, List<Wave> waves)
        {
            for (int i = 0; i < waves.Count; ++i)
            {
                if (waves[i].time <= time)
                {
                    Wave wave = waves[i];

                    waves.RemoveAt(i);

                    return wave;
                }
            }

            return null;
        }
        
        public static Vector3 RandomAround(Vector3 position, float range)
        {
            Vector2 circle = Random.insideUnitCircle;
            return new Vector3(position.x + circle.x * range, position.y, position.z + circle.y * range);
        }
        
        public static void RotateToDirection(Transform tran, Vector3 direction)
        {
            float angleY = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            tran.rotation = Quaternion.Euler(0, angleY, 0);
        }
        
        public static bool IsInRange(ICharacter myChar, ICharacter target, float range)
        {
            float sqrRange  = range * range;
            float sqr       = Vector3.SqrMagnitude(myChar.Position - target.Position);

            return sqr <= sqrRange;
        }
        
        public static bool CanApproach(ICharacter lockedTarget, float approachRange)
        {
            if(lockedTarget != null && approachRange > 0)
            {
                return true;
            }

            return false;
        }

        public static bool ShouldApproach(ICharacter myChar, ICharacter lockedTarget, float attackRange)
        {
            if (lockedTarget != null)
            {
                float   range = Vector3.Distance(lockedTarget.Position, myChar.Position);
                return  range > attackRange * 0.9f;
            }

            return false;
        }
    }
}
