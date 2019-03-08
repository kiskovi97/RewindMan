using UnityEngine;
using System.Collections.Generic;
using FixedPointy;

public class FixWorld : MonoBehaviour
{
    // PhysicalObjects And Or Forces need it
    public static Fix time = Fix.Zero;
    public static FixVec3 gravity = FixVec3.Zero;
    public static Fix deltaTime;

    public float timeOut = 0f;

    public static bool GameOver
    {
        get; private set;
    }

    public static bool Forward
    {
        get; private set;
    }

    public static bool Backward
    {
        get; private set;
    }

    public static FixVec3 GravitySizeVector(FixVec3 vector)
    {
        return vector.Normalize() * gravity.GetMagnitude();
    }

    public static void GameOverSet()
    {
        GameOver = true;
    }

    // Inner state
    private FixObject[] movingObjects;
    private FixPhyicObject[] collideables;
    private FixCollider[] colliders;
    private FixPlayer player;

    public void Awake()
    {
        gravity = FixConverter.ToFixVec3(Physics.gravity);
        deltaTime = FixConverter.ToFix(Time.fixedDeltaTime);
        time = Fix.Zero;
    }

    public void Start()
    {
        RigidObject[] rigidObjects = FindObjectsOfType<RigidObject>();
        FixCollider[] fixColliders = FindObjectsOfType<FixCollider>();
        MovingPlatform[] movingPlatforms = FindObjectsOfType<MovingPlatform>();
        
        List<FixObject> fixList = new List<FixObject>();
        fixList.AddRange(rigidObjects);
        fixList.AddRange(movingPlatforms);
        movingObjects = fixList.ToArray();

        List<FixPhyicObject> physicsList = new List<FixPhyicObject>();
        collideables = rigidObjects;

        List<FixCollider> colliderList = new List<FixCollider>();
        colliders = fixColliders;

        player = FindObjectOfType<FixPlayer>();
        GameOver = false;
    }

    private void FixedUpdate()
    {
        InputCheck();
        if (Forward && !GameOver)
        {
            MoveAll();
            time += deltaTime;
        }
        else if (Backward)
        {
            GameOver = false;
            time -= deltaTime;
            MoveAllBack();
        }
        timeOut = (float)time;
    }

    private void Update()
    {
        if (Forward)
        {
            player.KeyCheck();
        }
    }

    private void InputCheck()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Forward = false;
            if (time > Fix.Zero) Backward = true;
            else Backward = false;
        }
        else
        {
            Forward = true;
            Backward = false;
        }
    }

    private void MoveAll()
    {

        for (int i = 0; i < movingObjects.Length; i++)
        {
            movingObjects[i].Move();
        }
        for (int i = 0; i < collideables.Length; i++)
        {
            List<Collision> collisions = new List<Collision>();
            for (int j = colliders.Length - 1; j >= 0; j--)
            {
                Collision collision = collideables[i].GetCollision(colliders[j]);
                if (collision != null)
                    collisions.Add(collision);
            }
            collideables[i].Collide(collisions.ToArray());
        }
    }

    private void MoveAllBack()
    {
        for (int i = collideables.Length - 1; i >= 0; i--)
        {
            List<Collision> collisions = new List<Collision>();
            for (int j = colliders.Length - 1; j >= 0; j--)
            {
                Collision collision = collideables[i].GetCollision(colliders[j]);
                if (collision != null)
                    collisions.Add(collision);
            }
            collideables[i].CollideBack(collisions.ToArray());
        }
        for (int i = movingObjects.Length - 1; i >= 0; i--)
        {
            movingObjects[i].MoveBackwards();
        }
    }
}
