namespace Objects_Deck;
using Objects;

public struct Deck
{
    public List<Card> Cards;
    public Superstar Superstar;
    
    public Deck(List<Card> cards, Superstar superstar)
    {
        Cards = cards;
        Superstar = superstar;
    }

}