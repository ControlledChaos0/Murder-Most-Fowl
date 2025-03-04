using UnityEngine;

public class Script : MonoBehaviour
{
    [SerializeField]
    private float m_scrollSpeed = 1.0f;
    private float totalScrollLength = 60.0f;
    [SerializeField]
    private float startX = 37.7f;
    [SerializeField]
    private float scrollTuner = 3.0f;
    private Vector3 transformVector = new(0.005f, 0, 0);

    [SerializeField]
    private GameObject child1 = null;
    [SerializeField]
    private GameObject child2 = null;
    [SerializeField]
    private GameObject child3 = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //totalScrollLength = sr.bounds.size.x * 3.0f;
        transformVector *= m_scrollSpeed;
        startX = transform.position.x;

        SpriteRenderer sr = child1.GetComponent<SpriteRenderer>();
        totalScrollLength = sr.bounds.size.x * scrollTuner;

    }

    // Update is called once per frame
    void Update()
    {
        // CHILD 1 UPDATE
        if (child1.transform.position.x <= startX - totalScrollLength)
        {
            child1.transform.position = new Vector3(startX, child1.transform.position.y, child1.transform.position.z);
        }

        child1.transform.position = child1.transform.position - transformVector;

        // CHILD 2 UPDATE
        if (child2.transform.position.x <= startX - totalScrollLength)
        {
            child2.transform.position = new Vector3(startX, child2.transform.position.y, child2.transform.position.z);
        }

        child2.transform.position = child2.transform.position - transformVector;

        // CHILD 3 UPDATE
        if (child3.transform.position.x <= startX - totalScrollLength)
        {
            child3.transform.position = new Vector3(startX, child3.transform.position.y, child3.transform.position.z);
        }

        child3.transform.position = child3.transform.position - transformVector;

    }
}
