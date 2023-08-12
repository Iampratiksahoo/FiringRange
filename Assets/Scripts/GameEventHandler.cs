using System;

namespace FiringRange
{
    public static class GameEventHandler
    {
        public static Action<float, float> OnMove;
        public static Action<float, float> OnMouseLook;
        public static Action<bool> OnSprint;
        public static Action<bool> OnBlackOrbInteract;

    }
}
