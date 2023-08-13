using UnityEngine;

namespace FiringRange
{
    [CreateAssetMenu(fileName = "Weapon Data", menuName = "Data/Weapon Data")]
    public class WeaponData : ScriptableObject    
    {
        public float FireRate;
        public float Range;
        public float ReloadTime;
        public int MagazineSize;
        public int BulletPerTap; // Utilized for Burst Fire Weapon Types
        public bool Automatic;
        public bool Laser;
        public ParticleSystem MuzzleFlash;
        public GameObject BulletImpact;
        public LayerMask EnemyLayerMask;

        public Sound BulletFireSound;
        public Sound EmptyFireSound;

        [Header("Sway Parameters")]
        public bool Sway = true;
        public float SwayStep = 0.01f;
        public float MaxDistanceStep = 0.06f;
        public float SwayRotationStep = 4f;
        public float MaxRotationStep = 5f;
        public float SwaySmooth = 10f;
        public float SwaySmoothRot = 12f;
    }
}