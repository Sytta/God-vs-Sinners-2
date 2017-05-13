using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Factory
{

    public AICharacterControl generate(agentClassGlobal agent)
    {
        AICharacterControl character = new AICharacterControl(new Vector3G(agent.posX, agent.posY, agent.posZ), new Vector3G(agent.forVecX, agent.forVecY, agent.forVecZ), agent.agentID);
        return character;
    }

}
