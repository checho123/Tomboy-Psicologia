using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Emotional Model")]
    [SerializeField] private EmotionalRoot emotionalRoot = new();
    private const string FileEmotional = "File/Buga";

    [Header("Dialogue UI")]
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private GameObject panelButtons;
    [SerializeField] private Transform buttonContent;

    [Header("Emotional Prefab")]
    [SerializeField] private GameObject prefabEmotional;
    [SerializeField] private List<SelectionEmotional> emotionals = new();

    [Header("Typing Settings")]
    [SerializeField, Range(3, 100)] private int maxWordsPerPage = 18;
    [SerializeField, Range(0.005f, 0.2f)] private float charDelay = 0.03f;
    [SerializeField, Range(1f, 10f)] private float shortPauseFactor = 3f;
    [SerializeField, Range(1f, 15f)] private float longPauseFactor = 6f;
    [SerializeField] private bool useRealtime = true;

    [System.Serializable]
    public class SelectionEmotional
    {
        public string nameEmotional;
        public EmotionalState state;
        public Sprite iconEmotional;
    }

    // Estado interno
    private List<string> _pages = new();
    private int _pageIndex = 0;
    private Coroutine _typing;

    private void Awake()
    {
        EmotionaFileLoad();
        BuildButtons();
        if (panelButtons != null) panelButtons.SetActive(false);
    }

    private void OnDisable()
    {
        if (_typing != null) { StopCoroutine(_typing); _typing = null; }
    }

    private void Update()
    {
        // Solo responde a input si el texto está visible en pantalla
        if (speakerText != null && speakerText.gameObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0))
            {
                ContinueOrShowButtons(); // misma lógica: si tipa → revela; si revelado → avanza
            }
        }
    }

    // ---------- Carga JSON ----------
    private void EmotionaFileLoad()
    {
        TextAsset textFile = Resources.Load<TextAsset>(FileEmotional);
        if (textFile == null)
        {
            Debug.LogError($"[DialogueManager] No se encontró '{FileEmotional}' como TextAsset.");
            return;
        }

        try
        {
            emotionalRoot = Servicies.DeserializeFromJson<EmotionalRoot>(textFile.text);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error parseando JSON de '{FileEmotional}': {e.Message}");
        }
    }

    // ---------- API: abrir diálogo con línea por ID ----------
    public void StartDialogueById(int id)
    {
        if (panelButtons != null) panelButtons.SetActive(false);

        string full = GetTextById(id);
        _pages = PaginateByWords(full, maxWordsPerPage);
        _pageIndex = 0;
        PlayCurrentPage();
    }

    // ---------- Avance manual: botón/enter/clic ----------
    public void ContinueOrShowButtons()
    {
        // Si aún está tipeando, completar la página al instante (revelar todo)
        if (_typing != null)
        {
            StopCoroutine(_typing);
            _typing = null;
            if (speakerText != null && _pages.Count > 0)
                speakerText.text = _pages[_pageIndex];
            return;
        }

        // Si ya está revelada, pasa a la siguiente página o muestra botones al final
        if (_pageIndex < _pages.Count - 1)
        {
            _pageIndex++;
            PlayCurrentPage();
        }
        else
        {
            if (panelButtons != null) panelButtons.SetActive(true);
        }
    }

    // ---------- Construcción de botones ----------
    private void BuildButtons()
    {
        if (buttonContent == null || prefabEmotional == null) return;

        for (int i = buttonContent.childCount - 1; i >= 0; i--)
            Destroy(buttonContent.GetChild(i).gameObject);

        foreach (SelectionEmotional selection in emotionals)
        {
            GameObject go = Instantiate(prefabEmotional, buttonContent);
            go.name = selection.nameEmotional;

            TMP_Text label = go.GetComponentInChildren<TMP_Text>(true);
            if (label != null) label.text = selection.nameEmotional;

            Transform tSprite = go.transform.Find("Sprite");
            if (tSprite != null && selection.iconEmotional != null)
            {
                Image iconSelection = tSprite.GetComponent<Image>();
                if (iconSelection != null) iconSelection.sprite = selection.iconEmotional;
            }

            Button selectBtn = go.GetComponent<Button>();
            if (selectBtn != null)
            {
                var sel = selection; // captura local
                selectBtn.onClick.AddListener(() => OnSelectionEmotion(sel));
            }
        }
    }

    private void OnSelectionEmotion(SelectionEmotional emotional)
    {
        Debug.Log(emotional.state);
        // continúa tu flujo (guardar estado, cerrar panel, etc.)
    }

    // ---------- Tipeo & Paginado ----------
    private void PlayCurrentPage()
    {
        if (speakerText == null) return;
        if (_typing != null) StopCoroutine(_typing);

        string page = (_pages.Count > 0) ? _pages[_pageIndex] : string.Empty;
        _typing = StartCoroutine(TypeChars(page));
    }

    private IEnumerator TypeChars(string text)
    {
        if (speakerText == null) yield break;
        speakerText.text = string.Empty;

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            sb.Append(c);
            speakerText.text = sb.ToString();

            // pausa según puntuación
            float delay = charDelay;
            if (c == '.' || c == '!' || c == '?' || c == '\n')
                delay *= longPauseFactor;
            else if (c == ',' || c == ';' || c == ':')
                delay *= shortPauseFactor;

            if (useRealtime) yield return new WaitForSecondsRealtime(delay);
            else yield return new WaitForSeconds(delay);

            // si se canceló (porque el jugador presionó Enter/clic), salir
            if (_typing == null) yield break;
        }

        _typing = null; // terminó de escribir la página (no avanza solo)
    }

    private static List<string> PaginateByWords(string text, int maxWords)
    {
        var pages = new List<string>();
        if (maxWords <= 0) { pages.Add(text ?? ""); return pages; }
        if (string.IsNullOrWhiteSpace(text)) { pages.Add(""); return pages; }

        string[] words = text.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        int count = 0;
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < words.Length; i++)
        {
            if (count == maxWords)
            {
                pages.Add(sb.ToString());
                sb.Clear();
                count = 0;
            }
            if (sb.Length > 0) sb.Append(' ');
            sb.Append(words[i]);
            count++;
        }
        if (sb.Length > 0) pages.Add(sb.ToString());

        return pages;
    }

    private string GetTextById(int id)
    {
        var list = emotionalRoot?.Emotional?.Speaker;
        if (list == null || list.Count == 0) return string.Empty;

        for (int i = 0; i < list.Count; i++)
            if (list[i].id == id) return list[i].Text;

        return list[0].Text;
    }
}