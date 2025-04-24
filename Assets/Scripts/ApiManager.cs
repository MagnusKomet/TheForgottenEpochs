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
    public Text playerName;
    public Text highScoreText;
    public Text successState;
    public InputField punts;
    public InputField nom;
    public Canvas canvas;
    public GameObject row;
    List<GameObject> rows;

    public TMP_InputField username;
    public TMP_InputField password;

    private string apiUrl = "https://localhost:44351/api/highscores";
    //private HighScoreStruct highScoreStruct;

    //Mètode per ser clicable pel botó CallAPI
    public void CallAPISerial()
    {
        StartCoroutine(GetRequest(apiUrl, LoadJsonDataCallBack));
    }
    //Mètode per ser clicable per Guardar el récord
    public void SaveHighScoreAPISerial()
    {
        StartCoroutine(Upload(apiUrl));
    }

    //Mètode per ser clicable per eliminar l'últime element
    public void RemoveByNameAPISerial()
    {
        StartCoroutine(RemoveByPlayerName(apiUrl));
    }

    //Mètode per fer la petició a la API
    private IEnumerator GetRequest(string url, Action<string> callback)
    {
        string response;
        ClearHighscores();

        UnityWebRequest www = UnityWebRequest.Get(url);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();


        if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            response = null;
        }
        else if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            response = null;
        }
        else
        {
            response = www.downloadHandler.text;

        }

        callback(response);
    }

    //Mètode per afegit un nou rècord
    IEnumerator Upload(string url)
    {
        //Creem un nou objecte record amb els paràmetres dels camps Input corresponents
        HighScoreModel highScore = new HighScoreModel();
        highScore.HighScore = int.Parse(punts.text);
        highScore.PlayerName = nom.text;

        //Transformem el nostre objecte a JSON
        string body = JsonConvert.SerializeObject(highScore);

        Debug.Log(body);

        //Fem la petició http del tipus POST adjuntant el valor de body com a aparàmetre
        using (UnityWebRequest www = UnityWebRequest.Post(url, body, "application/json"))
        {
            //Fem la petició asíncrona
            yield return www.SendWebRequest();

            //Si ha donat error mostrarem l'error per consola
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            //Si no hi ha hagut error
            else
            {
                Debug.Log("Form upload complete!");
                //Refresquem el llistat de HighScores
                CallAPISerial();
            }
        }
    }

    IEnumerator RemoveByPlayerName(string url)
    {
        //Fem la petició http del tipus DELETE 
        using (UnityWebRequest www = UnityWebRequest.Delete(url + "/" + nom.text))
        {
            Debug.Log(url);
            //Fem la petició asíncrona
            yield return www.SendWebRequest();

            //Si ha donat error mostrarem l'error per consola
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            //Si no hi ha hagut error
            else
            {
                Debug.Log("Form upload complete!");
                //Refresquem la llista de HighScores
                CallAPISerial();
            }

        }
    }


    //Mètode per extreure la informació de un JSON i transformar-ho a objecte HighScoreModel
    private void LoadJsonDataCallBack(string res)
    {
        //Si tenim resposta del server
        if (res != null)
        {
            //Creem la llista amb les files
            rows = new List<GameObject>();
            //Transformem el JSON a un Llistat de HighScoreModels
            var itemsData = JsonConvert.DeserializeObject<List<HighScoreModel>>(res);
            Debug.Log(itemsData.Count);
            //Guardem quants registres tenim;

            //Recorrem el llistat de HighScore i per cada un afegirem una fila al llistat de HighScores
            for (int i = 0; i < itemsData.Count; i++)
            {
                Debug.Log(itemsData[i]);
                Vector3 nova = new Vector3(-200, -100 * (i + 1), 0);

                //Instanciem una nova fila
                GameObject novaFila = Instantiate(row, canvas.transform.position + nova, canvas.transform.rotation) as GameObject;
                //Afegim la novaFila a la colecció de files.
                rows.Add(novaFila);
                //Afegim la nova fila al Canvas
                novaFila.transform.SetParent(canvas.transform, false);

                //Recorrem el prefab de row per trobar els elements TEXT per posar el valor registrat al objecte Highscore
                foreach (Transform g in novaFila.transform.GetComponentsInChildren<Transform>())
                {
                    if (g.name.Equals("HighScore"))
                    {
                        g.gameObject.GetComponent<Text>().text = itemsData[i].HighScore.ToString();
                    }
                    if (g.name.Equals("PlayerName"))
                    {
                        g.gameObject.GetComponent<Text>().text = itemsData[i].PlayerName;

                    }

                    //Debug.Log(g.name);
                }
            }
            //Si ha anat bé mostrarem de color verd que la connexió ha sigut correcta.
            successState.color = Color.green;
            successState.text = "Success!";
        }
        else
        {
            //Si ha anat malament mostrarem de color vermell que la connexió ha fallat.
            successState.color = Color.red;
            successState.text = "Fail!";
        }
    }


    private void ClearHighscores()
    {
        if (rows != null)
        {
            foreach (GameObject g in rows)
            {
                Destroy(g);
            }
        }
    }


    //Necessitem definir l'objecte com a serialitzable
    [System.Serializable]
    //Definició de la classe HighScoreModel
    public class HighScoreModel
    {
        public String _id { get; set; }
        public String PlayerName { get; set; }
        public int HighScore { get; set; }
    }
}
