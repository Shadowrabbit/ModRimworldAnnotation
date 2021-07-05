using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001351 RID: 4945
	public class ITab_PenAutoCut : ITab_PenBase
	{
		// Token: 0x060077B1 RID: 30641 RVA: 0x002A2717 File Offset: 0x002A0917
		public ITab_PenAutoCut()
		{
			this.size = ITab_PenAutoCut.WinSize;
			this.labelKey = "TabPenAutoCut";
		}

		// Token: 0x060077B2 RID: 30642 RVA: 0x002A2740 File Offset: 0x002A0940
		public override void OnOpen()
		{
			base.OnOpen();
			this.plantFilterState.quickSearch.Reset();
		}

		// Token: 0x060077B3 RID: 30643 RVA: 0x002A2758 File Offset: 0x002A0958
		protected override void FillTab()
		{
			CompAnimalPenMarker selectedCompAnimalPenMarker = base.SelectedCompAnimalPenMarker;
			Rect position = new Rect(0f, 0f, ITab_PenAutoCut.WinSize.x, ITab_PenAutoCut.WinSize.y).ContractedBy(10f);
			GUI.BeginGroup(position);
			float num = 0f;
			this.DrawAutoCutOptions(ref num, position.width, selectedCompAnimalPenMarker);
			num += 4f;
			this.DrawPlantFilter(ref num, position.width, position.height - num, selectedCompAnimalPenMarker);
			GUI.EndGroup();
		}

		// Token: 0x060077B4 RID: 30644 RVA: 0x002A27DC File Offset: 0x002A09DC
		private void DrawPlantFilter(ref float curY, float width, float height, CompAnimalPenMarker marker)
		{
			Rect rect = new Rect(0f, curY, width, height);
			ThingFilterUI.UIState state = this.plantFilterState;
			ThingFilter autoCutFilter = marker.AutoCutFilter;
			ThingFilter fixedAutoCutFilter = marker.parent.Map.animalPenManager.GetFixedAutoCutFilter();
			int openMask = 1;
			IEnumerable<ThingDef> forceHiddenDefs = null;
			Map map = marker.parent.Map;
			ThingFilterUI.DoThingFilterConfigWindow(rect, state, autoCutFilter, fixedAutoCutFilter, openMask, forceHiddenDefs, this.HiddenSpecialThingFilters(), true, null, map);
		}

		// Token: 0x060077B5 RID: 30645 RVA: 0x002A2838 File Offset: 0x002A0A38
		private void DrawAutoCutOptions(ref float curY, float width, CompAnimalPenMarker marker)
		{
			Rect position = new Rect(0f, curY, 24f, 24f);
			Rect rect = new Rect(position.xMax + 4f, curY, width, 24f);
			Rect rect2 = new Rect(0f, rect.yMax + 4f, 150f, 27f);
			Designator_PlantsCut designator_PlantsCut = Find.ReverseDesignatorDatabase.Get<Designator_PlantsCut>();
			GUI.DrawTexture(position, designator_PlantsCut.icon);
			if (Widgets.ButtonText(rect2, "AutoCutNow".Translate(), true, true, true))
			{
				if (marker.PenState.Enclosed)
				{
					marker.DesignatePlantsToCut();
					SoundDef soundSucceeded = designator_PlantsCut.soundSucceeded;
					if (soundSucceeded != null)
					{
						soundSucceeded.PlayOneShotOnCamera(null);
					}
				}
				else
				{
					Messages.Message("MessageAutoCutInUnenclosedPen".Translate(), marker.parent, MessageTypeDefOf.RejectInput, false);
				}
			}
			Widgets.CheckboxLabeled(rect, "PenAutoCut_EnabledCheckbox".Translate(), ref marker.autoCut, false, null, null, true);
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, "PenAutoCut_EnabledCheckboxTip".Translate());
			}
			curY = rect2.yMax;
		}

		// Token: 0x060077B6 RID: 30646 RVA: 0x002A2963 File Offset: 0x002A0B63
		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			yield return SpecialThingFilterDefOf.AllowFresh;
			yield break;
		}

		// Token: 0x04004292 RID: 17042
		private static readonly Vector2 WinSize = new Vector2(300f, 480f);

		// Token: 0x04004293 RID: 17043
		private const float AutoCutRowHeight = 24f;

		// Token: 0x04004294 RID: 17044
		private const int CutNowButtonWidth = 150;

		// Token: 0x04004295 RID: 17045
		private const int CutNowButtonHeight = 27;

		// Token: 0x04004296 RID: 17046
		private ThingFilterUI.UIState plantFilterState = new ThingFilterUI.UIState();
	}
}
