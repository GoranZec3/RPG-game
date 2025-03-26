using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;


namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour 
    {

        GameObject player;
        private void Awake() 
        {     
            player = GameObject.FindWithTag("Player");
        }

        void OnEnable()
        {
             //unity events 
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        void DisableControl(PlayableDirector pd)
        {
            //disable current action    
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            //disable inputs from controller
            player.GetComponent<PlayerController>().enabled = false;

        }

        void EnableControl(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled = true;

        }
    }

}
