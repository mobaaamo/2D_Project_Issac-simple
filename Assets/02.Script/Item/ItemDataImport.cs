using System.Collections;
using System.IO;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class ItemDataImport : EditorWindow
{
    public string csvURL = "https://docs.google.com/spreadsheets/d/1D5XoMddGyAS8iof8kiuII3nyqC1Acl5ET0FhfaDJfYI/export?format=csv";
    private string savePath = "Assets/Data/Items";

    [MenuItem("Tools/Import Item Data From Google Sheets")]

    public static void ShowWindow()
    {
        GetWindow(typeof(ItemDataImport), false, "Item CSV Importer");
    }
    private void OnGUI()
    {
        GUILayout.Label("Google Sheet CSV URL", EditorStyles.boldLabel);
        csvURL = EditorGUILayout.TextField(csvURL);
        if (GUILayout.Button("Download and Generate SO"))
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(ImportSCV());
        }
    }
    IEnumerator ImportSCV()
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        UnityWebRequest www = UnityWebRequest.Get(csvURL);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            yield break;
        }
        string[] lines = www.downloadHandler.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = lines[i].Split(',');

            ItemDataSO item = ScriptableObject.CreateInstance<ItemDataSO>();
            item.ID = int.Parse(values[0]);
            item.itemName = values[1];
            item.type = (ItemType)System.Enum.Parse(typeof(ItemType), values[2]);
            item.power = int.Parse(values[3]); 
            item.description = values[4]; 

            string assetPath = $"{savePath}/Item_{item.ID}_{item.itemName}.asset";

            AssetDatabase.CreateAsset(item, assetPath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}


