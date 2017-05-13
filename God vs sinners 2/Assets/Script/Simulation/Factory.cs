using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Factory
{

    public static AICharacterControl generate(agentClassGlobal agent)
    {
        AICharacterControl character =  new AICharacterControl(Utilities.convert(agent.pos),
                                        Utilities.convert(agent.bodyForVec),
                                        agent.agentID);

        SimulationMap.Instance.add(character);

        return character;
    }
}
