using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : Bar
{
    [SerializeField] private EnemySpawner _spawner;
    private void OnEnable()
    {
      //  _spawner.EnemeCountChanged += OnValueChanged;
      // Slider.value = 1;
    }

    private void OnDisable()
    {
       // _spawner.EnemeCountChanged -= OnValueChanged;
    }
}
