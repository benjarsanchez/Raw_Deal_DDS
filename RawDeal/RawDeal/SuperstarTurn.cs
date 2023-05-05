using RawDealView;
using Objects;
using Objects_Player;

namespace RawDeal;

public class TurnHHH : Turn
{
    public TurnHHH(View view, Player player, Player opponent) : base(view, player, opponent)
    { }
}

public class TurnKane : Turn
{
    private const int DamageAppliedInAbility = 1;
    
    public TurnKane(View view, Player player, Player opponent) : base(view, player, opponent)
    {
        _view = view;
    }
    
    public override void StartTurnOfPlayer()
    {
        _view.SayThatATurnBegins(player.GetSuperstarName());
        UseAbility();
        player.AddCardToHandFromArsenal();
    }
    
    protected override void UseAbility()
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(player.GetSuperstarName(), player.GetSuperstarAbility());
        HandleDamageTakenByOpponent(DamageAppliedInAbility);
    }
}

public class TurnTheRock : Turn
{
    public TurnTheRock(View view, Player player, Player opponent) : base(view, player, opponent)
    {
        _view = view;
    }

    public override void StartTurnOfPlayer()
    {
        _view.SayThatATurnBegins(player.GetSuperstarName());
        HandleAbility();
        player.AddCardToHandFromArsenal();
    }

    protected override void HandleAbility()
    {
        if (!CanTheRockUseHisAbility()) return;
        if (DoesUserWantToUseHisAbility())
            UseAbility();
    }

    protected override void UseAbility()
    {
        int cardIndex = GetCardIndexFromUser();
        Card card = player.DrawCardFromRingsideOfIndex(cardIndex);
        player.AddCardToBottomOfArsenal(card);
    }

    private int GetCardIndexFromUser()
    {
        return _view.AskPlayerToSelectCardsToRecover(player.GetSuperstarName(), 1,
            GetStrListFromCardList(player.GetRingside()));
    }

    private bool DoesUserWantToUseHisAbility()
    {
        return _view.DoesPlayerWantToUseHisAbility(player.GetSuperstarName());
    }

    private bool CanTheRockUseHisAbility()
    {
        return player.IsThereAnyCardAtRingside();
    }

};

public class TurnUndertaker : Turn
{
    public bool HasUsedAbility;
    public TurnUndertaker(View view, Player player, Player opponent) : base(view, player, opponent)
    {
        _view = view;
        HasUsedAbility = false;
    }

    public override bool IsAbilityAvailable()
    {
        if(player.GetHandCount() >= 2 && !HasUsedAbility)
            return true;
        return false;
    }

    protected override void HandleAbility()
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(player.GetSuperstarName(), player.GetSuperstarAbility());
        DiscardCardFromHand(player, 2);
        RecoverCardFromRingside(player, 1);
        HasUsedAbility = true;
    }
    
    public void RecoverCardFromRingside(Player player, int amount)
    {
        int cardIndex = _view.AskPlayerToSelectCardsToPutInHisHand(player.GetSuperstarName(), amount, GetStrListFromCardList(player.GetRingside()));
        Card recoveredCard = player.DrawCardFromRingsideOfIndex(cardIndex);
        player.AddCardToTopOfHand(recoveredCard);
    }
    
    public void DiscardCardFromHand(Player player, int amount)
    {
        for (int i = amount; i > 0; i--)
        {
            int cardIndex = _view.AskPlayerToSelectACardToDiscard(GetStrListFromCardList(player.GetHand()),
                player.GetSuperstarName(), player.GetSuperstarName(), i);
            Card discardedCard = player.DrawCardFromHandOfIndex(cardIndex);
            player.AddCardToTopOfRingside(discardedCard);
        }
    }
    
    public override void EnableAbility()
    {
        HasUsedAbility = false;
    }

}

public class TurnJericho : Turn
{
    public bool HasUsedAbility;
    public TurnJericho(View view, Player player, Player opponent) : base(view, player, opponent)
    {
        _view = view;
        HasUsedAbility = false;
    }

    public override bool IsAbilityAvailable()
    {
        return player.GetHandCount() >= 1 && !HasUsedAbility;
    }

    protected override void HandleAbility()
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(player.GetSuperstarName(), player.GetSuperstarAbility());
        UseAbility();
        DisableAbility();
    }

    protected override void UseAbility()
    {
        PlayerRemoveCardFromHand();
        OpponentRemoveCardFromHand();
    }


    private void PlayerRemoveCardFromHand()
    {   
        int cardIndex = _view.AskPlayerToSelectACardToDiscard(GetStrListFromCardList(player.GetHand()), player.GetSuperstarName(), player.GetSuperstarName(), 1);
        Card discardedCard = player.DrawCardFromHandOfIndex(cardIndex);
        player.AddCardToTopOfRingside(discardedCard);
    }
    
    private void OpponentRemoveCardFromHand()
    {
        int cardIndex = _view.AskPlayerToSelectACardToDiscard(GetStrListFromCardList(opponent.GetHand()), opponent.GetSuperstarName(), opponent.GetSuperstarName(), 1);
        Card discardedCard = opponent.DrawCardFromHandOfIndex(cardIndex);
        opponent.AddCardToTopOfRingside(discardedCard);
    }

    public override void EnableAbility()
    {
        HasUsedAbility = false;
    }
    
    private void DisableAbility()
    {
        HasUsedAbility = true;
    }
    
}

public class TurnMankind : Turn
{
    private const int ExtraDamageReceived = -1;
    public TurnMankind(View view, Player player, Player opponent) : base(view, player, opponent)
    {
        _view = view;
        opponent.SetDamageExtra(ExtraDamageReceived);
    }
    
    public override void StartTurnOfPlayer()
    {
        _view.SayThatATurnBegins(player.GetSuperstarName());
        if (player.GetArsenalCount() == 1)
        {
            player.AddCardToHandFromArsenal();
        }
        else
        {
            player.AddCardToHandFromArsenal();
            player.AddCardToHandFromArsenal();
        }
    }
}

public class TurnStoneCold : Turn
{
    public bool HasUsedAbility;
    
    public TurnStoneCold(View view, Player player, Player opponent) : base(view, player, opponent)
    {
        _view = view;
        HasUsedAbility = false;
    }
    
    public override bool IsAbilityAvailable()
    {
        if(!HasUsedAbility && player.IsThereAnyCardAtArsenal())
            return true;
        return false;
    }

    protected override void HandleAbility()
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(player.GetSuperstarName(), player.GetSuperstarAbility());
        UseAbility();
        DisableAbility();
    }

    protected override void UseAbility()
    {
        PlayerDrawFirstCardFromArsenalToHand();
        PlayerDrawCardFromHandToArsenal();
    }
    
    private void PlayerDrawCardFromHandToArsenal()
    {
        int cardIndex = _view.AskPlayerToReturnOneCardFromHisHandToHisArsenal(player.GetSuperstarName(),
            GetStrListFromCardList(player.GetHand()));
        Card cardToReturn = player.DrawCardFromHandOfIndex(cardIndex);
        player.AddCardToBottomOfArsenal(cardToReturn);
    }
    
    private void PlayerDrawFirstCardFromArsenalToHand()
    {
        Card card = player.DrawCardFromTopOfArsenal();
        _view.SayThatPlayerDrawCards(player.GetSuperstarName(), 1);
        player.AddCardToTopOfHand(card);
    }

    public override void EnableAbility()
    {
        HasUsedAbility = false;
    }
    
    public override void DisableAbility()
    {
        HasUsedAbility = true;
    }
    
}














