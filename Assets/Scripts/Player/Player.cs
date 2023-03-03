using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    [SerializeField] private int _speed;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Transform _waypoint;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float AngleInDegrees;

    private Vector3 _fromTo;
    private Vector3 _fromToXZ;
    private GameObject _target;
    
    private float _lastAttackTime;
    private float g = Physics.gravity.y;
    private Animator _animator;
    
    public float CurrentASPD { get; private set; }
    public int CurrentHealth { get; private set; }
    public int CurrentDamage { get; private set; }
    public int Money { get; private set; }
    public event UnityAction<int> PowerChanged;
    public event UnityAction<float> ASPDChanged;
    public event UnityAction<int, int> HealthChanged;
    public event UnityAction<int> MoneyChanged;
    private void Start()
    {
        Money = 100;
        CurrentHealth = _health;
        CurrentDamage = _damage;
        CurrentASPD = _attackSpeed;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _target = GameObject.FindWithTag("Enemy");
        Move();
        _shootPoint.localEulerAngles = new Vector3(-AngleInDegrees, 0f, 0f);
        if (Vector3.Distance(transform.position, _target.transform.position) <= 3)
        {
            if (_lastAttackTime <= 0)
            {
                Shoot();
                _lastAttackTime = CurrentASPD;
            }
            _lastAttackTime -= Time.deltaTime;
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _waypoint.transform.position, _speed * Time.deltaTime);
        _animator.SetFloat("Speed", 1);
        if (transform.position == _waypoint.transform.position)
        {
            _animator.SetFloat("Speed", 0);
        }
    }

    private void Shoot()
    {
        _fromTo =_target.transform.position  - transform.position;
        _fromToXZ = new Vector3(_fromTo.x, 0f, _fromTo.z);
        float x = _fromToXZ.magnitude;
        float y = _fromTo.y;
        float AngleInRadians = AngleInDegrees * Mathf.PI / 180;

        float v2 = (g * x * x) / (2 * (y - Mathf.Tan(AngleInRadians) * x) * Mathf.Pow(Mathf.Cos(AngleInRadians), 2));
        float v = Mathf.Sqrt(Mathf.Abs(v2));

        GameObject newBullet = Instantiate(_bullet, _shootPoint.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().velocity = _shootPoint.forward * v;
        newBullet.GetComponent<Bullet>()._damage = CurrentDamage;
    }
    
    public void ApplyDamage(int damage)
    {
       _animator.SetTrigger("Damage1");
       CurrentHealth -= damage;
        HealthChanged?.Invoke(CurrentHealth, _health);
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddReward(int reward)
    {
        Money += reward;
        MoneyChanged?.Invoke(Money);
    }
     public void ChangePrice(int price)
    {
        Money -= price;
        MoneyChanged?.Invoke(Money);
    }
    
     public void ChangePower(int damage)
     {
         _damage += damage;
         CurrentDamage = _damage;
         PowerChanged?.Invoke(_damage);
     }
     
    public void ChangeHealth(int health)
    {
        CurrentHealth += health; 
        HealthChanged?.Invoke(CurrentHealth, _health);
    }
    public void ChangeASPD(float speedAttack)
    {
        CurrentASPD -= speedAttack; 
        ASPDChanged?.Invoke(CurrentASPD);
    }
    
}