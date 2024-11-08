using UnityEngine;
using System.Collections;


public class RoboCannon_Movement : MonoBehaviour
{
    //This script was created for use in the resource "RoboCannon", 

    public LayerMask whatCanBeClickedOn;


    public Transform HorizontalAxis;
    public Transform VerticalAxis;

    public string EnemiesTag;

    private Vector3 Target;

    // in degrees
    //public float leftExtent=-90;
    public float range = 300;
    public float FireRate = 5;
    public float SpeedTurn = 50;
    public float HorizontalConstraint = 90;
    public float UpConstraint = 90;
    public float DownConstraint = -90;
    public ParticleSystem p_smokeBarrel;
    public ParticleSystem p_PlasmaShot;
    public ParticleSystem p_StepDustL;
    public ParticleSystem p_StepDustR;

    public GameObject shell,bigShell;
 
    public float speedShell = 80f;
    public Transform GunEnd;


    private float nextFire, nextFire2;                     // Float to store the time the player will be allowed to fire again, after firing
    private Vector3 dirToTarget;
    private Quaternion newRotationX, newRotationY;
    private float TargetDistance;
    private Quaternion HorizontalAxis_v, VerticalAxis_v;
    [SerializeField, HideInInspector]
    Quaternion original, originalBarrel;
    private Vector3 dirXZ, forwardXZ, dirYZ, forwardYZ;
    //The search for the target will be carried out with the help of cortex every 0.1 
    private float searchTimeDelay = 0.1f;



    // movement vars
    public float m_SpeedMove = 3.0f;                 // How fast the unit moves forward and back.
    private float m_SpeedMoveAdd;                 //change speed moves for run or limp.
    public float m_TurnSpeed = 1f;            // How fast the unit turns in degrees per second.
    public int initialHealth = 100;              //The unit's current health point total
    private int currentHealth ;
    public AudioClip unitDead;
    public AudioClip SoundRoboStep;

    public ParticleSystem p_ExplosionParticles;      //  the particles that will play on explosion.
    public ParticleSystem p_SmokeEffect;
    public ParticleSystem p_SmallExplosion;
 
    private Rigidbody m_Rigidbody;              // Reference used to move the unit.

    private AudioSource m_AudioSource;         // Reference to the audio source used to play engine sounds.

    private WaitForSeconds shotDuration = new WaitForSeconds(15f);    // WaitForSeconds hide object 
    private int m_TimeIdleWait;                 //wait change Idle animation
    private bool TargetON; //is there Target 
    private bool m_dead;

    private Animator animator;
    private string animName, animName2, animNameOld, animNameOld2, animNameShort;


    private float timer_for_double_click=0;
  

    public Transform[] targetPointsPos;//  Points for positions  

    private byte sel_ltargetPointPos;                   //  selected targetPointPos in array
 
    private string _tag;
    private ArrayList crushPartsList = new ArrayList();


    // Use this for initialization

    void Start()
    {
       
        m_AudioSource = GetComponent<AudioSource>();
        originalBarrel = VerticalAxis.transform.rotation;
        StartCoroutine(FindClosestTarget());
        if (EnemiesTag == "") print("No NameEnemiesTag !");
      

        HorizontalAxis_v = HorizontalAxis.transform.rotation;
        VerticalAxis_v = VerticalAxis.transform.rotation;
        original = Quaternion.Euler(HorizontalAxis_v.eulerAngles.x, 0, 0);

        animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
      
        animName = "Idle1";

       
         Init();
    }
    public void Init()
    {
        currentHealth = initialHealth;
        crushPartsList.Clear();
        crushPartsList.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        Transform[] ts = transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts){
               foreach (int t2 in crushPartsList)   if (t.gameObject.name == "crushPart" + t2)  t.gameObject.SetActive(true);
               if (t.gameObject.name == "SmokeEffect(Clone)" ) Destroy(t.gameObject);
               

                }

