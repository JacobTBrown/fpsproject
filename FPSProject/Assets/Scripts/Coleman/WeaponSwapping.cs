using UnityEngine;

public class WeaponSwapping : MonoBehaviour
{
    public int selectedWeapon = 0;
    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedWeapon == 0) selectedWeapon++;
            else selectedWeapon--;
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if(i == selectedWeapon) weapon.gameObject.SetActive(true);
            else weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
