using System;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float forceMove = 10f;
    [SerializeField] float forceJump = 3f;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject winText;
    [SerializeField] GameObject listPickUp;
    [SerializeField] AudioClip collectionSound;

    private int jumpTimes = 0;
    private float normalForceMove;
    private int score = 0;
    private int winScore = 26;

    private Rigidbody rb;
    private Vector2 movementInput;
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        normalForceMove = forceMove;
        score = 0;
        SetScore();
        winScore = listPickUp.transform.childCount;
        winText.SetActive(false);
    }

    private void FixedUpdate()
    {

        Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);

        // Approaching 2
        //Vector3 targetPosition = rb.position + move * speed * Time.fixedDeltaTime;
        //rb.MovePosition(targetPosition);

        // Approaching 3
        rb.AddForce(move * forceMove);
    }
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnJump(InputValue jumpValue)
    {
        jumpTimes++;
        if (jumpTimes <= 2)
        {
            rb.AddForce(Vector3.up * forceJump, ForceMode.Impulse);
        }
    }

    async Task OnBoost(InputValue boostValue)
    {
        if (forceMove != normalForceMove) return;
        forceMove *= 2f;
        await Task.Delay(1000);
        forceMove = normalForceMove;       
    }

    void Update()
    {
        // Approaching 1
        //Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);
        //this.transform.position += speed * Time.deltaTime * move;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpTimes = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            score++;
            SetScore();
            ShowWinNotification();
            if(collectionSound)
            {
                AudioSource.PlayClipAtPoint(collectionSound, transform.position, 1f);
            }    
        }    
    }

    private void ShowWinNotification()
    {
        if(score >= winScore)
            winText.SetActive(true);
    }

    private void SetScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    private void OnReplay()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
