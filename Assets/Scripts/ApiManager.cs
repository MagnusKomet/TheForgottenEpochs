using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using PlayerSpace;
using static ApiDataManager;

public class ApiDataManager : MonoBehaviour
{
    public TMP_InputField userInput; 
    public TMP_InputField passwordInput;

    private string userId; 

    private string spellsApiUrl = "https://localhost:44351/api/grimoires";
    private string inventoriesApiUrl = "https://localhost:44351/api/inventories";
    private string exhibitsApiUrl = "https://localhost:44351/api/exhibits";


    // ------------------- //
    //      Spells         //
    // ------------------- //

    #region Spells
    public async void SaveSpellsToApi()
    {
        HashSet<string> localSpells = LoadSpellsLocally();
        bool success = await UpdateSpellsAsync(localSpells);

        if (success)
        {
            Debug.Log($"Spells guardados en la API correctamente: {string.Join(", ", localSpells)}");
        }
        else
        {
            Debug.LogError("Error al guardar los Spells en la API.");
        }
    }

    public async void LoadSpellsFromApi()
    {
        List<string> spellsFromApi = await GetSpellsAsync();
        if (spellsFromApi != null)
        {
            SaveSpellsLocally(new HashSet<string>(spellsFromApi));
            Debug.Log($"Spells cargados desde la API correctamente: {string.Join(", ", spellsFromApi)}");
        }
        else
        {
            Debug.LogError("Error al cargar los Spells desde la API.");
        }
    }

    private void SaveSpellsLocally(HashSet<string> spells)
    {
        string spellsString = string.Join("-", spells);
        PlayerPrefs.SetString("UnlockedSpells", spellsString);
        PlayerPrefs.Save();
        Debug.Log($"Spells guardados localmente: {string.Join(", ", spells)}");
    }

    private HashSet<string> LoadSpellsLocally()
    {
        if (PlayerPrefs.HasKey("UnlockedSpells"))
        {
            string spellsString = PlayerPrefs.GetString("UnlockedSpells");
            Debug.Log($"Spells cargados localmente: {spellsString.Replace("-", ", ")}");
            return new HashSet<string>(spellsString.Split('-'));
        }
        else
        {
            Debug.LogWarning("No se encontraron Spells guardados localmente. Usando valores predeterminados.");
            return new HashSet<string>() { "F", "A", "E", "W" };
        }
    }

    private async Task<List<string>> GetSpellsAsync()
    {
        string url = $"{spellsApiUrl}/{userId}";
        string response = await GetRequestAsync(url);
        return response != null ? JsonConvert.DeserializeObject<List<string>>(response) : null;
    }

    private async Task<bool> UpdateSpellsAsync(HashSet<string> spells)
    {
        return await PutRequestAsync($"{spellsApiUrl}/{userId}", spells.ToList());
    }
    #endregion

    // ------------------- //
    //      Inventory      //
    // ------------------- //

    #region Inventory
    public async void SaveInventoryToApi()
    {
        List<InventoryItem> localInventory = LoadInventoryLocally();
        List<ItemQuantity> inventoryToSend = localInventory.Select(item => new ItemQuantity
        {
            itemName = item.itemName,
            quantity = item.quantity
        }).ToList();

        bool success = await UpdateInventoryAsync(inventoryToSend);

        if (success)
        {
            Debug.Log($"Inventario guardado en la API correctamente: {JsonConvert.SerializeObject(inventoryToSend)}");
        }
        else
        {
            Debug.LogError("Error al guardar el Inventario en la API.");
        }
    }

    public async void LoadInventoryFromApi()
    {
        List<InventoryItem> inventoryFromApi = await GetInventoryAsync();
        if (inventoryFromApi != null)
        {
            SaveInventoryLocally(inventoryFromApi);
            Debug.Log($"Inventario cargado desde la API correctamente: {JsonConvert.SerializeObject(inventoryFromApi)}");
        }
        else
        {
            Debug.LogError("Error al cargar el Inventario desde la API.");
        }
    }

