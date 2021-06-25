using System;
using System.Collections;
using UnityEngine;

public class StarsFading : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private float _fadingTime = .5f;

    private const float HALF_TRANSPARENT = .5f;
    private const float STANDART_TRANSPARENT = 1f;

    private void Start() => StartCoroutine(nameof(Fading));
    
    private IEnumerator Fading()
    {
        while (true)
        {
            _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, HALF_TRANSPARENT);
            yield return new WaitForSeconds(_fadingTime);
            _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, STANDART_TRANSPARENT);
        }
    }
}
