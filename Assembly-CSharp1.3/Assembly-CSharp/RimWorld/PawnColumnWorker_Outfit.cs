using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001399 RID: 5017
	public class PawnColumnWorker_Outfit : PawnColumnWorker
	{
		// Token: 0x06007A08 RID: 31240 RVA: 0x002B1078 File Offset: 0x002AF278
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageOutfits".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageOutfits(null));
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Outfits, KnowledgeAmount.Total);
			}
			UIHighlighter.HighlightOpportunity(rect2, "ManageOutfits");
		}

		// Token: 0x06007A09 RID: 31241 RVA: 0x002B110C File Offset: 0x002AF30C
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.outfits == null)
			{
				return;
			}
			int num = Mathf.FloorToInt((rect.width - 4f) * 0.71428573f);
			int num2 = Mathf.FloorToInt((rect.width - 4f) * 0.2857143f);
			float num3 = rect.x;
			bool somethingIsForced = pawn.outfits.forcedHandler.SomethingIsForced;
			Rect rect2 = new Rect(num3, rect.y + 2f, (float)num, rect.height - 4f);
			if (somethingIsForced)
			{
				rect2.width -= 4f + (float)num2;
			}
			if (pawn.IsQuestLodger())
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect2, "Unchangeable".Translate().Truncate(rect2.width, null));
				TooltipHandler.TipRegionByKey(rect2, "QuestRelated_Outfit");
				Text.Anchor = TextAnchor.UpperLeft;
			}
			else
			{
				Widgets.Dropdown<Pawn, Outfit>(rect2, pawn, (Pawn p) => p.outfits.CurrentOutfit, new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<Outfit>>>(this.Button_GenerateMenu), pawn.outfits.CurrentOutfit.label.Truncate(rect2.width, null), null, pawn.outfits.CurrentOutfit.label, null, null, true);
			}
			num3 += rect2.width;
			num3 += 4f;
			Rect rect3 = new Rect(num3, rect.y + 2f, (float)num2, rect.height - 4f);
			if (somethingIsForced)
			{
				if (Widgets.ButtonText(rect3, "ClearForcedApparel".Translate(), true, true, true))
				{
					pawn.outfits.forcedHandler.Reset();
				}
				if (Mouse.IsOver(rect3))
				{
					TooltipHandler.TipRegion(rect3, new TipSignal(delegate()
					{
						string text = "ForcedApparel".Translate() + ":\n";
						foreach (Apparel apparel in pawn.outfits.forcedHandler.ForcedApparel)
						{
							text = text + "\n   " + apparel.LabelCap;
						}
						return text;
					}, pawn.GetHashCode() * 612));
				}
				num3 += (float)num2;
				num3 += 4f;
			}
			Rect rect4 = new Rect(num3, rect.y + 2f, (float)num2, rect.height - 4f);
			if (!pawn.IsQuestLodger() && Widgets.ButtonText(rect4, "AssignTabEdit".Translate(), true, true, true))
			{
				Find.WindowStack.Add(new Dialog_ManageOutfits(pawn.outfits.CurrentOutfit));
			}
			num3 += (float)num2;
		}

		// Token: 0x06007A0A RID: 31242 RVA: 0x002B138F File Offset: 0x002AF58F
		private IEnumerable<Widgets.DropdownMenuElement<Outfit>> Button_GenerateMenu(Pawn pawn)
		{
			using (List<Outfit>.Enumerator enumerator = Current.Game.outfitDatabase.AllOutfits.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Outfit outfit = enumerator.Current;
					yield return new Widgets.DropdownMenuElement<Outfit>
					{
						option = new FloatMenuOption(outfit.label, delegate()
						{
							pawn.outfits.CurrentOutfit = outfit;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
						payload = outfit
					};
				}
			}
			List<Outfit>.Enumerator enumerator = default(List<Outfit>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06007A0B RID: 31243 RVA: 0x002B07E7 File Offset: 0x002AE9E7
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x06007A0C RID: 31244 RVA: 0x002B07FF File Offset: 0x002AE9FF
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x06007A0D RID: 31245 RVA: 0x002B031D File Offset: 0x002AE51D
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x06007A0E RID: 31246 RVA: 0x002B13A0 File Offset: 0x002AF5A0
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06007A0F RID: 31247 RVA: 0x002B13C3 File Offset: 0x002AF5C3
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.outfits != null && pawn.outfits.CurrentOutfit != null)
			{
				return pawn.outfits.CurrentOutfit.uniqueId;
			}
			return int.MinValue;
		}

		// Token: 0x04004393 RID: 17299
		public const int TopAreaHeight = 65;

		// Token: 0x04004394 RID: 17300
		public const int ManageOutfitsButtonHeight = 32;
	}
}
