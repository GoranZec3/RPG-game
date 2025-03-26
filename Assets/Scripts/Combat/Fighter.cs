using System;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierPrivider
    {
        
        [SerializeField] float timeBetweenAttack = 1f; 
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        

        [Range(0,1)]
        [SerializeField] float speedRun = 1f;

        Health target; 
        WeaponConfig currentWeaponConfig = null;
        //Infinity sets true for initial attack
        float timeSinceLastAttack = Mathf.Infinity;

        void Start()
        {
            
            if(currentWeaponConfig == null)
            {
                EquipWeapon(defaultWeapon);
            }
            

        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) return;
            //stop attacking target if its dead
            if(target.IsDead()) return;

     
            if (!GetIsInRange(target.transform))
            {
                //moving to target and keep distance when is in range
                GetComponent<Mover>().MoveTo(target.transform.position, speedRun);
            }
            else
            {
                //cancel moving
                GetComponent<Mover>().Cancel();
                AttackBehaviour();

            }
        }

        
        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon; 
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform,  animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        private void AttackBehaviour()
        {

            transform.LookAt(target.transform);

            if(timeSinceLastAttack > timeBetweenAttack)
            {
                //this will trigger the Hit() event
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {  
            GetComponent<Animator>().ResetTrigger("StopAttack");
            GetComponent<Animator>().SetTrigger("Attack");         
        }


        //Animation Event in Unity
        void Hit()
        {
            if(target == null){return; }

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
           

            if(currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }


        //Anim event for arrow animation
        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetRange();
        }

        //Avoid blocking attack with collision of dead enemy
        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget == null){return false;}
            //if target is too far 
            if(!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) &&
                !GetIsInRange(combatTarget.transform)) {return false;}

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {    
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();

        }

        public void Cancel()
        {
            //cancel attack with moving
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
            
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("StopAttack");
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetDamage();
            }
        }


        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
        }

        //ISaveable
        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string) state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }


    }
}