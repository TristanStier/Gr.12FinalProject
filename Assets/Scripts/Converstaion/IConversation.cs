using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public interface IConversation
{
    void beginConversation();
    void say(string pPrompt, string senderName);
    void endConversation();

    bool letsTalk(IConversation iOther);

    int getImportance();
}
