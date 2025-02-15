using UnityEngine;

public class YarnLinkTest : MonoBehaviour
{
    YarnStateLinker ysl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ysl = GetComponent<YarnStateLinker>();

        string result = "";
        Debug.Log(result);
        ysl.TryGetValue<string>("TestString", out result);
        Debug.Log(result);
        ysl.SetValue("TestString", "Bye!");
        string x;
        ysl.TryGetValue<string>("TestString", out x);
        Debug.Log(x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
