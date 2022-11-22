using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoving : MonoBehaviour
{
    // move transform value between the start position and start position + offset over the duration    
    public Vector2 Offset;
    // how long to oscillate between the two positions. Will be smoothed out via dotween easing
    public float Duration;
    // how long to wait before starting to oscilate
    public float Delay = 0.0f;
    // if set provide a random value between 0 and the delay property
    public bool RandomizeDelay = false;
    
    void Start()
    {
        float delayValue = !RandomizeDelay ? Delay : Random.Range(0, Delay);
        transform.DOMove(new Vector3(transform.position.x + Offset.x, transform.position.y + Offset.y, 0), Duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad)
            .SetDelay(delayValue)
            .SetId(this.GetInstanceID());
    }
    void OnDestroy() {
        DOTween.Kill(this.GetInstanceID());
    }
}
