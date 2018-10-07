using UnityEngine;

public class BallMoveMono : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.AngleAxis(Mathf.Sin(Time.timeSinceLevelLoad) * 100, Vector3.up);
        this.transform.Translate(new Vector3(0, Mathf.Sin(Time.timeSinceLevelLoad) / 20f, 0));
    }
}

