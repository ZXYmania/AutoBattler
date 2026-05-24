# Simulation
## Definition
A simulation will take parameters of a list of Armies and a terrain definition.
The simulation will generate troops from the armies given and create a tile map from the terrain.
The simulation will move the troops and will create engagements to handle interactions between any two troops.
The simulation will emit events that will be read by the battle player.







Engagement ->
                Troop[]
                      -> Unit
                            -> Perk
                      -> Phase

Tick -> Troop -> phase
                -> change phase
                    -> choose reaction
                    -> Activate Perk
                -> Unit Action
                    Enter Action
                        -> Activate Perk
                        -> Determine if Action occurs
                    Resolve Action
                        -> Apply action results
                    Clean Up Action
                        -> Send result events
                    

Troop Phases
    Proactive
        * Poke (use range advantage to strike)
        * Push (close the gap to enter a pushing fight)
    Reactions
        * Withstand (accept the phase of your enemies)
        * Resist (withdraw to not allow your opponents to enter their phase range)
        * Strike (drive the enemies back with a strike)
    Status
        * Routed
        * Rotate (when out of range refresh your frontline with backliners)



Engagement
    Handle Phase change
    Ask units for unit actions within the phases
        # if both combattants phase change compare ranges and choose a defending side
            allow the defending side to react
        # reactions do not affect unit action values


                
