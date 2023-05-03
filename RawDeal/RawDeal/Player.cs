namespace Objects_Player;
using Objects;
using Objects_Deck;
using RawDealView;

public class Player
{
    public Deck Deck;
    public List<Card> Hand;
    public List<Card> Arsenal; 
    public List<Card> Ringside;
    public Superstar Superstar;
    public int FortitudeRating;
    public List<Card> RingArea;
    private bool _IsDead;
    public int DamageExtra;

    public Player(Deck deck)
    {
        Deck = deck;
        Hand = new List<Card>();
        FortitudeRating = 0;
        Arsenal = new List<Card>();
        Arsenal.AddRange(Deck.Cards);
        Superstar = Deck.Superstar;
        Ringside = new List<Card>();
        RingArea = new List<Card>();
        _IsDead = false;
        DamageExtra = 0;
    }
    public bool IsDead()
    {
        return _IsDead;
    }
    
    public void SetPlayerAsDead()
    {
        _IsDead = true;
    }
    public void SetPlayerAsAlive()
    {
        _IsDead = false;
    }
    
    public void AddCardToHandFromArsenal()
    {
        Card card = DrawCardFromTopOfArsenal();
        AddCardToTopOfHand(card);
    }
    
    public int GetSuperstarHandSize()
    {
        return Superstar.HandSize;
    }
    public Superstar GetSuperstar()
    {
        return Superstar;
    }
    
    public Card DrawPlayedCardFromHand(int indexOfPlayableCardSelected)
    {
        List<Card> playableCards = GetPlayableCards();
        Card cardSelected = new Card();
        int counter = 0;
        for(int i = 0; i < Hand.Count; i++)
        {
            if (Hand[i].Title == playableCards[counter].Title)
            {
                if (counter == indexOfPlayableCardSelected)
                {
                    cardSelected = DrawCardFromHandOfIndex(i);
                    break;
                }
                counter++;
            }
        }
        return cardSelected;
    }
    
    public bool IsThereAnyCardAtRingside()
    {
        return Ringside.Any();
    }
    
    public List<Card> GetPlayableCards()
    {
        List<Card> playableCards = new List<Card>();
        foreach (Card card in Hand)
        {
            if (int.Parse(card.Fortitude) <= FortitudeRating && !IsCardTypeReversal(card))
                playableCards.Add(card);
        }
        return playableCards;
    }
    
    public bool IsCardTypeReversal(Card card)
    {
        if (card.Types.Contains("Reversal"))
            return true;
        return false;
    }
    
    public void IncreaseFortitudeRating(int amount)
    {
        FortitudeRating += amount;
    }

    public bool IsThereAnyCardAtArsenal()
    {
        return Arsenal.Any();
    }
    
    public string GetSuperstarName()
    {
        return Superstar.Name;
    }
    
    public string GetSuperstarAbility()
    {
        return Superstar.SuperstarAbility;
    }
    
    public int GetArsenalCount()
    {
        return Arsenal.Count;
    }
    
    public int GetHandCount()
    {
        return Hand.Count;
    }

    public Card DrawCardFromTopOfArsenal()
    {
        Card cardToRemove = Arsenal[^1];
        Arsenal.RemoveAt(Arsenal.Count - 1);
        return cardToRemove;
    }

    public Card DrawCardFromHandOfIndex(int cardIndex)
    {
        Card cardToRemove = Hand[cardIndex];
        Hand.RemoveAt(cardIndex);
        return cardToRemove;
    }
    
    public Card DrawCardFromRingsideOfIndex(int cardIndex)
    {
        Card cardToRemove = Ringside[cardIndex];
        Ringside.RemoveAt(cardIndex);
        return cardToRemove;
    }
    
    public void AddCardToTopOfHand(Card card)
    {
        Hand.Add(card);
    }

    public void AddCardToTopOfRingside(Card card)
    {
        Ringside.Add(card);
    }
    
    public void AddCardToTopOfRingArea(Card card)
    {
        RingArea.Add(card);
    }
    
    public void AddCardToBottomOfArsenal(Card card)
    {
        Arsenal.Insert(0, card);
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}