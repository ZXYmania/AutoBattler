using System;

public class InvalidTroopId : Exception
{
    public InvalidTroopId(Guid invalidId, Guid leftId, Guid rightId) 
    : base("Invalid troopId of: " + invalidId + ". Valid options are: " +leftId+", " +rightId)
    {
        
    }

    public InvalidTroopId(Guid invalidId, Guid id): base("Invalid troopId of: " + invalidId + "Must be " + id)
    {
        
    }
}