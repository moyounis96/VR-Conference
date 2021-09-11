using Paroxe.PdfRenderer;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Michsky.UI.ModernUIPack;
using UnityEngine.EventSystems;

public class Loader : MonoBehaviour
{
    public static Loader Instance;
    public Transform xRig;
    public List<string> questions;
    public GameObject questionCard, addQuestionCard;
    public Transform questionsParent;
    public UITransition fade;
    public UITransition levelPanel, importPanel, questionsPanel, levelsPanel, onScreenQuestion;
    public TextMeshProUGUI onScreenQuestionText;
    public PDFViewer pdfViewer;
    public string pdfPath = "";
    private Transform addQuestionTransform;
    private string currentEditingQuestion;
    public List<HeadCanvas> heads;
    private int questionsIndex = 0;
    public AudioClip clappingEffect;
    private void Awake()
    {
        if (!Instance) {
            DontDestroyOnLoad (transform.parent.gameObject);
            Instance = this;
            SceneManager.sceneLoaded += delegate {
                fade.Invoke ("Hide", 0.5f);
            };
            questions = new List<string> ();
            string questionsPref = PlayerPrefs.GetString ("Questions", "");
            if (!string.IsNullOrEmpty (questionsPref)) {
                questions = new List<string> (questionsPref.Split ('|'));
            }
            foreach (Transform child in questionsParent) {
                Destroy (child);
            }
            foreach (string question in questions) {
                AddQuestion (question);
            }
            addQuestionTransform = Instantiate (addQuestionCard, questionsParent).transform;
            addQuestionTransform.Find ("AddQuestion").GetComponent<Button> ().onClick.AddListener (() => {
                string question = addQuestionTransform.GetComponentInChildren<TMP_InputField> ().text;
                AddNewQuestion (question);
            });
            addQuestionTransform.GetComponentInChildren<TMP_InputField> ().onSubmit.AddListener (AddNewQuestion);
            questionsPanel.onShown.AddListener (delegate {
                ResetScroll ();
            });
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void ResetScroll () {
        questionsPanel.GetComponentInChildren<ScrollRect> ().verticalNormalizedPosition = 0;
        EventSystem.current.SetSelectedGameObject (null);
    }

    private void AddNewQuestion (string question) {
        if (!string.IsNullOrEmpty (question)) {
            questions.Add (question);
            AddQuestion (question);
            addQuestionTransform.GetComponentInChildren<TMP_InputField> ().text = "";
            Invoke ("ResetScroll", 0.1f);
            addQuestionTransform.SetAsLastSibling ();
        }
    }

    private void AddQuestion (string question) {
        Transform qPref = Instantiate (questionCard, questionsParent).transform;
        qPref.GetComponentInChildren<TMP_InputField> ().text = question;
        qPref.Find ("DeleteQuestion").GetComponent<Button> ().onClick.AddListener (() =>
        {
            questions.Remove (question);
            Destroy (qPref.gameObject);
        });
        qPref.GetComponentInChildren<TMP_InputField> ().onSelect.AddListener ((string s) => {
            currentEditingQuestion = s;
        });
        qPref.GetComponentInChildren<TMP_InputField> ().onEndEdit.AddListener ((string s) => {
            if (currentEditingQuestion == s)
                return;
            if (string.IsNullOrEmpty (s)) {
                questions.Remove (currentEditingQuestion);
                Destroy (qPref.gameObject);
            } else {
                questions[questions.FindIndex (x => x == currentEditingQuestion)] = s;
            }
        });
    }
    public void ShowQuestion () {
        questionsIndex = UnityEngine.Random.Range (0, questions.Count);
        string question = questions[questionsIndex];
        if (heads.Count > 0) {
            heads[UnityEngine.Random.Range (0, heads.Count)].ShowQuestion (question);
        }
        onScreenQuestionText.text = question;
        onScreenQuestion.CancelInvoke ();
        onScreenQuestion.Show ();
        onScreenQuestion.Invoke ("Hide", 5f);
        Invoke("Clap", 3f);

    }
    void Clap()
    {
        GetComponent<AudioSource>().PlayOneShot(clappingEffect);
        foreach (NPC npc in FindObjectsOfType<NPC>())
        {
            if (UnityEngine.Random.Range(0, 100) <= 80)
                npc.Clap();
        }
    }
    public void SaveQuestions () {
        if (questions.Count > 0) {
            string prefs = questions[0];
            for (int i = 1; i < questions.Count; i++) {
                prefs += "|" + questions[i];
            }
            PlayerPrefs.SetString ("Questions", prefs);
        } else {
            PlayerPrefs.SetString ("Questions", "");
        }
        questionsPanel.Hide ();
        levelsPanel.Show ();
    }
    int index;
    public void LoadLevel(int index)
    {
        questionsIndex = 0;
        heads = new List<HeadCanvas> ();
        fade.Show();
        this.index = index;
        Invoke("LoadScene", fade.Duration() + 0.5f);
    }
    void LoadScene()
    {
        GetComponent<AudioSource> ().Stop ();
        xRig.position = CameraMove.initPos;
        xRig.rotation = CameraMove.initRot;
        levelPanel.Show();
        levelsPanel.Hide();
        questionsPanel.Hide();
        importPanel.Hide();
        pdfViewer.enabled = false;
        pdfViewer.FilePath = pdfPath;
        pdfViewer.enabled = true;
        SceneManager.LoadSceneAsync(index);
    }
    public void ShowMenu()
    {
        GetComponent<AudioSource> ().Play ();
        pdfViewer.enabled = false;
        levelPanel.Hide();
        levelsPanel.Hide();
        questionsPanel.Show();
        importPanel.Hide();
    }
    public void ImportPDF()
    {
        string filePath = "";
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        Ookii.Dialogs.VistaOpenFileDialog dialog = new Ookii.Dialogs.VistaOpenFileDialog();
        dialog.Title = "Choose your profile picture";
        dialog.DefaultExt = ".pdf";
        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            filePath = dialog.FileName;
        }
        pdfPath = filePath;
        pdfViewer.FilePath = pdfPath;
#elif UNITY_ANDROID || UNITY_IOS
        NativeFilePicker.PickFile((string path) =>
        {
            filePath = path;
            pdfPath = filePath;
            pdfViewer.FilePath = pdfPath;
            Debug.Log(pdfPath);
        }, new string[1] { "application/pdf" });
#endif
        levelPanel.Hide();
        levelsPanel.Hide();
        questionsPanel.Show();
        importPanel.Hide();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
