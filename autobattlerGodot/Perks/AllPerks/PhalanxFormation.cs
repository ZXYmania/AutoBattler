using System;
using static Engagement;
using static Phase;
using System.Collections.Generic;
using static Movement;
using static Troop;
using static AffectsMovement;
using System.Linq;

public class PhalanxFormation : TroopPerk, ActionRequester, AffectsMovement
{
    public SubscriptionHadler? subscriptionHandler;
    
    public HashSet<MovementType> movement = [MovementType.Charge, MovementType.March, MovementType.Advance];
    public const PhaseType requiredPhase = PhaseType.Poke;
    public const ActionType actionType = ActionType.Strike;
    
    public bool actionRequested {get; private set;}
    public bool movementActive {get; private set;}
    public Guid troopId;
    public Guid enemyId;
    public int hitAmount;

    public PhalanxFormation(Guid troopId)
    {
        this.troopId = troopId;
        subscriptionHandler = null;
    }

    public void InitialiseSubscription(SubscriptionHadler subscriber)
    {
       subscriptionHandler = subscriber;
       subscriptionHandler.UpdateMovementOrderSubscription(Trigger, true);
    }

    public void Dispose()
    {
        if(subscriptionHandler != null)
        {
            subscriptionHandler.UpdateMovementOrderSubscription(Trigger, false);
            subscriptionHandler.UpdateStrikeSubscription(Trigger, false);
        }
    }

    private static Guid id = Guid.NewGuid();

    public bool Equals(TroopPerk? other)
    {
        if(other == null){return false;}
        return other.GetId() == id;
    }

    public Guid GetId()
    {
        return id;
    }

    public Guid GetTroop()
    {
        return troopId;
    }

    public bool IsActive()
    {
        return actionRequested || movementActive;
    }

    public void ResetPerk()
    {
        movementActive = false;
        actionRequested = false;
        hitAmount = 0;
        enemyId = Guid.Empty;
        if(subscriptionHandler != null)
        {
            subscriptionHandler.UpdateStrikeSubscription(Trigger, false);
            subscriptionHandler.UpdateOnEndOfRoundSubscription(Trigger,false);
        }
    }

    public TroopPerk? Trigger(MovementOrders order)
    {
        if(order.currentPhase == requiredPhase)
        {
            if(troopId == order.antagonistOrder.troopId && movement.Contains(order.protagonistOrder.movement))
            {
                actionRequested = true;
                subscriptionHandler!.UpdateStrikeSubscription(Trigger, true);
                subscriptionHandler.UpdateOnEndOfRoundSubscription(Trigger, true);
                return this;
            }
        }
        return null;
    }

    public TroopPerk? Trigger(Combat strike)
    {
        subscriptionHandler!.UpdateStrikeSubscription(Trigger, false);
        if(actionRequested && strike.IsProtagonist(troopId))
        {
            hitAmount = strike.strikeList.Count(t => t.Value.area != BodyPart.Shield && t.Value.area != BodyPart.Leg);
            movementActive = true;
            enemyId = strike.antagonistId;
            actionRequested = false;
            return null;
        }
        return null;
    }

    public void Trigger(EndOfRound trigger)
    {
        ResetPerk();
    }

    public List<ActionRequest> RequestAction()
    {
        List<ActionRequest> output = new List<ActionRequest>();
        if(actionRequested)
        {
            output.Add(new ActionRequest{type=actionType, protagonist=troopId});
        }
        return output;
    }

    public Nullable<MovementModifier> GetMovementModifier()
    {
        Nullable<MovementModifier> output = null;
        if(movementActive)
        {
            int modifier = hitAmount > 0? hitAmount*5: -5;
            output = new MovementModifier{troopId = enemyId, currentPhase = requiredPhase, type = MovementModifierType.Additive, value=modifier};
        
        }
        ResetPerk();
        return output;
    }

    public TroopPerk ResolveClash(TroopPerk otherPerk)
    {
        return this;
    }
}