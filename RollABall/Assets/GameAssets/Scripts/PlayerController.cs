using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TMPro;
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
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] int totalSecond = 1000000;
    [SerializeField] AudioClip colidWithEnemy;
    [SerializeField] AudioClip victorySound;
    [SerializeField] AudioClip gameOverSound;

    private int jumpTimes = 0;
    private float normalForceMove;
    private int score = 0;
    private int winScore;
    private System.Timers.Timer timer;

    private bool isGameFinished;

    private Rigidbody rb;
    private Vector2 movementInput;
    void Start()
    {
        isGameFinished = false;
        rb = this.gameObject.GetComponent<Rigidbody>();
        normalForceMove = forceMove;
        score = 0;
        SetScore();
        winScore = listPickUp.transform.childCount;
        SetScore();
        winText.SetActive(false);
        countDownAsync();
    }

    private async Task countDownAsync()
    {
        int remainTime = totalSecond;
        TimeSpan timeSpan;
        while (!isGameFinished)
        {
            timeSpan = TimeSpan.FromSeconds(remainTime);
            timeText.text = timeSpan.ToString(@"mm\:ss");
            if(remainTime <= 0)
            {
                break;
            }
            await Task.Delay(1000);
            remainTime--;
        }

        

        if(score < winScore && !isGameFinished)
        {
            isGameFinished = true;
            onLose();
        }

        
    }

    private void onLose()
    {
        
        winText.GetComponentInChildren<TextMeshProUGUI>().text = "You Lose";
        AudioSource.PlayClipAtPoint(gameOverSound, transform.position, 1f);
        winText.SetActive(true);
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
        jumpTimes = 0;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            isGameFinished = true;
            AudioSource.PlayClipAtPoint(colidWithEnemy, transform.position, 1f);
            onLose();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp") && !isGameFinished)
        {
            other.gameObject.SetActive(false);
            score++;
            SetScore();
            onWin();
            if(collectionSound)
            {
                AudioSource.PlayClipAtPoint(collectionSound, transform.position, 1f);
            }    
        } 
           
    }

    private void onWin()
    {

        if (score >= winScore)
        {

            
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            isGameFinished = true;

            winText.GetComponentInChildren<TextMeshProUGUI>().text = "You Win";
            winText.SetActive(true);
            AudioSource.PlayClipAtPoint(victorySound, transform.position, 1f);
        }
    }

    private void SetScore()
    {
        scoreText.text = "Score: " + score.ToString() + " / " + winScore.ToString();
    }
}
