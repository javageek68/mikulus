using UnityEngine;
using System.Threading;
using System.IO;
using System.IO.Compression;
using System;

/// <summary>
/// 
/// </summary>
public class TextureUtilities
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tex"></param>
    /// <returns></returns>
    public static byte[] TextureToBytes(Texture2D tex)
    {
        return tex.EncodeToPNG();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strFileName"></param>
    /// <param name="tex"></param>
    public static void SaveTexureAsFile(string strFileName, Texture2D tex)
    {
        byte[] bytes = TextureToBytes(tex);
        SaveTexureAsFile(strFileName, bytes);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strFileName"></param>
    /// <param name="bytTexture"></param>
    public static void SaveTexureAsFile(string strFileName, byte[] bytTexture)
    {
        System.IO.File.WriteAllBytes(strFileName, bytTexture);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="intWidth"></param>
    /// <param name="intHeight"></param>
    /// <param name="format"></param>
    /// <param name="blnMipMap"></param>
    /// <param name="bytTexture"></param>
    /// <returns></returns>
    public static Texture2D BytesToTexture(int intWidth, int intHeight, TextureFormat format, bool blnMipMap, byte[] bytTexture)
    {
        Texture2D txtNewTexture = new Texture2D(intWidth, intHeight, format, blnMipMap);
        txtNewTexture.LoadImage(bytTexture);
        return txtNewTexture;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static Texture2D CaptureScreenShot()
    {
        // create a texture to pass to encoding
        Texture2D txtNewTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // put buffer into texture
        txtNewTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        txtNewTexture.Apply();
        return txtNewTexture;
    }


    /// <summary>
    /// 
    /// </summary>
    public class TextureScale
    {
        /// <summary>
        /// 
        /// </summary>
        public class ThreadData
        {
            public int start;
            public int end;
            public ThreadData(int s, int e)
            {
                start = s;
                end = e;
            }
        }

        private static Color[] texColors;
        private static Color[] newColors;
        private static int w;
        private static float ratioX;
        private static float ratioY;
        private static int w2;
        private static int finishCount;
        private static Mutex mutex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        public static void Point(Texture2D tex, int newWidth, int newHeight)
        {
            ThreadedScale(tex, newWidth, newHeight, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
        {
            ThreadedScale(tex, newWidth, newHeight, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <param name="useBilinear"></param>
        private static void ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
        {
            texColors = tex.GetPixels();
            newColors = new Color[newWidth * newHeight];
            if (useBilinear)
            {
                ratioX = 1.0f / (float)newWidth / (tex.width - 1);
                ratioY = 1.0f / (float)newHeight / (tex.height - 1);
            }
            else
            {
                ratioX = ((float)tex.width) / newWidth;
                ratioY = ((float)tex.height) / newHeight;
            }
            w = tex.width;
            w2 = newWidth;
            var cores = Mathf.Min(SystemInfo.processorCount, newHeight);
            var slice = newHeight / cores;

            finishCount = 0;
            if (mutex == null)
            {
                mutex = new Mutex(false);
            }
            if (cores > 1)
            {
                int i = 0;
                ThreadData threadData;
                for (i = 0; i < cores - 1; i++)
                {
                    threadData = new ThreadData(slice * i, slice * (i + 1));
                    ParameterizedThreadStart ts = useBilinear ? new ParameterizedThreadStart(BilinearScale) : new ParameterizedThreadStart(PointScale);
                    Thread thread = new Thread(ts);
                    thread.Start(threadData);
                }
                threadData = new ThreadData(slice * i, newHeight);
                if (useBilinear)
                {
                    BilinearScale(threadData);
                }
                else
                {
                    PointScale(threadData);
                }
                while (finishCount < cores)
                {
                    Thread.Sleep(1);
                }
            }
            else
            {
                ThreadData threadData = new ThreadData(0, newHeight);
                if (useBilinear)
                {
                    BilinearScale(threadData);
                }
                else
                {
                    PointScale(threadData);
                }
            }

            tex.Resize(newWidth, newHeight);
            tex.SetPixels(newColors);
            tex.Apply();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public static void BilinearScale(System.Object obj)
        {
            ThreadData threadData = (ThreadData)obj;
            for (var y = threadData.start; y < threadData.end; y++)
            {
                int yFloor = (int)Mathf.Floor(y * ratioY);
                var y1 = yFloor * w;
                var y2 = (yFloor + 1) * w;
                var yw = y * w2;

                for (var x = 0; x < w2; x++)
                {
                    int xFloor = (int)Mathf.Floor(x * ratioX);
                    var xLerp = x * ratioX - xFloor;
                    newColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor + 1], xLerp),
                                                           ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor + 1], xLerp),
                                                           y * ratioY - yFloor);
                }
            }

            mutex.WaitOne();
            finishCount++;
            mutex.ReleaseMutex();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public static void PointScale(System.Object obj)
        {
            ThreadData threadData = (ThreadData)obj;
            for (var y = threadData.start; y < threadData.end; y++)
            {
                var thisY = (int)(ratioY * y) * w;
                var yw = y * w2;
                for (var x = 0; x < w2; x++)
                {
                    newColors[yw + x] = texColors[(int)(thisY + ratioX * x)];
                }
            }

            mutex.WaitOne();
            finishCount++;
            mutex.ReleaseMutex();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
        {
            return new Color(c1.r + (c2.r - c1.r) * value,
                              c1.g + (c2.g - c1.g) * value,
                              c1.b + (c2.b - c1.b) * value,
                              c1.a + (c2.a - c1.a) * value);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum sqSquareType
    {
        highest,
        lowest,
        closest
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="intSource"></param>
    /// <param name="squareType"></param>
    /// <returns></returns>
    public static int getSquare(int intSource, sqSquareType squareType)
    {
        int intRetVal = 0;
        int intSquare = 0;
        int intLastSquare = 0;
        int intExp = 0;
        bool blnFound = false;

        while (blnFound == false)
        {
            intExp++;
            intLastSquare = intSquare;
            intSquare = (int)Math.Pow(2, intExp);

            if (intSquare >= intSource)
            {
                blnFound = true;
                if (intSquare == intSource)
                {
                    //if source is already a square, then return the source
                    intRetVal = intSource;
                }
                else
                {
                    //at this point, intSquare >= intSource >= intLastSquare
                    //so we pick the one we want
                    if (squareType == sqSquareType.highest) intRetVal = intSquare;
                    else if (squareType == sqSquareType.lowest) intRetVal = intLastSquare;
                    else if (squareType == sqSquareType.closest)
                    {
                        if (Math.Abs(intSource - intSquare) < Math.Abs(intSource - intLastSquare))
                            intRetVal = intSquare;
                        else
                            intRetVal = intLastSquare;
                    }
                }
            }

        }

        return intRetVal;
    }


}

