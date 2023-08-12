using UnityEngine;

namespace FiringRange
{
    [CreateAssetMenu(fileName = "Weapon Data", menuName = "Data/Weapon Data")]
    public class WeaponData : ScriptableObject    
    {
        public int Damage;
        public float TimeBetweenShooting;
        public float Spread;
        public float Range;
        public float ReloadTime;
        public float TimeBetweenShots;
        public int MagazineSize;
        public int BulletPerTap;
        public bool Automatic;
        public GameObject BulletImpact;
        public LayerMask EnemyLayerMask;
    }
}