using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveFlag : MonoBehaviour
{
    //we will instantiate the prefab from elsewhere? or in here?
    //should be added onto the player's model when we walk over the collider.
    //should be destroyed on player death
    //should be destroyed when walking over the player's team's capture zone & increment score


    private void Awake()
    {
        //add listener
        //initialize spawn point?
    }
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/* FROM FPSGAME OBJECTIVEREACHPOINT.CS
 * [RequireComponent(typeof(Collider))]
    public class ObjectiveReachPoint : Objective
    {
        [Tooltip("Visible transform that will be destroyed once the objective is completed")]
        public Transform DestroyRoot;

        void Awake()
        {
            if (DestroyRoot == null)
                DestroyRoot = transform;
        }

        void OnTriggerEnter(Collider other)
        {
            if (IsCompleted)
                return;

            var player = other.GetComponent<PlayerCharacterController>();
            // test if the other collider contains a PlayerCharacterController, then complete
            if (player != null)
            {
                CompleteObjective(string.Empty, string.Empty, "Objective complete : " + Title);

                // destroy the transform, will remove the compass marker if it has one
                Destroy(DestroyRoot.gameObject);
            }
        }
    }
 */