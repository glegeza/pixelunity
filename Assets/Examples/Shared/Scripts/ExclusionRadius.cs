using UnityEngine;

public class ExclusionRadius : MonoBehaviour
{
    public float Radius;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
