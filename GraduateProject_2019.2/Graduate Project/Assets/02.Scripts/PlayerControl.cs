using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;
    private float j = 0.0f;
    private float mouseY = 0.0f;
    private float mouseX = 0.0f;

    //접근하는 컴포넌트는 변수에 할당 후 사용.--> 하지 않았을 경우, 어떤 단점있는지(??)
    private Transform tr;
    //이동 속도 변수(public = Inspector에 노출시킴)
    [SerializeField]
    private float MoveSpeed = 10.0f;

    [SerializeField]
    private float rotationSpeed = 1.0f;


    [SerializeField]
    private Vector3 playerDirection;

    [SerializeField]
    private Animator animator;

    //void Awake()
    //{
    //실행될때 한번 호출.
    //게임의 상태값, 변수 초기화에 사용.
    //스크립트 비활성화돼 있어도 실행.
    //코루틴 실행 불가
    //}
    // Start is called before the first frame update
    void Start()
    {//Update 이전에 한번 호출.
     //활성화되야 실행.
     //다른 스크립트의 모든 Awake가 실행되고나서 실행됨.
     //코루틴 실행 가능

        // 최적화 위해 start에서 미리 접근시켜두고, position을 update에서 조금씩 바꾸기.
        //스크립트 실행 후 처음 수행되는 start함수에 Transform 컴포넌트 할당.

        playerDirection = new Vector3();

        tr = GetComponent<Transform>();
        //tr = this.GetComponent<Transform>(); 과 같음.이 스크립트가 포함된 게임오브젝트가 가진 컴포넌트 중에서 transform을 추출, tr변수에 넣는다.

        //Generic타입. = 형식 매개 변수->형변환 필요 없음. 소스코드 간결
        //tr = GetComponent("Transform") as Transform;
        //tr = (Transform)GetComponent(typeof(Transform));

        animator = GetComponent<Animator>();

    }
    // Update is called once per frame
    void Update()
    {//활성화되야 실행.
     //핵심 로직 작성되있음.
     //매 프레임마다 실행->최적화에 주의->transform값 start에서 미리 접근시켜두고, position을 update에서 조금씩 바꾸기.

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        j = Input.GetAxis("Jump");

        mouseY = Input.GetAxis("Mouse Y");
        mouseX = Input.GetAxis("Mouse X");

        Vector3 rotation = new Vector3(0/*-mouseY*/, mouseX, 0);

        tr.Rotate(rotation * rotationSpeed);

        //Debug.Log:키보드 입력되었을 때, 반환값 확인. 디버깅 정보 텍스트 형식으로 출력해줌.
        //Debug.Log(" 출력할 문자열. 이때, h와 v는 float타입변수. ToString()이용해서 문자열로 변형한다. ");
        //Debug.Log("h = " + h.ToString());
        //Debug.Log("v = " + v.ToString());
        //Debug.Log("j = " + j.ToString());

        //print("mouse Y : " + mouseY.ToString());
        //print("mouse X : " + mouseX.ToString());


        // G좌표계와 Player Local좌표계의 차이 Quaternion
        Quaternion quat = Quaternion.FromToRotation(Vector3.forward, tr.forward);


        //Translate(move direction,* moveSpeed * deltatime * 기준 좌표)
        playerDirection = new Vector3(h, 0, v);
        playerDirection.Normalize();
        playerDirection = quat * playerDirection;

        // 실제 이동
        tr.position += (playerDirection * MoveSpeed * Time.deltaTime * (int)Space.Self);

        //tr.Translate(playerDirection * MoveSpeed * Time.deltaTime * (int)Space.Self);

        //if (Input.GetKey(KeyCode.W))
        //{
        //    tr.Translate(tr.forward * MoveSpeed * Time.deltaTime * (int)Space.Self);

        //}

        //if (Input.GetKey(KeyCode.S))
        //{
        //    tr.Translate(-tr.forward * MoveSpeed * Time.deltaTime * (int)Space.Self);

        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    tr.Translate(-tr.right * MoveSpeed * Time.deltaTime * (int)Space.Self);

        //}

        //if (Input.GetKey(KeyCode.D))
        //{
        //    tr.Translate(tr.right * MoveSpeed * Time.deltaTime * (int)Space.Self);

        //}

        // 애니메이션 최신화
        // move > idle 
        if (h == 0 && v == 0)
        {
            animator.SetBool("isMoving", false);

        }
        else // idle > move
        {
            animator.SetBool("isMoving", true);
            // move direction / speed
            animator.SetFloat("positionX", h);
            animator.SetFloat("positionY", v);
        }



    }

    //void LateUpdate()
    //{
    //Update함수에서 전처리가 끝난 후 실행해야 하는 로직에 사용-->카메라 이동에 주로 사용.(?? update에 넣지 않는 이유: )
    //}

    //void FixedUpdate()
    //{
    //}

    //void OnEnable()
    //{
    //이벤트 연결에 사용
    //활성화되야 실행
    //코루틴 불가
    //}

    //void OnDisable()
    //{
    //비활성화(게임오브젝트, 스크립트)때 호출됨.
    //이벤트 연결 종료
    //코루틴 불가
    //}

    //void OnGUI()
    //{
    //(???)
    //}
}
