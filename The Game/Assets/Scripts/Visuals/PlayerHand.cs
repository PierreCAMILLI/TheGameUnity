using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

namespace Visual
{
    public class PlayerHand : MonoBehaviour
    {
        [SerializeField]
        private RectTransform[] _cardShapes;

        [SerializeField]
        private bool _showCards;

        SortedDictionary<int, Card> _cards;

        public int PlayerNumber
        {
            get; set;
        }

        private void Awake()
        {
            _cards = new SortedDictionary<int, Card>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateCardPositions();
        }

        private void UpdateCardPositions()
        {
            byte index = 0;
            foreach (KeyValuePair<int, Card> keyValue in _cards)
            {
                _cardShapes[index].gameObject.SetActive(true);
                keyValue.Value.TargetPosition = _cardShapes[index].position;
                keyValue.Value.TargetRotation = _cardShapes[index].rotation.eulerAngles.z;
                keyValue.Value.SetCardState(_showCards, true);
                ++index;
            }
            for (; index < _cardShapes.Length; ++index)
            {
                _cardShapes[index].gameObject.SetActive(false);
            }
        }

        public void Add(Card card)
        {
            _cards.Add(card.Value, card);
        }

        public Card Remove(int cardValue)
        {
            Card card;
            if (_cards.TryGetValue(cardValue, out card))
            {
                _cards.Remove(cardValue);
                return card;
            }
            return null;
        }

        public void SortCardsInHierarchy()
        {
            foreach (KeyValuePair<int, Card> card in this._cards)
            {
                card.Value.transform.SetAsLastSibling();
            }
        }
    }
}