        m_dead = false;
        transform.gameObject.tag = "Team1";
        transform.gameObject.layer = LayerMask.NameToLayer(_tag);
        _tag = gameObject.tag;
        GetComponent<RoboCannon_Movement>().enabled = true;


        
    }

    private void Update() // movement 
    {
        if (m_dead)
            return;



        // select points to move position
        if (targetPointsPos.Length > 0)
        {
            var heading = transform.position - targetPointsPos[sel_ltargetPointPos].position;


            //move forward
            if (heading.sqrMagnitude > 20)

            { //if the target is far move otherwise stand
                if (animNameOld != "Run") animName = "Move";


                //turn towards  
                Vector3 targetDir = targetPointsPos[sel_ltargetPointPos].position - transform.position;
                float step = m_TurnSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
                newDir.y = 0;
                transform.rotation = Quaternion.LookRotation(newDir);



            }

            else
            {
                //The unit got to the target, choose another target position for movement
                if (targetPointsPos.Length > 1)
                {

                    if (sel_ltargetPointPos < targetPointsPos.Length - 1)
                        sel_ltargetPointPos++;
                    else
                        sel_ltargetPointPos = 0;
                }
                else
                {

                    // Idle
                    if (animName != "BigShot" && animName != "TurnLeft" && animName != "TurnRight")
                        if ((animNameShort != "Idle") || (animNameShort == "Idle" && m_TimeIdleWait == 0))
                        {
                            float t = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                            if (t - (int)t < 0.1f)
                            {
                                m_TimeIdleWait = (int)Random.Range(900, 990);
                                animName = "Idle" + (int)Random.Range(1, 4);
                            }
                        }
                    if (m_TimeIdleWait > 0) m_TimeIdleWait--;

                }
            }
            //   print(Mathf.Abs(HorizontalAxis.eulerAngles.y - transform.eulerAngles.y)+"  "+ animName);

            if ((TargetON) && (animNameShort == "Idle" || animNameOld == "TurnLeft" || animNameOld == "TurnRight") && TargetDistance < range && TargetDistance >= range - (range / 2)) {
               

                if (Vector3.Angle(Target - transform.position, transform.forward) > 10)
                {
                    Vector3 targetDir = Target - transform.position;

                    //   print(Mathf.DeltaAngle( angle360(transform.position,Target) , transform.eulerAngles.y));

                    float step = m_TurnSpeed * Time.deltaTime;
                    Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
                    newDir.y = 0;
                    transform.rotation = Quaternion.LookRotation(newDir);
                    if (HorizontalAxis.eulerAngles.y < transform.eulerAngles.y) animName = "TurnLeft";
                    else animName = "TurnRight";

                }
                else if (animNameShort != "Idle") { animName = "Idle1"; m_TimeIdleWait = 0; }

             }
                    //Clicked
                    if (Input.GetMouseButtonDown(0))
            {

                if ((Time.time - timer_for_double_click) < 0.3F) animName = "Run";
                timer_for_double_click = Time.time;

                Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(myRay, out hitInfo, 100, whatCanBeClickedOn))
                {
                    targetPointsPos[sel_ltargetPointPos].position = hitInfo.point;
                }

            }



        }


        //Change animation
 
        if (animName != animNameOld ) 
        {

   //print("old:" + animNameOld + "- new:" + animName);


            animNameOld = animName;
           if(animName.Length>3) animNameShort = animName.Substring(0, 4);

            animator.SetTrigger(animName);

            if (animName == "Move") m_SpeedMoveAdd = m_SpeedMove;
            if (animName == "Run") m_SpeedMoveAdd = m_SpeedMove * 1.8f;
            if (animName == "MoveLimp") m_SpeedMoveAdd = m_SpeedMove * 0.2f;


        }
        //Change animation
        if (animName2 != animNameOld2)
        {
            animNameOld2 = animName2;
            animator.SetTrigger(animName2);
        }


   }



    // Update is called once per frame
    void LateUpdate()
    {

        if (m_dead)
            return;





        // movement
        // Adjust the rigidbodies position and orientation in FixedUpdate.

        if (animNameOld == "Move" || animNameOld == "Run" || animNameOld == "MoveLimp" || animNameOld == "MoveBack") Move();


        // TurretTurn

        // HorizontalAxis.localRotation = original;
             if (!TargetON)
    {//==new Vector3(0,0,0)) {

            HorizontalAxis_v = Quaternion.RotateTowards(HorizontalAxis_v, HorizontalAxis.transform.rotation, SpeedTurn / 10);
            HorizontalAxis.rotation = HorizontalAxis_v;
            VerticalAxis_v = Quaternion.RotateTowards(VerticalAxis_v, VerticalAxis.transform.rotation, SpeedTurn / 10);
            VerticalAxis.transform.rotation = Quaternion.Euler(VerticalAxis_v.eulerAngles.x, VerticalAxis.eulerAngles.y, 0);


            return;
        }
 

        dirToTarget = (Target - HorizontalAxis.transform.position);


          
        Vector3 yAxis = Vector3.up; // world y axis
        dirXZ = Vector3.ProjectOnPlane(dirToTarget, yAxis);

        forwardXZ = Vector3.ProjectOnPlane(transform.forward, yAxis);
 
        float parentY = transform.rotation.eulerAngles.y;
 

         if (parentY < 0) parentY = -parentY;
           if (parentY > 180) parentY -= 360;
        float yAngle = parentY + Vector3.Angle(dirXZ, forwardXZ) * Mathf.Sign(Vector3.Dot(yAxis, Vector3.Cross(forwardXZ, dirXZ)));
        float yClamped = Mathf.Clamp(yAngle, parentY - HorizontalConstraint  , parentY+HorizontalConstraint);
 
        Quaternion yRotation = Quaternion.AngleAxis(yClamped, Vector3.up);

        Quaternion xRotation = Quaternion.Euler(0, 0, 0);
        float xClamped = 0;
        float xAngle = 0;
        if (yClamped == yAngle)
        {
            dirToTarget = (Target - VerticalAxis.transform.position);
          Vector3  originalForward = yRotation * new Vector3(0, 0, 1);
            Vector3 xAxis = yRotation * Vector3.right; // our local x axis
            dirYZ = Vector3.ProjectOnPlane(dirToTarget, xAxis);
            forwardYZ = Vector3.ProjectOnPlane(originalForward, xAxis);
            xAngle = Vector3.Angle(dirYZ, forwardYZ) * Mathf.Sign(Vector3.Dot(xAxis, Vector3.Cross(forwardYZ, dirYZ)));
            xClamped = Mathf.Clamp(xAngle, -UpConstraint, -DownConstraint);
            xRotation = Quaternion.AngleAxis(xClamped, Vector3.right);
        }


        if (HorizontalAxis.transform == VerticalAxis.transform)
        {
            newRotationX = yRotation * original * xRotation;
            HorizontalAxis_v = Quaternion.RotateTowards(HorizontalAxis_v, newRotationX, SpeedTurn / 10);
        }
        else
        {


            newRotationX = yRotation * original;
            HorizontalAxis_v = Quaternion.RotateTowards(HorizontalAxis_v, newRotationX, SpeedTurn / 10);

            newRotationY = originalBarrel * xRotation;
            VerticalAxis_v = Quaternion.RotateTowards(VerticalAxis_v, newRotationY, SpeedTurn / 10);
            VerticalAxis.transform.rotation = Quaternion.Euler(VerticalAxis_v.eulerAngles.x, VerticalAxis.eulerAngles.y, 0);

        }
        HorizontalAxis.rotation = HorizontalAxis_v;



        //fire

        if (xClamped == xAngle && yClamped == yAngle && HorizontalAxis_v.eulerAngles == newRotationX.eulerAngles && TargetDistance < range - (range / 4))
        {


            if (TargetDistance < range - (range / 2))
            {
                if (nextFire <= 0)
                {

                    nextFire = FireRate;
                    animName2 = "Shot";
                    animNameOld2 = "";
                }
                else
                if (nextFire > 0) nextFire -= 0.01f;
            }
            else if (animNameShort == "Idle" || animNameShort == "BigS")
                if (nextFire2 <= 0)
                {
                animName = "BigShot";
                nextFire2 = FireRate*5;
                animNameOld = "";
                }
            if (nextFire2 > 0) nextFire2 -= 0.01f;
        }
        else if (animName2 != "StopShot") animName2 = "StopShot";




    }

    private void Move()
    {
        // Create a vector in the direction the unit is facing with a magnitude based on the input, speed and the time between frames. 

        Vector3 movementSpeed = transform.forward * m_SpeedMoveAdd * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        m_Rigidbody.MovePosition(m_Rigidbody.position + movementSpeed);

    }
    void OnTriggerEnter(Collider col)
    {
        if (m_dead) return;
        if (col.gameObject.tag == "Shell")
        {
            RC_Shell shell = col.GetComponent<RC_Shell>();
            Damage(shell.shellDamage);
        }
    }
    public void Damage(int damageAmount)
    {
        //subtract damage amount when Damage function is called
        currentHealth -= damageAmount;
 
        //destruction of individual parts
        if (damageAmount>8 && currentHealth <= initialHealth*0.7f )
        {
            if (crushPartsList.Count > 0) { 
                int i = (int)Random.Range(0, crushPartsList.Count) ;
                Transform[] ts = transform.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in ts)
                    if (t.gameObject.name == "crushPart" + crushPartsList[i])
                    {

                        t.gameObject.SetActive(false);
                        var part = (ParticleSystem)Instantiate(p_SmallExplosion, t.position, t.rotation);
                        part.transform.parent = t.parent;

                    }
                crushPartsList.RemoveAt(i);


            }
            if ((int)Random.Range(0, 2) == 1)    animName = "DMG"+ (int)Random.Range(1, 5);
        }
        //Check if health has fallen below zero
        if (currentHealth <= 0)
        { //DEAD
            m_dead = true;
            animator.SetBool("Dead" + (int)Random.Range(1, 5), true);
 
            m_AudioSource.PlayOneShot(unitDead);

            transform.gameObject.tag = "Respawn";
         //   transform.gameObject.layer = LayerMask.NameToLayer(_tag);
            GetComponent<RoboCannon_Movement>().enabled=false ;
 

            var part = (ParticleSystem)Instantiate(p_SmokeEffect, VerticalAxis.position, VerticalAxis.rotation);
            Instantiate(p_ExplosionParticles, VerticalAxis.position  , VerticalAxis.rotation);

            part.transform.parent = VerticalAxis;



            StartCoroutine(hideUnit());
        }
    }
    protected virtual IEnumerator FindClosestTarget()
    {
        while (true)
        {

            //The nearest target caught in the radius of the review
            TargetDistance = range * 2;

            Vector3 closest = new Vector3();
            TargetON = false;
            GameObject[] targets = GameObject.FindGameObjectsWithTag(EnemiesTag);
            foreach (GameObject obj in targets)
            {

                //Find the distance between the turret and the intended target
                Vector3 diff = obj.transform.position - transform.position;
                float nearest = diff.sqrMagnitude;

                //	If a target is found in the radius of the lesion, then we remember it
                if (nearest < range && nearest < TargetDistance)
                {
                    TargetDistance = nearest;
                    closest = obj.transform.position  ;
                     if (obj.GetComponent<Collider>()) closest += new Vector3(0, obj.GetComponent<Collider>().bounds.size.y / 2, 0);// target to middle of unit
                    TargetON = true;
                }
            }

            Target = closest;

            yield return new WaitForSeconds(searchTimeDelay);
        }
    }
     void StepsEvent( int side)
    {
       m_AudioSource.PlayOneShot(SoundRoboStep, 0.1f);
        if (side==1) p_StepDustL.Play();
        else p_StepDustR.Play();
    }

    void PlaySoundEvent(AudioClip SoundPlay)
    {
        m_AudioSource.PlayOneShot(SoundPlay, 0.8f);
    }

    public void shotEvent(AudioClip SoundPlay) //shot
    {

        m_AudioSource.PlayOneShot(SoundPlay, 0.8f);
 
        var gameOb = (GameObject)Instantiate(shell, GunEnd.position, VerticalAxis.rotation);
        gameOb.GetComponent<Rigidbody>().linearVelocity = speedShell * GunEnd.transform.forward;
//        gameOb.layer = LayerMask.NameToLayer(_tag);
        p_smokeBarrel.Play();

    }
    public void bigShotEvent(AudioClip SoundPlay) //shot
    {

        m_AudioSource.PlayOneShot(SoundPlay, 0.8f);
        p_PlasmaShot.Play();
        StartCoroutine(bigShot());
    }

    private IEnumerator hideUnit()
    {
        //Wait for 15 seconds
        yield return shotDuration;
        //hide unit
        //	gameObject.SetActive (false);
    }
   private IEnumerator bigShot()
    {
        //Wait for 15 seconds
        yield return new WaitForSeconds(1.8f);
        var gameOb = (GameObject)Instantiate(bigShell, GunEnd.position, VerticalAxis.rotation);
        gameOb.GetComponent<Rigidbody>().linearVelocity = speedShell * GunEnd.transform.forward;
   //     gameOb.transform.localScale = new Vector3(4.0f, 4.0f, 4.0f);
     //   gameOb.layer = LayerMask.NameToLayer(_tag);
        gameOb.GetComponent<RC_Shell>().shellDamage =40;
        p_smokeBarrel.Play();
     
    }
    void OnDrawGizmos()
    {
        /*	
            Gizmos.color = Color.blue;
            Gizmos.DrawLine (HorizontalAxis.transform.position, HorizontalAxis.transform.position + dirXZ);
            Gizmos.DrawLine (HorizontalAxis.transform.position, HorizontalAxis.transform.position + forwardXZ);
            Gizmos.color = Color.green;
            Gizmos.DrawLine (HorizontalAxis.transform.position, HorizontalAxis.transform.position + dirYZ);
            Gizmos.DrawLine (HorizontalAxis.transform.position, HorizontalAxis.transform.position + forwardYZ);
            */
    }
}