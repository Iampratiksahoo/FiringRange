using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FiringRange
{
    public class MainHUD : MonoBehaviour
    {
        [SerializeField] Image Crosshair;
        [SerializeField] GameObject Reload;
        [SerializeField] TextMeshProUGUI BulletCount;

        private void OnEnable()
        {
            GameEventHandler.OnHoverOverWeapon += OnHoverOverWeapon;
            GameEventHandler.OnReloadStarted += OnReloadStarted;
            GameEventHandler.OnReloadCompleted += OnReloadCompleted;


            GameEventHandler.OnBulletCountChanged += OnBulletCountChanged;
            GameEventHandler.OnWeaponEquipped += OnWeaponEquipped;
        }

        private void OnDisable()
        {
            GameEventHandler.OnHoverOverWeapon -= OnHoverOverWeapon;
            GameEventHandler.OnReloadStarted -= OnReloadStarted;
            GameEventHandler.OnReloadCompleted -= OnReloadCompleted;
            GameEventHandler.OnWeaponEquipped -= OnWeaponEquipped;
        }

        private void OnWeaponEquipped(Weapon weapon)
        {
            OnBulletCountChanged(weapon.BulletsLeft);
        }

        private void OnBulletCountChanged(int bulletCount)
        {
            BulletCount.text = $"Bullets: {bulletCount}";
        }

        private void OnReloadStarted()
        {
            Reload.SetActive(true); 
        }

        private void OnReloadCompleted()
        {
            Reload.SetActive(false);
        }


        private void OnHoverOverWeapon(bool isHovering)
        {
            Crosshair.color = isHovering ? Color.green : Color.black;
        }
    }
}