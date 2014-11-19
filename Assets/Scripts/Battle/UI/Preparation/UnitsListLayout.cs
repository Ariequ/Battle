using UnityEngine;
using System.Collections;

public class UnitsListLayout : UIDragDropLayout {

	public UIScrollView srollView;

	private UIGrid uiGrid;

	private UITable uiTable;

	void Start ()
	{
		this.uiGrid = GetComponent<UIGrid>();
		this.uiTable = GetComponent<UITable>();
	}

	override public void Layout (Transform item)
	{
		UIDragScrollView dragScrollView = item.GetComponent<UIDragScrollView>();

		// Re-enable the drag scroll view script
		if (dragScrollView != null)
		{
			dragScrollView.scrollView = this.srollView;
			dragScrollView.enabled = true;
		}
		
		// Notify the widgets that the parent has changed
		NGUITools.MarkParentAsChanged(item.gameObject);
		
		if (uiTable != null) uiTable.repositionNow = true;
		if (uiGrid != null) uiGrid.repositionNow = true;
	}
}
