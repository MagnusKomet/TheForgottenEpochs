using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using PlayerSpace;
using System;
using System.Text;

public class ApiDataManager : MonoBehaviour
{
    public TMP_InputField userInput;
    public TMP_InputField passwordInput;

    private string userToken;

    private string spellsApiUrl = "http://theforgottenepochsapi.somee.com/API/api/grimoires/token";
    private string inventoriesApiUrl = "http://theforgottenepochsapi.somee.com/API/api/inventories/token";
    private string exhibitsApiUrl = "http://theforgottenepochsapi.somee.com/API/api/exhibits/token";

    public void Start()
    {
        if (IsTokenValid())
        {
            userToken = PlayerPrefs.GetString("SecureToken");
        }
    }

    // ------------------- //
    //      Spells         //
    // ------------------- //

    #region Spells
    public async Task<bool> SaveSpellsToApi()
    {
        
        try
        {
            HashSet<string> localSpells = LoadSpellsLocally();
            return await UpdateSpellsAsync(localSpells);
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> LoadSpellsFromApi()
    {
        
        try
        {
            List<string> spellsFromApi = await GetSpellsAsync();
            if (spellsFromApi != null)
            {
                SaveSpellsLocally(new HashSet<string>(spellsFromApi));
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    private void SaveSpellsLocally(HashSet<string> spells)
    {
        string spellsString = string.Join("-", spells);
        PlayerPrefs.SetString("UnlockedSpells", spellsString);
        PlayerPrefs.Save();
    }

    private HashSet<string> LoadSpellsLocally()
    {
        if (PlayerPrefs.HasKey("UnlockedSpells"))
        {
            string spellsString = PlayerPrefs.GetString("UnlockedSpells");
            return new HashSet<string>(spellsString.Split('-'));
        }
        else
        {
            return new HashSet<string>() { "F", "A", "E", "W" };
        }
    }

    private async Task<List<string>> GetSpellsAsync()
    {
        string url = $"{spellsApiUrl}/{userToken}";
        string response = await GetRequestAsync(url);
        return response != null ? JsonConvert.DeserializeObject<List<string>>(response) : null;
    }

    private async Task<bool> UpdateSpellsAsync(HashSet<string> spells)
    {
        return await PutRequestAsync($"{spellsApiUrl}/{userToken}", spells.ToList());
    }
    #endregion

    // ------------------- //
    //      Inventory      //
    // ------------------- //

    #region Inventory
    public async Task<bool> SaveInventoryToApi()
    {
        
        try
        {
            List<InventoryItem> localInventory = LoadInventoryLocally();
            List<ItemQuantity> inventoryToSend = localInventory.Select(item => new ItemQuantity
            {
                itemName = item.itemName,
                quantity = item.quantity
            }).ToList();

            return await UpdateInventoryAsync(inventoryToSend);
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> LoadInventoryFromApi()
    {
        
        try
        {
            List<InventoryItem> inventoryFromApi = await GetInventoryAsync();
            if (inventoryFromApi != null)
            {
                SaveInventoryLocally(inventoryFromApi);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    private void SaveInventoryLocally(List<InventoryItem> inventory)
    {
        string json = JsonUtility.ToJson(new ItemSerializableList<InventoryItem>(inventory));
        PlayerPrefs.SetString("InventoryData", json);
        PlayerPrefs.Save();
    }

    private List<InventoryItem> LoadInventoryLocally()
    {
        if (PlayerPrefs.HasKey("InventoryData"))
        {
            string json = PlayerPrefs.GetString("InventoryData");
            ItemSerializableList<InventoryItem> inventoryDataArray = JsonUtility.FromJson<ItemSerializableList<InventoryItem>>(json);
            return inventoryDataArray.items;
        }
        else
        {
            return new List<InventoryItem>();
        }
    }

    private async Task<List<InventoryItem>> GetInventoryAsync()
    {
        string url = $"{inventoriesApiUrl}/{userToken}";
        string response = await GetRequestAsync(url);
        return response != null ? JsonConvert.DeserializeObject<List<InventoryItem>>(response) : null;
    }

    private async Task<bool> UpdateInventoryAsync(List<ItemQuantity> inventory)
    {
        return await PutRequestAsync($"{inventoriesApiUrl}/{userToken}", inventory);
    }
    #endregion

    // ------------------- //
    //      Exhibits       //
    // ------------------- //

    #region Exhibits
    public async Task<bool> SaveExhibitsToApi()
    {        
        try
        {
            List<Exhibit> localExhibits = LoadAllExhibitsLocally();
            foreach (var exhibit in localExhibits)
            {
                exhibit.exhibitPosition = exhibit.exhibitPosition.Replace("Exhibit", "");
            }
            return await UpdateExhibitsAsync(localExhibits);
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> LoadExhibitsFromApi()
    {
        
        try
        {
            List<Exhibit> exhibitsFromApi = await GetExhibitsAsync();
            if (exhibitsFromApi != null)
            {
                foreach (var exhibit in exhibitsFromApi)
                {
                    SaveExhibitLocally("Exhibit" + exhibit.exhibitPosition, exhibit.invokedMimic);
                }
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    private void SaveExhibitLocally(string exhibitNumber, string mimicName)
    {
        PlayerPrefs.SetString(exhibitNumber, mimicName);
        PlayerPrefs.Save();
    }

    private string LoadExhibitLocally(string exhibitNumber)
    {
        if (PlayerPrefs.HasKey(exhibitNumber))
        {
            string mimicName = PlayerPrefs.GetString(exhibitNumber);
            return mimicName;
        }
        else
        {
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

        return exhibits;
    }

    private async Task<List<Exhibit>> GetExhibitsAsync()
    {
        string url = $"{exhibitsApiUrl}/{userToken}";
        string response = await GetRequestAsync(url);
        return response != null ? JsonConvert.DeserializeObject<List<Exhibit>>(response) : null;
    }

    private async Task<bool> UpdateExhibitsAsync(List<Exhibit> exhibits)
    {
        return await PutRequestAsync($"{exhibitsApiUrl}/{userToken}", exhibits);
    }


    #endregion

    // ------------------- //
    //        Users        //
    // ------------------- //

    #region Users
    public async void Login()
    {
        ToastNotification.Show("Connecting to the server to log in...");
        string username = userInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ToastNotification.Show("Please enter a username and password.");
            return;
        }

        var userCredentials = new { Username = username, Password = password };
        string jsonData = JsonConvert.SerializeObject(userCredentials);

        using (UnityWebRequest www = new UnityWebRequest("http://theforgottenepochsapi.somee.com/API/api/users/login", "POST"))
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
                string responseText = www.downloadHandler.text;

                if (responseText != "Invalid username or password.")
                {
                    userToken = responseText;
                    SaveTokenWithExpiration(userToken);
                    ToastNotification.Show("Login successful.");
                }
                else
                {
                    ToastNotification.Show("Login failed: Invalid username or password.");
                }
            }
            else
            {
                ToastNotification.Show($"Error during login.");
            }
        }
    }

    public async void Register()
    {
        ToastNotification.Show("Connecting to the server to register...");
        string username = userInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ToastNotification.Show("Please enter a username and password.");
            return;
        }

        var userCredentials = new { Username = username, Password = password };
        string jsonData = JsonConvert.SerializeObject(userCredentials);

        using (UnityWebRequest www = new UnityWebRequest("http://theforgottenepochsapi.somee.com/API/api/users/register", "PUT"))
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
                string responseText = www.downloadHandler.text;

                // Verificar si el registro fue exitoso
                if (responseText != "Username already exists.")
                {
                    ToastNotification.Show("Registration successful.");
                }
                else
                {
                    ToastNotification.Show("Registration failed: Username already exists.");
                }
            }
            else
            {
                ToastNotification.Show($"Error during registration: {www.error}");
            }
        }
    }

    public static void SaveTokenWithExpiration(string token)
    {
        DateTime expiration = DateTime.UtcNow.AddMonths(1);

        PlayerPrefs.SetString("SecureToken", token);
        PlayerPrefs.SetString("TokenExpiration", expiration.ToString("o"));
        PlayerPrefs.Save();
    }

    public static bool IsTokenValid()
    {
        if (PlayerPrefs.HasKey("TokenExpiration"))
        {
            DateTime expiration = DateTime.Parse(PlayerPrefs.GetString("TokenExpiration"));
            return DateTime.UtcNow < expiration;
        }
        return false;
    }

    public static void LogOut()
    {
        PlayerPrefs.DeleteKey("SecureToken");
        PlayerPrefs.DeleteKey("TokenExpiration");
        PlayerPrefs.Save();
        ToastNotification.Show("User logged out successfully");
    }
    #endregion

    // ------------------- //
    //        Resto        //
    // ------------------- //

    #region Resto
    public async void SaveAllToApi()
    {
        ToastNotification.Show("Connecting to the server to save all data...");
        bool spellsSaved = await SaveSpellsToApi();
        bool inventorySaved = await SaveInventoryToApi();
        bool exhibitsSaved = await SaveExhibitsToApi();

        if (spellsSaved && inventorySaved && exhibitsSaved)
        {
            ToastNotification.Show("All data successfully saved to the server.");
        }
        else if (!spellsSaved && !inventorySaved && !exhibitsSaved)
        {
            if (string.IsNullOrEmpty(userToken))
            {
                ToastNotification.Show("Please go to settings and log in to save data.");
            }
            else
            {
                ToastNotification.Show("Connection to the server cannot be established");
            }
        }
        else
        {
            string errorMessage = "Some data could not be saved to the server:";
            if (!spellsSaved) errorMessage += " Spells";
            if (!inventorySaved) errorMessage += " Inventory";
            if (!exhibitsSaved) errorMessage += " Exhibits";
            ToastNotification.Show(errorMessage);
        }
    }

    public async void LoadAllFromApi()
    {
        ToastNotification.Show("Connecting to the server to load all data...");
        bool spellsLoaded = await LoadSpellsFromApi();
        bool inventoryLoaded = await LoadInventoryFromApi();
        bool exhibitsLoaded = await LoadExhibitsFromApi();

        if (spellsLoaded && inventoryLoaded && exhibitsLoaded)
        {
            ToastNotification.Show("All data successfully loaded from the server.");
        }
        else if (!spellsLoaded && !inventoryLoaded && !exhibitsLoaded)
        {
            if (string.IsNullOrEmpty(userToken))
            {
                ToastNotification.Show("Please go to settings and log in to load data.");
            }
            else
            {
                ToastNotification.Show("Connection to the server cannot be established");
            }
        }
        else
        {
            string errorMessage = "Some data could not be loaded from the server:";
            if (!spellsLoaded) errorMessage += " Spells";
            if (!inventoryLoaded) errorMessage += " Inventory";
            if (!exhibitsLoaded) errorMessage += " Exhibits";
            ToastNotification.Show(errorMessage);
        }
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
