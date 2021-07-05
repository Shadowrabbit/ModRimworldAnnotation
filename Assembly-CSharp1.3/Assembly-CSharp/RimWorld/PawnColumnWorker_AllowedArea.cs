using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200138F RID: 5007
	public class PawnColumnWorker_AllowedArea : PawnColumnWorker
	{
		// Token: 0x17001565 RID: 5477
		// (get) Token: 0x060079C2 RID: 31170 RVA: 0x0001276E File Offset: 0x0001096E
		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		// Token: 0x060079C3 RID: 31171 RVA: 0x002B02F0 File Offset: 0x002AE4F0
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 200);
		}

		// Token: 0x060079C4 RID: 31172 RVA: 0x002B0303 File Offset: 0x002AE503
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(273, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x060079C5 RID: 31173 RVA: 0x002B031D File Offset: 0x002AE51D
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x060079C6 RID: 31174 RVA: 0x002B032D File Offset: 0x002AE52D
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Faction == Faction.OfPlayer)
			{
				if (pawn.playerSettings.SupportsAllowedAreas)
				{
					AreaAllowedGUI.DoAllowedAreaSelectors(rect, pawn);
					return;
				}
				if (AnimalPenUtility.NeedsToBeManagedByRope(pawn))
				{
					AnimalPenGUI.DoAllowedAreaMessage(rect, pawn);
				}
			}
		}

		// Token: 0x060079C7 RID: 31175 RVA: 0x002B0360 File Offset: 0x002AE560
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			if (Widgets.ButtonText(new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f), "ManageAreas".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageAreas(Find.CurrentMap));
			}
		}

		// Token: 0x060079C8 RID: 31176 RVA: 0x002B03DC File Offset: 0x002AE5DC
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x060079C9 RID: 31177 RVA: 0x002B0400 File Offset: 0x002AE600
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer || !pawn.playerSettings.SupportsAllowedAreas)
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

		// Token: 0x060079CA RID: 31178 RVA: 0x002B0448 File Offset: 0x002AE648
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift && Find.CurrentMap != null)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					if (pawnsListForReading[i].Faction == Faction.OfPlayer && pawnsListForReading[i].playerSettings.SupportsAllowedAreas)
					{
						if (Event.current.button == 0)
						{
							pawnsListForReading[i].playerSettings.AreaRestriction = Find.CurrentMap.areaManager.Home;
						}
						else if (Event.current.button == 1)
						{
							pawnsListForReading[i].playerSettings.AreaRestriction = null;
						}
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

		// Token: 0x060079CB RID: 31179 RVA: 0x002B0531 File Offset: 0x002AE731
		protected override string GetHeaderTip(PawnTable table)
		{
			return base.GetHeaderTip(table) + "\n" + "AllowedAreaShiftClickTip".Translate();
		}

		// Token: 0x04004386 RID: 17286
		private const int TopAreaHeight = 65;

		// Token: 0x04004387 RID: 17287
		private const int ManageAreasButtonHeight = 32;
	}
}
