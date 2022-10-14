using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameInstances
{
    public class AmmoCounterInstance : MonoBehaviour
    {
        public GameObject AmmoCounter{get;set;}
        // Start is called before the first frame update
        void Start()
        {
            AmmoCounter = gameObject;
        }

    }
}