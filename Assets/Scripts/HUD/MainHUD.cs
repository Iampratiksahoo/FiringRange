using UnityEngine;
using UnityEngine.UI;

namespace FiringRange
{
    public class MainHUD : MonoBehaviour
    {
        [SerializeField] Image Crosshair;

        private void OnEnable()
        {
            GameEventHandler.OnHoverOverWeapon += OnHoverOverWeapon;
        }

        private void OnDisable()
        {
            GameEventHandler.OnHoverOverWeapon -= OnHoverOverWeapon;
        }

        private void OnHoverOverWeapon(bool isHovering)
        {
            Crosshair.color = isHovering ? Color.blue : Color.white;
        }
    }
}