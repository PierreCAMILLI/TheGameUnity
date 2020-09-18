using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visual
{
    public class Deck : MonoBehaviour
    {
        [SerializeField]
        private Transform _cardParent;

        [SerializeField]
        private Transform _topCardTransform;

        [SerializeField]
        private Pool<Card> _pool;

        public Card CreateCard(int number, bool faceUp)
        {
            Card card = _pool.Instantiate(transform.position, transform.rotation, _cardParent);
            card.Value = number;
            card.SetDirty();
            card.SetCardState(faceUp, false);
            return card;
        }

        public void SetTopCardActive(bool toggle)
        {
            _topCardTransform.gameObject.SetActive(toggle);
        }
    }
}
