using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour, IDamagable
{
    private UIManager uiManager;
    [SerializeField] private int coinsCollected;
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
    [SerializeField] private int currentHealth;


    [SerializeField] private Transform swordAttackPosition;
    [SerializeField] private float swordAttackRange;
    [SerializeField] private LayerMask dragonLayerMask;
    [SerializeField] private int swordDamage;
    [SerializeField] private float swordAttackCooldown;
    [SerializeField] private float swordAttackCooldownTimeLeft;
    [SerializeField] private bool canSwordAttack;
    public int StartingHealth { get; set; }

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
        StartingHealth = 100;
        playerSprite = GetComponent<SpriteRenderer>().sprite;
        uiManager = FindObjectOfType<UIManager>();
        currentMoveSpeed = originalMoveSpeed;
        currentHealth = StartingHealth;
        uiManager.ChangeKnightHealth(0, currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        FlipSprite();
        ScreenCollision();
        SwordAttack();
        if(Input.GetKeyDown(KeyCode.I) && !canStartDashing)
        {
            canStartDashing = true;
        }

        if(canStartDashing && !startDashCoolDown)
        {
            DashAbility();
        }

        if(startDashCoolDown)
        {
            if(coolDownTime < totalCoolDownTime)
            {
                coolDownTime += Time.deltaTime;
            }
            else
            {
                coolDownTime = 0.0f;
                startDashCoolDown = false;
            }
        }

        if(currentHealth <= 0)
        {
            //print("Game over");
        }
    }

    void Movement()
    {
        //Should always be moving in the direction of the last player input that wasn't zero.
        //Start moving to the right when game starts

        if(Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
        {
             lastPlayerInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f);
            moveDirection = new Vector3(lastPlayerInput.x, lastPlayerInput.y, 0.0f);
        }


        transform.position += moveDirection.normalized * currentMoveSpeed * Time.deltaTime;
    }   


    void ScreenCollision()
    {
        //Ensure player can't collide with the 4 sides of the screen.
        //If they go over a side push them back.

        Vector3 screenVector = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        if (transform.position.x - playerSprite.bounds.extents.x < -screenVector.x)
        {
            transform.position = new Vector3 (-screenVector.x + playerSprite.bounds.extents.x,transform.position.y,transform.position.z);
            print("Left edge exceeded");
        }
        else if (transform.position.x + playerSprite.bounds.extents.x > screenVector.x)
        {
           transform.position = new Vector3(screenVector.x - playerSprite.bounds.extents.x, transform.position.y, transform.position.z);
            print("Right edge exceeded");
        }
        else if(transform.position.y + playerSprite.bounds.extents.y > screenVector.y)
        {
            transform.position = new Vector3(transform.position.x, screenVector.y - playerSprite.bounds.extents.y, transform.position.z);
            print("Top edge exceeded");
        }
        else if (transform.position.y - playerSprite.bounds.extents.y < -screenVector.y)
        {
            transform.position = new Vector3(transform.position.x, -screenVector.y + playerSprite.bounds.extents.y, transform.position.z);
            print("Bottom edge exceeded");
        }

    }

    void DashAbility()
    {
        if(dashTime < totalDashTime && canStartDashing)
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

    void FlipSprite()
    {
        if(Input.GetAxisRaw("Horizontal") < 0)
        {
            Vector3 tempScale = transform.localScale;
            tempScale.x = -1f;
            transform.localScale = tempScale;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            Vector3 tempScale = transform.localScale;
            tempScale.x = 1f;
            transform.localScale = tempScale;
        }
    }

    void SwordAttack()
    {
        if (swordAttackCooldownTimeLeft > 0 && !canSwordAttack)
        {
            swordAttackCooldownTimeLeft -= Time.deltaTime;
        }

        if (swordAttackCooldownTimeLeft <= 0)
        {
            swordAttackCooldownTimeLeft = 0f;
            canSwordAttack = true;
        }

        if (canSwordAttack && Input.GetKeyDown(KeyCode.J))
        {
           Collider2D[] col = Physics2D.OverlapCircleAll(swordAttackPosition.position, swordAttackRange, dragonLayerMask);

            for (int i = 0; i < col.Length; i++)
            {
                print("Hello" + col[i].gameObject);
                //We need to damage the dragon.
                //How? We need to get the dragon's IDamagable script
                IDamagable dragonDamagable = col[i].GetComponent<IDamagable>();
                if (dragonDamagable != null)
                {
                    print("HWEWE");
                    dragonDamagable.TakeDamage(swordDamage);
                }
            }
            swordAttackCooldownTimeLeft = swordAttackCooldown;
            canSwordAttack = false;
        }
    }

    public void TakeDamage(int _damage)
    {
        print("Current health: " + currentHealth);
        print("Damage" + _damage);
        currentHealth -= _damage;
        print("Current health: " + currentHealth);
        print("Damage" + _damage);
        uiManager.ChangeKnightHealth(_damage, currentHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Pillar"))
        {
            moveDirection = -moveDirection;
            transform.localScale = -transform.localScale;
        }

        if(collision.CompareTag("Coin"))
        {
            coinsCollected++;
            uiManager.ChangeCoinText(coinsCollected);
            Destroy(collision.gameObject);
        }
    }
}
