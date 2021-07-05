using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001391 RID: 5009
	public class PawnColumnWorker_Carry : PawnColumnWorker
	{
		// Token: 0x060079CF RID: 31183 RVA: 0x002B057C File Offset: 0x002AE77C
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.inventoryStock == null)
			{
				return;
			}
			float num = rect.width - 4f;
			int num2 = Mathf.FloorToInt(num * 0.33333334f);
			float x = rect.x;
			InventoryStockGroupDef group = InventoryStockGroupDefOf.Medicine;
			Widgets.Dropdown<Pawn, ThingDef>(new Rect(x, rect.y + 2f, (float)num2, rect.height - 4f), pawn, (Pawn p) => p.inventoryStock.GetDesiredThingForGroup(group), (Pawn p) => this.GenerateThingButtons(p, group), null, pawn.inventoryStock.GetDesiredThingForGroup(group).uiIcon, null, null, null, true);
			float x2 = x + (float)num2 + 4f;
			int num3 = Mathf.FloorToInt(num * 0.6666667f);
			Widgets.Dropdown<Pawn, int>(new Rect(x2, rect.y + 2f, (float)num3, rect.height - 4f), pawn, (Pawn p) => p.inventoryStock.GetDesiredCountForGroup(group), (Pawn p) => this.GenerateCountButtons(p, group), pawn.inventoryStock.GetDesiredCountForGroup(group).ToString(), null, null, null, null, true);
		}

		// Token: 0x060079D0 RID: 31184 RVA: 0x002B069B File Offset: 0x002AE89B
		private IEnumerable<Widgets.DropdownMenuElement<ThingDef>> GenerateThingButtons(Pawn pawn, InventoryStockGroupDef group)
		{
			using (List<ThingDef>.Enumerator enumerator = group.thingDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef thing = enumerator.Current;
					yield return new Widgets.DropdownMenuElement<ThingDef>
					{
						option = new FloatMenuOption(thing.LabelCap, delegate()
						{
							pawn.inventoryStock.SetThingForGroup(group, thing);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
						payload = thing
					};
				}
			}
			List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060079D1 RID: 31185 RVA: 0x002B06B2 File Offset: 0x002AE8B2
		private IEnumerable<Widgets.DropdownMenuElement<int>> GenerateCountButtons(Pawn pawn, InventoryStockGroupDef group)
		{
			int num;
			for (int i = group.min; i <= group.max; i = num + 1)
			{
				int localI = i;
				yield return new Widgets.DropdownMenuElement<int>
				{
					option = new FloatMenuOption(i.ToString(), delegate()
					{
						pawn.inventoryStock.SetCountForGroup(group, localI);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
					payload = i
				};
				num = i;
			}
			yield break;
		}

		// Token: 0x060079D2 RID: 31186 RVA: 0x002B06C9 File Offset: 0x002AE8C9
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(54f));
		}

		// Token: 0x060079D3 RID: 31187 RVA: 0x002B06E1 File Offset: 0x002AE8E1
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(104f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x060079D4 RID: 31188 RVA: 0x002B031D File Offset: 0x002AE51D
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x060079D5 RID: 31189 RVA: 0x002B0700 File Offset: 0x002AE900
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x060079D6 RID: 31190 RVA: 0x002B0723 File Offset: 0x002AE923
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.inventoryStock != null)
			{
				return pawn.inventoryStock.GetDesiredCountForGroup(InventoryStockGroupDefOf.Medicine);
			}
			return int.MinValue;
		}

		// Token: 0x04004388 RID: 17288
		private const int TopAreaHeight = 65;
	}
}
