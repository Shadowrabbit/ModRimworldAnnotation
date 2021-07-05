using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016D9 RID: 5849
	public static class BillRepeatModeUtility
	{
		// Token: 0x0600807E RID: 32894 RVA: 0x0026066C File Offset: 0x0025E86C
		public static void MakeConfigFloatMenu(Bill_Production bill)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption(BillRepeatModeDefOf.RepeatCount.LabelCap, delegate()
			{
				bill.repeatMode = BillRepeatModeDefOf.RepeatCount;
			}, MenuOptionPriority.Default, null, null, 0f, null, null));
			FloatMenuOption item = new FloatMenuOption(BillRepeatModeDefOf.TargetCount.LabelCap, delegate()
			{
				if (!bill.recipe.WorkerCounter.CanCountProducts(bill))
				{
					Messages.Message("RecipeCannotHaveTargetCount".Translate(), MessageTypeDefOf.RejectInput, false);
					return;
				}
				bill.repeatMode = BillRepeatModeDefOf.TargetCount;
			}, MenuOptionPriority.Default, null, null, 0f, null, null);
			list.Add(item);
			list.Add(new FloatMenuOption(BillRepeatModeDefOf.Forever.LabelCap, delegate()
			{
				bill.repeatMode = BillRepeatModeDefOf.Forever;
			}, MenuOptionPriority.Default, null, null, 0f, null, null));
			Find.WindowStack.Add(new FloatMenu(list));
		}
	}
}
