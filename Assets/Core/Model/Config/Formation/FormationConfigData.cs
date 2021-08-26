using System.Collections.Generic;
using Core.Util.Attributes;
using UnityEngine;

namespace Core.Model.Config.Formation
{
    // Formation just can be created by Formation Editor
    public class FormationConfigData : ScriptableObject
    {
        [ReadOnly]
        public string FormationName;
        [ReadOnly]
        public int UnitSize;
        [ReadOnly]
        public List<Vector2> Positions;
    }
}