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

    //M�tode per ser clicable pel bot� CallAPI
    public void CallAPISerial()
    {
        StartCoroutine(GetRequest(apiUrl, LoadJsonDataCallBack));
    }
    //M�tode per ser clicable per Guardar el r�cord
    public void SaveHighScoreAPISerial()
    {
        StartCoroutine(Upload(apiUrl));
    }

    //M�tode per ser clicable per eliminar l'�ltime element
    public void RemoveByNameAPISerial()
    {
        StartCoroutine(RemoveByPlayerName(apiUrl));
    }

    //M�tode per fer la petici� a la API
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

    //M�tode per afegit un nou r�cord
    IEnumerator Upload(string url)
    {
        //Creem un nou objecte record amb els par�metres dels camps Input corresponents
        HighScoreModel highScore = new HighScoreModel();
        highScore.HighScore = int.Parse(punts.text);
        highScore.PlayerName = nom.text;

        //Transformem el nostre objecte a JSON
        string body = JsonConvert.SerializeObject(highScore);

        Debug.Log(body);

        //Fem la petici� http del tipus POST adjuntant el valor de body com a apar�metre
        using (UnityWebRequest www = UnityWebRequest.Post(url, body, "application/json"))
        {
            //Fem la petici� as�ncrona
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
        //Fem la petici� http del tipus DELETE 
        using (UnityWebRequest www = UnityWebRequest.Delete(url + "/" + nom.text))
        {
            Debug.Log(url);
            //Fem la petici� as�ncrona
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


    //M�tode per extreure la informaci� de un JSON i transformar-ho a objecte HighScoreModel
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
                //Afegim la novaFila a la colecci� de files.
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
            //Si ha anat b� mostrarem de color verd que la connexi� ha sigut correcta.
            successState.color = Color.green;
            successState.text = "Success!";
        }
        else
        {
            //Si ha anat malament mostrarem de color vermell que la connexi� ha fallat.
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
    //Definici� de la classe HighScoreModel
    public class HighScoreModel
    {
        public String _id { get; set; }
        public String PlayerName { get; set; }
        public int HighScore { get; set; }
    }
}
