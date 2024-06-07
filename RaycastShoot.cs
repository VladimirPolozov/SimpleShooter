using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour
{
    public int GunDamage = 1; // определяет, сколько урона будет наносится объекту при выстреле
    public float FireRate = 0.25f; // будет контролировать, как часто игрок может стрелять из своего оружия
    public float WeaponRange = 50f; // определяет, как далеко наш луч будет направлен в сцену
    public float HitForce = 100f; // сила, которая будет прилагаться к объекту
    public Transform GunEnd; // будет компонентом Transform пустого GameObject, который будет отмечать позицию, в которой будет начинаться наш луч
    private Camera _camera; // будет содержать ссылку на камеру от первого лица, чтобы определить позицию, из которой будет целится игрок.
    private WaitForSeconds _shotDuration = new WaitForSeconds(0.07f); // определить, как долго мы хотим, чтобы лазер оставался видимым в игровом представлении после того, как игрок выстрелил. 
    private AudioSource _gunAudio; // будем использовать для воспроизведения звукового эффекта стрельбы
    private LineRenderer _laserLine; // берет массив из 2 или более точек в 3D-пространстве и рисует прямую линию между ними в игровом пространстве.
    private float _nextFire; // будет удерживать время, в которое игроку будет разрешено снова стрелять после выстрела.

    private void Awake()
    {
        _laserLine = GetComponent<LineRenderer>();
        _gunAudio = GetComponent<AudioSource>();
        _camera = Camera.main;
    }


    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > _nextFire)
        {
            _nextFire = Time.time + FireRate;

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            RaycastHit hit;

            _laserLine.SetPosition(0, GunEnd.position);

            if (Physics.Raycast(rayOrigin, _camera.transform.forward, out hit, WeaponRange))
            {
                _laserLine.SetPosition(1, hit.point);
                ShootableBox Health = hit.collider.GetComponent<ShootableBox>();

                if (Health != null)
                {
                    Health.Damage(GunDamage);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * HitForce);
                }
            } else
            {
                _laserLine.SetPosition(1, rayOrigin + (_camera.transform.forward * WeaponRange));
            }
        }
    }


    private IEnumerator ShotEffect()
    {
        _gunAudio.Play();

        _laserLine.enabled = true;

        yield return _shotDuration;

        _laserLine.enabled = false;
    }
}
