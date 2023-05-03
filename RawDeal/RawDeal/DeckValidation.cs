namespace Deck_Validation;
using Objects_Deck;
using Objects;

public class DeckValidation
{
    private Deck deck;
    private List<Superstar> superStars;
    
    public DeckValidation(Deck deck, List<Superstar> superStars)
    {
        this.deck = deck;
        this.superStars = superStars;
    }
    public bool CheckIfDeckIsValid()
    {
        if (CheckDeckSize() == false)
            return false;
        if (CheckRepeatedCardsInDeck() == false)
            return false;
        if (CheckHeelOrFace() == false)
            return false;
        if (CheckSuperstarCards() == false)
            return false;
        
        return true;
    }
    
    private bool CheckDeckSize()
    {
        if (deck.Cards.Count == 60)
            return true;
        return false;
    }

    private bool CheckRepeatedCardsInDeck()
    {
        var duplicates = deck.Cards.GroupBy(x => x.Title)
            .Select(y => new { Name = y.Key, Count = y.Count() })
            .ToList();
        foreach (var i in duplicates)
        {
            Card card = deck.Cards.Find(c => c.Title == i.Name);
            if (card.Subtypes.Contains("Unique") && i.Count > 1)
                return false;

            if (i.Count > 3 && card.Subtypes.Contains("SetUp") == false)
                return false;
        }

        return true;
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
    private bool CheckSuperstarCards()
    {
        List<string> superstarNames = superStars.Select(s => s.Logo).ToList();
        foreach (Card card in deck.Cards)
        {
            foreach (string superstarName in superstarNames)
            {
                if (card.Subtypes.Contains(superstarName))
                {
                    if (superstarName != deck.Superstar.Logo)
                        return false;
                }
            }
        }
        return true;
    }
    
    
   




}