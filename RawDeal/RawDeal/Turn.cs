using RawDealView;
using Objects;
using Objects_Player;

namespace RawDeal;

public class Turn
{
    public View _view;
    public Player player;
    public Player opponent;
    private bool turnIsOver;
    private const int comeBackMenu = -1;

    public Turn(View view, Player player, Player opponent)
    {
        _view = view;
        this.player = player;
        this.opponent = opponent;
        turnIsOver = false;
    }
    
    public void PlayTurn()
    {
        turnIsOver = false;
        StartTurnOfPlayer();
        while (!turnIsOver)
        {
            ShowPlayersInfo();
            NextPlay nextPlay = GetNextPlayFromUser();
            if (nextPlay == NextPlay.PlayCard)
                PlayCard();
            else if (nextPlay == NextPlay.ShowCards)
                ShowSeeCardsOptions();
            else if (nextPlay == NextPlay.UseAbility)
                UseAbility();
            else if (nextPlay == NextPlay.EndTurn)
                EndTurn();
            else if (nextPlay == NextPlay.GiveUp)
                GiveUp();
            UpdateOpponentLifeStatus();
            if (opponent.IsDead())
                EndTurn();
        }
        SetAbilityUsageToAvailable();
        UpdatePlayersLifeStatus();
    }

    private void GiveUp()
    {
        player.SetPlayerAsDead();
        EndTurn();
    }

    private void EndTurn()
    {
        turnIsOver = true;
    }
    
    public void PlayCard()
    {
        int userCardSelection = GetSelectedPlayIndex();
        if (userCardSelection == comeBackMenu)
            return;
        Card cardPlayed = player.DrawPlayedCardFromHand(userCardSelection);
        string cardPlayedInfo = GetCardInfo(cardPlayed);
        player.AddCardToTopOfRingArea(cardPlayed);
        _view.SayThatPlayerIsTryingToPlayThisCard(player.Superstar.Name, cardPlayedInfo);
        _view.SayThatPlayerSuccessfullyPlayedACard();
        int damageAmount = GetDamageAmount(cardPlayed);
        _view.SayThatOpponentWillTakeSomeDamage(opponent.Superstar.Name, damageAmount);
        DamageOpponent(damageAmount);
        int fortitudeDamage = GetDamageFortitude(cardPlayed);
        player.IncreaseFortitudeRating(fortitudeDamage);
    }
    
    private void UpdatePlayersLifeStatus()
    {
        if (!player.IsThereAnyCardAtArsenal() && opponent.IsDead())
        {
            player.SetPlayerAsAlive();
            opponent.SetPlayerAsDead();
        }
        else if (!player.IsThereAnyCardAtArsenal() && !opponent.IsDead())
        {
            player.SetPlayerAsDead();
            opponent.SetPlayerAsAlive();
        }
    }

    public virtual void SetAbilityUsageToAvailable() {}
    
    public virtual void UseAbility() {}
    
    public void UpdateOpponentLifeStatus()
    {
        if (!opponent.IsThereAnyCardAtArsenal())
            opponent.SetPlayerAsDead();
        else
            opponent.SetPlayerAsAlive();
    }

    public virtual bool IsAbilityAvailable()
    {
        return false;
    }
    public NextPlay GetNextPlayFromUser()
    {
        if(IsAbilityAvailable())
            return _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible();;
        return _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
    }

    public virtual void StartTurnOfPlayer()
    {
        _view.SayThatATurnBegins(player.Superstar.Name);
        player.AddCardToHandFromArsenal();
    }
    
    public int GetDamageAmount(Card cardPlayed)
    {
        int damageAmount = int.Parse(cardPlayed.Damage) + player.DamageExtra;
        return damageAmount;
    }
    
    public int GetDamageFortitude(Card cardPlayed)
    { 
        return int.Parse(cardPlayed.Damage);
    }

    public int GetSelectedPlayIndex()
    {
        List<Card> playableCards = player.GetPlayableCards();
        List<string> plays = GetPlays(playableCards);
        int cardIndexSelection = _view.AskUserToSelectAPlay(plays);
        return cardIndexSelection;
    }
    
    public string GetCardInfo(Card card)
    {
        string cardInfo = GetStrFromCard(card);
        return Formatter.PlayToString(cardInfo, card.Types[0].ToUpper());
    }
    
    public virtual void DamageOpponent(int amount)
    {
        for (int i = 1; i <= amount; i++)
        {
            if (!opponent.IsThereAnyCardAtArsenal()) break;
            Card drawnCard = opponent.DrawCardFromTopOfArsenal();
            opponent.AddCardToTopOfRingside(drawnCard);
            string cardToRemoveStr = GetStrFromCard(drawnCard);
            _view.ShowCardOverturnByTakingDamage(cardToRemoveStr,i, amount);
        }
    }

    public List<string> GetPlays(List<Card> playableCards)
    {   List<string> plays = new List<string>();
        foreach(Card card in playableCards)
        {
            string cardInfo = GetStrFromCard(card);
            string play = Formatter.PlayToString(cardInfo, card.Types[0].ToUpper());
            plays.Add(play);
        }
        return plays;
    }
    
    public void ShowSeeCardsOptions()
    {
        List<string> cardStrList = null;
        CardSet userElection = _view.AskUserWhatSetOfCardsHeWantsToSee(); 
        if (userElection == CardSet.Hand)
            cardStrList = GetStrListFromCardList(player.Hand);
        else if (userElection == CardSet.RingArea)
            cardStrList = GetStrListFromCardList(player.RingArea);
        else if (userElection == CardSet.RingsidePile)
            cardStrList = GetStrListFromCardList(player.Ringside);
        else if (userElection == CardSet.OpponentsRingArea)
            cardStrList = GetStrListFromCardList(opponent.RingArea);
        else if (userElection == CardSet.OpponentsRingsidePile)
           cardStrList = GetStrListFromCardList(opponent.Ringside);
        _view.ShowCards(cardStrList);
    }
    
    public List<string> GetStrListFromCardList(List<Card> cardList)
    {
        List<string> cardStrList = new List<string>();
        foreach (Card card in cardList)
        {
            string cardStr = GetStrFromCard(card);
            cardStrList.Add(cardStr);
        }
        return cardStrList;
    }
    
    public string GetStrFromCard(Card card)
    {
        string cardStr = Formatter.CardToString( card.Title, card.Fortitude,
            card.Damage, card.StunValue, card.Types, card.Subtypes, card.CardEffect);
        return cardStr;
    }
    
    public void ShowPlayersInfo()
    {
        PlayerInfo playerInfo = new PlayerInfo(player.Superstar.Name,
            player.FortitudeRating, player.Hand.Count, player.Arsenal.Count);
        PlayerInfo opponentInfo = new PlayerInfo(opponent.Superstar.Name, 
            opponent.FortitudeRating, opponent.Hand.Count, opponent.Arsenal.Count);
        _view.ShowGameInfo(playerInfo, opponentInfo);
    }

}