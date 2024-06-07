// Copyright (c) Antonio Di Dia. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

public partial class ChatG : System.Web.UI.Page
{
    //chiave API per l'accesso all'API di OpenAI
    private const string apiKey = "insert here your api key";

    //URL dell'endpoint dell'API di OpenAI
    private const string apiUrl = "https://api.openai.com/v1/chat/completions";

    protected void Page_Load(object sender, EventArgs e)
    {
        //puoi gestire l'inizializzazione della pagina qui se necessario
    }

    //evento chiamato quando il pulsante "Send" viene cliccato
    protected async void BtnSend_Click(object sender, EventArgs e)
    {
        //ottieni il messaggio dell'utente dalla textbox
        string userMessage = txtMessage.Text.Trim();

        if (!string.IsNullOrEmpty(userMessage))
        {

            //aggiungi il messaggio dell'utente al contenitore di testo della chat
            ltChat.Text += "You: " + userMessage + "<br />";

            //ottieni la risposta da ChatGPT
            string aiResponse = await GetChatGPTResponse(userMessage);

            //aggiungi la risposta di ChatGPT al contenitore di testo della chat
            ltChat.Text += "<strong>ChatGPT</strong>: " + aiResponse + "<br />";

            txtMessage.Text = "";

        }
    }

    //metodo per ottenere la risposta da ChatGPT
    private async Task<string> GetChatGPTResponse(string userMessage)
    {
        //crea una lista di messaggi da inviare all'API
        var listaMessaggi = new List<object>
        {
            new { role = "system", content = "You are a helpful assistant." },
            new { role = "user", content = userMessage }
        };

        //prepara i dati della richiesta
        var requestData = new
        {
            model = "gpt-3.5-turbo", //sostituisci con il modello specifico che desideri utilizzare
            messages = listaMessaggi
        };

        //serializza i dati della richiesta in formato JSON
        string jsonContent = SerializeToJson(requestData);

        try
        {
            //crea la richiesta HTTP
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + apiKey);

            //scrivi i dati JSON nel corpo della richiesta
            using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonContent);
            }

            //ottieni la risposta dall'API
            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    //leggi il contenuto della risposta come stringa
                    string result = await streamReader.ReadToEndAsync();
                    return EstraiMessaggio(result);
                }
            }
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                string errorResponse;
                using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    errorResponse = streamReader.ReadToEnd();
                }
                return "Error: " + ((HttpWebResponse)ex.Response).StatusCode + ", Detail: " + EstraiErrore(errorResponse) + "<br>";
            }
            else{
                //gestisci eventuali eccezioni e restituisci un messaggio di errore
                return "Exception: " + ex.Message + "<br>";
            }
        }
    }

    //serializza l’oggetto requestData in formato JSON.
    public static string SerializeToJson(object obj)
    {
        var serializer = new JavaScriptSerializer();
        return serializer.Serialize(obj);
    }

    //estrapola il messaggio dalla stringa Json
    public static string EstraiMessaggio(string stringaJson)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Dictionary<string, object> jsonObject = serializer.Deserialize<Dictionary<string, object>>(stringaJson);

        //controlla se la chiave "choices" esiste nel dizionario e se il valore associato è un array
        object choicesObj;
        if (jsonObject.TryGetValue("choices", out choicesObj) && choicesObj is object[])
        {
            //converti il valore associato alla chiave "choices" in un array di oggetti
            object[] choicesArray = (object[])choicesObj;

            //controlla se l'array contiene almeno un elemento
            if (choicesArray.Length > 0)
            {
                var choice = choicesArray[0] as Dictionary<string, object>;
                var messaggio = choice["message"] as Dictionary<string, object>;
                return messaggio["content"].ToString();
            }
            else{ return stringaJson; }
        }
        else
        {
            return stringaJson;
        }
    }

    //estrapola l'errore dalla stringa Json
    public static string EstraiErrore(string stringaJson)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Dictionary<string, object> jsonObject = serializer.Deserialize<Dictionary<string, object>>(stringaJson);

        var error = jsonObject["error"] as Dictionary<string, object>;
        string messaggio = error["message"].ToString();

        return messaggio;
    }
}
