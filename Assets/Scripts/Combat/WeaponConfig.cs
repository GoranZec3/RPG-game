using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject 
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] float weaponDamage = 5f; 
        [SerializeField] float percentageBonus = 0f; 
        [SerializeField] float weaponRange = 2.5f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        
        const string weaponName = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if(equippedPrefab != null)
            {
                Transform handTransfrom = GetTransform(rightHand, leftHand);
                Weapon weapon = Instantiate(equippedPrefab, handTransfrom);
                weapon.gameObject.name = weaponName;
            }
            //find parent if there is no animation override
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }else if(overrideController != null){

                //if there is no animationOverride find its parent and put it back in animator
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            
            }
            
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            //check if weapon is right or left hand 
            if(oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if(oldWeapon == null) return;
            //change name to avoid bug
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransfrom;
            if (isRightHanded) { handTransfrom = rightHand; }
            else { handTransfrom = leftHand; }

            return handTransfrom;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public float GetDamage()
        {
            return weaponDamage;
        }

        public float GetPercentageBonus()
        {
            return percentageBonus;
        }
        
        public float GetRange()
        {
            return weaponRange;
        }
        
    }
}