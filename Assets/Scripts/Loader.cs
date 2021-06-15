using Paroxe.PdfRenderer;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class Loader : MonoBehaviour
{
    public VideoClip[] clips;
    public VideoPlayer player;
    public GameObject levelPanel, importPanel, levelsPanel, level5;
    public PDFViewer pdfViewer;
    public string pdfPath = "/storage/emulated/0/chickchack.pdf";
    public void LoadLevel(int index)
    {
        levelPanel.SetActive(true);
        levelsPanel.SetActive(false);
        importPanel.SetActive(false);
        if (index < clips.Length)
        {
            player.gameObject.SetActive(true);
            player.Stop();
            player.clip = clips[index];
            player.Play();
            pdfViewer.FilePath = pdfPath;
            pdfViewer.LoadDocumentFromFile(pdfPath);
        }
        else if(index == 4)
        {
            player.gameObject.SetActive(false);
            level5.SetActive(true);
        }
        else
        {
            levelPanel.SetActive(false);
            levelsPanel.SetActive(true);
            importPanel.SetActive(false);
        }
    }
    public void ShowMenu()
    {
        player.Stop();
        player.gameObject.SetActive(false);
        level5.SetActive(false);
        levelPanel.SetActive(false);
        levelsPanel.SetActive(true);
        importPanel.SetActive(false);
        WebViewLoader.Instance.DestroyWebView();
    }
    public void ImportPDF()
    {
        string filePath = "";
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        Ookii.Dialogs.VistaOpenFileDialog dialog = new Ookii.Dialogs.VistaOpenFileDialog();
        dialog.Title = "Choose your profile picture";
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
        importPanel.SetActive(false);
        levelsPanel.SetActive(true);
        levelPanel.SetActive(false);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
