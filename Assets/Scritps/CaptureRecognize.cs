using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GestureRecognizer;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CaptureRecognize : MonoBehaviour {


    public bool RegisterFigure = false;
    public string RegisterFigureName = "";
    public bool useProtractor = false;
    public string libraryToLoad = "figures";

    public Image imageView;
    public Text timer;
    public Text score;
    public static float maxTime = 10;
    float timeLeft = maxTime;

    public Button restart;

    public static int _score;   


    public RectTransform gestureLimitRectBounds;
    Rect gestureLimitRect;
    Canvas parentCanvas;


    public float distanceBetweenPoints = 10f;
    public int minimumPointsToRecognize = 10;

    LineRenderer gestureRenderer;

    [SerializeField]
    private GameObject TrailPrefab;
    private GameObject trail;
    

    Vector3 virtualKeyPosition = Vector2.zero;
    Vector2 point;
    List<Vector2> points = new List<Vector2>();
    private Camera MainCam;
    GestureLibrary gl;
    Gesture gesture;
    Result result;

    public static bool EndGame = false;

    Sprite[] sprites;

    public delegate void GameEvent(Result r);
    public static event GameEvent OnRecognition;

    void Awake() {            
        sprites = Resources.LoadAll<Sprite>("Figures");
        RandomGesture();
    }

    void RandomGesture()
    {
        imageView.sprite = sprites[Random.Range(0, sprites.Length)];
    }

  
    void Start() {
        EndGame = false;
        MainCam = Camera.main;
        gl = new GestureLibrary(libraryToLoad);       
    }


    void Update() {


        if (Input.GetMouseButtonDown(0))
        {
            trail = Instantiate(TrailPrefab, (Input.mousePosition), transform.rotation);
        }


        if (Utility.IsTouchDevice())
        {
            if (Input.touchCount > 0)
            {
                virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            }
        }

        if (Input.GetMouseButton(0)) {

            virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            trail.transform.position = MainCam.ScreenPointToRay(Input.mousePosition).GetPoint(2);
                 RegisterPoint();

        }
           
            if (Input.GetMouseButtonUp(0)) {
                Destroy(trail, 1f);

                if (points.Count > minimumPointsToRecognize) {
                if (RegisterFigure)
                {
                    if(RegisterFigureName != "")
                    {
                        gesture = new Gesture(points, RegisterFigureName);
                        gl.AddGesture(gesture);
                    }
                    else
                    {
                        Debug.Log("The name for the new element is empty");
                    }                                 
                }
                else
                {

                    gesture = new Gesture(points);
                    result = gesture.Recognize(gl, useProtractor);

                    if (OnRecognition != null)
                    {
                        OnRecognition(result);
                    }

                    if (result.Name == imageView.sprite.name)
                    {
                        Debug.Log("Next Level");
                        maxTime -= 2f;
                        timeLeft = maxTime;
                        _score += 1;
                        RandomGesture();
                    }
                }


                }

                ClearGesture();
            }

            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                timer.text = "Time left: " + Mathf.Round(timeLeft).ToString();
            }
            else
            {
                imageView.gameObject.SetActive(false);
                timer.gameObject.SetActive(false);
                restart.gameObject.SetActive(true);
                score.text = "Score: " + _score.ToString();
                score.gameObject.SetActive(true);
                EndGame = true;
            }
        

    }
  
    void RegisterPoint() {
        //print(EndGame);
        if (!EndGame)
        {
            point = new Vector2(virtualKeyPosition.x, -virtualKeyPosition.y);

            if (points.Count == 0 || (points.Count > 0 && Vector2.Distance(point, points[points.Count - 1]) > distanceBetweenPoints))
            {
                points.Add(point);
            }
        }
        
    }
   
    void ClearGesture() {
        points.Clear();
      
    }

}
