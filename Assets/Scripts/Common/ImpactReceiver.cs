using UnityEngine;
using Player;
using System.Collections;

//На каком языке мы пишем комментарии? Я так и не понял, буду писать на английском
public class ImpactReceiver : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100)]
    private float mass = 3.0F; 
    private Vector3 impact = Vector3.zero;
    private CharacterController2D character;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<CharacterController2D>();      
    }
    // call this function to add an impact force:
    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
        StartCoroutine(ApplyImpact());
    }

    IEnumerator ApplyImpact()
    {
        if (impact.magnitude > 0.2F)
        {
            // apply the impact force + recalculating collision state
            character.move(impact * Time.deltaTime);
            // consumes the impact energy each cycle:
            impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
            yield return null;
            //without it player slides on the ground for some time
            if (!character.isGrounded)
                StartCoroutine(ApplyImpact());
        }
        else
        {
            yield break;
        }
    }
}

