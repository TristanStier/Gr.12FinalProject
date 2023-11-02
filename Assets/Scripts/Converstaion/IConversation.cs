using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConversation
{
    string getName();

    void beginConversation();

    void endConversation();
}
