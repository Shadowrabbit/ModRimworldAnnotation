using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200111E RID: 4382
	public class QuestPart_ThingsProduced : QuestPartActivable
	{
		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x06005FCA RID: 24522 RVA: 0x001E31D0 File Offset: 0x001E13D0
		public override string DescriptionPart
		{
			get
			{
				return string.Concat(new object[]
				{
					"ThingsProduced".Translate().CapitalizeFirst() + ": ",
					this.produced,
					" / ",
					this.count
				});
			}
		}

		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x06005FCB RID: 24523 RVA: 0x000423DA File Offset: 0x000405DA
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in base.Hyperlinks)
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				yield return new Dialog_InfoCard.Hyperlink(this.def, -1);
				yield break;
				yield break;
			}
		}

		// Token: 0x06005FCC RID: 24524 RVA: 0x000423EA File Offset: 0x000405EA
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.produced = 0;
		}

		// Token: 0x06005FCD RID: 24525 RVA: 0x001E3230 File Offset: 0x001E1430
		public override void Notify_ThingsProduced(Pawn actor, List<Thing> things)
		{
			base.Notify_ThingsProduced(actor, things);
			if (base.State == QuestPartState.Enabled)
			{
				for (int i = 0; i < things.Count; i++)
				{
					Thing innerIfMinified = things[i].GetInnerIfMinified();
					if (innerIfMinified.def == this.def && innerIfMinified.Stuff == this.stuff)
					{
						this.produced += things[i].stackCount;
					}
				}
				if (this.produced >= this.count)
				{
					this.produced = this.count;
					base.Complete();
				}
			}
		}

		// Token: 0x06005FCE RID: 24526 RVA: 0x001E32C4 File Offset: 0x001E14C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.produced, "produced", 0, false);
		}

		// Token: 0x06005FCF RID: 24527 RVA: 0x000423FA File Offset: 0x000405FA
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = ThingDefOf.MealSimple;
			this.count = 10;
		}

		// Token: 0x04004009 RID: 16393
		public ThingDef def;

		// Token: 0x0400400A RID: 16394
		public ThingDef stuff;

		// Token: 0x0400400B RID: 16395
		public int count;

		// Token: 0x0400400C RID: 16396
		private int produced;
	}
}
