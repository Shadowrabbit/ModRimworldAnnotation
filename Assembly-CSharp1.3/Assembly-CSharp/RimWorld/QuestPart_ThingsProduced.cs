using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB2 RID: 2994
	public class QuestPart_ThingsProduced : QuestPartActivable
	{
		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x060045DE RID: 17886 RVA: 0x001720FC File Offset: 0x001702FC
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

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x060045DF RID: 17887 RVA: 0x0017215C File Offset: 0x0017035C
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

		// Token: 0x060045E0 RID: 17888 RVA: 0x0017216C File Offset: 0x0017036C
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.produced = 0;
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x0017217C File Offset: 0x0017037C
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

		// Token: 0x060045E2 RID: 17890 RVA: 0x00172210 File Offset: 0x00170410
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.produced, "produced", 0, false);
		}

		// Token: 0x060045E3 RID: 17891 RVA: 0x00172267 File Offset: 0x00170467
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = ThingDefOf.MealSimple;
			this.count = 10;
		}

		// Token: 0x04002A93 RID: 10899
		public ThingDef def;

		// Token: 0x04002A94 RID: 10900
		public ThingDef stuff;

		// Token: 0x04002A95 RID: 10901
		public int count;

		// Token: 0x04002A96 RID: 10902
		private int produced;
	}
}
