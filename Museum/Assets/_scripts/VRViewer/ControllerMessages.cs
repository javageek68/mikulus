/// <summary>
/// 
/// </summary>
public class ControllerMessages
{

    /// <summary>
    /// 
    /// </summary>
    public class TouchMsg
    {
        private float x = 0;
        /// <summary>
        /// 
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }

        private float y = 0;
        /// <summary>
        /// 
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        private long timestamp = 0;
        /// <summary>
        /// 
        /// </summary>
        public long Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        private int pointer = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Pointer
        {
            get { return pointer; }
            set { pointer = value; }
        }

        private int active = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            return "Touch (x " + x + " y " + y + " timestamp " + timestamp + " pointer " + pointer + "active" + active + ")";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AccelMsg
    {
        private bool initialized = false;
        /// <summary>
        /// 
        /// </summary>
        public bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }

        private float x = 0;
        /// <summary>
        /// 
        /// </summary>
        public float X
        {
            get { return x; }
            set {x = value; }
        }
        private float y = 0;
        /// <summary>
        /// 
        /// </summary>
        public float Y
        {
            get { return y; }
            set {y = value; }
        }
        private float z = 0;
        /// <summary>
        /// 
        /// </summary>
        public float Z
        {
            get { return z; }
            set {z = value; }
        }
        private int timestamp = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Timestamp
        {
            get { return timestamp; }
            set {
                initialized = true;
                timestamp = value; 
            }
        }

