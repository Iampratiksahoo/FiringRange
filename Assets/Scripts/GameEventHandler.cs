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

        public static Action OnShootAutomatic;
        public static Action OnShootNonAutomatic;
        public static Action OnReloadPressed;
        public static Action OnReloadStarted;
        public static Action OnReloadCompleted;
        public static Action<int> OnBulletCountChanged;

        public static Action<bool> OnHoverOverWeapon;

        public static Action OnInteractPressed;

        public static Action<Weapon> OnWeaponEquipped;

        public static Action<bool> OnMusicToggled;
        public static Action<bool> OnSFXToggled;
        public static Action<float> OnVolumeChanged;
    }
}
