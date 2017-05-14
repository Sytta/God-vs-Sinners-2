using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Factory
{

    public static AICharacterControl generate(agentClassGlobal agent,DNA dna)
    {
        AICharacterControl character =  new AICharacterControl(Utilities.convert(agent.pos),
                                        Utilities.convert(agent.bodyForVec),
                                        agent.agentID,dna, agent.scale,agent);
        SimulationMap.Instance.add(character);


        return character;
    }
}
