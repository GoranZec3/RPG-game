using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {

        
        enum DestinationIdentifier
        {
            A,B,C,D,E
        }
        //scene index File->Build Profiles
        [SerializeField] int sceneToLoad = -1;

        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;

        [SerializeField] float fadeOutTime = 0.5f;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeWaitTime = 0.5f;
        

        private void OnTriggerEnter(Collider other) 
        {  
    
            if(other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
            
        }

        private IEnumerator Transition()
        {
            if(sceneToLoad < 0) 
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }
           
            
            DontDestroyOnLoad(gameObject);

            Fader fader = FindFirstObjectByType<Fader>();
            SavingWrapper savingWrapper = FindAnyObjectByType<SavingWrapper>();
            //disable moving while is scene in fade in
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            yield return fader.FadeOut(fadeOutTime);

            savingWrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            //disable moving againg while scene is fade but player is loaded
            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;


            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            //saving after load player in new scene
            savingWrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);

            //enable player movement
            newPlayerController.enabled = true;
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");

            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;

        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsByType<Portal>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if(portal == this) {continue;}
                if(portal.destination != destination) {continue;}
                return portal;
            }
            return null;
        }
    }
}
