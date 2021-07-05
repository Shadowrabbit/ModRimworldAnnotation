using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B8E RID: 2958
	public class QuestPart_PlantsHarvested : QuestPartActivable
	{
		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x0600452C RID: 17708 RVA: 0x0016ED2C File Offset: 0x0016CF2C
		public override string DescriptionPart
		{
			get
			{
				return string.Concat(new object[]
				{
					"PlantsHarvested".Translate().CapitalizeFirst() + ": ",
					this.harvested,
					" / ",
					this.count
				});
			}
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x0600452D RID: 17709 RVA: 0x0016ED8C File Offset: 0x0016CF8C
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in base.Hyperlinks)
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				yield return new Dialog_InfoCard.Hyperlink(this.plant, -1);
				yield break;
				yield break;
			}
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x0016ED9C File Offset: 0x0016CF9C
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.harvested = 0;
		}

		// Token: 0x0600452F RID: 17711 RVA: 0x0016EDAC File Offset: 0x0016CFAC
		public override void Notify_PlantHarvested(Pawn actor, Thing harvested)
		{
			base.Notify_PlantHarvested(actor, harvested);
			if (base.State == QuestPartState.Enabled && harvested.def == this.plant)
			{
				this.harvested += harvested.stackCount;
				if (this.harvested >= this.count)
				{
					this.harvested = this.count;
					base.Complete();
				}
			}
		}

		// Token: 0x06004530 RID: 17712 RVA: 0x0016EE0B File Offset: 0x0016D00B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plant, "plant");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.harvested, "harvested", 0, false);
		}

		// Token: 0x06004531 RID: 17713 RVA: 0x0016EE47 File Offset: 0x0016D047
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.plant = ThingDefOf.RawPotatoes;
			this.count = 10;
		}

		// Token: 0x04002A10 RID: 10768
		public ThingDef plant;

		// Token: 0x04002A11 RID: 10769
		public int count;

		// Token: 0x04002A12 RID: 10770
		private int harvested;
	}
}
