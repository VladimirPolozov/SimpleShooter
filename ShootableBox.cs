using UnityEngine;

public class ShootableBox : MonoBehaviour
{
    public int СurrentHealth = 3;

    public void Damage(int damageAmount)
    {
        СurrentHealth -= damageAmount;

        if (СurrentHealth <= 0)

        {
            gameObject.SetActive(false);
        }
    }
}
