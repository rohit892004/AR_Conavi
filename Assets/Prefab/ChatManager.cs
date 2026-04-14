using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class ChatManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField inputField;
    public Transform content;
    public ScrollRect scrollRect;

    [Header("Prefabs")]
    public GameObject userRowPrefab; // RIGHT
    public GameObject botRowPrefab;  // LEFT

    [Header("API")]
    public string apiUrl = "";
    public int timeoutSeconds = 20;

    // =============================
    // SEND BUTTON
    // =============================
    public void OnSend()
    {
        if (string.IsNullOrWhiteSpace(inputField.text))
            return;

        string userMessage = inputField.text.Trim();

        // USER MESSAGE (RIGHT)
        CreateMessage(userMessage, true);

        inputField.text = "";
        inputField.ActivateInputField();

        // CALL API
        StartCoroutine(SendToHF(userMessage));
    }

    // =============================
    // HF API CALL
    // =============================
    IEnumerator SendToHF(string userMessage)
    {
        QuestionData data = new QuestionData();
        data.query = userMessage;

        string json = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler =
            new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = timeoutSeconds;

        yield return request.SendWebRequest();

        // ❌ NETWORK / TIMEOUT
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.DataProcessingError)
        {
            CreateMessage("❌ Network error.\nCheck internet or server.", false);
            yield break;
        }

        // ❌ SERVER ERROR
        if (request.result == UnityWebRequest.Result.ProtocolError)
        {
            CreateMessage(
                $"❌ Server error ({request.responseCode})\n{request.downloadHandler.text}",
                false
            );
            yield break;
        }

        // ✅ SUCCESS
        try
        {
            AIResponse response =
                JsonUtility.FromJson<AIResponse>(request.downloadHandler.text);

            if (response == null || string.IsNullOrEmpty(response.answer))
            {
                CreateMessage("❌ Empty response from AI.", false);
            }
            else
            {
                CreateMessage(response.answer, false); // BOT (LEFT)
            }
        }
        catch
        {
            CreateMessage("❌ Failed to parse AI response.", false);
        }
    }

    // =============================
    // CREATE CHAT MESSAGE
    // =============================
    void CreateMessage(string msg, bool isUser)
    {
        GameObject prefab = isUser ? userRowPrefab : botRowPrefab;
        GameObject row = Instantiate(prefab, content);

        TMP_Text txt = row.GetComponentInChildren<TMP_Text>();
        txt.text = msg;

        // AUTO SCROLL
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