    private void SaveInventoryLocally(List<InventoryItem> inventory)
    {
        string json = JsonUtility.ToJson(new ItemSerializableList<InventoryItem>(inventory));
        PlayerPrefs.SetString("InventoryData", json);
        PlayerPrefs.Save();
        Debug.Log($"Inventario guardado localmente: {JsonConvert.SerializeObject(inventory)}");
    }

    private List<InventoryItem> LoadInventoryLocally()
    {
        if (PlayerPrefs.HasKey("InventoryData"))
        {
            string json = PlayerPrefs.GetString("InventoryData");
            ItemSerializableList<InventoryItem> inventoryDataArray = JsonUtility.FromJson<ItemSerializableList<InventoryItem>>(json);
            Debug.Log($"Inventario cargado localmente: {JsonConvert.SerializeObject(inventoryDataArray.items)}");
            return inventoryDataArray.items;
        }
        else
        {
            Debug.LogWarning("No se encontró Inventario guardado localmente. Usando una lista vacía.");
            return new List<InventoryItem>();
        }
    }

    private async Task<List<InventoryItem>> GetInventoryAsync()
    {
        string url = $"{inventoriesApiUrl}/{userId}";
        string response = await GetRequestAsync(url);
        return response != null ? JsonConvert.DeserializeObject<List<InventoryItem>>(response) : null;
    }

    private async Task<bool> UpdateInventoryAsync(List<ItemQuantity> inventory)
    {
        return await PutRequestAsync($"{inventoriesApiUrl}/{userId}", inventory);
    }
    #endregion

    // ------------------- //
    //      Exhibits       //
    // ------------------- //

    #region Exhibits
    public async void SaveExhibitsToApi()
    {
        List<Exhibit> localExhibits = LoadAllExhibitsLocally();
        foreach (var exhibit in localExhibits)
        {
            exhibit.exhibitPosition = exhibit.exhibitPosition.Replace("Exhibit", "");
        }
        bool success = await UpdateExhibitsAsync(localExhibits);

        if (success)
        {
            Debug.Log($"Exhibits guardados en la API correctamente: {JsonConvert.SerializeObject(localExhibits)}");
        }
        else
        {
            Debug.LogError("Error al guardar los Exhibits en la API.");
        }
    }

    public async void LoadExhibitsFromApi()
    {
        List<Exhibit> exhibitsFromApi = await GetExhibitsAsync();
        if (exhibitsFromApi != null)
        {
            foreach (var exhibit in exhibitsFromApi)
            {

                SaveExhibitLocally("Exhibit" + exhibit.exhibitPosition, exhibit.invokedMimic);
            }
            Debug.Log($"Exhibits cargados desde la API correctamente: {JsonConvert.SerializeObject(exhibitsFromApi)}");
        }
        else
        {
            Debug.LogError("Error al cargar los Exhibits desde la API.");
        }
    }

    private void SaveExhibitLocally(string exhibitNumber, string mimicName)
    {
        PlayerPrefs.SetString(exhibitNumber, mimicName);
        PlayerPrefs.Save();
        Debug.Log($"Exhibit guardado localmente: {exhibitNumber} -> {mimicName}");
    }

    private string LoadExhibitLocally(string exhibitNumber)
    {
        if (PlayerPrefs.HasKey(exhibitNumber))
        {
            string mimicName = PlayerPrefs.GetString(exhibitNumber);
            Debug.Log($"Exhibit cargado localmente: {exhibitNumber} -> {mimicName}");
            return mimicName;
        }
        else
        {
            Debug.LogWarning($"No se encontró Exhibit guardado localmente para: {exhibitNumber}");
            return string.Empty;
        }
    }

