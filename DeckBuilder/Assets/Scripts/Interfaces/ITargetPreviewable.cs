using UnityEngine;

public interface ITargetPreviewable
{

    public abstract bool TargetPreviewActive { get; }

    public void SetTargetPreview(Color color);
    public bool IsSelectable();

    public void HideTargetPreview();
}
