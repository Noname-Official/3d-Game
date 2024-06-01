using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected GameObject foundItem;
    [SerializeField] protected string text;

    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }

    // TODO (Timon): Spam proof
    public void Interact()
    {
        TMP_Text textObj = GameObject.Find("InspectText").GetComponent<TMP_Text>();
        textObj.text = text;
        StartCoroutine(ExecuteAfterTime(1f, () => textObj.text = ""));

        if (foundItem != null)
        {
            PlayerController.PickUp(Instantiate(foundItem).transform);
        }
        OnInteract();
    }

    public virtual void OnInteract() { }
}
