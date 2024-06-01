public class HoldableItem : Interactable
{
    public override void OnInteract()
    {
        Destroy(transform.parent.gameObject);
    }
}
