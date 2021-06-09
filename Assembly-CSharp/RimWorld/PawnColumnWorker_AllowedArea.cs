using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B79 RID: 7033
	public class PawnColumnWorker_AllowedArea : PawnColumnWorker
	{
		// Token: 0x1700186A RID: 6250
		// (get) Token: 0x06009AF8 RID: 39672 RVA: 0x0000A2E4 File Offset: 0x000084E4
		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		// Token: 0x06009AF9 RID: 39673 RVA: 0x0006721B File Offset: 0x0006541B
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 200);
		}

		// Token: 0x06009AFA RID: 39674 RVA: 0x0006722E File Offset: 0x0006542E
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(273, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06009AFB RID: 39675 RVA: 0x00067248 File Offset: 0x00065448
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x06009AFC RID: 39676 RVA: 0x00067258 File Offset: 0x00065458
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Faction != Faction.OfPlayer)
			{
				return;
			}
			AreaAllowedGUI.DoAllowedAreaSelectors(rect, pawn);
		}

		// Token: 0x06009AFD RID: 39677 RVA: 0x002D80E8 File Offset: 0x002D62E8
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			if (Widgets.ButtonText(new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f), "ManageAreas".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageAreas(Find.CurrentMap));
			}
		}

		// Token: 0x06009AFE RID: 39678 RVA: 0x002D8164 File Offset: 0x002D6364
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06009AFF RID: 39679 RVA: 0x002D8188 File Offset: 0x002D6388
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer)
			{
				return int.MinValue;
			}
			Area areaRestriction = pawn.playerSettings.AreaRestriction;
			if (areaRestriction == null)
			{
				return -2147483647;
			}
			return areaRestriction.ID;
		}

		// Token: 0x06009B00 RID: 39680 RVA: 0x002D81C4 File Offset: 0x002D63C4
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift && Find.CurrentMap != null)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					if (pawnsListForReading[i].Faction != Faction.OfPlayer)
					{
						return;
					}
					if (Event.current.button == 0)
					{
						pawnsListForReading[i].playerSettings.AreaRestriction = Find.CurrentMap.areaManager.Home;
					}
					else if (Event.current.button == 1)
					{
						pawnsListForReading[i].playerSettings.AreaRestriction = null;
					}
				}
				if (Event.current.button == 0)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					return;
				}
				if (Event.current.button == 1)
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x06009B01 RID: 39681 RVA: 0x0006726F File Offset: 0x0006546F
		protected override string GetHeaderTip(PawnTable table)
		{
			return base.GetHeaderTip(table) + "\n" + "AllowedAreaShiftClickTip".Translate();
		}

		// Token: 0x040062D1 RID: 25297
		private const int TopAreaHeight = 65;

		// Token: 0x040062D2 RID: 25298
		private const int ManageAreasButtonHeight = 32;
	}
}
