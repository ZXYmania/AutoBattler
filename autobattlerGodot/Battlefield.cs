using System;
using System.Collections.Generic;
using System.Linq;
using static MovementHandler;

public class Battlefield
{
    private Dictionary<Guid, Troop> troops;
    private Dictionary<Position, Tile> map;
    public Battlefield(Army left, Army right)
    {
        
    }

    public struct Position
    {
        int x;
        int y;
    }

    private Troop GetTroopByGuid(Guid troopId)
    {
        return troops[troopId];
    }

    private Troop UpdateTroop(Guid troopId, Troop givenTroop)
    {
        if(troopId == givenTroop.id)
        {
            troops[troopId] = givenTroop;
            return givenTroop;
        }
        return troops[troopId];
    }
}
