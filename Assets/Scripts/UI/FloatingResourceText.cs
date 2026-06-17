using TMPro;
using UnityEngine;

public class FloatingResourceText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float lifeTime = 1f;

    private Camera mainCamera;
    private float timer;

    private void Awake()
    {
        if (text == null)
            text = GetComponentInChildren<TMP_Text>();

        mainCamera = Camera.main;
    }

    public void Initialize(string message)
    {
        if (text != null)
            text.text = message;
    }

    private void Update()
    {
        transform.position += moveSpeed * Time.deltaTime * Vector3.up;

        if (mainCamera != null)
            transform.LookAt(transform.position + mainCamera.transform.forward);

        timer += Time.deltaTime;

        if (timer >= lifeTime)
            Destroy(gameObject);
    }
}
