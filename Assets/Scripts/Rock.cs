using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Animator _anim;
    private Rigidbody2D _rb;
    [SerializeField]
    private GameObject _ladder; //drops only if it's the last rock spawned 
    private int id;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        if(_anim == null)
        {
            Debug.Log("Animator is NULL.");
        }
        _rb = GetComponent<Rigidbody2D>();
        if(_rb == null)
        {
            Debug.Log("Box Collider is NULL.");
        }

        id = SpawnManager._rockID; //sets the ID for this rock.
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Sword")
        {
            Debug.Log("We hit the Sword");
            _anim.SetBool("isBroken", true);
            _rb.simulated = false;
            if (id == SpawnManager._rockID) //if last rock spawned. bc at this point _rockID probably equals the max number of spawned rocks.
            {
                Debug.Log("Ladder Dropped");
                _ladder.SetActive(true);
                _ladder.transform.parent = null;
            }
            Destroy(this.gameObject, 2.0f);
        }     
    }
}
