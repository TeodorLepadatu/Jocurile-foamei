using UnityEngine;
using System.Collections;

public class PlayerContemporary : PlayerBehaviour
{
    private GameObject heldObject = null;
    private GameObject nearbyObject = null;
    private string portName = null;
    public GameObject pccgeSprite;
    [SerializeField] private GameObject[] objectsToActivate;
    [SerializeField] private GameObject[] objectToDeactivate;


    protected override void Update()
    {
        base.Update(); // Keep movement

        HandlePickup();
    }

    private void HandlePickup()
    {
        if (Input.GetKeyDown(KeyCode.E) && nearbyObject != null && heldObject == null)
        {
            heldObject = nearbyObject;
            heldObject.GetComponent<Collider2D>().enabled = false;
            portName = heldObject.name;

            Debug.Log(portName);
        }

        if (Input.GetKeyDown(KeyCode.Q) && heldObject != null)
        {
            GameObject dropZone = GameObject.Find("PCNOCGESprite");
            if (dropZone != null && Vector2.Distance(heldObject.transform.position, dropZone.transform.position) < 3f)
            {
                Destroy(heldObject);

                if (pccgeSprite != null) 
                    pccgeSprite.SetActive(true);

                dropZone.SetActive(false);
                heldObject = null;

                StartCoroutine(DelayedActivate());

                return;
            }

            heldObject.GetComponent<Collider2D>().enabled = true;
            heldObject = null;
        }

        if (heldObject != null)
        {
            heldObject.transform.position = transform.position + new Vector3(0.5f, 0.5f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pickable"))
        {
            nearbyObject = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pickable") && other.gameObject == nearbyObject)
        {
            nearbyObject = null;
        }
    }

    private IEnumerator DelayedActivate()
    {
        yield return new WaitForSeconds(3f);
        
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        foreach (GameObject obj in objectToDeactivate)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

}
