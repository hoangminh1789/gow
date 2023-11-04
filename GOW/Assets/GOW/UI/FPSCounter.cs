using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float _expSmoothingFactor = 0.9f;
    [SerializeField] private float _refreshFrequency = 0.4f;

    private float _timeSinceUpdate = 0f;
    private float _averageFps = 1f;
    private float _tickUpdate = 0;
    private TMP_Text _text;
    
    void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        _tickUpdate += Time.unscaledTime;
        _averageFps = _expSmoothingFactor * _averageFps + (1f - _expSmoothingFactor) * 1f / Time.unscaledDeltaTime;

        if (_timeSinceUpdate < _refreshFrequency)
        {
            _timeSinceUpdate += Time.deltaTime;
            return;
        }

        if (_tickUpdate > 0.3f)
        {
            _tickUpdate = 0;
            
            int fps = Mathf.RoundToInt(_averageFps);
            _text.text = fps.ToString();
        }

        _timeSinceUpdate = 0f;
    }
}
