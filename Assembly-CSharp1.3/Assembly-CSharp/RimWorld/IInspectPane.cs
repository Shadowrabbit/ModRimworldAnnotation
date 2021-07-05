using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001355 RID: 4949
	public interface IInspectPane
	{
		// Token: 0x1700150F RID: 5391
		// (get) Token: 0x060077CE RID: 30670
		// (set) Token: 0x060077CF RID: 30671
		float RecentHeight { get; set; }

		// Token: 0x17001510 RID: 5392
		// (get) Token: 0x060077D0 RID: 30672
		// (set) Token: 0x060077D1 RID: 30673
		Type OpenTabType { get; set; }

		// Token: 0x17001511 RID: 5393
		// (get) Token: 0x060077D2 RID: 30674
		bool AnythingSelected { get; }

		// Token: 0x17001512 RID: 5394
		// (get) Token: 0x060077D3 RID: 30675
		IEnumerable<InspectTabBase> CurTabs { get; }

		// Token: 0x17001513 RID: 5395
		// (get) Token: 0x060077D4 RID: 30676
		bool ShouldShowSelectNextInCellButton { get; }

		// Token: 0x17001514 RID: 5396
		// (get) Token: 0x060077D5 RID: 30677
		bool ShouldShowPaneContents { get; }

		// Token: 0x17001515 RID: 5397
		// (get) Token: 0x060077D6 RID: 30678
		float PaneTopY { get; }

		// Token: 0x060077D7 RID: 30679
		void DrawInspectGizmos();

		// Token: 0x060077D8 RID: 30680
		string GetLabel(Rect rect);

		// Token: 0x060077D9 RID: 30681
		void DoInspectPaneButtons(Rect rect, ref float lineEndWidth);

		// Token: 0x060077DA RID: 30682
		void SelectNextInCell();

		// Token: 0x060077DB RID: 30683
		void DoPaneContents(Rect rect);

		// Token: 0x060077DC RID: 30684
		void DoLabelIcons(string label);

		// Token: 0x060077DD RID: 30685
		void CloseOpenTab();

		// Token: 0x060077DE RID: 30686
		void Reset();
	}
}
