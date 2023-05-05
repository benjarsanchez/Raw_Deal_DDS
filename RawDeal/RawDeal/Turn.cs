using RawDealView;
using Objects;
using Objects_Player;

namespace RawDeal;

public class Turn
{
    protected View _view;
    protected Player player;
    protected Player opponent;
    private bool turnIsOver;
    private const int comeBackMenu = -1;

    protected Turn(View view, Player player, Player opponent)
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
                HandlePlayCard();
            else if (nextPlay == NextPlay.ShowCards)
                ShowSeeCardsOptions();
            else if (nextPlay == NextPlay.UseAbility)
                HandleAbility();
            else if (nextPlay == NextPlay.EndTurn)
                EndTurn();
            else if (nextPlay == NextPlay.GiveUp)
                GiveUp();
            UpdateOpponentLifeStatus();
            if (opponent.IsDead())
                EndTurn();
        }
        EnableAbility();
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
    
    public void HandlePlayCard()
    {
        int indexOfPlayableCardSelected = GetSelectedPlayIndex();
        if (indexOfPlayableCardSelected == comeBackMenu)
            return;
        Card cardToPlay = player.DrawPlayedCardFromHand(indexOfPlayableCardSelected);
        PlayCard(cardToPlay);
    }

    private void PlayCard(Card card)
    {
        player.AddCardToTopOfRingArea(card);
        string cardPlayedInfo = GetCardInfo(card);
        _view.SayThatPlayerIsTryingToPlayThisCard(player.GetSuperstarName(), cardPlayedInfo);
        _view.SayThatPlayerSuccessfullyPlayedACard();
        ApplyCardEffect(card);
    }

    private void ApplyCardEffect(Card card)
    {
        int damageAmount = GetDamageAmountOfCard(card);
        HandleDamageTakenByOpponent(damageAmount);
        int fortitudeDamage = GetDamageFortitude(card);
        player.IncreaseFortitudeRating(fortitudeDamage);
    }
    
    private void UpdatePlayersLifeStatus()
    {
        if (!player.IsThereAnyCardAtArsenal() && opponent.IsDead())
            SetPlayerAsWinner();
        else if (!player.IsThereAnyCardAtArsenal() && !opponent.IsDead()) 
            SetOpponentAsWinner();
    }
    
    private void SetPlayerAsWinner()
    {
        player.SetPlayerAsAlive();
        opponent.SetPlayerAsDead();
    }
    
    private void SetOpponentAsWinner()
    {
        player.SetPlayerAsDead();
        opponent.SetPlayerAsAlive();
    }



    public virtual void EnableAbility() {}
    public virtual void DisableAbility() {}


    protected virtual void HandleAbility() {}
    protected virtual void UseAbility() {}
    
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
        _view.SayThatATurnBegins(player.GetSuperstarName());
        player.AddCardToHandFromArsenal();
    }
    
    public int GetDamageAmountOfCard(Card cardPlayed)
    {
        return int.Parse(cardPlayed.Damage) + player.GetDamageExtra();;
    }
    
    public int GetDamageFortitude(Card cardPlayed)
    { 
        return int.Parse(cardPlayed.Damage);
    }

    public int GetSelectedPlayIndex()
    {
        List<Card> playableCards = player.GetPlayableCards();
        List<string> plays = GetPlayableCardsAsPlays(playableCards);
        int cardIndexSelection = _view.AskUserToSelectAPlay(plays);
        return cardIndexSelection;
    }
    
    public string GetCardInfo(Card card)
    {
        string cardInfo = GetCardAsStr(card);
        return Formatter.PlayToString(cardInfo, card.Types[0].ToUpper());
    }

    public void HandleDamageTakenByOpponent(int damageAmount)
    {
        _view.SayThatOpponentWillTakeSomeDamage(opponent.GetSuperstarName(), damageAmount);
        for (int currentDamage = 1; currentDamage <= damageAmount; currentDamage++)
        {
            if (!opponent.IsThereAnyCardAtArsenal()) break;
            DamageOpponent(currentDamage, damageAmount);
        }
    }
    
    protected void DamageOpponent(int currentDamage ,int totalDamage)
    {
        Card drawnCard = opponent.DrawCardFromTopOfArsenal();
        opponent.AddCardToTopOfRingside(drawnCard);
        string cardToRemoveStr = GetCardAsStr(drawnCard);
        _view.ShowCardOverturnByTakingDamage(cardToRemoveStr, currentDamage, totalDamage);
    }

    private static List<string> GetPlayableCardsAsPlays(List<Card> playableCards)
    {   
        List<string> plays = new List<string>();
        foreach(Card card in playableCards)
        {
            string play = GetCardAsPlay(card);
            plays.Add(play);
        }
        return plays;
    }

    private static string GetCardAsPlay(Card card)
    {
        string cardInfo = GetCardAsStr(card);
        return Formatter.PlayToString(cardInfo, card.Types[0].ToUpper());
    }
    
    public void ShowSeeCardsOptions()
    {
        List<string> cardStrList = null;
        CardSet userElection = _view.AskUserWhatSetOfCardsHeWantsToSee(); 
        if (userElection == CardSet.Hand)
            cardStrList = GetStrListFromCardList(player.GetHand());
        else if (userElection == CardSet.RingArea)
            cardStrList = GetStrListFromCardList(player.GetRingArea());
        else if (userElection == CardSet.RingsidePile)
            cardStrList = GetStrListFromCardList(player.GetRingside());
        else if (userElection == CardSet.OpponentsRingArea)
            cardStrList = GetStrListFromCardList(opponent.GetRingArea());
        else if (userElection == CardSet.OpponentsRingsidePile)
           cardStrList = GetStrListFromCardList(opponent.GetRingside());
        _view.ShowCards(cardStrList);
    }
    
    public List<string> GetStrListFromCardList(List<Card> cardList)
    {
        List<string> cardStrList = new List<string>();
        foreach (Card card in cardList)
        {
            string cardStr = GetCardAsStr(card);
            cardStrList.Add(cardStr);
        }
        return cardStrList;
    }

    private static string GetCardAsStr(Card card)
    {
        return Formatter.CardToString( card.Title, card.Fortitude,
            card.Damage, card.StunValue, card.Types, card.Subtypes, card.CardEffect);;
    }
    
    public void ShowPlayersInfo()
    {
        PlayerInfo playerInfo = new PlayerInfo(player.GetSuperstarName(),
            player.GetFortitudeRating(), player.GetHandCount(), player.GetArsenalCount());
        PlayerInfo opponentInfo = new PlayerInfo(opponent.GetSuperstarName(), 
            opponent.GetFortitudeRating(), opponent.GetHandCount(), opponent.GetArsenalCount());
        _view.ShowGameInfo(playerInfo, opponentInfo);
    }

}