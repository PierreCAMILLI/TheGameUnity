using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Visual;

public class LocalPlayerController : SingletonBehaviour<LocalPlayerController>
{
    [SerializeField]
    private int _playerNumber;
    public int PlayerNumber
    {
        get { return _playerNumber; }
        set { _playerNumber = value; }
    }

    public GameObject SelectedGameObject
    {
        get; set;
    }

    public bool IsHisTurn
    {
        get { return GameManager.Instance.PlayerTurn == _playerNumber; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
