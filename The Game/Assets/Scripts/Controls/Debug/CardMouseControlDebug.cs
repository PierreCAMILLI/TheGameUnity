using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Visual.Card))]
public class CardMouseControlDebug : MonoBehaviour
{
    private Visual.Card _card;

    [SerializeField]
    private KeyCode _changePositionButton;

    [SerializeField]
    private float _mouseScrollMultiplier = 10f;

    [SerializeField]
    private KeyCode _flipCardButton;

    private void Awake()
    {
        _card = GetComponent<Visual.Card>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(_changePositionButton))
        {
            _card.SetPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), true);
        }

        _card.TargetRotation = _card.TargetRotation + Input.mouseScrollDelta.y * _mouseScrollMultiplier;

        if (Input.GetKeyUp(_flipCardButton))
        {
            _card.Flip(true);
        }
    }
}
