
/// <summary>
/// 
/// </summary>
public class ViewerConstants
{
    public static string ServerIPAddressKey = "ServerIPAddressKey";
    public static string ClientIPAddressKey = "ClientIPAddressKey";
    public static byte ServerStartUpMsg = 05;
    public static string LocalHost = "127.0.0.1";


	public static  int MESSAGE_IMAGE = 5;
	public static  int MESSAGE_TOUCH_INPUT = 1;
	public static  int MESSAGE_ACCELEROMETER_INPUT = 2;
	public static  int MESSAGE_TRACKBALL_INPUT = 3;
	public static  int MESSAGE_OPTIONS = 6;
	public static  int MESSAGE_KEY = 7;
    public static  int MESSAGE_MAGNOTOMETER_INPUT = 8;
    public static  int MESSAGE_ATTITUDE_INPUT = 9;

    public static float RAD2DEG = 180f / 3.14f;

    public static int PORT = 50005;

    public static int ViewerWidth = 50;
    public static int ViewerHeight = 50;

	// Use 64k buffer for output (touch/trackball events, etc.)
	// and 768k buffer for input (images, status messages).
	private static  int OUTPUT_DATA_SIZE = 1024 * 64;
	private static  int INPUT_DATA_SIZE = 1024 * 768;

}

