using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ApiTest : MonoBehaviour
{
    public ApiManager apiManager;

    private void Start()
    {
        // Asegúrate de que el ApiManager esté asignado
        if (apiManager == null)
        {
            Debug.LogError("ApiManager no está asignado en el inspector.");
            return;
        }

        // Pruebas de los métodos del ApiManager
        TestGetGrimoires();
        TestUpdateGrimoires();
        TestGetInventories();
        TestUpdateInventories();
        TestGetExhibits();
        TestUpdateExhibits();
    }

    private void TestGetGrimoires()
    {
        Debug.Log("Probando GetGrimoires...");
        apiManager.userIdInput.text = "2"; // Simula un userId
        apiManager.GetGrimoires();
    }

    private void TestUpdateGrimoires()
    {
        Debug.Log("Probando UpdateGrimoires...");
        apiManager.userIdInput.text = "2"; // Simula un userId
        List<string> grimoires = new List<string> { "F", "A", "FFA" };
        apiManager.dataInput.text = JsonConvert.SerializeObject(grimoires); // Simula los datos de entrada
        apiManager.UpdateGrimoires();
    }

    private void TestGetInventories()
    {
        Debug.Log("Probando GetInventories...");
        apiManager.userIdInput.text = "2"; // Simula un userId
        apiManager.GetInventories();
    }

    private void TestUpdateInventories()
    {
        Debug.Log("Probando UpdateInventories...");
        apiManager.userIdInput.text = "2"; // Simula un userId
        List<ApiManager.ItemQuantity> inventory = new List<ApiManager.ItemQuantity>
        {
            new ApiManager.ItemQuantity { itemName = "Cristal de aire", quantity = 10 },
            new ApiManager.ItemQuantity { itemName = "Núcleo de Migu", quantity = 4 }
        };
        apiManager.dataInput.text = JsonConvert.SerializeObject(inventory); // Simula los datos de entrada
        apiManager.UpdateInventories();
    }

    private void TestGetExhibits()
    {
        Debug.Log("Probando GetExhibits...");
        apiManager.userIdInput.text = "2"; // Simula un userId
        apiManager.GetExhibits();
    }

    private void TestUpdateExhibits()
    {
        Debug.Log("Probando UpdateExhibits...");
        apiManager.userIdInput.text = "2"; // Simula un userId
        List<ApiManager.Exhibit> exhibits = new List<ApiManager.Exhibit>
        {
            new ApiManager.Exhibit { exhibitPosition = "1", invokedMimic = "Migu" },
            new ApiManager.Exhibit { exhibitPosition = "2", invokedMimic = "Hanabirkle" }
        };
        apiManager.dataInput.text = JsonConvert.SerializeObject(exhibits); // Simula los datos de entrada
        apiManager.UpdateExhibits();
    }
}
