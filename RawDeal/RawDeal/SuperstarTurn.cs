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
    public TurnKane(View view, Player player, Player opponent) : base(view, player, opponent)
    {
        _view = view;
    }
    
    public override void StartTurnOfPlayer()
    {
        _view.SayThatATurnBegins(player.GetSuperstarName());
        _view.SayThatPlayerIsGoingToUseHisAbility(player.GetSuperstarName(), player.GetSuperstarAbility());
        _view.SayThatOpponentWillTakeSomeDamage(opponent.GetSuperstarName(), 1);
        DamageOpponent(1);
        player.AddCardToHandFromArsenal();
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
        if (player.IsThereAnyCardAtRingside())
        {
            bool useAbility = _view.DoesPlayerWantToUseHisAbility(player.GetSuperstarName());
            if (useAbility)
            {
                int cardIndex = _view.AskPlayerToSelectCardsToRecover(player.GetSuperstarName(), 1,
                    GetStrListFromCardList(player.Ringside));
                Card card = player.DrawCardFromRingsideOfIndex(cardIndex);
                player.AddCardToBottomOfArsenal(card);
            }
        }
        player.AddCardToHandFromArsenal();
    }

}

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

    public override void UseAbility()
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(player.GetSuperstarName(), player.GetSuperstarAbility());
        DiscardCardFromHand(player, 2);
        RecoverCardFromRingside(player, 1);
        HasUsedAbility = true;
    }
    
    public void RecoverCardFromRingside(Player player, int amount)
    {
        int cardIndex = _view.AskPlayerToSelectCardsToPutInHisHand(player.GetSuperstarName(), amount, GetStrListFromCardList(player.Ringside));
        Card recoveredCard = player.DrawCardFromRingsideOfIndex(cardIndex);
        player.AddCardToTopOfHand(recoveredCard);
    }
    
    public void DiscardCardFromHand(Player player, int amount)
    {
        for (int i = amount; i > 0; i--)
        {
            int cardIndex = _view.AskPlayerToSelectACardToDiscard(GetStrListFromCardList(player.Hand),
                player.GetSuperstarName(), player.GetSuperstarName(), i);
            Card discardedCard = player.DrawCardFromHandOfIndex(cardIndex);
            player.AddCardToTopOfRingside(discardedCard);
        }
    }
    
    public override void SetAbilityUsageToAvailable()
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
        if(player.GetHandCount() >= 1 && !HasUsedAbility)
            return true;
        return false;
    }

    public override void UseAbility()
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(player.GetSuperstarName(), player.GetSuperstarAbility());
        int cardIndex1 = _view.AskPlayerToSelectACardToDiscard(GetStrListFromCardList(player.Hand), player.GetSuperstarName(), player.GetSuperstarName(), 1);
        Card discardedCard1 = player.DrawCardFromHandOfIndex(cardIndex1);
        player.AddCardToTopOfRingside(discardedCard1);
        
        int cardIndex2 = _view.AskPlayerToSelectACardToDiscard(GetStrListFromCardList(opponent.Hand), opponent.GetSuperstarName(), opponent.GetSuperstarName(), 1);
        Card discardedCard2 = opponent.DrawCardFromHandOfIndex(cardIndex2);
        opponent.AddCardToTopOfRingside(discardedCard2);
        HasUsedAbility = true;
        
    }

    public override void SetAbilityUsageToAvailable()
    {
        HasUsedAbility = false;
    }
}

public class TurnMankind : Turn
{
    public TurnMankind(View view, Player player, Player opponent) : base(view, player, opponent)
    {
        _view = view;
        opponent.DamageExtra = -1;
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
        if(!HasUsedAbility && player.Arsenal.Any())
            return true;
        return false;
    }

    public override void UseAbility()
    {
        Card card = player.DrawCardFromTopOfArsenal();
        _view.SayThatPlayerIsGoingToUseHisAbility(player.GetSuperstarName(), player.GetSuperstarAbility());
        _view.SayThatPlayerDrawCards(player.GetSuperstarName(), 1);
        player.AddCardToTopOfHand(card);
        int cardIndex = _view.AskPlayerToReturnOneCardFromHisHandToHisArsenal(player.GetSuperstarName(),
            GetStrListFromCardList(player.Hand));
        Card cardToReturn = player.DrawCardFromHandOfIndex(cardIndex);
        player.AddCardToBottomOfArsenal(cardToReturn);
        HasUsedAbility = true;
    }
    
    public override void SetAbilityUsageToAvailable()
    {
        HasUsedAbility = false;
    }
}














