using UnityEngine;

namespace FiringRange
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "Data/Player Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("Functional Parameters")]
        public bool CanMove = true;
        public bool CanSprint = true;
        public bool CanHeadbob = true;

        [Header("Movement Parameters")]
        public float WalkSpeed = 3.0f;
        public float SprintSpeed = 6.0f;
        [Range(1f, 5f)] 
        public float MovementSpeedBlendTime = 1.0f;
        [Space(10)]
        public float Gravity = 30.0f;

        [Header("Look Parameters")]
        public bool MouseInverted = false;
        [Range(0f, 100f)] 
        public float LookSpeedX = 1f;
        [Range(0f, 100f)] 
        public float LookSpeedY = 1f;
        [Range(1, 180)] 
        public float UpperLookLimit = 80.0f;
        [Range(1, 180)] 
        public float LowerLookLimit = 80.0f;

        [Header("Headbob Parameters")]
        public float WalkBobSpeed = 14.0f;
        public float WalkBobAmount = 0.025f;
        public float SprintBobSpeed = 18.0f;
        public float SprintBobAmount = 0.05f;
        [Range(1f, 5f)] 
        public float HeadbobSpeedBlendTime = 1.0f;


        [Header("Interaction Parameters")]
        public float InteractionRange;
        public LayerMask InteractionLayer;

        [HideInInspector]public bool IsSprinting = false;
    }
}