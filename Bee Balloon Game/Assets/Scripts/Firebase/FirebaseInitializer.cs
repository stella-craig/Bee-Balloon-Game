using UnityEngine;
using System.Collections;

public class FirebaseInitializer : MonoBehaviour
{
    public string firebaseConfigPath = "google-services.js"; // File in StreamingAssets folder

    private void Start()
    {
        // Load Firebase config on start
        LoadFirebaseConfig();
    }

    private void LoadFirebaseConfig()
    {
        string configPath = Application.streamingAssetsPath + "/" + firebaseConfigPath;

        // Load the config file asynchronously
        StartCoroutine(LoadConfigFromFile(configPath));
    }

    private IEnumerator LoadConfigFromFile(string path)
    {
        using (var www = new UnityEngine.Networking.UnityWebRequest(path))
        {
            www.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                string config = www.downloadHandler.text;

                try
                {
                    // Extract the JSON part of the file
                    string jsonConfig = ExtractFirebaseConfig(config);
                    Debug.Log("Parsed Firebase Config JSON: " + jsonConfig);

                    // Use postMessage to send config to JavaScript
                    SendMessageToBrowser(jsonConfig);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Failed to parse Firebase config: " + e.Message);
                }
            }
            else
            {
                Debug.LogError($"Failed to load Firebase config from {path}: {www.error}");
            }
        }
    }

    private string ExtractFirebaseConfig(string config)
    {
        // Extract the JSON part of the google-services.js file
        int startIndex = config.IndexOf("{", config.IndexOf("const firebaseConfig ="));
        int endIndex = config.LastIndexOf("}");

        if (startIndex == -1 || endIndex == -1 || startIndex >= endIndex)
        {
            throw new System.Exception("Invalid Firebase config file format.");
        }

        // Return the JSON substring
        return config.Substring(startIndex, endIndex - startIndex + 1);
    }


    private void SendMessageToBrowser(string jsonConfig)
    {
        string escapedConfig = jsonConfig.Replace("\"", "\\\""); // Escape quotes for JavaScript
        string jsCode = $"window.postMessage({escapedConfig}, '*');";
        Debug.Log("Sending to browser: " + jsCode);
        Application.ExternalEval(jsCode);
    }
}
