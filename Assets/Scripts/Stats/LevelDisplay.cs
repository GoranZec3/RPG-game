using System;
using TMPro;
using UnityEngine;


namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;

        void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }
        void Start()
        {
            
        }

        void Update()
        {
               GetComponent<TMP_Text>().text = String.Format("{0:0}", baseStats.GetLevel());
        }
    }

}
