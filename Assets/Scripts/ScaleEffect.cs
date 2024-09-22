using UnityEngine;

public class ScaleEffect : MonoBehaviour
{
    public float scaleSpeed = 2f;
    public float scaleMin = 0.9f;
    public float scaleMax = 1.1f;

    void Update()
    {
        float scale = Mathf.PingPong(Time.time * scaleSpeed, scaleMax - scaleMin) + scaleMin;
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
