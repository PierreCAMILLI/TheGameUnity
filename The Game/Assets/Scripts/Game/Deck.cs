using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck
{
    Queue<int> _cards;

    public Deck()
    {
        _cards = new Queue<int>();
    }

    public Deck(List<int> cards)
    {
        _cards = new Queue<int>();
        Fill(cards);
    }
    
    public static List<int> GetCardsInRandomOrder()
    {
        List<int> cards = new List<int>(98);
        for (int i = 2; i <= 98; ++i)
        {
            cards.Add(i);
        }
        cards.Shuffle();
        return cards;
    }

    public void Fill(List<int> cards)
    {
        cards.ForEach(c => _cards.Enqueue(c));
    }

    public int Draw()
    {
        return _cards.Dequeue();
    }

    public List<int> Draw(int size)
    {
        int realSize = Mathf.Min(size, _cards.Count);
        List<int> cards = new List<int>(realSize);
        for (int i = 0; i < realSize; ++i)
        {
            cards.Add(_cards.Dequeue());
        }
        return cards;
    }

    public bool IsEmpty()
    {
        return _cards.Count == 0;
    }
}
