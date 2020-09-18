using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game
{
    public enum State
    {
        Preparation,
        OnGoing,
        GameOver,
        GameWin
    }

    private List<Player> _players;
    public IList<Player> Players
    {
        get { return _players; }
    }

    private List<StackCards> _stacks;
    public IList<StackCards> Stacks
    {
        get { return _stacks; }
    }

    public Deck Deck
    {
        get; private set;
    }

    public int PlayerTurn
    {
        get; set;
    }

    public Player PlayingPlayer
    {
        get { return _players[this.PlayerTurn]; }
    }

    public int MinCardsToPlay
    {
        get { return Deck.IsEmpty() ? 1 : 2; }
    }

    public int RemainingCardsToPlay
    {
        get; private set;
    }

    public Game(int playersSize)
    {
        _players = new List<Player>(playersSize);
        for (int i = 1; i <= playersSize; ++i)
        {
            _players.Add(new Player(i));
        }
        _stacks = new List<StackCards>(4)
        {
            new StackCards(true),
            new StackCards(true),
            new StackCards(false),
            new StackCards(false)
        };
        this.Deck = new Deck();
        this.PlayerTurn = 0;
        this.ResetRemainingCardsToPlay();
    }

    public bool IsGameOver()
    {
        return !IsAbleToPlay(PlayerTurn);
    }

    public bool IsGameWin()
    {
        foreach (Player player in _players)
        {
            if (player.Cards.Count > 0)
            {
                return false;
            }
        }
        return this.Deck.IsEmpty();
    }

    public int IncrementTurn()
    {
        return PlayerTurn = (PlayerTurn + 1) % Players.Count;
    }

    public void ResetRemainingCardsToPlay()
    {
        this.RemainingCardsToPlay = this.MinCardsToPlay;
    }

    public bool IsAbleToPlay(int playerNumber)
    {
        Player player = Players[playerNumber];
        return (player.Cards.AsParallel().Any(c => Stacks.AsParallel().Any(s => s.IsAbleToStack(c))));
    }

    public bool IsTurnAbleToEnd()
    {
        return this.RemainingCardsToPlay <= 0;
    }

    public bool StackCard(int playerNumber, int stackNumber, int card)
    {
        Player player = Players[playerNumber];
        if (player.Contains(card))
        {
            StackCards stack = Stacks[stackNumber];
            if (stack.IsAbleToStack(card))
            {
                player.Remove(card);
                stack.Stack(card);
                --RemainingCardsToPlay;
                return true;
            }
        }
        return false;
    }

    public List<int> DistributeCards(int playerNumber)
    {
        Player _player = Players[playerNumber];
        List<int> cards = Deck.Draw(Player.MAX_CARDS - _player.Cards.Count);
        _player.Add(cards);
        return cards;
    }
}
