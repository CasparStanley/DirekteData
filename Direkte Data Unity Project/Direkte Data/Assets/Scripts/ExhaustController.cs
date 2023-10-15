using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustController : MonoBehaviour
{
    [Header("TESTING ----------------------------------")]
    [SerializeField] private bool TEST = false;
    [SerializeField] private int TEST_SPEED = 1;
    private bool direction = false;

    [Header("REFERENCES ------------------------------")]
    [SerializeField] private Renderer insideNozzleRenderer;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Renderer rend;

    [Header("VARIABLES -------------------------------")]
    [Range(0, 100)] public float surfaceToVacuumBlendshapeValue;
    [Range(0, 100)] public float startupBlendshapeValue;

    [Space(5)]
    
    [SerializeField] private bool useAnimationCurve = true;
    [ConditionalHide("useAnimationCurve", true)]
    [SerializeField] private AnimationCurve nozzleExitGlowCurve;

    [HideInInspector] public float startupTime;
    private bool hasIgnited = false;
    float currentAltitude = 0;

    private bool actualTest = false;

    // Start is called before the first frame update
    private void Awake()
    {
        rend = GetComponent<Renderer>();
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        if (TEST)
        {
            StartCoroutine(WaitASecond());
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (actualTest)
        {
            hasIgnited = true;

            if (startupTime <= 0)
            {
                startupTime = 0.8f;
            }

            Test();
        }

        if (hasIgnited)
        {
            UpdateExhaust();
        }
    }

    public void StartExhaust(Color startupColor1, Color startupColor2)
    {
        hasIgnited = true;
        startupBlendshapeValue = 100;

        if (rend != null)
        {
            foreach (var mat in rend.materials)
            {
                // Making them bright
                startupColor1 *= 8f;
                startupColor2 *= 8f;
                mat.SetColor("Exh_Color_Startup1", startupColor1);
                mat.SetColor("Exh_Color_Startup2", startupColor2);
            }
        } else
        {
            Debug.LogError("Renderer was Null");
        }

        Debug.Log($"<color=YELLOW>[STARTING ENGINE] Startup Blend Shape Value: {startupBlendshapeValue}, Startup Time: {startupTime}</color>");
    }

    private void UpdateExhaust()
    {
        // Change Shape - BlendShape
        skinnedMeshRenderer.SetBlendShapeWeight(0, surfaceToVacuumBlendshapeValue);
        skinnedMeshRenderer.SetBlendShapeWeight(1, startupBlendshapeValue); // TODO: Hydrogen exhaust doesn't have a startup blend shape

        if (startupTime != 0)
        {
            // Run Startup
            if (startupBlendshapeValue > 5)
            {

                startupBlendshapeValue = Mathf.Lerp(0, 100, startupTime);
                startupTime -= Time.deltaTime;
                //Debug.Log($"<color=ORANGE>[THROTTLE UP] Startup Blend Shape Value: {startupBlendshapeValue}</color>");

                foreach (var mat in rend.materials)
                {
                    mat.SetFloat("Exh_Startup", startupBlendshapeValue / 100);
                }
            }

            else
            { 
                startupTime = 0;
                startupBlendshapeValue = 0;
            }
        }

        // Change Color of Mesh

        // Set the colors in the materials to match the alitude
        if (surfaceToVacuumBlendshapeValue < 50)
        {
            foreach (var mat in rend.materials)
            {
                mat.SetFloat("Exh_SurfaceToHiAtmos", surfaceToVacuumBlendshapeValue / 50);
            }
        } else
        {
            foreach (var mat in rend.materials)
            {
                mat.SetFloat("Exh_HiAtmosToVacuum", (surfaceToVacuumBlendshapeValue / 50) - 1);
            }
        }

        // Change Color of the Nozzle Exit
        insideNozzleRenderer.material.SetFloat("NozzleGlow_SeaLvlToVac", nozzleExitGlowCurve.Evaluate(surfaceToVacuumBlendshapeValue / 100));
    }

    private void Test()
    {
        if (surfaceToVacuumBlendshapeValue <= 0 || surfaceToVacuumBlendshapeValue >= 100)
        {
            direction = !direction;
        }
        if (direction)
        {
            surfaceToVacuumBlendshapeValue += TEST_SPEED * Time.deltaTime;
        }
        else
        {
            surfaceToVacuumBlendshapeValue -= TEST_SPEED * Time.deltaTime;
        }
    }

    private IEnumerator WaitASecond()
    { 
        yield return new WaitForSeconds(1);
        actualTest = true;
    }

    private void OnDisable()
    {
        //hasIgnited = false;
    }
}
