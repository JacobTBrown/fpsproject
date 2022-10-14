using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameInstances
{
    public class HitMarkerInstance : MonoBehaviour
    {
        public GameObject HitMarker{get;set;}
        // Start is called before the first frame update
        void Start()
        {
            HitMarker = gameObject;
        }

    }
}