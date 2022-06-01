using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour, IDamagable
{
    private UIManager uiManager;
    [SerializeField] private int currentMoveSpeed;
    [SerializeField] private int originalMoveSpeed;
    [SerializeField] private int dashSpeed;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private int damage;
    private Vector3 lastPlayerInput;
    private Sprite playerSprite;
    private bool canStartDashing;
    private bool startDashCoolDown;
    [SerializeField] private float totalDashTime;
    [SerializeField] private float dashTime;
    [SerializeField] private float totalCoolDownTime;
    [SerializeField] private float coolDownTime;

    public int StartingHealth { get; set; }
    [SerializeField] private int currentHealth;
    public int Health
    {
        get
        {
            return currentHealth;
        }
        set
        {
            Health = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        StartingHealth = 100;
        playerSprite = GetComponent<SpriteRenderer>().sprite;
        currentMoveSpeed = originalMoveSpeed;
        currentHealth = StartingHealth;
        uiManager.ChangeDragonHealth(0, currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
       // ScreenCollision();
        if (Input.GetKeyDown(KeyCode.I) && !canStartDashing)
        {
            canStartDashing = true;
        }

        if (canStartDashing && !startDashCoolDown)
        {
            DashAbility();
        }

        if (startDashCoolDown)
        {
            if (coolDownTime < totalCoolDownTime)
            {
                coolDownTime += Time.deltaTime;
            }
            else
            {
                coolDownTime = 0.0f;
                startDashCoolDown = false;
            }
        }
    }

    void Movement()
    {
        //Should always be moving in the direction of the last player input that wasn't zero.
        //Start moving to the right when game starts

        if (Input.GetAxisRaw("WASDHorizontal") != 0.0f || Input.GetAxisRaw("WASDVertical") != 0.0f)
        {
            lastPlayerInput = new Vector3(Input.GetAxisRaw("WASDHorizontal"), Input.GetAxisRaw("WASDVertical"), 0.0f);
            moveDirection = new Vector3(lastPlayerInput.x, lastPlayerInput.y, 0.0f);
        }
        transform.position += moveDirection.normalized * currentMoveSpeed * Time.deltaTime;
    }


    void ScreenCollision()
    {
        //Ensure player can't collide with the 4 sides of the screen.
        //If they go over a side push them back.
        Vector3 edgesOfCamera = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * Camera.main.transform.position.x, Screen.height + Camera.main.transform.position.y, 0));
        if (transform.position.x + playerSprite.bounds.extents.x < edgesOfCamera.x)
        {
            print("Left edge exceeded");
        }
        else if (transform.position.x + playerSprite.bounds.extents.x < edgesOfCamera.x)
        {
            print("Right edge exceeded");
        }
        else if (transform.position.y + playerSprite.bounds.extents.y > edgesOfCamera.y)
        {
            print("player " + transform.position.y + playerSprite.bounds.extents.y);
            print("camera" + edgesOfCamera.y);
            print("Top edge exceeded");
        }
        else if (transform.position.y + playerSprite.bounds.extents.y < edgesOfCamera.y)
        {
            print("Bottom edge exceeded");
        }

    }

    void DashAbility()
    {
        if (dashTime < totalDashTime && canStartDashing)
        {
            currentMoveSpeed = dashSpeed;
            dashTime += Time.deltaTime;
        }
        else
        {
            currentMoveSpeed = originalMoveSpeed;
            dashTime = 0.0f;
            canStartDashing = false;
            startDashCoolDown = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Knight"))
        {
            IDamagable knightDamagable = collision.gameObject.GetComponent<IDamagable>();
            print("Dragon deals damage to knight");
            knightDamagable.TakeDamage(damage);
        }

        if (collision.CompareTag("Pillar"))
        {
            moveDirection = -moveDirection;
            transform.localScale = -transform.localScale;
        }
    }

    public void TakeDamage(int _damage)
    {
        currentHealth -= damage;
    }
}
