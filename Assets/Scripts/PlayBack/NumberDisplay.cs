using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberDisplay : MonoBehaviour
{
    public static NumberDisplay Instance;
    int count;
    public int Count
    {
        get { return count; }
        set { count = value; textMesh.text = value.ToString(); }
    }
    public TextMeshProUGUI textMesh;
    private void Start()
    {
        Instance = this;
        Count = 0;
    }
}
