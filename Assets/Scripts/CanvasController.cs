using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class CanvasController : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private CanvasGroup initialCanvas;
    [SerializeField] private float fadeDuration = 0.3f;

    // Pilha para armazenar o histórico de menus
    private Stack<CanvasGroup> menuHistory = new Stack<CanvasGroup>();
    private CanvasGroup currentCanvas;

    void Start()
    {
        // Inicializa o menu principal se definido
        if (initialCanvas != null)
        {
            ShowMenu(initialCanvas, false);
        }
    }

    void Update()
    {
        // Detecta o botão "Voltar" do Android (Escape no PC)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    /// <summary>
    /// Abre um novo menu e o adiciona ao histórico.
    /// Chame esta função via Botão no Inspector.
    /// </summary>
    public void OpenMenu(CanvasGroup nextCanvas)
    {
        if (nextCanvas == null || nextCanvas == currentCanvas) return;

        if (currentCanvas != null)
        {
            menuHistory.Push(currentCanvas);
            StartCoroutine(FadeCanvas(currentCanvas, 0, false));
        }

        currentCanvas = nextCanvas;
        StartCoroutine(FadeCanvas(currentCanvas, 1, true));
    }

    /// <summary>
    /// Volta para o menu anterior na pilha de histórico.
    /// </summary>
    public void GoBack()
    {
        if (menuHistory.Count > 0)
        {
            StartCoroutine(FadeCanvas(currentCanvas, 0, false));

            currentCanvas = menuHistory.Pop();
            StartCoroutine(FadeCanvas(currentCanvas, 1, true));
        }
        else
        {
            UnityEngine.Application.Quit();
        }
    }

    public void OpenInstagram()
    {
        string url = "https://www.instagram.com/perfil_do_seu_jogo";
        UnityEngine.Application.OpenURL(url);
    }


    /// <summary>
    /// Atalho para mostrar um menu sem adicionar ao histórico (útil no Start).
    /// </summary>
    public void ShowMenu(CanvasGroup menu, bool addToHistory)
    {
        if (currentCanvas != null && !addToHistory)
            StartCoroutine(FadeCanvas(currentCanvas, 0, false));

        currentCanvas = menu;
        StartCoroutine(FadeCanvas(currentCanvas, 1, true));
    }

    private IEnumerator FadeCanvas(CanvasGroup cg, float targetAlpha, bool isOpening)
    {
        float startAlpha = cg.alpha;
        float time = 0;

        if (isOpening)
        {
            cg.gameObject.SetActive(true);
            cg.blocksRaycasts = true;
            cg.interactable = true;
        }
        else
        {
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        cg.alpha = targetAlpha;

        if (!isOpening)
        {
            cg.gameObject.SetActive(false);
        }
    }
}