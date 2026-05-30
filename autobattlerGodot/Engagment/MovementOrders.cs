using static Movement;
using static Phase;

public struct MovementOrders : Trigger
{
    public Order protagonistOrder;
    public Order antagonistOrder;
    public PhaseType currentPhase;

    public Trigger.TriggerType GetTriggerType()
    {
        return Trigger.TriggerType.OnMove;
    }

    public MovementOrders(in Order leftOrder, in Order rightOrder, PhaseType currentPhase)
	{
        this.currentPhase = currentPhase;
		MovementType leftMovement = DifferenceBetweenPhaseType(this.currentPhase, leftOrder.desiredPhase);
		MovementType rightMovement = DifferenceBetweenPhaseType(this.currentPhase, rightOrder.desiredPhase);
		int leftPriority = GetMovementPriority(leftMovement);
		int rightPriority = GetMovementPriority(rightMovement);
		if(leftPriority == rightPriority)
		{
			if(leftMovement == MovementType.Stay && rightMovement == MovementType.Stay)
			{
				NoProtagonist(leftOrder, rightOrder);
			}
			else if(leftOrder.effort > rightOrder.effort)
			{
				SetLeftProtagonist(leftOrder, rightOrder);
			}
			else
			{
				SetRightProtagonist(leftOrder, rightOrder);
			}
		}
		else if (leftPriority < rightPriority)
		{
			SetLeftProtagonist(leftOrder, rightOrder);
		}
		else
		{
			SetRightProtagonist(leftOrder, rightOrder);
		}
	}

	public void NoProtagonist(in Order leftOrder,in Order rightOrder)
	{
		protagonistOrder = leftOrder; 
        antagonistOrder = rightOrder;
	}

	public void SetLeftProtagonist(in Order leftOrder, in Order rightOrder)
	{
        protagonistOrder = leftOrder;
        antagonistOrder = rightOrder;
		protagonistOrder.SetProtagonist(currentPhase);
		antagonistOrder.SetAntagonist(currentPhase, leftOrder.movement);
	}

	public void SetRightProtagonist(in Order leftOrder, in Order rightOrder)
	{
        protagonistOrder = rightOrder;
        antagonistOrder = leftOrder;
		rightOrder.SetProtagonist(currentPhase);
		leftOrder.SetAntagonist(currentPhase, rightOrder.movement);
	}
    
}