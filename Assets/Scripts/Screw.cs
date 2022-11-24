using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Screw : MonoBehaviour
{    
    // move transform value between the start position and start position + offset over the duration    
    public Vector2 Offset;
    // how long to oscillate between the two positions. Will be smoothed out via dotween easing
    public float Duration;

    public AudioSource CollectSound;

    private Level level;
    private SpriteRenderer SpriteRenderer;
    private BoxCollider2D BoxCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(new Vector3(transform.position.x + Offset.x, transform.position.y + Offset.y, 0), Duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad)
            .SetId(this.GetInstanceID());
        // should only ever be 1
        level = FindObjectOfType<Level>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            level.CollectScrew();
            // need to wait until the audio effect is done to actually destroy the object
            StartCoroutine(Destroy());            
        }        
    }

    void OnDestroy() {
        DOTween.Kill(this.GetInstanceID());
    }

    IEnumerator Destroy() {
        CollectSound.Play();
        SpriteRenderer.enabled = false;
        BoxCollider2D.enabled = false;
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
}
