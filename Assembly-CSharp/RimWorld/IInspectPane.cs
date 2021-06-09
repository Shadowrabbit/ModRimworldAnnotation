using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AED RID: 6893
	public interface IInspectPane
	{
		// Token: 0x170017E5 RID: 6117
		// (get) Token: 0x060097D4 RID: 38868
		// (set) Token: 0x060097D5 RID: 38869
		float RecentHeight { get; set; }

		// Token: 0x170017E6 RID: 6118
		// (get) Token: 0x060097D6 RID: 38870
		// (set) Token: 0x060097D7 RID: 38871
		Type OpenTabType { get; set; }

		// Token: 0x170017E7 RID: 6119
		// (get) Token: 0x060097D8 RID: 38872
		bool AnythingSelected { get; }

		// Token: 0x170017E8 RID: 6120
		// (get) Token: 0x060097D9 RID: 38873
		IEnumerable<InspectTabBase> CurTabs { get; }

		// Token: 0x170017E9 RID: 6121
		// (get) Token: 0x060097DA RID: 38874
		bool ShouldShowSelectNextInCellButton { get; }

		// Token: 0x170017EA RID: 6122
		// (get) Token: 0x060097DB RID: 38875
		bool ShouldShowPaneContents { get; }

		// Token: 0x170017EB RID: 6123
		// (get) Token: 0x060097DC RID: 38876
		float PaneTopY { get; }

		// Token: 0x060097DD RID: 38877
		void DrawInspectGizmos();

		// Token: 0x060097DE RID: 38878
		string GetLabel(Rect rect);

		// Token: 0x060097DF RID: 38879
		void DoInspectPaneButtons(Rect rect, ref float lineEndWidth);

		// Token: 0x060097E0 RID: 38880
		void SelectNextInCell();

		// Token: 0x060097E1 RID: 38881
		void DoPaneContents(Rect rect);

		// Token: 0x060097E2 RID: 38882
		void CloseOpenTab();

		// Token: 0x060097E3 RID: 38883
		void Reset();
	}
}
