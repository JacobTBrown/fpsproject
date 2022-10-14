using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameInstances
{
    public class SliderHInstance : MonoBehaviour
    {
        public static SliderHInstance Instance;
        public static GameObject SliderH{get;set;}
        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            SliderH = gameObject;
        }

    }
}