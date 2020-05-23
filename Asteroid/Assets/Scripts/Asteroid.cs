using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Asteroid : MonoBehaviour
{
    public Transform explosionPrefab;
    public AudioClip audioHit;
    public AudioClip audioExplosion;
    public GameObject end_text;
    private Renderer rend;
    IEnumerator waiter(GameObject instancia)
    {
        yield return new WaitForSeconds (0.5f);
       Destroy(instancia, 0.5f);
    }


    IEnumerator deathWaiter(GameObject instancia)
    {
        yield return new WaitForSeconds (0.5f);
        Destroy(instancia, 0.5f);
       yield return new WaitForSeconds (2f);
       SceneManager.LoadScene("Menu");
       Destroy(gameObject);

    }

   void OnCollisionEnter(Collision collision)
   {
        if (collision.gameObject.tag == "Bullet") {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            var instancia = Instantiate(explosionPrefab, pos, rot);

            AudioSource.PlayClipAtPoint(audioHit, pos, 1f);

            StartCoroutine(waiter(instancia.gameObject));
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Player") {
            end_text.GetComponent<Text>().text = "GAME OVER";
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            var instancia = Instantiate(explosionPrefab, pos, rot);

            AudioSource.PlayClipAtPoint(audioExplosion, pos, 1f);

            StartCoroutine(deathWaiter(instancia.gameObject));
            rend.enabled = false;
            
        }

   }

 
   void Start()
   {
       end_text = GameObject.Find("EndText");
       rend = GetComponent<Renderer>();
   }
 
   void Update()
   {
   }
}


