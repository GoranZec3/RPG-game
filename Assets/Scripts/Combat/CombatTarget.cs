using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {

                if(!callingController.GetComponent<Fighter>().CanAttack(gameObject))
                {
                    return false;
                }

                if(Input.GetMouseButtonDown(0))
                {
                    callingController.GetComponent<Fighter>().Attack(gameObject);  
                }
                return true;
        }


    }
}