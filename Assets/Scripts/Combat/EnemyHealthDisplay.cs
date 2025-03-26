using System;
using RPG.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;

        void Awake()
        {      
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }
        void Start()
        {
            
        }

        void Update()
        {
            if(fighter.GetTarget() == null)
            {
                GetComponent<TMP_Text>().text = "N/A";
                return;
            }
            

            Health health = fighter.GetTarget();
            GetComponent<TMP_Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }

}
