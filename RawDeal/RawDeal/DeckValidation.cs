namespace Deck_Validation;
using Objects_Deck;
using Objects;

public class DeckValidation
{
    private Deck deck;
    private List<Superstar> superStars;
    private int deckSize = 60;
    private int maxNumberOfUniqueCards = 1;
    private int maxNumberOfNonUniqueCards = 3;
    
    public DeckValidation(Deck deck, List<Superstar> superStars)
    {
        this.deck = deck;
        this.superStars = superStars;
    }
    public bool CheckIfDeckIsValid()
    {
        if (CheckDeckSize() == false)
            return false;
        if (CheckRepeatedCardsRestrictionsInDeck() == false)
            return false;
        if (CheckHeelOrFace() == false)
            return false;
        if (CheckSuperstarCardsRestriction() == false)
            return false;
        
        return true;
    }
    
    private bool CheckDeckSize()
    {
        if (deck.Cards.Count == deckSize)
            return true;
        return false;
    }

    private bool CheckRepeatedCardsRestrictionsInDeck()
    {
        List<(Card card, int Cardinality)> cardsByCardinality  = GetCardsByCardinality();
        foreach (var tuple in cardsByCardinality)
        {
            int cardinality = tuple.Cardinality;
            Card card = tuple.card;
            if (card.Subtypes.Contains("Unique") && cardinality > maxNumberOfUniqueCards)
                return false;

            if (cardinality > maxNumberOfNonUniqueCards && card.Subtypes.Contains("SetUp") == false)
                return false;
        }
        return true;
    }

    private List<(Card card, int Cardinality)> GetCardsByCardinality()
    {
        var groupedCards = deck.Cards.GroupBy(card => card.Title)
            .Select(group => (Card: group.First(), Cardinality: group.Count()))
            .ToList();
        return groupedCards;
    }

    private bool CheckHeelOrFace()
    {
        bool heel = false;
        bool face = false;
        foreach (Card card in deck.Cards)
        {
            if (card.Subtypes.Contains("Heel"))
                heel = true;
            if (card.Subtypes.Contains("Face"))
                face = true;
        }
        return !heel || !face;
    }

    //OJO: Checkear nombres de funciones
    private bool CheckSuperstarCardsRestriction()
    {
        List<string> superstarNames = superStars.Select(s => s.Logo).ToList();
        foreach (Card card in deck.Cards)
        {
            if(!IsThereMatchBetweenCardSubtypeAndSuperstarNames(card, superstarNames))
                return false;
        }
        return true;
    }

    private bool IsThereMatchBetweenCardSubtypeAndSuperstarNames(Card card, List<string> superstarNames)
    {
        if(!IsThereAnySuperstarNameInsideCardSubtype(card, superstarNames)) return true;
        return CheckIfSubtypeMatchesSuperstar(card);
    }

    private bool CheckIfSubtypeMatchesSuperstar(Card card)
    {
        return card.Subtypes.Contains(deck.Superstar.Logo);
    }
        
    private bool IsThereAnySuperstarNameInsideCardSubtype(Card card, List<string> superstarNames)
    {
        return card.Subtypes.Any(superstarNames.Contains);
    }
    
    
    
   




}