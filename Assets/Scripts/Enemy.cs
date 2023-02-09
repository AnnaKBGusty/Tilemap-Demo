using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform _pointA, _pointB;
    [SerializeField]
    private float _speed = 1.0f;

    private Animator _anim;

    private Vector3 _target;

    void Start()
    {
        _target = _pointA.position;
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.Log("Animator is NULL.");
        }
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);

        if (transform.position == _pointA.position)
        {
            _target = _pointB.position;
        }
        else if (transform.position == _pointB.position)
        {
            _target = _pointA.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sword")
        {
            Debug.Log("We hit the Sword");
            _anim.SetBool("isBroken", true);
            Destroy(this.gameObject, 2.0f);
        }
    }
}
