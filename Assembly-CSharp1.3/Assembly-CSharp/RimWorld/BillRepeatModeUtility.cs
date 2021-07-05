using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200108C RID: 4236
	public static class BillRepeatModeUtility
	{
		// Token: 0x060064DE RID: 25822 RVA: 0x0021FAF0 File Offset: 0x0021DCF0
		public static void MakeConfigFloatMenu(Bill_Production bill)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption(BillRepeatModeDefOf.RepeatCount.LabelCap, delegate()
			{
				bill.repeatMode = BillRepeatModeDefOf.RepeatCount;
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			FloatMenuOption item = new FloatMenuOption(BillRepeatModeDefOf.TargetCount.LabelCap, delegate()
			{
				if (!bill.recipe.WorkerCounter.CanCountProducts(bill))
				{
					Messages.Message("RecipeCannotHaveTargetCount".Translate(), MessageTypeDefOf.RejectInput, false);
					return;
				}
				bill.repeatMode = BillRepeatModeDefOf.TargetCount;
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			list.Add(item);
			list.Add(new FloatMenuOption(BillRepeatModeDefOf.Forever.LabelCap, delegate()
			{
				bill.repeatMode = BillRepeatModeDefOf.Forever;
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			Find.WindowStack.Add(new FloatMenu(list));
		}
	}
}
