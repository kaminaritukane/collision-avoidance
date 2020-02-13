using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class dlltest : MonoBehaviour
{
    //[DllImport("Dll1")]
    //public static extern int calAdd(int a, int b);

    [DllImport("RVO")]
    public static extern IntPtr CreateSimulator();

    [DllImport("RVO")]
    public static extern void ReleaseSimulator(IntPtr sim);

    [DllImport("RVO")]
    public static extern void SetTimeStep(IntPtr sim, float timeStep);

    [StructLayout(LayoutKind.Sequential)]
    public struct RvoVector3
    {
        public float x;
        public float y;
        public float z;

        private static RvoVector3 _zero = new RvoVector3(0, 0, 0);

        public static RvoVector3 Zero => _zero;

        public RvoVector3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public RvoVector3(Vector3 v3Pos)
        {
            x = v3Pos.x;
            y = v3Pos.y;
            z = v3Pos.z;
        }
    }

    [DllImport("RVO")]
    public static extern void SetAgentDefaults(IntPtr sim,
        float neighborDist, ulong maxNeighbors,
        float timeHorizon, float radius, float maxSpeed,
        RvoVector3 velocity);

    [DllImport("RVO")]
    public static extern ulong AddAgent(IntPtr sim, RvoVector3 position);

    [DllImport("RVO")]
    public static extern ulong GetNumAgents(IntPtr sim);

    [DllImport("RVO")]
    public static extern bool GetAgentPosition(ref RvoVector3 pos, IntPtr sim, ulong agentNo);

    [DllImport("RVO")]
    public static extern bool GetAgentVelocity(ref RvoVector3 velocity, IntPtr sim, ulong agentNo);

    [DllImport("RVO")]
    public static extern bool GetAgentRadius(ref float pos, IntPtr sim, ulong agentNo);

    [DllImport("RVO")]
    public static extern void SetAgentPrefVelocity(IntPtr sim, ulong agentNo, RvoVector3 prefVelocity);

    [DllImport("RVO")]
    public static extern void DoStep(IntPtr sim);

    private IntPtr sim = IntPtr.Zero;

    private readonly List<Vector3> goals = new List<Vector3>();

    private bool reachedGoal = false;

    [SerializeField] GameObject prefab = null;
    private readonly List<Transform> gos = new List<Transform>();

    private float speed = 3.0f;

    private void Start()
    {
        //Debug.Log(calAdd(1, 2));

        sim = CreateSimulator();
        SetAgentDefaults(sim, 10.0f, 10, 1.0f, 0.5f, speed, RvoVector3.Zero);


        AddGO(new Vector3(10f, 0, 0), new Vector3(-10f, 0, 0));
        AddGO(new Vector3(-10f, 0, 0), new Vector3(10f, 0, 0));
        //AddGO(new Vector3(0, 0.1f, 10f), new Vector3(0, 0, -10f));
    }

    private void AddGO( Vector3 pos, Vector3 targetPos )
    {
        AddAgent(sim, new RvoVector3(pos));
        goals.Add(targetPos);

        var go = GameObject.Instantiate(prefab, pos, Quaternion.identity);
        gos.Add(go.transform);
    }

    private void Update()
    {
        if (reachedGoal)
        {
            return;
        }

        SetTimeStep(sim, Time.deltaTime);

        var nAgents = GetNumAgents(sim);
        for( ulong i=0; i<nAgents; ++i )
        {
            RvoVector3 pos = RvoVector3.Zero;
            RvoVector3 vel = RvoVector3.Zero;
            if ( GetAgentPosition(ref pos, sim, i ) 
                && GetAgentVelocity(ref vel, sim, i ) )
            {
                //Debug.Log($"{i}:{pos.x}, {pos.y}, {pos.z}");
                // update go position
                var v3Pos = new Vector3(pos.x, pos.y, pos.z);
                gos[(int)i].position = v3Pos;

                var v3Vel = new Vector3(vel.x, vel.y, vel.z);
                if (v3Vel.sqrMagnitude > float.Epsilon)
                {
                    gos[(int)i].rotation = Quaternion.LookRotation(v3Vel);
                }

                var goalVector = goals[(int)i] - v3Pos;
                if ( goalVector.sqrMagnitude > 1.0f )
                {
                    goalVector = goalVector.normalized * speed;
                }

                SetAgentPrefVelocity(sim, i, new RvoVector3(goalVector));
            }
        }

        DoStep(sim);

        reachedGoal = ReachGoal();
    }

    private bool ReachGoal()
    {
        RvoVector3 pos = RvoVector3.Zero;
        if ( GetAgentPosition(ref pos, sim, 0) )
        {
            float radius = 0.0f;
            if (GetAgentRadius(ref radius, sim, 0))
            {
                var v3Pos = new Vector3(pos.x, pos.y, pos.z);
                var goalVector = goals[0] - v3Pos;

                if (goalVector.sqrMagnitude < radius * radius)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnDisable()
    {
        ReleaseSimulator(sim);
    }
}
