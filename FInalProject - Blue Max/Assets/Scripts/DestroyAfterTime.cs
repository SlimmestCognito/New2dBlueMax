using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float destroyAfterTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyAfterTime);
    }
}
