using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Visual
{
    public class Card : PoolElement<Card>
    {
        [System.Serializable]
        struct OutlineColors
        {
            public Color unselected;
            public Color selected;
        }

        private const float POSITION_TIME = 0.1f;
        private const float ROTATION_TIME = 0.1f;
        private const float FLIP_TIME = 0.1f;

        [Header("Component")]
        [SerializeField]
        private RectTransform _faceTransform;

        [SerializeField]
        private RectTransform _downTransform;

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private Outline _outline;

        [Header("Variables")]
        [SerializeField]
        private bool _isFaceUp;
        public bool IsFaceUp
        {
            get { return _isFaceUp; }
            set { _isFaceUp = value; }
        }

        [SerializeField]
        [Range(1, 100)]
        private int _value;
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        [SerializeField]
        private OutlineColors _outlineColors;

        private bool _isDirty;

        private float _flipVelocity;

        public Vector2 TargetPosition
        {
            get; set;
        }
        private Vector2 _positionVelocity;

        public float TargetRotation
        {
            get; set;
        }
        private float _rotationVelocity;

        public bool IsSelected
        {
            get; set;
        }

        public RectTransform rectTransform
        {
            get { return (RectTransform)this.transform; }
        }

        // Start is called before the first frame update
        void Start()
        {
            OnInstantiate();
            SetCardState(_isFaceUp, false);
            UpdateText();
        }

        // Update is called once per frame
        void Update()
        {
            if (_isDirty)
            {
                UpdateText();
            }
            UpdateCardPositionAndRotation();
            UpdateCardState();
            UpdateSelected();
        }

        public new string ToString()
        {
            return string.Concat("Card(", Value, ")");
        }

        public void SetDirty()
        {
            _isDirty = true;
        }

        public void SetPosition(Vector2 position, bool smooth)
        {
            TargetPosition = position;
            if (!smooth)
            {
                this.rectTransform.position = position;
            }
        }

        public void SetRotation(float rotation, bool smooth)
        {
            TargetRotation = rotation;
            if (!smooth)
            {
                this.rectTransform.rotation = Quaternion.Euler(0f, 0f, rotation);
            }
        }

        public void SetCardState(bool faceUp, bool smooth)
        {
            _isFaceUp = faceUp;
            if (!smooth)
            {
                _faceTransform.localScale = new Vector3(faceUp ? 1f : -1f, 0f, 0f);
                _downTransform.localScale = new Vector3(!faceUp ? 1f : -1f, 0f, 0f);
                _faceTransform.gameObject.SetActive(faceUp);
                _faceTransform.gameObject.SetActive(!faceUp);
            }
        }

        public void Flip(bool smooth)
        {
            this.SetCardState(!_isFaceUp, smooth);
        }

        private void UpdateCardPositionAndRotation()
        {
            this.rectTransform.SetPositionAndRotation(
                Vector2.SmoothDamp(this.rectTransform.position, TargetPosition, ref _positionVelocity, POSITION_TIME, Mathf.Infinity, Time.deltaTime),
                Quaternion.Euler(0f, 0f,
                    Mathf.SmoothDampAngle(this.rectTransform.rotation.eulerAngles.z, TargetRotation, ref _rotationVelocity, ROTATION_TIME, Mathf.Infinity, Time.deltaTime))
                );
        }

        private void UpdateCardState()
        {
            RectTransform toAppear = _downTransform, toDisappear = _faceTransform;
            if (_isFaceUp)
            {
                toAppear = _faceTransform;
                toDisappear = _downTransform;
            }

            float x = Mathf.SmoothDamp(toAppear.localScale.x, 1f, ref _flipVelocity, FLIP_TIME, Mathf.Infinity, Time.deltaTime);
            toAppear.localScale = new Vector3(x, 1f, 1f);
            toDisappear.localScale = new Vector3(-x, 1f, 1f);
            toAppear.gameObject.SetActive(x > 0f);
            toDisappear.gameObject.SetActive(x <= 0f);
        }

        private void UpdateText()
        {
            _text.text = Value.ToString();
        }

        private void UpdateSelected()
        {
            if (IsSelected)
            {
                this._outline.effectColor = _outlineColors.selected;
            } else
            {
                this._outline.effectColor = _outlineColors.unselected;
            }
        }

        public override void OnInstantiate()
        {
            this.TargetPosition = this.rectTransform.position;
            this.TargetRotation = this.rectTransform.rotation.eulerAngles.z;
            //this.rectTransform.localScale = Vector3.one;
        }

    }
}