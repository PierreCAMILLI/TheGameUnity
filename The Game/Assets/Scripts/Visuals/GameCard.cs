using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Visual
{
    [RequireComponent(typeof(Card))]
    public class GameCard : MonoBehaviour
    {
        public enum CardState
        {
            InDeck,
            InHand,
            OnStack
        }

        public Card Card
        {
            get; private set;
        }

        [SerializeField]
        private CardState _state;
        public CardState State
        {
            set { _state = value; }
            get { return _state; }
        }

        [SerializeField]
        private int _stackNumber;
        public int StackNumber
        {
            set { _stackNumber = value; }
            get { return _stackNumber; }
        }

        public int Owner
        {
            get; set;
        }

        // Start is called before the first frame update
        void Awake()
        {
            this.Card = GetComponent<Card>();
        }

        private void Update()
        {
            this.Card.IsSelected = (LocalPlayerController.Instance.SelectedGameObject == gameObject) ;
        }

        private void OnClickInDrawingDeck()
        {
            GameManager.Instance.EndTurn();
        }

        private void OnClickInHand()
        {
            LocalPlayerController.Instance.SelectedGameObject = gameObject;
        }

        private void OnClickOnStack()
        {
            if (LocalPlayerController.Instance.SelectedGameObject)
            {
                GameCard selectedCard = LocalPlayerController.Instance.SelectedGameObject.GetComponent<GameCard>();
                if (selectedCard)
                {
                    GameManager.Instance.StackCards(selectedCard.Owner, StackNumber, selectedCard.Card.Value);
                }
            }
        }

        public void OnClick()
        {
            Debug.Log(this.Card.ToString());
            if (LocalPlayerController.Instance.IsHisTurn)
            {
                switch (_state)
                {
                    case CardState.InDeck:
                        OnClickInDrawingDeck();
                        break;
                    case CardState.InHand:
                        OnClickInHand();
                        break;
                    case CardState.OnStack:
                        OnClickOnStack();
                        break;
                }
            }
        }
    }
}
