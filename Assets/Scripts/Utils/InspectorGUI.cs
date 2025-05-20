using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    public float[] listA = new float[6] { 1, 2, 3, 4, 5, 6 };
    public float[] listB = new float[3] { 10, 20, 30 };
    public LanderController lander;
    public DeepQAgent agent;
    private bool showMenu = true;
    private bool showListA = true;
    private bool showListB = true;

    private bool showListC = true;
    private bool showVector = true;

    private Vector2 scrollPos;

    void Update()
    {
        listA = lander.angle;
        listB = lander.throttle;
        if (Input.GetKeyDown(KeyCode.F1))
            showMenu = !showMenu;
    }
    string maxStepInput;
    void OnGUI()
    {
        maxStepInput = agent.MaxStep.ToString();
        if (!showMenu) return;

        GUI.Box(new Rect(10, 10, 300, 400), "Debug Menu");
        GUILayout.BeginArea(new Rect(15, 35, 290, 360));
        scrollPos = GUILayout.BeginScrollView(scrollPos);

        showListA = EditorFoldout(showListA, "Thruster Angle");
        if (showListA)
        {
            for (int i = 0; i < listA.Length; i++)
                GUILayout.Label($"[{i}] = {listA[i]:F2}");
        }

        showListB = EditorFoldout(showListB, "Thruster Throttle");
        if (showListB)
        {
            for (int i = 0; i < listB.Length; i++)
                GUILayout.Label($"[{i}] = {listB[i]:F2}");
        }

        showListC = EditorFoldout(showListC, "Environment Constant");
        if (showListC)
        {
            GUILayout.Label($"Time Scale: {Time.timeScale:F2}");

            GUILayout.Label("Agent Max Step:");
            maxStepInput = GUILayout.TextField(maxStepInput);
            if (int.TryParse(maxStepInput, out int newMaxStep))
            {
                agent.MaxStep = newMaxStep;
            }
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    bool EditorFoldout(bool display, string title)
    {
        return GUILayout.Toggle(display, title, GUI.skin.button);
    }
}
