using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System;
using System.Runtime;
using System.Diagnostics;
using System.IO;
using CustomClasses.IO;
using System.Threading;
    
/// <summary>
/// 
/// </summary>
public class VRUtilities
{
    /// <summary>
    /// 
    /// </summary>
    public static bool blnSendingScreenShot = false;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static IEnumerator StartVRClient(ISocketClient socketClient)
    {
        //keep the client from sending video until we have connected
        blnSendingScreenShot = true;

        int intADBProcExitCode = 0;
        string strADBProcStatus = "";

        //have the android debug bridge run port forwarding so we can use this port
        //to send data over the USB 
        intADBProcExitCode = RunADBPortForward2(ViewerConstants.PORT, ref strADBProcStatus);
        yield return 0;
        UnityEngine.Debug.Log("ADB Port Forward exit code : " + intADBProcExitCode + " status : " + strADBProcStatus);
        //connect to the server on the Android using USB
        ClientControl.Instance.Init(ViewerConstants.LocalHost, ViewerConstants.PORT, ProtocolType.Tcp, socketClient);

        //tell the client it is okay to send video now.  we should be connected.
        blnSendingScreenShot = false;
        yield return 0;
    }

     /**
     * Runs the android debug bridge command of forwarding the ports
     *
     */
    public static int RunADBPortForward(int intPort) 
    {
     int intExitCode = 0;

     // run the adb bridge
     try {
         //
         // Setup the process with the ProcessStartInfo class.
         //
         ProcessStartInfo start = new ProcessStartInfo();
         start.FileName = @"C:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe "; // Specify exe name.
         start.Arguments = " forward tcp:" + intPort.ToString() + " tcp:" + intPort.ToString();
         start.UseShellExecute = false;
         start.RedirectStandardOutput = true;
         start.RedirectStandardError = true;
         //
         // Start the process.
         //
         using (Process process = Process.Start(start))
         {
             //
             // Read in all the text from the process with the StreamReader.
             //
             using (StreamReader reader = process.StandardOutput)
             {
                 string result = reader.ReadToEnd();
                 UnityEngine.Debug.Log(result);
             }
             using (StreamReader reader = process.StandardError)
             {
                 string result = reader.ReadToEnd();
                 UnityEngine.Debug.Log(result);
             }

         }

         } catch (Exception e) {
             UnityEngine.Debug.LogError(e.ToString());
         }
    return intExitCode;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="intPort"></param>
    /// <param name="strStatus"></param>
    /// <returns></returns>
    public static int RunADBPortForward2(int intPort, ref string strStatus)
    {
        string filename = @"C:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe "; // Specify exe name.
        string arguments = " forward tcp:" + intPort.ToString() + " tcp:" + intPort.ToString();
        Int32 timeout = 5000;
        int intExitCode = 0;

        intExitCode = StartProcess(filename, arguments, timeout, ref strStatus);

        return intExitCode;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="arguments"></param>
    /// <param name="timeout"></param>
    /// <param name="strStatus"></param>
    /// <returns></returns>
    public static int StartProcess(string filename, string arguments, Int32 timeout, ref string strStatus)
    {
        int intExitCode = 0;
        using (Process process = new Process())
        {
            process.StartInfo.FileName = filename;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();

            using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
            using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        output.AppendLine(e.Data);
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        errorWaitHandle.Set();
                    }
                    else
                    {
                        error.AppendLine(e.Data);
                    }
                };

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                if (process.WaitForExit(timeout) &&
                    outputWaitHandle.WaitOne(timeout) &&
                    errorWaitHandle.WaitOne(timeout))
                {
                    // Process completed. Check process.ExitCode here.
                    intExitCode = process.ExitCode;
                }
                else
                {
                    // Timed out.
                    intExitCode = -2;
                }
                strStatus = output.ToString() + "\n" + error.ToString();
                strStatus = strStatus.Trim();
            }
        }
        return intExitCode;
    }





    /// <summary>
    /// Controller data is in the format "hhh(x,y,z);"
    /// </summary>
    /// <param name="strCntlrData"></param>
    /// <param name="fltHorizontalHeading"></param>
    /// <param name="vctAccel"></param>
    public static void ParseControllerData(string strCntlrData, ref float fltHorizontalHeading, ref Vector3 vctAccel)
    {
        bool blnParseIssue = false;
        int intEnding = -1;
        int intOpenPar = -1;
        int intClosePar = -1;
        string strHeading = "";
        string strVector = "";
        string[] strVectParts = null;
        float x = 0f;
        float y = 0f;
        float z = 0f;

        //trim the input
        strCntlrData = strCntlrData.Trim();
        //get the length of the input
        int intDataLength = strCntlrData.Length;
        //make sure we have data
        if (intDataLength > 0)
        {
            intEnding = strCntlrData.IndexOf(";");
            intOpenPar = strCntlrData.IndexOf("(");
            intClosePar = strCntlrData.IndexOf(")");

            if (intEnding > 0 && intOpenPar > 0 && intClosePar > 0)
            {
                try
                {
                    //parse the heading from the controller data
                    strHeading = strCntlrData.Substring(0, intOpenPar);
                    //convert the heading to a float
                    fltHorizontalHeading = float.Parse(strHeading);


                    //parse the accel vector controller data
                    strVector = strCntlrData.Substring(intOpenPar + 1, intClosePar - intOpenPar - 1);
                    //split the vector by the commas
                    strVectParts = strVector.Split(',');
                    x = float.Parse(strVectParts[0]);
                    y = float.Parse(strVectParts[1]);
                    z = float.Parse(strVectParts[2]);
                    vctAccel = new Vector3(x, y, z);
                }
                catch (Exception)
                {
                    blnParseIssue = true;
                }
            }
            else
            {
                //missing a format character
                //the transmission is corrupt
                blnParseIssue = true;
            }
        }
        else
        {
            //we dont have any data
            blnParseIssue = true;
        }

        //we had a parse issue. return default values
        if (blnParseIssue == true)
        {
            fltHorizontalHeading = 0;
            vctAccel = new Vector3(0, 0, 0);
        }


    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static IEnumerator SendScreenshotToClient()
    {
        byte[] bytTexture;
        
        //make sure we dont start a new image tx until the last one is complete
        if (blnSendingScreenShot == false)
        {
            blnSendingScreenShot = true;
            // wait for graphics to render
            yield return new WaitForEndOfFrame();

            // create a texture to pass to encoding
            Texture2D txtScreenCapture = TextureUtilities.CaptureScreenShot();

            yield return 0;

            // scale the texture down
            TextureUtilities.TextureScale.Point(txtScreenCapture, 100, 100);

            yield return 0;

            bytTexture = TextureUtilities.TextureToBytes(txtScreenCapture);

            //Debug.Log("texture size : " + bytTexture.Length);

            //send the texture to the clients
            ServerControl.Instance.SendTxToAll(bytTexture);
            yield return 0;

            //System.Threading.Thread.Sleep(5000);

            // Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
            //DestroyObject(txtScreenCapture);
            blnSendingScreenShot = false;
        }
        //Debug.Log("Leaving ScreenshotEncode");
    }

    public static IEnumerator SendScreenshotToServer()
    {
        byte[] bytTexture;

        //make sure we dont start a new image tx until the last one is complete
        if (blnSendingScreenShot == false)
        {
            blnSendingScreenShot = true;
            // wait for graphics to render
            yield return new WaitForEndOfFrame();

            // create a texture to pass to encoding
            Texture2D txtScreenCapture = TextureUtilities.CaptureScreenShot();

            yield return 0;

            // scale the texture down
            TextureUtilities.TextureScale.Point(txtScreenCapture, ViewerConstants.ViewerWidth, ViewerConstants.ViewerHeight);

            

            yield return 0;

            //convert the texture to a byte array
            bytTexture = TextureUtilities.TextureToBytes(txtScreenCapture);

            yield return 0;

            //Debug.Log("texture size : " + bytTexture.Length);

            //send the texture to the clients
            ClientControl.Instance.SendTx(bytTexture);
            yield return 0;

            //System.Threading.Thread.Sleep(5000);

            // Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
            //DestroyObject(txtScreenCapture);
            blnSendingScreenShot = false;
        }
        //Debug.Log("Leaving ScreenshotEncode");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static IEnumerator SendVideoStream(int intDeviceScreenWidth, int intDeviceScreenHeight)
    {
        byte[] bytTexture;

        //UnityEngine.Debug.Log("Entered SendVideoStream");
        //make sure we dont start a new image tx until the last one is complete
        //also make sure that we are already connected to the device
        if (blnSendingScreenShot == false && ClientControl.Instance.IsConnected() == true)
        {
            UnityEngine.Debug.Log("sending new screen shot");
            blnSendingScreenShot = true;
            // wait for graphics to render
            yield return new WaitForEndOfFrame();

            // create a texture to pass to encoding
            Texture2D txtScreenCapture = TextureUtilities.CaptureScreenShot();

            
            //if the device screen size is different than this one, then resize it
            if (txtScreenCapture.width != intDeviceScreenWidth || txtScreenCapture.height != intDeviceScreenHeight)
            {
                yield return 0;
                // scale the texture down
                TextureUtilities.TextureScale.Point(txtScreenCapture, intDeviceScreenWidth, intDeviceScreenHeight);
            }
            yield return 0;

            //txtScreenCapture.format = TextureFormat.ATC_RGBA8;


            bytTexture = TextureUtilities.TextureToBytes(txtScreenCapture);
            Int32 intCompressedSize = bytTexture.Length;

            UnityEngine.Debug.Log("about to write data to stream");
            MemoryStream stream = new MemoryStream();
            using (EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(stream, false))
            {
                writer.Write(ViewerConstants.MESSAGE_IMAGE);
                writer.Write(intDeviceScreenWidth);
                writer.Write(intDeviceScreenHeight);
                writer.Write(intCompressedSize);
                writer.Write(bytTexture);
            }

            UnityEngine.Debug.Log("convert stream to byte array");

            byte[] bytTxData = stream.ToArray();

            //send the texture to the clients
            ClientControl.Instance.SendTx(bytTxData);
            yield return 0;

            //System.Threading.Thread.Sleep(5000);

            // Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
            //DestroyObject(txtScreenCapture);
            blnSendingScreenShot = false;
        }
        //UnityEngine.Debug.Log("Leaving SendVideoStream");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strFileName"></param>
    public static IEnumerator SendVideoViaRenderTexture(int intDeviceScreenWidth, int intDeviceScreenHeight)
    {
        if (blnSendingScreenShot == false)
        {
            blnSendingScreenShot = true;
            yield return new WaitForEndOfFrame();

            RenderTexture rtRenderTexture = new RenderTexture(intDeviceScreenWidth, intDeviceScreenHeight, 24);
            Camera.main.targetTexture = rtRenderTexture;
            Texture2D txtScreenShot = new Texture2D(intDeviceScreenWidth, intDeviceScreenHeight, TextureFormat.RGB24, false);
            Camera.main.Render();
            yield return 0;
            RenderTexture.active = rtRenderTexture;
            txtScreenShot.ReadPixels(new Rect(0, 0, intDeviceScreenWidth, intDeviceScreenHeight), 0, 0);
            Camera.main.targetTexture = null;
            RenderTexture.active = null;
            GameObject.Destroy(rtRenderTexture);

            yield return 0;
            byte[] bytTexture = TextureUtilities.TextureToBytes(txtScreenShot);
            Int32 intCompressedSize = bytTexture.Length;

            yield return 0;
            UnityEngine.Debug.Log("about to write data to stream");
            MemoryStream stream = new MemoryStream();
            using (EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(stream, false))
            {
                writer.Write(ViewerConstants.MESSAGE_IMAGE);
                writer.Write(intDeviceScreenWidth);
                writer.Write(intDeviceScreenHeight);
                writer.Write(intCompressedSize);
                writer.Write(bytTexture);
            }

            UnityEngine.Debug.Log("convert stream to byte array");

            byte[] bytTxData = stream.ToArray();

            //send the texture to the clients
            ClientControl.Instance.SendTx(bytTxData);


            blnSendingScreenShot = false;
        }

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytController"></param>
    /// <param name="cntlMsgs"></param>
    /// <param name="strControllerData"></param>
    public static void DecodeControllerData(byte[] bytController, ref ControllerMessages cntlMsgs)
    {
        //UnityEngine.Debug.Log("Entered DecodeControllerData()");
        int intMsgType = 0;

        MemoryStream stream = new MemoryStream(bytController);
        using (EndianAwareBinaryReader reader = new EndianAwareBinaryReader(stream, false))
        {
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                intMsgType = reader.ReadInt32();
                //UnityEngine.Debug.Log("intMsgType = " + intMsgType);

                if (intMsgType == ViewerConstants.MESSAGE_TOUCH_INPUT)
                {
                    UnityEngine.Debug.Log("reading touch");
                    cntlMsgs.TouchMessage.X = reader.ReadSingle();
                    cntlMsgs.TouchMessage.Y = reader.ReadSingle();
                    cntlMsgs.TouchMessage.Timestamp = reader.ReadInt64();
                    cntlMsgs.TouchMessage.Pointer = reader.ReadInt32();
                    cntlMsgs.TouchMessage.Active = reader.ReadInt32();
                }
                else if (intMsgType == ViewerConstants.MESSAGE_ACCELEROMETER_INPUT)
                {
                    //UnityEngine.Debug.Log("reading accel");
                    cntlMsgs.AccelMessage.X = reader.ReadSingle();
                    cntlMsgs.AccelMessage.Y = reader.ReadSingle();
                    cntlMsgs.AccelMessage.Z = reader.ReadSingle();
                    cntlMsgs.AccelMessage.Timestamp = reader.ReadInt32();
                }
                else if (intMsgType == ViewerConstants.MESSAGE_MAGNOTOMETER_INPUT)
                {
                    //UnityEngine.Debug.Log("reading magnotometer");
                    cntlMsgs.MagnoMessage.X = reader.ReadSingle();
                    cntlMsgs.MagnoMessage.Y = reader.ReadSingle();
                    cntlMsgs.MagnoMessage.Z = reader.ReadSingle();
                    cntlMsgs.MagnoMessage.Timestamp = reader.ReadInt32();
                }
                else if (intMsgType == ViewerConstants.MESSAGE_ATTITUDE_INPUT)
                {
                    //UnityEngine.Debug.Log("reading attitude");
                    cntlMsgs.AttMessage.X = reader.ReadSingle();
                    cntlMsgs.AttMessage.Y = reader.ReadSingle();
                    cntlMsgs.AttMessage.Z = reader.ReadSingle();
                    cntlMsgs.AttMessage.Timestamp = reader.ReadInt32();
                }

                else if (intMsgType == ViewerConstants.MESSAGE_TRACKBALL_INPUT)
                {
                    UnityEngine.Debug.Log("reading trackball");
                    cntlMsgs.TrackMessage.X = reader.ReadSingle();
                    cntlMsgs.TrackMessage.Y = reader.ReadSingle();
                }
                else if (intMsgType == ViewerConstants.MESSAGE_OPTIONS)
                {
                    UnityEngine.Debug.Log("reading options");
                    cntlMsgs.OptionMessage.ScreenWidth = reader.ReadInt32();
                    cntlMsgs.OptionMessage.ScreenHeight = reader.ReadInt32();
                }
                else if (intMsgType == ViewerConstants.MESSAGE_KEY)
                {
                    UnityEngine.Debug.Log("reading keys");
                    cntlMsgs.KeyMessage.State = reader.ReadInt32();
                    cntlMsgs.KeyMessage.Key = reader.ReadInt32();
                    cntlMsgs.KeyMessage.Unicode = reader.ReadInt32();
                }
            }
        }

    }


}

