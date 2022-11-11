using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Screw : MonoBehaviour
{    
    // move transform value between the start position and start position + offset over the duration    
    public Vector2 Offset;
    // how long to oscillate between the two positions. Will be smoothed out via dotween easing
    public float Duration;

    private Level level;

    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(new Vector3(transform.position.x + Offset.x, transform.position.y + Offset.y, 0), Duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad)
            .SetId(this.GetInstanceID());
        // should only ever be 1
        level = FindObjectOfType<Level>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            level.CollectScrew();
            Destroy(this.gameObject);
        }        
    }

    void OnDestroy() {
        DOTween.Kill(this.GetInstanceID());
    }
}
