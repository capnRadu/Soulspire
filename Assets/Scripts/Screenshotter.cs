using UnityEngine;

public class Screenshotter : MonoBehaviour
{
    private int count = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            string directory = "C:\\Users\\Radu\\Desktop\\";
            string filename = $"screenshot_{count++}.png";
            string fullpath = System.IO.Path.Combine(directory, filename);
            ScreenCapture.CaptureScreenshot(fullpath);

            Debug.Log($"Screenshot saved to: {fullpath}");
        }
    }
}