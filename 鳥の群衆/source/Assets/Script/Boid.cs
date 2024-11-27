using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    //��{�ݒ�
    private float speed = 5f;
    private float rotationSpeed = 2f;
    private float neighborRadius = 15f;
    private float separationDistance = 6f;
    private float separationStrength = 3.0f;
    private float maxSpeed = 6f;
    private float minSpeed = 2f;

    public float treeAvoidanceRange = 10f;
    private float treeAvoidanceStrength = 10f;

    public List<Boid> allBoids; //�V�[�����̂��ׂĂ�Boid��ێ�

    private Vector3 velocity;   //���݂̈ړ����x

    private int frameCounter = 0;
    private const int calculationFrequency = 5; //�v�Z�̕p�x�i5�t���[�����ƂɌv�Z�j

    private float minAltitude = 15f;
    private float maxAltitude = 25f;
    private float altitudeAdjustmentStrength = 2f;

    void Start()
    {
        //�������x
        velocity = transform.forward * speed;
    }

    void Update()
    {
        frameCounter++;

        //���t���[�����Ƃɗ͂��v�Z
        if (frameCounter % calculationFrequency == 0)
        {
            CalculateForces();
            frameCounter = 0;
        }

        //Boid���ړ�
        MoveBoid();
    }

    void CalculateForces()
    {
        //���͂�Boid���擾
        List<Boid> nearbyBoids = GetNearbyBoids();

        //�e�͂̌v�Z
        Vector3 alignment = Align(nearbyBoids) * 0.5f; //����̗�
        Vector3 cohesion = Cohere(nearbyBoids) * 1.0f; //�����̗�
        Vector3 separation = Separate(nearbyBoids) * separationStrength; //�����̗�
        Vector3 treeAvoidance = DetectAndAvoidTreesInPath() * 2.5f;      //��Q������̗�
        Vector3 altitudeAdjustment = MaintainAltitude(); //���x�ێ��̗�

        //�S�Ă̗͂����v���đ��x�ɉ��Z
        Vector3 force = alignment + cohesion + separation + treeAvoidance + altitudeAdjustment;
        velocity += force * Time.deltaTime;

        //���x����
        velocity = LimitSpeed(velocity);
    }

    void MoveBoid()
    {
        //�ʒu���X�V
        Vector3 newPosition = transform.position + velocity * Time.deltaTime;
        transform.position = newPosition;

        // Boid�̌������X�V
        UpdateRotation();
    }

    Vector3 LimitSpeed(Vector3 vel)
    {
        //���x���ŏ��l�ƍő�l�̊Ԃɐ���
        float speed = vel.magnitude;
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        return vel.normalized * speed;
    }

    Vector3 MaintainAltitude()
    {
        //���x�����͈͓��ɕۂ�
        Vector3 altitudeForce = Vector3.zero;

        if (transform.position.y < minAltitude)
        {
            altitudeForce += Vector3.up * (minAltitude - transform.position.y) * altitudeAdjustmentStrength;
        }
        else if (transform.position.y > maxAltitude)
        {
            altitudeForce += Vector3.down * (transform.position.y - maxAltitude) * altitudeAdjustmentStrength;
        }

        return altitudeForce;
    }

    List<Boid> GetNearbyBoids()
    {
        //���͂�Boid�����������X�g�Ɋi�[
        List<Boid> nearbyBoids = new List<Boid>();
        foreach (Boid otherBoid in allBoids)
        {
            if (otherBoid != this && Vector3.Distance(transform.position, otherBoid.transform.position) < neighborRadius)
            {
                nearbyBoids.Add(otherBoid);
            }
        }
        return nearbyBoids;
    }

    Vector3 Align(List<Boid> nearbyBoids)
    {
        //����̗͂��v�Z
        Vector3 alignmentForce = Vector3.zero;
        foreach (Boid otherBoid in nearbyBoids)
        {
            alignmentForce += otherBoid.velocity;
        }

        if (nearbyBoids.Count > 0)
        {
            alignmentForce /= nearbyBoids.Count;
            alignmentForce = alignmentForce.normalized * speed;
        }

        return alignmentForce;
    }

    Vector3 Cohere(List<Boid> nearbyBoids)
    {
        //�����̗͂��v�Z
        Vector3 cohesionForce = Vector3.zero;
        foreach (Boid otherBoid in nearbyBoids)
        {
            cohesionForce += otherBoid.transform.position;
        }

        if (nearbyBoids.Count > 0)
        {
            cohesionForce /= nearbyBoids.Count;
            cohesionForce -= transform.position;
            cohesionForce = cohesionForce.normalized * speed;
        }

        return cohesionForce;
    }

    Vector3 Separate(List<Boid> nearbyBoids)
    {
        //�����̗͂��v�Z
        Vector3 separationForce = Vector3.zero;
        foreach (Boid otherBoid in nearbyBoids)
        {
            float distance = Vector3.Distance(transform.position, otherBoid.transform.position);
            if (distance < separationDistance)
            {
                separationForce += (transform.position - otherBoid.transform.position).normalized / distance;
            }
        }

        return separationForce.normalized * separationStrength;
    }

    Vector3 DetectAndAvoidTreesInPath()
    {
        //Tree�^�O�����o���ĉ��
        Vector3 avoidDirection = Vector3.zero;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, treeAvoidanceRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Tree"))
            {
                Vector3 directionAway = transform.position - hitCollider.bounds.center;
                avoidDirection += directionAway.normalized;
            }
        }

        return avoidDirection.normalized * treeAvoidanceStrength;
    }

    void UpdateRotation()
    {
        //Boid�̌��������݂̈ړ������ɍ��킹��
        Vector3 direction = velocity.normalized;
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
