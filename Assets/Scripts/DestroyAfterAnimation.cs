using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    // This function will be called at the end of the animation
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
