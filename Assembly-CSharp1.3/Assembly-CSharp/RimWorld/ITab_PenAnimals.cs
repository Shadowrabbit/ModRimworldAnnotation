using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001350 RID: 4944
	public class ITab_PenAnimals : ITab_PenBase
	{
		// Token: 0x060077AD RID: 30637 RVA: 0x002A2641 File Offset: 0x002A0841
		public ITab_PenAnimals()
		{
			this.size = ITab_PenAnimals.WinSize;
			this.labelKey = "TabPenAnimals";
		}

		// Token: 0x060077AE RID: 30638 RVA: 0x002A266A File Offset: 0x002A086A
		public override void OnOpen()
		{
			base.OnOpen();
			this.animalFilterState.quickSearch.Reset();
		}

		// Token: 0x060077AF RID: 30639 RVA: 0x002A2684 File Offset: 0x002A0884
		protected override void FillTab()
		{
			CompAnimalPenMarker selectedCompAnimalPenMarker = base.SelectedCompAnimalPenMarker;
			Map map = selectedCompAnimalPenMarker.parent.Map;
			Rect rect;
			Rect rect2;
			new Rect(0f, 0f, ITab_PenAnimals.WinSize.x, ITab_PenAnimals.WinSize.y).ContractedBy(10f).SplitHorizontally(18f, out rect, out rect2, 0f);
			ThingFilterUI.DoThingFilterConfigWindow(rect2, this.animalFilterState, selectedCompAnimalPenMarker.AnimalFilter, AnimalPenUtility.GetFixedAnimalFilter(), 1, null, null, false, null, map);
		}

		// Token: 0x04004290 RID: 17040
		private static readonly Vector2 WinSize = new Vector2(300f, 480f);

		// Token: 0x04004291 RID: 17041
		private ThingFilterUI.UIState animalFilterState = new ThingFilterUI.UIState();
	}
}
