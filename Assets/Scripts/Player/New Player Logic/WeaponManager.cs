using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager
{
    Weapon _weapon;

    public KeyCode _keyToAttack;
    public string _buttonToAttack;

    //private float _arrowSpeed;
    //private int _arrowDamage;

    public WeaponManager(Weapon weaponType)
    {
        _weapon = weaponType;
    }

    public void Attack()
    {
        _weapon.Attack();
    }

    public void KeysToAttack(KeyCode key)
    {
        _keyToAttack = key;
    }
    public void ButtonToAttack(string button)
    {
        _buttonToAttack = button;
    }

    public void SetArrowProperties(float speed, int damage)
    {
        _weapon.SetProperties(speed, damage);
    }
}


