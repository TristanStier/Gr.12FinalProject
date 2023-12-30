using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.SceneManagement;
using System.Numerics;

public class ChatBubble : MonoBehaviour
{
    public static void Create(Transform parent, UnityEngine.Vector3 localPosition, string text, float duration)
    {
        GameObject empty = new GameObject();
        empty.transform.localPosition = parent.localPosition;
        empty.transform.localRotation = parent.localRotation;
        empty.transform.localScale =  new UnityEngine.Vector3(1, 1, 1);

        Transform chatBubbleTransform = Instantiate(GameAssets.i.pfChatBubble, empty.transform);
        chatBubbleTransform.localPosition = localPosition;
        chatBubbleTransform.GetComponent<ChatBubble>().Setup(text);

        Destroy(empty, duration);
        Destroy(chatBubbleTransform.gameObject, duration);
    }

    private SpriteRenderer backgroundSprite;
    private TextMeshProUGUI chatText;

    private void Awake()
    {
        backgroundSprite = transform.Find("Background").GetComponent<SpriteRenderer>();
        chatText = transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Setup(string text)
    {
        chatText.SetText(text);
        chatText.ForceMeshUpdate();

        UnityEngine.Vector2 textSize = chatText.GetRenderedValues(false)/40;
        UnityEngine.Vector2 padding = new UnityEngine.Vector2(2f, 0f);
        UnityEngine.Vector2 adjustedTextSize = new UnityEngine.Vector2(textSize.x/1.3f, textSize.y/1.5f);

        backgroundSprite.size = adjustedTextSize + padding;
    }
}
