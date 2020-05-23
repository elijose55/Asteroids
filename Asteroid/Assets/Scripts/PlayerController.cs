using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
   public float speed = 8; // velocidade de rotacao
   public float speedMove = 30; // velocidade de translacao
   public float gravity = -9.8f; // valor da gravidade
   public LayerMask groundMask;
   CharacterController character;
   Vector3 velocity;
   bool isGrounded;
   private Animator anim;

   public AudioClip audioCoin;
   public AudioClip audioDeath;
   public AudioClip victoryClip;
    private int lives;
    private int checkpointNumber;
    private bool alive;
    public GameObject end_text;
    public GameObject victory_text;
    public GameObject start_text;

    public GameObject coinParent;

      void Start()
   {
       alive = true;
       checkpointNumber = 1;
       lives = 3;
       anim = GetComponent<Animator>();
       end_text = GameObject.Find("EndText");
       start_text = GameObject.Find("StartText");
       victory_text = GameObject.Find("VictoryText");

       end_text.SetActive(false);
       victory_text.SetActive(false);
       character = gameObject.GetComponent<CharacterController>();
       StartCoroutine(startWaiter()); 

   }

    IEnumerator startWaiter()
    {
       start_text.SetActive(true);
       yield return new WaitForSeconds (2f);
       start_text.SetActive(false);

    }
    IEnumerator waiter()
    {
       anim.SetFloat("virar", 0f);
       anim.SetFloat("correr", 0f);
       alive = false;
       end_text.SetActive(true);
       yield return new WaitForSeconds (1.5f);
       SceneManager.LoadScene("Menu");
    }

    IEnumerator victory()
    {
       anim.SetFloat("virar", 0f);
       anim.SetFloat("correr", 0f);
       alive = false;
       victory_text.SetActive(true);
       AudioSource.PlayClipAtPoint(victoryClip, this.gameObject.transform.position, 1.0f);
       yield return new WaitForSeconds (4f);

       SceneManager.LoadScene("Menu");
    }

   void OnCollisionEnter(Collision collision)
   {
        if (collision.gameObject.tag == "Coin") {
            ContactPoint contact = collision.contacts[0];
            Vector3 pos = contact.point;
            AudioSource.PlayClipAtPoint(audioCoin, pos, 1f);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Shrek" && alive) {
            StartCoroutine(waiter()); 

            ContactPoint contact = collision.contacts[0];
            Vector3 pos = contact.point;
            AudioSource.PlayClipAtPoint(audioDeath, pos, 1f);
        }

   }
 
   void Update()
   {

       if(coinParent.transform.childCount == 0 && alive){
           StartCoroutine(victory()); 
       }
 
       // Verifica se encostando no chão (o centro do objeto deve ser na base)
       isGrounded = Physics.CheckSphere(transform.position, 0.2f, groundMask);
      
       // Se no chão e descendo, resetar velocidade
       if(isGrounded && velocity.y < 0.0f) {
           velocity.y = -1.0f;
       }
 
       float x = Input.GetAxis("Horizontal");
       float z = Input.GetAxis("Vertical");
       if (alive){
            anim.SetFloat("virar", x);
            anim.SetFloat("correr", z);
        
            // Rotaciona personagem
            transform.Rotate(0, x * speed * 10 * Time.deltaTime, 0);
        
            // Move personagem
            Vector3 move = transform.forward * z;
            character.Move(move * Time.deltaTime * speedMove);
       }

 
       // Aplica gravidade no personagem
       velocity.y += gravity * Time.deltaTime;
       character.Move(velocity * Time.deltaTime);
 
   }


}
