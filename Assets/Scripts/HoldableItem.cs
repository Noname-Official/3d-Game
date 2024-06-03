public class HoldableItem : Interactable
{
    private void Start()
    {
        foundItem = gameObject;
    }

    public override void OnInteract()
    {
        Destroy(gameObject);
    }

    public virtual void OnDrop() { }
}