        public string ToString()
        {
            return "Accel (x " + x + " y " + y + " z " + z + " timestamp " + timestamp + ")";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MagnoMsg
    {
        private bool initialized = false;
        /// <summary>
        /// 
        /// </summary>
        public bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }

        private float x = 0;
        /// <summary>
        /// 
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        private float y = 0;
        /// <summary>
        /// 
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        private float z = 0;
        /// <summary>
        /// 
        /// </summary>
        public float Z
        {
            get { return z; }
            set { z = value; }
        }
        private int timestamp = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Timestamp
        {
            get { return timestamp; }
            set
            {
                initialized = true;
                timestamp = value;
            }
        }

        public string ToString()
        {
            return "Magno (x " + x + " y " + y + " z " + z + " timestamp " + timestamp + ")";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AttitudeMsg
    {
        private bool initialized = false;
        /// <summary>
        /// 
        /// </summary>
        public bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }

        private float x = 0;
        /// <summary>
        /// 
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        private float y = 0;
        /// <summary>
        /// 
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        private float z = 0;
        /// <summary>
        /// 
        /// </summary>
        public float Z
        {
            get { return z; }
            set { z = value; }
        }
        private int timestamp = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Timestamp
        {
            get { return timestamp; }
            set
            {
                initialized = true;
                timestamp = value;
            }
        }

        public string ToString()
        {
            return "Attitude (x " + x + " y " + y + " z " + z + " timestamp " + timestamp + ")";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InitialAttitude
    {
        private bool initialized = false;
        /// <summary>
        /// 
        /// </summary>
        public bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }

        private float x = 0;
        /// <summary>
        /// 
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        private float y = 0;
        /// <summary>
        /// 
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        private float z = 0;
        /// <summary>
        /// 
        /// </summary>
        public float Z
        {
            get { return z; }
            set { z = value; }
        }
        private int timestamp = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Timestamp
        {
            get { return timestamp; }
            set
            {
                initialized = true;
                timestamp = value;
            }
        }

        public string ToString()
        {
            return "Attitude (x " + x + " y " + y + " z " + z + " timestamp " + timestamp + ")";
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class TrackMsg
    {
        private float x = 0;
        /// <summary>
        /// 
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        private float y = 0;
        /// <summary>
        /// 
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            return "Track (x " + x + " y " + y + ")";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OptionsMsg
    {
        private int screenWidth = 0;
        /// <summary>
        /// 
        /// </summary>
        public int ScreenWidth
        {
            get { return screenWidth; }
            set { screenWidth = value; }
        }
        
        private int screenHeight = 0;
        /// <summary>
        /// 
        /// </summary>
        public int ScreenHeight
        {
            get { return screenHeight; }
            set { screenHeight = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            return " Options (" + screenWidth + "," + screenHeight + ")";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class KeyMsg
    {
        
        private int state = 0;
        /// <summary>
        /// 
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }
        
        private int key = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Key
        {
            get { return key; }
            set { key = value; }
        }
        
        private int unicode = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Unicode
        {
            get { return unicode; }
            set { unicode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            return "KeyMsg (state :" + state + " key " + key + " unicode " + unicode;
        }
    }

    private TouchMsg touchMessage = new TouchMsg();
    /// <summary>
    /// 
    /// </summary>
    public TouchMsg TouchMessage
    {
        get { return touchMessage; }
        set { touchMessage = value; }
    }
    
    private AccelMsg accelMessage = new AccelMsg();
    /// <summary>
    /// 
    /// </summary>
    public AccelMsg AccelMessage
    {
        get { return accelMessage; }
        set {accelMessage = value; }
    }
    
    private MagnoMsg magnoMessage = new MagnoMsg();
    /// <summary>
    /// 
    /// </summary>
    public MagnoMsg MagnoMessage
    {
        get { return magnoMessage; }
        set {magnoMessage = value; }
    }
    
    private AttitudeMsg attMessage = new AttitudeMsg();
    /// <summary>
    /// 
    /// </summary>
    public AttitudeMsg AttMessage
    {
        get {
            testForInit();
            return attMessage; 
        }
        set {attMessage = value; }
    }
    
    private InitialAttitude initAttMessage = new InitialAttitude();
    /// <summary>
    /// 
    /// </summary>
    public InitialAttitude InitAttMessage
    {
        get { return initAttMessage; }
        set {initAttMessage = value; }
    }
    
    private TrackMsg trackMessage = new TrackMsg();
    /// <summary>
    /// 
    /// </summary>
    public TrackMsg TrackMessage
    {
        get { return trackMessage; }
        set { trackMessage = value; }
    }
    
    private OptionsMsg optionMessage = new OptionsMsg();
    /// <summary>
    /// 
    /// </summary>
    public OptionsMsg OptionMessage
    {
        get { return optionMessage; }
        set { optionMessage = value; }
    }
    
    private KeyMsg keyMessage = new KeyMsg();
    /// <summary>
    /// 
    /// </summary>
    public KeyMsg KeyMessage
    {
        get { return keyMessage; }
        set { keyMessage = value; }
    }

    /// <summary>
    /// As soon as we have data for the accelerometer, the magnetometer, and the attitude, we need to
    /// save the initial value of the attitude. We will assume that the user is facing forward at that point.
    /// </summary>
    private void testForInit()
    {
        if (accelMessage.Initialized == true &&
            magnoMessage.Initialized == true && 
            attMessage.Initialized == true && 
            initAttMessage.Initialized == false)
        {
            SetInitialAttitude();
        }
    }

    /// <summary>
    /// record the initial attitude of the controller device. we will use that as the 
    /// 0,0,0 angle.  the delta will be base off of that.  if we didnt use the delta, then
    /// the user would be required to always face north when he plays the game.
    /// </summary>
    public void SetInitialAttitude()
    {
        initAttMessage.X = attMessage.X;
        initAttMessage.Y = attMessage.Y;
        initAttMessage.Z = attMessage.Z;
        initAttMessage.Timestamp = attMessage.Timestamp;
    }

    public float attDeltaX
    {
        get { return attMessage.X - initAttMessage.X; }
    }

    public float attDeltaY
    {
        get { return attMessage.Y - initAttMessage.Y; }
    }

    public float attDeltaZ
    {
        get { return attMessage.Z - initAttMessage.Z; }
    }

    

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string ToString()
    {
        string strRetVal = optionMessage.ToString() + "\n" +
                           touchMessage.ToString() + "\n" +
                           accelMessage.ToString() + "\n" +
                           magnoMessage.ToString() + "\n" +
                           attMessage.ToString() + "\n" +
                           trackMessage.ToString() + "\n" +
                           keyMessage.ToString();

        return strRetVal;
    }

}

