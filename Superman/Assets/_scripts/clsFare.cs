using UnityEngine;

public class clsFare
{
    public float fare = 0f;
    public float fareRate = 0.01f;
    public float initialFare = 100.00f;
    public bool Paid = false;



    /// <summary>
    /// 
    /// </summary>
    public void InitFare()
    {
        Paid = false;
        fare = initialFare;
    }

    /// <summary>
    /// 
    /// </summary>
    public void UpdateFare()
    {
        if (fare > 0)
        {
            fare -= fareRate * Time.deltaTime;
            if (fare < 0) fare = 0;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetFare()
    {
        float fltRetVal = 0;

        //make sure we only pay the fare once
        if (Paid == false) fltRetVal = fare;
        //note that the fare has been paid
        Paid = true;
        //we dont owe any more fare
        fare = 0;
        //return what we owe
        return fltRetVal;
    }
}

