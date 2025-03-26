using System;
using TMPro;
using UnityEngine;


namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;

        void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }
        void Start()
        {
            
        }

        void Update()
        {
               GetComponent<TMP_Text>().text = String.Format("{0:0}", experience.GetPoints());
        }
    }

}
