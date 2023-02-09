using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private GameManager _gm;

    private void Start()
    {
        _gm = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gm == null)
        {
            Debug.Log("Game manager is nULL.");
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            Debug.Log("hit sword");
            _gm.ChangeLevel();
            Destroy(this.gameObject);           
        } 
    }
}
