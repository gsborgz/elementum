using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class ListenerController : MonoBehaviour {

    [Header("Speech Recognition Settings")]
    [SerializeField] private ConfidenceLevel minimumConfidence = ConfidenceLevel.High;

    private KeywordRecognizer keywordRecognizer;
    protected readonly Dictionary<string, Action> keywords = new();

    public void StartListening()
    {
        if (keywords.Count == 0)
        {
            Debug.LogWarning("No keywords available to listen for!");
            return;
        }

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray(), minimumConfidence);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }

    public void StopListening()
    {
        keywordRecognizer.Stop();
    }

    private void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.confidence <= minimumConfidence)
        {
            if (keywords.TryGetValue(args.text, out Action keywordAction))
            {
                keywordAction.Invoke();
            }
        }
    }

    protected void RemoveKeyword(string keyword)
    {
        keywords.Remove(keyword);
    }

    private void OnDestroy()
    {
        keywords.Clear();

        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }

}
