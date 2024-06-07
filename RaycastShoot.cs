using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour
{
    public int GunDamage = 1; // ����������, ������� ����� ����� ��������� ������� ��� ��������
    public float FireRate = 0.25f; // ����� ��������������, ��� ����� ����� ����� �������� �� ������ ������
    public float WeaponRange = 50f; // ����������, ��� ������ ��� ��� ����� ��������� � �����
    public float HitForce = 100f; // ����, ������� ����� ����������� � �������
    public Transform GunEnd; // ����� ����������� Transform ������� GameObject, ������� ����� �������� �������, � ������� ����� ���������� ��� ���
    private Camera _camera; // ����� ��������� ������ �� ������ �� ������� ����, ����� ���������� �������, �� ������� ����� ������� �����.
    private WaitForSeconds _shotDuration = new WaitForSeconds(0.07f); // ����������, ��� ����� �� �����, ����� ����� ��������� ������� � ������� ������������� ����� ����, ��� ����� ���������. 
    private AudioSource _gunAudio; // ����� ������������ ��� ��������������� ��������� ������� ��������
    private LineRenderer _laserLine; // ����� ������ �� 2 ��� ����� ����� � 3D-������������ � ������ ������ ����� ����� ���� � ������� ������������.
    private float _nextFire; // ����� ���������� �����, � ������� ������ ����� ��������� ����� �������� ����� ��������.

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
