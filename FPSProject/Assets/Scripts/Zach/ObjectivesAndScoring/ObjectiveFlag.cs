using System.Collections;
using System.Collections.Generic;
using Unity.Scripts.Jonathan;
using UnityEngine;
namespace Unity.Scripts.zach
{
    public class ObjectiveFlag : Objective
    {
        //we will instantiate the prefab from elsewhere? or in here?
        //should be added onto the player's model when we walk over the collider.
        //should be destroyed on player death
        //should be destroyed when walking over the player's team's capture zone & increment score

        [Header("Game object to pickup")]
        public GameObject ItemToPickup;


        private void Awake()
        {
            //add listener
            //initialize spawn point?
        }
        protected override void Start()
        {
            base.Start();
            EventManager.AddListener<PickupEvent>(OnPickupEvent);
        }

        void OnPickupEvent(PickupEvent evt)
        {
            if (IsCompleted || ItemToPickup != evt.Pickup)
                return;

            // this will trigger the objective completion
            // it works even if the player can't pickup the item (i.e. objective pickup healthpack while at full heath)
            CompleteObjective(string.Empty, string.Empty, "Objective complete : " + Title);

            if (gameObject)
            {
                Destroy(gameObject);
            }
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<PickupEvent>(OnPickupEvent);
        }
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
 }*/