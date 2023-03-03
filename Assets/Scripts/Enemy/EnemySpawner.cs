using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _wayPoint;
    [SerializeField] private Player _target;
    [SerializeField] private List<Wave> _waves;
    
    private Wave _currentWave;
    private int _currentWaveNumber = 0;
    private float _timeAfterLastSpawn;
    private int _spawned;
    private int _aliveEnemy;

    public event UnityAction AllEnemySpawned;
    public event UnityAction<int, int> EnemyCountChanged;
    private void Start()
    {
        SetWave(_currentWaveNumber);
    }

    private void Update()
    {
        if (_currentWave == null) 
            return;
        
        _timeAfterLastSpawn += Time.deltaTime;
        
        if (_timeAfterLastSpawn >= _currentWave.Delay)
        {
            InstantiateEnemy();
            _spawned++;
            _timeAfterLastSpawn = 0;
            EnemyCountChanged?.Invoke(_spawned,_currentWave.Count);
        }

        if (_currentWave.Count <= _spawned)
        {
            if (_waves.Count > _currentWaveNumber + 1)
                AllEnemySpawned?.Invoke();
            _currentWave = null;
        }
    }

    private void InstantiateEnemy()
    {
        Enemy enemy = Instantiate(_currentWave.Template, _spawnPoint.position, _spawnPoint.rotation,_spawnPoint).GetComponent<Enemy>();
        enemy.Init(_target);
        enemy.Dying += OnEnemyDying;
        _aliveEnemy++;
    }
    private void SetWave(int index)
    {
        _currentWave = _waves[index];
        EnemyCountChanged?.Invoke(0,1);
    }

    public void NextWave()
    {
        transform.position = new Vector3(transform.position.x + 12, transform.position.y, transform.position.z);
        _wayPoint.position = new Vector3(_wayPoint.position.x + 10, _wayPoint.position.y, _wayPoint.position.z);
        if (_currentWaveNumber != _waves.Count)
        {
            SetWave(++_currentWaveNumber);
        }
        _spawned = 0;
    }
    private void OnEnemyDying(Enemy enemy)
    {
        enemy.Dying -= OnEnemyDying;
        _target.AddReward(enemy.Reward);
        _aliveEnemy--;
        if (_aliveEnemy == 0)
        {
            NextWave();
        }
    }
}
[System.Serializable]
public class Wave
{
    public GameObject Template;
    public float Delay;
    public int Count;
}
