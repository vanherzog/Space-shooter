using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Enemy : MonoBehaviour
{
    public float hpmax;
    private float hp;
    public float damage;
    public Rigidbody2D rb;
    public float movementDistance = 2f; // Distance to move up and down
    public float movementSpeed = 2f; // Speed of movement

    private Vector2 startPosition;
    private bool movingUp;
    private float currentDistance;

    public AudioClip boostSound;
    public AudioClip hitSound;
    private AudioSource audioSource;
    public GameObject laserPrefab;
    public GameObject laserPrefabplus45;
    public GameObject laserPrefabminus45;
    public GameObject laserPrefabspecial;
    public float moveSpeed;
    public float wait;
    public SpaceShip spaceship;
    public float wave;
    public TextMeshProUGUI damageTextPrefab;
    public new ParticleSystem particleSystem;
    private Transform canvasTransform;
    private float destroyDelay = 1f;
    public float moveSpeed2 = 1f;
    public float fadeSpeed2 = 1f;


    private bool dead;


    private void OnEnable()
    {
        dead = false;
        hp = hpmax;

        audioSource = GetComponent<AudioSource>();
        transform.position = new Vector2(0f, 3f);
        startPosition = transform.position;
        movingUp = true;
        currentDistance = 0f;
        StartCoroutine(Fire());
        StartCoroutine(Fire2());
        StartCoroutine(Fire3());
    }

    private void Update()
    {
        if (dead == false)
        {
            if (movingUp)
            {
                currentDistance += movementSpeed * Time.deltaTime;
                if (currentDistance >= movementDistance)
                {
                    currentDistance = movementDistance;
                    movingUp = false;
                }
            }
            else
            {
                currentDistance -= movementSpeed * Time.deltaTime;
                if (currentDistance <= -movementDistance)
                {
                    currentDistance = -movementDistance;
                    movingUp = true;
                }
            }

            Vector2 newPosition = startPosition + Vector2.right * currentDistance;
            transform.position = newPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Laser"))
        {
            UpdateHP(-damage);
            Destroy(other.gameObject);
            audioSource.clip = hitSound;
            audioSource.Play();
            GameObject canvasObject = GameObject.Find("Canvas");
            canvasTransform = canvasObject.transform;
            Vector3 gameObjPosition = transform.position;

            // Convert the game object's position to viewport coordinates
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(gameObjPosition);

            // Convert the viewport coordinates to the local position of the canvas
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform as RectTransform, new Vector2(viewportPos.x * Screen.width, viewportPos.y * Screen.height), null, out canvasPos);

            // Instantiate the text object at the canvas position
            TextMeshProUGUI dT = Instantiate(damageTextPrefab, canvasPos, Quaternion.identity);
            dT.transform.SetParent(canvasTransform, false);

            StartCoroutine(MoveAndFade(dT));
        }
    }

    public IEnumerator MoveAndFade(TextMeshProUGUI dT)
    {
        RectTransform dTTransform = dT.GetComponent<RectTransform>();
        float timer = 0f;
        while (timer < destroyDelay)
        {
            Vector3 newPos = dTTransform.position + Vector3.up * moveSpeed2 * Time.deltaTime * 50f;
            dTTransform.position = newPos;
            Debug.Log(newPos);
            timer += Time.deltaTime;
            yield return null;
        }

        Color startColor = dT.color;
        float alpha = startColor.a;

        while (alpha > 0)
        {
            alpha -= fadeSpeed2 * Time.deltaTime;
            dT.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        Destroy(dT.gameObject);
    }


    public void UpdateHP(float dif)
    {
        hp += dif;
        if(hp < 0 && dead == false)
        {
            dead = true;
            particleSystem.Play();
            StartCoroutine(Hit());
        }
    }

    private IEnumerator Hit()
    {
        Debug.Log(gameObject.ToString());
        Debug.Log(hp);
        audioSource.clip = boostSound;
        audioSource.Play();
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
        spaceship.Wave(true);
    }

    private IEnumerator Fire()
    {
        while (true && dead == false)
        {
            Vector2 vector2 = new Vector2(transform.position.x, transform.position.y - 1.5f) ;
            if (wave == 5f)
            {
                vector2 = new Vector2(transform.position.x, transform.position.y - 2f) ;
            }
            GameObject laser = Instantiate(laserPrefab, vector2, Quaternion.identity);
            Rigidbody2D laserRB = laser.GetComponent<Rigidbody2D>();
            laserRB.velocity = new Vector2(0f, -moveSpeed);

            yield return new WaitForSeconds(wait);
            /**
            if (!IsGameObjectInView(laser))
            {
                Debug.Log("JAP");
                Destroy(laser);
            }
            **/
        }
    }

    private IEnumerator Fire2()
    {
        if(wave==5f && dead == false)
        {
            while (true)
            {
                Vector2 startPosition = new Vector2(transform.position.x + 0.8f, transform.position.y - 1.5f);
                GameObject laser = Instantiate(laserPrefabminus45, startPosition, Quaternion.identity);
                Rigidbody2D laserRB = laser.GetComponent<Rigidbody2D>();
                laserRB.velocity = new Vector2(2.5f, -moveSpeed);         

                Vector2 startPosition2 = new Vector2(transform.position.x - 0.8f, transform.position.y - 1.5f);
                GameObject laser2 = Instantiate(laserPrefabplus45, startPosition2, Quaternion.identity);
                Rigidbody2D laserRB2 = laser2.GetComponent<Rigidbody2D>();
                laserRB2.velocity = new Vector2(-2.5f, -moveSpeed);         

                yield return new WaitForSeconds(wait * 2f);
            }
        }
    }

    private IEnumerator Fire3()
    {
        if(wave==5f && dead == false)
        {
            while (true)
            {
                Vector2 startPosition3 = new Vector2(transform.position.x, transform.position.y - 1.5f);
                GameObject laser3 = Instantiate(laserPrefabspecial, startPosition3, Quaternion.identity);
                Rigidbody2D laserRB3 = laser3.GetComponent<Rigidbody2D>();
                laserRB3.velocity = new Vector2(0f, -moveSpeed);
                yield return new WaitForSeconds(1f);
                
                float angle = 10f;
                while (angle < 360)
                {
                    angle += 10f; 

                    Vector2 direction = Quaternion.Euler(0f, 0f, -angle) * Vector2.down; // Calculate the direction vector based on the angle
                    Vector2 velocity = direction.normalized * moveSpeed/1.5f;
                    laserRB3.velocity =  velocity; 
                    yield return new WaitForSeconds(0.1f);
                }
                
                yield return new WaitForSeconds(wait * 1.5f);
            }
        }
    }

    bool IsGameObjectInView(GameObject go)
    {
        Vector3 positionInViewport = Camera.main.WorldToViewportPoint(go.transform.position);
        return positionInViewport.x > 0f && positionInViewport.x < 1f &&
            positionInViewport.y > 0f && positionInViewport.y < 1f;
    }
}
