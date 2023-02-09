using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.0f;

    [SerializeField]
    private GameObject _sword;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float _horizontalInput = Input.GetAxis("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");
        Vector3 _velocity = new Vector3(_horizontalInput, _verticalInput, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _sword.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _sword.SetActive(false);
        }

        if(_horizontalInput > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else if (_horizontalInput < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, -90);
        }
        if (_verticalInput > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        else if (_verticalInput < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        
        transform.Translate(_velocity * _speed * Time.deltaTime, Space.World);
    }

    public Vector3Int GetPlayerPosition()
    {
        return new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
       
    }
}
