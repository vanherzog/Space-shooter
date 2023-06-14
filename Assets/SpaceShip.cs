using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SpaceShip : MonoBehaviour
{
    public Playerinputactions playerControls;
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private InputAction fire;
    private InputAction fire3;
    private InputAction fireExtra;

    public GameObject laserPrefab;
    public AudioClip laserSound;
    private bool isFiring = false;
    private float fireDelay = 0.7f;
    private float lastFireTime = 0f;
    public Image fuelBarImage;
    public Image fuelBarImage2;

    private float timer = 0f;
    private float timer2 = 0f;

    public AudioClip hitSound;
    public AudioClip godClip;
    private AudioSource audioSource;
    public TextMeshProUGUI godText;
    public float hp;
    public float explosionPositionY;
    private bool god = false;
    private float wave;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    public GameObject enemy5;
    public Enemy enemy_1;
    public Enemy enemy_2;
    public Enemy enemy_3;
    public Enemy enemy_4;
    public Enemy enemy_5;
    public GameObject menu;
    public float level;
    public float maxLevel;
    private float waveLevel;
    private float counter = 0f;

    private void Awake()
    {
        playerControls = new Playerinputactions();
        audioSource = GetComponent<AudioSource>();
        wave = 1f;
        waveLevel = 1f;
        maxLevel = PlayerPrefs.GetFloat("MaxLevel");
        Wave(false);
        enemy_1 = enemy1.GetComponent<Enemy>();
        enemy_2 = enemy2.GetComponent<Enemy>();
        enemy_3 = enemy3.GetComponent<Enemy>();
        enemy_4 = enemy4.GetComponent<Enemy>();
        enemy_5 = enemy5.GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        counter += 1f;
        Wave(false);
        move = playerControls.Player.Move;
        move.Enable();
        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += StartFiring;
        fire.canceled += StopFiring;

        fire3 = playerControls.Player.Fire3;
        fire3.Enable();
        fire3.performed += Fire3;

        fireExtra = playerControls.Player.FireExtra;
        fireExtra.Enable();
        fireExtra.performed += FireExtra;
        if(counter > 1)
        {
            changeStats(false);
        }
        

    }

    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
        fire3.Disable();
        fireExtra.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        //Wave(false);

        moveDirection = move.ReadValue<Vector2>();
        timer -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        UpdateTimer3();
        UpdateTimerExtra();

        if (Input.GetKeyDown(KeyCode.G))
        {
            if(god == true)
            {
                god = false;
            }
            else
            {
                god = true;
            }
            godMode();
        }
    }

    private void UpdateTimer3()
    {
        if (timer > 0f)
        {
            fuelBarImage.gameObject.SetActive(true);
            float fillAmount = timer/5f;
            Vector3 newScale = new Vector3(fillAmount, 1f, 1f);
            fuelBarImage.rectTransform.localScale = newScale;
        }
        else
        {
            fuelBarImage.gameObject.SetActive(false);
        }
    }

    private void UpdateTimerExtra()
    {
        if (timer2 > 0f)
        {
            fuelBarImage2.gameObject.SetActive(true);
            float fillAmount2 = timer2 / 10f;
            Vector3 newScale2 = new Vector3(fillAmount2, 1f, 1f);
            fuelBarImage2.rectTransform.localScale = newScale2;
        }
        else
        {
            fuelBarImage2.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Laser") && hp > 0f)
        {
            StartCoroutine(FadeOutAudio());

            UpdateHP(-2);
            Debug.Log("Got Hit from Laser");
            Destroy(other.gameObject);
        }
    }

    public void Wave(bool next)
    {
        StartCoroutine(Wavee(next));
    }

    private void changeStats(bool up)
    {
        if (up)
        {
            enemy_1.hpmax *= 1.5f;
            enemy_1.wait *= 0.75f;
            enemy_1.movementSpeed *= 2f;
            enemy_2.hpmax *= 1.5f;
            enemy_2.wait *= 0.75f;
            enemy_2.movementSpeed *= 2f;
            enemy_3.hpmax *= 1.5f;
            enemy_3.wait *= 0.75f;
            enemy_3.movementSpeed *= 2f;
            enemy_4.hpmax *= 1.5f;
            enemy_4.wait *= 0.75f;
            enemy_4.movementSpeed *= 2f;
            enemy_5.hpmax *= 1.5f;
            enemy_5.wait *= 0.75f;
            enemy_5.movementSpeed *= 2f;
        }
        /**
        else
        {
            enemy_1.hpmax *= 0.75f;
            enemy_1.wait *= 1.33f;
            enemy_1.movementSpeed *= 0.5f;
            enemy_2.hpmax *= 0.75f;
            enemy_2.wait *= 1.33f;
            enemy_2.movementSpeed *= 0.5f;
            enemy_3.hpmax *= 0.75f;
            enemy_3.wait *= 1.33f;
            enemy_3.movementSpeed *= 0.5f;
            enemy_4.hpmax *= 0.75f;
            enemy_4.wait *= 1.33f;
            enemy_4.movementSpeed *= 0.5f;
            enemy_5.hpmax *= 0.75f;
            enemy_5.wait *= 1.33f;
            enemy_5.movementSpeed *= 0.5f;
        }
        **/
    }

    public IEnumerator Wavee(bool next)
    {
        
        if(next == true)
        {
            if(maxLevel<waveLevel)
            {
                maxLevel = waveLevel;
                // Saving the maxLevel value
                PlayerPrefs.SetFloat("MaxLevel", maxLevel);
                PlayerPrefs.Save();
                Debug.Log(maxLevel);
            }

            if(wave == 5f)
            {
                wave = 0f;
                hp += 2f;
                changeStats(true);
            }

            waveLevel += 1f;
            wave += 1f;
            Debug.Log("next wave");
            Debug.Log(wave);
        }

        if (wave == 1f) 
        {
            //enemy5.SetActive(false);
            yield return new WaitForSeconds(2f);
            enemy1.SetActive(true);
        }
        else if(wave == 2f)
        {
            //enemy1.SetActive(false);
            yield return new WaitForSeconds(2f);
            enemy2.SetActive(true);
        }
        else if(wave == 3f)
        {
            //enemy2.SetActive(false);
            yield return new WaitForSeconds(2f);
            enemy3.SetActive(true);
        }
        else if(wave == 4f)
        {
            //enemy3.SetActive(false);
            yield return new WaitForSeconds(2f);
            enemy4.SetActive(true);
        }
        else if(wave == 5f)
        {
            //enemy4.SetActive(false);
            yield return new WaitForSeconds(2f);
            enemy5.SetActive(true);
        }
    }

    private IEnumerator FadeOutAudio()
    {
        Debug.Log("Fadeout");
        audioSource.clip = hitSound;
        audioSource.Play();
        float elapsedTime = 0f;
        float startVolume = audioSource.volume;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / 1f);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        Debug.Log("Fadeout2");

    }

    public void UpdateHP(float dif)
    {
        if (god == false)
        {
            hp += dif;
            if (hp < 0)
            {
                StartCoroutine(Death());
            }
        }
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(2f);
        wave = 1f;
        waveLevel = 1f;
        gameObject.SetActive(false);
        enemy1.SetActive(false);
        enemy2.SetActive(false);
        enemy3.SetActive(false);
        enemy4.SetActive(false);
        enemy5.SetActive(false);
        menu.SetActive(true);
        hp = 1f;
    }

    private void StartFiring(InputAction.CallbackContext context)
    {
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(FireContinuously());
        }
    }

    private void StopFiring(InputAction.CallbackContext context)
    {
        if (isFiring)
        {
            isFiring = false;
        }
    }

    private IEnumerator FireContinuously()
    {
        while (isFiring)
        {
            if (Time.time - lastFireTime >= fireDelay)
            {
                Vector2 vector2 = new Vector2(transform.position.x, transform.position.y + 1f);
                GameObject laser = Instantiate(laserPrefab, vector2, Quaternion.identity);
                Rigidbody2D laserRB = laser.GetComponent<Rigidbody2D>();
                laserRB.velocity = new Vector2(0f, moveSpeed);

                // Play laser sound effect
                audioSource.clip = laserSound;
                audioSource.Play();

                lastFireTime = Time.time;
            }

            yield return null;
        }
    }

    private void Fire3(InputAction.CallbackContext context)
    {
        // 3 lasers at once with timer
        if (timer < 0)
        {
            Vector2 position2 = transform.position;
            Vector2 position3 = transform.position;
            position2.x = position2.x - 1f;
            position3.x = position3.x + 1f;

            Vector2 vector2 = new Vector2(transform.position.x, transform.position.y + 1f);
            GameObject laser = Instantiate(laserPrefab, vector2, Quaternion.identity);
            Rigidbody2D laserRB = laser.GetComponent<Rigidbody2D>();
            laserRB.velocity = new Vector2(0f, moveSpeed);

            GameObject laser2 = Instantiate(laserPrefab, position2, Quaternion.identity);
            Rigidbody2D laserRB2 = laser2.GetComponent<Rigidbody2D>();
            laserRB2.velocity = new Vector2(0f, moveSpeed);

            GameObject laser3 = Instantiate(laserPrefab, position3, Quaternion.identity);
            Rigidbody2D laserRB3 = laser3.GetComponent<Rigidbody2D>();
            laserRB3.velocity = new Vector2(0f, moveSpeed);

            // Play laser sound effect
            audioSource.clip = laserSound;
            Debug.Log("Fire2");
            audioSource.Play();
            timer = 5f;

            if (!IsGameObjectInView(laser))
            {
                Destroy(laser);
                Destroy(laser2);
                Destroy(laser3);
            }
        }
        
        

    }

    public GameObject explosionPrefab; // Prefab for the explosion effect
    public float explosionRadius = 5f; // Radius of the explosion

    private void FireExtra(InputAction.CallbackContext context)
    {
        if (timer2 < 0)
        {
            // Shoot a single fire
            Vector2 vector2 = new Vector2(transform.position.x, transform.position.y + 1f);
            GameObject laser = Instantiate(laserPrefab, vector2, Quaternion.identity);
            Rigidbody2D laserRB = laser.GetComponent<Rigidbody2D>();
            laserRB.velocity = new Vector2(0f, moveSpeed);

            // Play laser sound effect
            audioSource.clip = laserSound;
            Debug.Log("Fire3");
            audioSource.Play();
            timer2 = 20f;

            StartCoroutine(CheckExplosion(laserRB));
        }
    }

    private IEnumerator CheckExplosion(Rigidbody2D laserRB)
    {
        while (true)
        {
            if (laserRB.position.y >= explosionPositionY)
            {
                Explode(laserRB.position);
                break;
            }

            yield return null;
        }
    }

    private void Explode(Vector2 explosionPosition)
    {
        // Instantiate explosion effect
        GameObject explosion = Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);

        // Destroy the fire
        Destroy(explosion, 1f);

        // Get all colliders within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius);

        foreach (Collider2D collider in colliders)
        {
            // Apply explosion force to rigidbodies
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (rb.position - explosionPosition).normalized;
                rb.AddForce(direction * 0.5f, ForceMode2D.Impulse);
            }
        }
        StartCoroutine(Explosion(explosionPosition));
    }

    IEnumerator Explosion(Vector2 explosionPosition)
    {
        god = true;
        for (int j = 0; j < 2; j++)
        {
            yield return new WaitForSeconds(0.15f);
            float angleIncrement = 360f / 20f;
            for (int h = 0; h < 3; h++)
            {
                for (int i = 0; i < 20; i++)
                {
                    GameObject dogeE = Instantiate(laserPrefab, explosionPosition, Quaternion.identity);
                    Vector3 direction = Quaternion.Euler(0f, 0f, i * angleIncrement) * Vector3.right;
                    StartCoroutine(MoveDoge(dogeE, direction));
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
        yield return new WaitForSeconds(1.5f);
        god=false;
    }

    IEnumerator MoveDoge(GameObject doge, Vector3 direction)
    {
        while (IsGameObjectInView(doge))
        {
            Vector3 newPosition = doge.transform.position + direction;
            doge.transform.position = Vector3.Lerp(doge.transform.position, newPosition, 0.5f);
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(doge);
    }
    bool IsGameObjectInView(GameObject go)
    {
        Vector3 positionInViewport = Camera.main.WorldToViewportPoint(go.transform.position);
        return positionInViewport.x > 0f && positionInViewport.x < 1f &&
            positionInViewport.y > 0f && positionInViewport.y < 1f;
    }

    private void godMode()
    {
        if(god==true)
        {
            audioSource.clip = godClip;
            audioSource.Play();
            godText.gameObject.SetActive(true);
        }
        else
        {
            godText.gameObject.SetActive(false);
        }
        
    }

}
