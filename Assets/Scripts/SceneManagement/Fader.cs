using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour 
    {
        CanvasGroup canvasGroup;
        Coroutine currentActiveFade = null;

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
            // StartCoroutine(FadeOutIn());
        }

        
        // IEnumerator FadeOutIn()
        // {
        //     yield return FadeOut(3f);
        //     print("Faded out");
        //     yield return FadeIn(1f);
        //     print("Faded In");
        // }
        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeOut(float time)
        {   
            return Fade(1, time);

        }

        public Coroutine Fade(float target, float time)
        {
            if(currentActiveFade !=null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeRoutine(target, time));
            return currentActiveFade;
        }


        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }


        private IEnumerator FadeRoutine(float target, float time)
        {
            while(!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}
