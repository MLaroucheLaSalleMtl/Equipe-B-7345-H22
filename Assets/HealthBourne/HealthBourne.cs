using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBourne : MonoBehaviour
{
    [SerializeField] private Animator crystal;
    [SerializeField] private PlayerStats player;
    private CapsuleCollider col;
    [SerializeField][Range(5,200)] private float resetTimer;
    // Start is called before the first frame update
    void Start()
    {
        crystal = GetComponentInChildren<Animator>();
        col = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(player.HealthPoints != player.MaxHP)
            {
                player.HealthPoints = player.MaxHP;
                crystal.gameObject.SetActive(false);
                col.enabled = false;
                StartCoroutine(ReactivateHealthbourne());
            }
        }
    }
    private IEnumerator ReactivateHealthbourne()
    {
        yield return new WaitForSeconds(resetTimer);
        crystal.gameObject.SetActive(true);
        col.enabled = true;
    }

}
