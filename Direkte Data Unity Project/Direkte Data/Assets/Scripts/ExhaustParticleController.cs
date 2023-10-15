using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ExhaustParticleController : MonoBehaviour
{
    [Header("TESTING ----------------------------------")]
    [SerializeField] private bool TEST = false;
    [SerializeField] private int TEST_SPEED = 1;
    private bool direction = false;

    [Header("REFERENCES ------------------------------")]
    [SerializeField] [Range(0, 100)] private float surfaceToVacuum;
    [SerializeField] private Color exhParticlesColorSea;
    [SerializeField] private Color exhParticlesColorVac;
    [SerializeField] private float startSizeSea = 11;
    [SerializeField] private float startSizeVac = 25;
    [SerializeField] private float lifetimeSea = 1;
    [SerializeField] private float lifetimeVac = 2;
    [SerializeField] private float speedSea = 100;
    [SerializeField] private float speedVac = 50;

    private ParticleSystem exhaustParticles;

    // Start is called before the first frame update
    void Awake()
    {
        exhaustParticles = GetComponent<ParticleSystem>();
        exhaustParticles.startSize = startSizeSea;
        exhaustParticles.startLifetime = lifetimeSea;
        exhaustParticles.startSpeed = speedSea;
    }

    // Update is called once per frame
    void Update()
    {
        if (TEST)
        {
            Test();
        }

        UpdateExhaust();
    }

    private void UpdateExhaust()
    {
        // Change Color of Particle System
        exhaustParticles.startColor = Color.Lerp(exhParticlesColorSea, exhParticlesColorVac, surfaceToVacuum / 100);
        exhaustParticles.startSize = Mathf.Lerp(startSizeSea, startSizeVac, surfaceToVacuum / 100);
        exhaustParticles.startLifetime = Mathf.Lerp(lifetimeSea, lifetimeVac, surfaceToVacuum / 100);
        exhaustParticles.startSpeed = Mathf.Lerp(speedSea, speedVac, surfaceToVacuum / 100);
    }

    private void Test()
    {
        if (surfaceToVacuum <= 0 || surfaceToVacuum >= 100)
        {
            direction = !direction;
        }
        if (direction)
        {
            surfaceToVacuum += TEST_SPEED * Time.deltaTime;
        }
        else
        {
            surfaceToVacuum -= TEST_SPEED * Time.deltaTime;
        }
    }
}
