using System;
using System.Collections.Generic;
using System.Text;

//Universele health class. 
public class Health
{
    public delegate void DiedHandler();
    public event DiedHandler HasDiedEvent;

    public int HP => health;

    public delegate void HealthChangedHandler(int health);
    public event HealthChangedHandler HealthChangedEvent;

    private readonly int startingHealth;
    private int health;

    public Health(int health)
    {
        this.health = health;
        this.startingHealth = health;
    }

    public void Hit(int damage)
    {
        health -= damage;

        HealthChangedEvent?.Invoke(health);

        if (health <= 0)
        {
            HasDiedEvent?.Invoke();
        }
    }

    public void Reset()
    {
        health = startingHealth;
    }
}
