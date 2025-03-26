using UnityEngine;

namespace RPG.Core 
{
    public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    public float moveSpeed = 0.01f;
    public float smoothSpeed = 5f;
    
    void LateUpdate()
    {
        // transform.position = target.position;
        //lerp following transition camera player
        transform.position = Vector3.Lerp(transform.position, target.position, smoothSpeed * Time.deltaTime);
    }
}

}