    private List<Exhibit> LoadAllExhibitsLocally()
    {
        List<Exhibit> exhibits = new List<Exhibit>();
        for (int i = 1; i <= 6; i++)
        {
            string exhibitNumber = $"Exhibit{i}";
            string mimicName = LoadExhibitLocally(exhibitNumber);
            if (!string.IsNullOrEmpty(mimicName))
            {
                exhibits.Add(new Exhibit { exhibitPosition = exhibitNumber, invokedMimic = mimicName });
            }
        }
        Debug.Log($"Todos los Exhibits cargados localmente: {JsonConvert.SerializeObject(exhibits)}");
        return exhibits;
    }

    private async Task<List<Exhibit>> GetExhibitsAsync()
    {
        string url = $"{exhibitsApiUrl}/{userId}";
        string response = await GetRequestAsync(url);
        return response != null ? JsonConvert.DeserializeObject<List<Exhibit>>(response) : null;
    }

    private async Task<bool> UpdateExhibitsAsync(List<Exhibit> exhibits)
    {
        return await PutRequestAsync($"{exhibitsApiUrl}/{userId}", exhibits);
    }
    #endregion

    // ------------------- //
    //        Users        //
    // ------------------- //

    #region Users
    public async void Login()
    {
        string username = userInput.text;
        string password = passwordInput.text;

        var userCredentials = new { Username = username, Password = password };
        string jsonData = JsonConvert.SerializeObject(userCredentials);

        using (UnityWebRequest www = new UnityWebRequest("https://localhost:44351/api/users/login", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            var operation = www.SendWebRequest();
            while (!operation.isDone)
                await Task.Yield();

            if (www.result == UnityWebRequest.Result.Success)
            {
                userId = www.downloadHandler.text; // Almacenar el ID del usuario autenticado
                Debug.Log($"Login exitoso. User ID: {userId}");
            }
            else
            {
                Debug.LogError($"Error en el login: {www.error}");
            }
        }
    }

    public async void Register()
    {
        string username = userInput.text;
        string password = passwordInput.text;

        var userCredentials = new { Username = username, Password = password };
        string url = "https://localhost:44351/api/users/register";

        // Usar PutRequestAsync para registrar al usuario
        bool success = await PutRequestAsync(url, userCredentials);

        if (success)
        {
            Debug.Log("Registro exitoso.");
            // Realizar login automáticamente después del registro
            Login();
        }
        else
        {
            Debug.LogError("Error en el registro.");
        }
    }
    #endregion


    // ------------------- //
    //        Resto        //
    // ------------------- //

    #region Resto
    public void SaveAllToApi()
    {
        SaveSpellsToApi();
        SaveInventoryToApi();
        SaveExhibitsToApi();
    }

    public void LoadAllFromApi()
    {
        LoadSpellsFromApi();
        LoadInventoryFromApi();
        LoadExhibitsFromApi();
    }
    private async Task<string> GetRequestAsync(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            var operation = www.SendWebRequest();
            while (!operation.isDone)
                await Task.Yield();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Respuesta recibida: {www.downloadHandler.text}");
                return www.downloadHandler.text;
            }
            else
            {
                Debug.LogError($"Error en la solicitud GET: {www.error}");
                return null;
            }
        }
    }

    private async Task<bool> PutRequestAsync<T>(string url, T data)
    {
        string body = JsonConvert.SerializeObject(data);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(body);

        using (UnityWebRequest www = new UnityWebRequest(url, "PUT"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            var operation = www.SendWebRequest();
            while (!operation.isDone)
                await Task.Yield();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Datos enviados correctamente: {body}");
                return true;
            }
            else
            {
                Debug.LogError($"Error en la solicitud PUT: {www.error}");
                return false;
            }
        }
    }

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

    [System.Serializable]
    public class ItemSerializableList<T>
    {
        public List<T> items;

        public ItemSerializableList(List<T> items)
        {
            this.items = items;
        }
    }
    #endregion
}
