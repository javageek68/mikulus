using UnityEngine;
using System.Collections;
using System.IO;

public class Custom : MonoBehaviour {

    public Camera camScreenShot;
    private Camera camMainCamera;
    

	// Use this for initialization
	void Start () {
        
        camMainCamera = Camera.mainCamera;
        //camScreenShot = this.gameObject;
    
	}
	
	// Update is called once per frame
	void Update () {
        //if the user presses the R (reset) key, recalibrate the camera 
        //with the control device
        if (Input.GetKeyDown(KeyCode.Keypad1) == true)
        {
            CaptureScreenshot1(@"C:\temp\UnityScreenshot\cap1.png");
        }
        if (Input.GetKeyDown(KeyCode.Keypad2) == true)
        {
            StartCoroutine(ScreenshotEncode(@"C:\temp\UnityScreenshot\cap2.png"));
        }
        if (Input.GetKeyDown(KeyCode.Keypad3) == true)
        {
            ScreenShotRenderTexture(@"C:\temp\UnityScreenshot\cap3.png");
        }
        if (Input.GetKeyDown(KeyCode.Keypad4) == true)
        {
            ScreenShotRenderTexture2(@"C:\temp\UnityScreenshot\cap4.png");
        }


	}

    void CaptureScreenshot1(string strFileName)
    {
        Debug.Log("entered CaptureScreenshot1()");
        Application.CaptureScreenshot(strFileName);
    }

    IEnumerator ScreenshotEncode(string strFileName)
    {
        Debug.Log("entered ScreenshotEncode()");
        // wait for graphics to render
        yield return new WaitForEndOfFrame();
 
        // create a texture to pass to encoding
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
 
        // put buffer into texture
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();
 
        // split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
        yield return 0;
 
        byte[] bytes = texture.EncodeToPNG();
 
        // save our test image (could also upload to WWW)
        File.WriteAllBytes(strFileName, bytes);
 
        // Added by Karl. - Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
        DestroyObject( texture );
 
        //Debug.Log( Application.dataPath + "/../testscreen-" + count + ".png" );
    }

    void ScreenShotRenderTexture(string strFileName)
    {
        Debug.Log("entered ScreenShotRenderTexture()");
        camScreenShot.enabled = true;
        //place the screen shot cam over main cam
        camScreenShot.transform.position = camMainCamera.transform.position;
        camScreenShot.transform.rotation = camMainCamera.transform.rotation;

        //create a new render texture
        RenderTexture rtTex = new RenderTexture(512, 256, 24);
        //set it as the camera's target
        camScreenShot.targetTexture = rtTex;
        //render the scene onto the texture
        camScreenShot.Render();
        //set the render texture as active
        RenderTexture.active = rtTex;

        //create a texture2D object to hold the image
        Texture2D txScreenShot = new Texture2D(rtTex.width, rtTex.height, TextureFormat.RGB24, false);
        //copy the render texture onto a texture2d
        txScreenShot.ReadPixels(new Rect(0, 0, rtTex.width, rtTex.height), 0, 0);
        //apply the changes
        txScreenShot.Apply();
        //remove the active texture
        RenderTexture.active = null;

        camScreenShot.enabled = false;

        //convert the image to a byte array
        byte[] bytScreenShot = txScreenShot.EncodeToPNG();
        File.WriteAllBytes(strFileName, bytScreenShot);
    }

    void ScreenShotRenderTexture2(string strFileName)
    {
        int resWidth = Screen.width;
        int resHeight = Screen.height;

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        Camera.main.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Camera.main.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        Camera.main.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        GameObject.Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(strFileName, bytes);

    }
}
