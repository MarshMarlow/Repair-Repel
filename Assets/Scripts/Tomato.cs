using UnityEngine;

public class Tomato : MonoBehaviour
{
    public Transform target; //pos of player
    public float heightOffset = 10f; //peak min
    public float gravity = -9.81f;
    private Vector3 adjustment = new Vector3(0f, 0.5f, 0f); //slight adjustment of end position
    private Rigidbody rb; //rb of the tomato
    public Transform visualModel; //mesh of the tomato
    public float spinSpeed = 270f; //how fast tomato rotates midair
    Vector3 spinAxis; //how the tomato will rotate

    public AudioClip hit_clip;
    public AudioClip blocked_clip;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //might have to change this later on with VR cuz we should probably target just the head
        target = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
        visualModel = transform.GetChild(0);
        spinAxis = Random.onUnitSphere; //random axis to spin on
        LaunchArc();
    }

    void Update()
    {
        visualModel.Rotate(spinAxis * spinSpeed * Time.deltaTime, Space.Self);
    }

    void LaunchArc()
    {
        Vector3 start = transform.position;
        Vector3 end = target.position + adjustment;

        //horizontal direction
        Vector3 horizontal = new Vector3(end.x - start.x, 0, end.z - start.z) + adjustment;
        float distance = horizontal.magnitude;

        //spawn vs end height diff
        float heightDiff = end.y - start.y;

        //how high projectile will go
        float peak = Mathf.Max(start.y, end.y) + heightOffset;

        //note the 2f is just cause of physics
        //how long it will take to reach the peak
        float timeUp = Mathf.Sqrt(2f * (peak - start.y) / -gravity);
        //how long it will take to reach the end
        float timeDown = Mathf.Sqrt(2f * (peak - end.y) / -gravity);

        //increase totalTime if slower projectile required
        float totalTime = timeUp + timeDown;

        //velocity needed to reach within timeframe
        Vector3 horizontalVel = horizontal.normalized * (distance / totalTime);

        //vertical velocity to reach peak
        float verticalVel = Mathf.Sqrt(2f * -gravity * (peak - start.y));

        Vector3 launchVelocity = horizontalVel + Vector3.up * verticalVel;

        rb.velocity = launchVelocity;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Shield") && collider.gameObject.GetComponent<ObjectGrabbable>().isHeld())
        {
            Debug.Log("BLOCKED");
            // play audio that it blocked
            PlayImpact(blocked_clip);
            //Destroy(gameObject);
            return;
        }
        if (collider.CompareTag("PlayerTomato"))
        {
            Debug.Log("HIT");
            collider.gameObject.GetComponent<TomatoHit>().FlashRed();
            // play audio that it hit player
            PlayImpact(hit_clip);
            //Destroy(gameObject);
            return;
        }

        Destroy(gameObject, 6f);
    }

    // plays the audio, then destroys the gameobject
    void PlayImpact(AudioClip clip) {
        // stop tomato from moving and collisions
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        
        // hide the visuals
        visualModel.gameObject.SetActive(false);
        visualModel.gameObject.GetComponent<TrailRenderer>().enabled = false;
        
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = clip;
        audio.Play();
        
        Destroy(gameObject, clip.length);
    }
}
