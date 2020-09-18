using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackCards
{
    public bool Ascending
    {
        get; private set;
    }

    public int Top
    {
        get; private set;
    }

    public StackCards(bool ascending)
    {
        this.Ascending = ascending;
        this.Top = (Ascending ? 1 : 100);
    }

    public bool IsAbleToStack(int card)
    {
        int factor = this.Ascending ? 1 : -1;
        return factor * card > factor * Top || card == (Top - (factor * 10));
    }

    public void Stack(int card)
    {
        this.Top = card;
    }
}
