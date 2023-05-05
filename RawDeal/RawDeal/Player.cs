namespace Objects_Player;
using Objects;
using Objects_Deck;
using RawDealView;

public class Player
{
    private Deck Deck;
    private List<Card> Hand;
    private List<Card> Arsenal; 
    private List<Card> Ringside;
    private Superstar Superstar;
    private int FortitudeRating;
    private List<Card> RingArea;
    private bool _IsDead;
    private int DamageExtra;

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
    public Deck GetDeck()
    {
        return Deck;
    }
    
    public int GetDamageExtra()
    {
        return DamageExtra;
    }
    
    public void SetDamageExtra(int damageExtra)
    {
        DamageExtra = damageExtra;
    }
    
    public List<Card> GetRingArea()
    {
        return RingArea;
    }
    
    public List<Card> GetRingside()
    {
        return Ringside;
    }
    
    public int GetFortitudeRating()
    {
        return FortitudeRating;
    }
    public List<Card> GetHand()
    {
        return Hand;
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
        List<int> playableCardsIndex = GetIndexOfPlayableCards();
        Card cardSelected = DrawCardFromHandOfIndex(playableCardsIndex[indexOfPlayableCardSelected]);
        return cardSelected;
    }

    public bool IsThereAnyCardAtRingside()
    {
        return Ringside.Any();
    }
    
    public List<int> GetIndexOfPlayableCards()
    {
        List<int> indexOfPlayableCards = new List<int>();
        for(int index = 0; index < Hand.Count; index++)
        {
            if (IsCardPlayable(Hand[index]))
                indexOfPlayableCards.Add(index);
        }
        return indexOfPlayableCards;
    }

    public List<Card> GetPlayableCards()
    {
        List<Card> playableCards = new List<Card>();
        foreach (Card card in Hand)
        {
            if (IsCardPlayable(card))
                playableCards.Add(card);
        }
        return playableCards;
    }

    public bool IsCardPlayable(Card card)
    {
        if (int.Parse(card.Fortitude) <= FortitudeRating && !IsCardTypeReversal(card))
            return true;
        return false;
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