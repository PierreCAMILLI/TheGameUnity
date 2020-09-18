using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public static readonly int MAX_CARDS = 6;

    public int Number
    {
        get; private set;
    }

    private HashSet<int> _cards;
    public ISet<int> Cards
    {
        get { return _cards; }
    }

    private Player() { }
    public Player(int number)
    {
        Number = number;
        _cards = new HashSet<int>();
    }

    public bool Contains(int card)
    {
        return _cards.Contains(card);
    }

    public void Remove(int card)
    {
        _cards.Remove(card);
    }

    public void Add(int card)
    {
        _cards.Add(card);
    }

    public void Add(List<int> card)
    {
        _cards.UnionWith(card);
    }
}
