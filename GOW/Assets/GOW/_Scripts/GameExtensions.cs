using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{
    public static class GameExtensions
    {
        public static void SetActive(this Transform transform, bool active)
        {
            transform.gameObject.SetActive(active);
        }

        public static void SetActive(this MonoBehaviour behaviour, bool active)
        {
            behaviour.gameObject.SetActive(active);
        }

        public static bool IsValid(this string st)
        {
            return string.IsNullOrWhiteSpace(st) == false;
        }
    }
}