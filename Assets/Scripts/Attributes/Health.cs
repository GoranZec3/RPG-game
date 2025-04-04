using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.UI.DamageText;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {

        [SerializeField] float regenerationPercentage = 70f;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        //Unity event doesnt take float directly, have to create class to avoid
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        //set healthPoints to -1 to avoid bug between RestoreState and Start. 
        float healthPoints = -1f;  
        bool isDead = false;


        void Start()
        {     
            if(healthPoints < 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);  
            }
        }

        void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealt;
        }

        void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealt;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {


            healthPoints = Mathf.Max(healthPoints - damage, 0);
            
            if(healthPoints==0)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }else
            {
                takeDamage.Invoke(damage);
            }
        }

        public void Heal(float healthToRestore)
        {
            healthPoints = Mathf.Min(healthPoints + healthToRestore, GetMaxHealthPoints());
        }

        public float GetHealthPoints()
        {
            return healthPoints;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }


        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Die()
        {
            if(isDead)return;
            isDead = true;
            //trigger die animation
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            
                  
        }

        private void AwardExperience(GameObject instigator)
        {

            Experience experience = instigator.GetComponent<Experience>();

            if(experience == null){return;}

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));

        }

        private void RegenerateHealt()
        {
            float regenerationHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints = Mathf.Max(healthPoints, regenerationHealthPoints);
        }

        //saving components
        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if(healthPoints<=0)
            {
                Die();
            }
        }

    }
}