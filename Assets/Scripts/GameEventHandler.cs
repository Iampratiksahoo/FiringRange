using System;

namespace FiringRange
{
    public static class GameEventHandler
    {
        // Input Events 
        public static Action<float, float> OnMove;
        public static Action<float, float> OnMouseLook;
        public static Action<bool> OnSprint;
        public static Action<bool> OnBlackOrbInteract;
        public static Action OnShoot;

        public static Action<bool> OnHoverOverWeapon;
        public static Action OnInteractPressed;

        public static Action OnWeaponPicked;
    }
}
