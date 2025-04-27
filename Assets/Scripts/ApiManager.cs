using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;

public class ApiManager : MonoBehaviour
{
    public TMP_InputField userIdInput;
    public TMP_InputField dataInput;
    public Text successState;

    private string grimoiresApiUrl = "https://localhost:44351/api/grimoires";
    private string inventoriesApiUrl = "https://localhost:44351/api/inventories";
    private string exhibitsApiUrl = "https://localhost:44351/api/exhibits";

    // Método para obtener Grimoires por userId
    public void GetGrimoires()
    {
        int userId = int.Parse(userIdInput.text);
        StartCoroutine(GetRequest($"{grimoiresApiUrl}/{userId}", HandleGrimoiresResponse));
    }

    // Método para actualizar Grimoires
    public void UpdateGrimoires()
    {
        int userId = int.Parse(userIdInput.text);
        List<string> spellNames = JsonConvert.DeserializeObject<List<string>>(dataInput.text);
        StartCoroutine(PutRequest($"{grimoiresApiUrl}/{userId}", spellNames));
    }

    // Método para obtener Inventories por userId
    public void GetInventories()
    {
        int userId = int.Parse(userIdInput.text);
        StartCoroutine(GetRequest($"{inventoriesApiUrl}/{userId}", HandleInventoriesResponse));
    }

    // Método para actualizar Inventories
    public void UpdateInventories()
    {
        int userId = int.Parse(userIdInput.text);
        List<ItemQuantity> newInventory = JsonConvert.DeserializeObject<List<ItemQuantity>>(dataInput.text);
        StartCoroutine(PutRequest($"{inventoriesApiUrl}/{userId}", newInventory));
    }

    // Método para obtener Exhibits por userId
    public void GetExhibits()
    {
        int userId = int.Parse(userIdInput.text);
        StartCoroutine(GetRequest($"{exhibitsApiUrl}/{userId}", HandleExhibitsResponse));
    }

    // Método para actualizar Exhibits
    public void UpdateExhibits()
    {
        int userId = int.Parse(userIdInput.text);
        List<Exhibit> newExhibits = JsonConvert.DeserializeObject<List<Exhibit>>(dataInput.text);
        StartCoroutine(PutRequest($"{exhibitsApiUrl}/{userId}", newExhibits));
    }

    // Método genérico para realizar una solicitud GET
    private IEnumerator GetRequest(string url, Action<string> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            callback(www.downloadHandler.text);
        }
        else
        {
            Debug.LogError(www.error);
            successState.color = Color.red;
            successState.text = "Fail!";
        }
    }

    // Método genérico para realizar una solicitud PUT
    private IEnumerator PutRequest<T>(string url, T data)
    {
        string body = JsonConvert.SerializeObject(data);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(body);

        UnityWebRequest www = new UnityWebRequest(url, "PUT");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Update successful!");
            successState.color = Color.green;
            successState.text = "Success!";
        }
        else
        {
            Debug.LogError(www.error);
            successState.color = Color.red;
            successState.text = "Fail!";
        }
    }

    // Manejo de la respuesta de Grimoires
    private void HandleGrimoiresResponse(string response)
    {
        List<string> grimoires = JsonConvert.DeserializeObject<List<string>>(response);
        Debug.Log($"Grimoires: {string.Join(", ", grimoires)}");
    }

    // Manejo de la respuesta de Inventories
    private void HandleInventoriesResponse(string response)
    {
        List<ItemQuantity> inventory = JsonConvert.DeserializeObject<List<ItemQuantity>>(response);
        Debug.Log($"Inventory: {JsonConvert.SerializeObject(inventory)}");
    }

    // Manejo de la respuesta de Exhibits
    private void HandleExhibitsResponse(string response)
    {
        List<Exhibit> exhibits = JsonConvert.DeserializeObject<List<Exhibit>>(response);
        Debug.Log($"Exhibits: {JsonConvert.SerializeObject(exhibits)}");
    }

    // Clases auxiliares
    [System.Serializable]
    public class ItemQuantity
    {
        public string itemName { get; set; }
        public int quantity { get; set; }
    }

    [System.Serializable]
    public class Exhibit
    {
        public string exhibitPosition { get; set; }
        public string invokedMimic { get; set; }
    }
}
