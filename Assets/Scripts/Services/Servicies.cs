using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Servicio mínimo de red: solo GET y POST con JSON.
/// Uso:
///   StartCoroutine(Servicies.GetRequest("https://api.tuapp.com/ping", OnGet));
///   StartCoroutine(Servicies.PostRequest("https://api.tuapp.com/login", json, OnPost));
/// </summary>
public class Servicies : MonoBehaviour
{
    #region GET Request
    /// <summary>
    /// GET simple. Devuelve el texto de respuesta en el callback (o null si hay error).
    /// </summary>
    public static IEnumerator GetRequest(string url, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Tiempo máximo (segundos) para evitar corrutinas colgadas
            request.timeout = 30;

            // Envía la solicitud
            yield return request.SendWebRequest();

            // Manejo de error/un éxito
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[GET] Error: {request.error} | URL: {url} | Code: {request.responseCode}");
                callback?.Invoke(null);
                yield break;
            }

            // Respuesta correcta
            callback?.Invoke(request.downloadHandler.text);
        }
    }
    #endregion

    #region POST Request
    /// <summary>
    /// POST simple con cuerpo JSON (UTF-8).
    /// Devuelve el texto de respuesta en el callback (o null si hay error).
    /// </summary>
    public static IEnumerator PostRequest(string url, string json, Action<string> callback)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            // Cuerpo JSON
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Header estándar para JSON
            request.SetRequestHeader("Content-Type", "application/json");

            // Timeout de seguridad
            request.timeout = 30;

            // Envía la solicitud
            yield return request.SendWebRequest();

            // Manejo de error/un éxito
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[POST] Error: {request.error} | URL: {url} | Code: {request.responseCode}");
                callback?.Invoke(null);
                yield break;
            }

            // Respuesta correcta
            callback?.Invoke(request.downloadHandler.text);
        }
    }
    #endregion

    #region Setting JSON
    
    /// <summary>
    /// Objeto -> JSON (opcional prettyPrint).
    /// Devuelve null si ocurre un error.
    /// </summary>
    public static string SerializeToJson<T>(T obj, bool prettyPrint = false)
    {
        try
        {
            Formatting formatting = prettyPrint ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(obj, formatting);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SerializeToJson] {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// JSON -> Objeto tipado T.
    /// Devuelve default(T) si ocurre un error.
    /// </summary>
    public static T DeserializeFromJson<T>(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[DeserializeFromJson] {ex.Message}");
            return default(T);
        }
    }

    #endregion
}
