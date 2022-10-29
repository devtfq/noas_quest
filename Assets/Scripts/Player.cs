using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float walkSpeed = 4;
    public float runSpeed = 6;
    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;
    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;

    [SerializeField] private float rotSpeed = 200f;
    private bool shouldAim;

    private bool showLaser;
    public Transform FirePoint;
    public float MaxLength;
    public GameObject Prefab;
    private GameObject Instance;
    private EGA_Laser LaserScript;

    public TrailRenderer tracer;
    public ParticleSystem muzzleFlash,hitimpact;

    Animator anim;
    CharacterController controller;
    // Start is called before the first frame update
    void Awake(){
        Instance = Instantiate(Prefab, FirePoint.position, FirePoint.rotation);
        Instance.transform.parent = FirePoint;
        LaserScript = Instance.GetComponent<EGA_Laser>();
        Instance.SetActive(false);
        tracer.enabled = false;
    }

    void Start()
    {
        anim = GetComponent<Animator>() ;
        controller = GetComponent<CharacterController>();
        shouldAim = false;
        showLaser = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        if(inputDir != Vector2.zero){
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }
        if(Input.GetKey(KeyCode.D)){
            transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.A)){
            transform.Rotate(Vector3.up * -rotSpeed * Time.deltaTime);
        }
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = ((isRunning)? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        controller.Move(transform.forward * currentSpeed * Time.deltaTime);
        float animationPercent = ((isRunning)? 1 : 0.5f)* inputDir.magnitude;
        anim.SetFloat("speedPercent", animationPercent, speedSmoothTime, Time.deltaTime);
        if(Input.GetKeyDown(KeyCode.W)){
            shouldAim = !shouldAim;
            anim.SetBool("shouldAim", shouldAim);
        }
        if(shouldAim && Input.GetKeyDown(KeyCode.Z)){
            Fire();
        }
        if(shouldAim && Input.GetKeyDown(KeyCode.S)){
            Shoot();
        }
        
    }

    void Fire(){
        muzzleFlash.Play();
        tracer.enabled = true;
        tracer.AddPosition(FirePoint.position);
        RaycastHit hit2;
        if(Physics.Raycast(FirePoint.position, FirePoint.forward, out hit2, MaxLength)){
            Debug.Log("Hit the wall!");
            tracer.transform.position = hit2.point;
            hitimpact.transform.position = hit2.point;
            hitimpact.transform.forward = hit2.normal;
            hitimpact.Play();            
        }
        else{
            tracer.transform.position = FirePoint.position + FirePoint.forward * MaxLength ;

        }
    }

    void Shoot(){
        showLaser = !showLaser;
        if(showLaser){
            Instance.SetActive(true);
            RaycastHit hit;
            if (Physics.Raycast(FirePoint.position, FirePoint.forward, out hit, MaxLength)) {
                Debug.Log("Hit the wall!");
            }
        }
        else{
            Instance.SetActive(false);
        }
        
    }
}
