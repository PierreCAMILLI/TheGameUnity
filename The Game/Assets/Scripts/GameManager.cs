using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visual;

public class GameManager : SingletonBehaviour<GameManager>
{
    [Header("Components")]
    [SerializeField]
    private Visual.Deck _deck;

    [SerializeField]
    private Visual.PlayerHand[] _uiHands;
    private List<Visual.PlayerHand> _hands;

    [SerializeField]
    private RectTransform[] _stacks;

    [Header("Variables")]
    [SerializeField]
    private int _playerCount;
    public int PlayerCount
    {
        get { return _playerCount; }
    }

    public Game.State State
    {
        get; private set;
    }

    private Game Game
    {
        get; set;
    }

    public int PlayerTurn
    {
        get { return this.Game.PlayerTurn; }
        set { this.Game.PlayerTurn = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.State = Game.State.Preparation;
        this.Game = new Game(_playerCount);
        ConstructHands();
        InitiateGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ConstructHands()
    {
        this._hands = new List<PlayerHand>();
        for (int i = 0; i < this.PlayerCount; ++i)
        {
            int index = Math.Mod(i - LocalPlayerController.Instance.PlayerNumber, this.PlayerCount);
            _uiHands[index].PlayerNumber = i;
            this._hands.Add(_uiHands[index]);
        }
    }

    private void InitiateGame()
    {
        this.FillDeck();
        for (int i = 0; i < this.PlayerCount; ++i)
        {
            DistributeCards(i);
        }
        this.Game.PlayerTurn = LocalPlayerController.Instance.PlayerNumber; // TODO: Change with chosen first player by players
        this.Game.ResetRemainingCardsToPlay();
        this.State = Game.State.OnGoing;
    }

    private void FillDeck()
    {
        // TODO: Replace this function to handle multiplayer
        // If server, fill the deck with random cards, send the cards to players and wait
        // If client, wait for server to send you the cards
        List<int> cards = Deck.GetCardsInRandomOrder();
        this.Game.Deck.Fill(cards);
    }

    public void DistributeCards(int playerNumber)
    {
        Visual.PlayerHand hand = _hands[playerNumber];
        List<int> newCards = this.Game.DistributeCards(playerNumber);
        if (this.Game.Deck.IsEmpty())
        {
            this._deck.SetTopCardActive(false);
        }
        StartCoroutine(VisualDistributeCards(playerNumber, newCards));
    }

    private IEnumerator VisualDistributeCards(int playerNumber, List<int> newCards)
    {
        Visual.PlayerHand hand = _hands[playerNumber];
        foreach (int newCard in newCards)
        {
            Visual.GameCard card = _deck.CreateCard(newCard, false).GetComponent<GameCard>();
            card.State = GameCard.CardState.InHand;
            card.Owner = playerNumber;
            hand.Add(card.Card);
            hand.SortCardsInHierarchy();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public bool StackCards(int playerNumber, int stackNumber, int cardNumber)
    {
        if (Game.StackCard(playerNumber, stackNumber, cardNumber))
        {
            Visual.PlayerHand hand = _hands[playerNumber];
            GameCard card = hand.Remove(cardNumber).GetComponent<GameCard>();
            card.Card.TargetPosition = _stacks[stackNumber].position;
            card.Card.TargetRotation = _stacks[stackNumber].rotation.eulerAngles.z;
            card.Card.SetCardState(true, true);

            card.State = GameCard.CardState.OnStack;
            card.StackNumber = stackNumber;

            card.transform.SetAsLastSibling();

            LocalPlayerController.Instance.SelectedGameObject = null;
            if (this.Game.PlayingPlayer.Cards.Count == 0 && this.Game.IsGameWin())
            {
                GameWin();
            }
            else if (this.Game.IsGameOver())
            {
                GameOver();
            }
            return true;
        }
        return false;
    }

    public bool EndTurn()
    {
        if (this.Game.IsTurnAbleToEnd())
        {
            this.DistributeCards(this.Game.PlayerTurn);
            this.Game.ResetRemainingCardsToPlay();
            this.Game.IncrementTurn();
            Debug.Log(string.Concat("Player ", this.Game.PlayerTurn, " starts playing!"));
            if (this.Game.IsGameOver())
            {
                GameOver();
            }
            return true;
        }
        Debug.LogWarning(string.Concat("There is ", this.Game.RemainingCardsToPlay, " cards remaining to be played by player number ", this.Game.PlayerTurn, "."));
        return false;
    }

    private void GameWin()
    {
        this.State = Game.State.GameWin;
    }

    private void GameOver()
    {
        this.State = Game.State.GameOver;
    }
}
