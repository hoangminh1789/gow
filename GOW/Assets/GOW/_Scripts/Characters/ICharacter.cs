using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{
    public interface ICharacter
    {
        Transform   Transform       { get; }
        Vector3     Position        { get; }
        Vector3     WeaponAnchor    { get; }
        Vector3     CenterAnchor    { get; }
        Team        Team            { get; set; }
        bool        IsAlive         { get; set; }
    }
}
