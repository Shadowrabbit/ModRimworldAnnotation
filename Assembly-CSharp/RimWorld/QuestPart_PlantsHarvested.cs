using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F1 RID: 4337
	public class QuestPart_PlantsHarvested : QuestPartActivable
	{
		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x06005EC5 RID: 24261 RVA: 0x001E0744 File Offset: 0x001DE944
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

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x06005EC6 RID: 24262 RVA: 0x0004196A File Offset: 0x0003FB6A
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

		// Token: 0x06005EC7 RID: 24263 RVA: 0x0004197A File Offset: 0x0003FB7A
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.harvested = 0;
		}

		// Token: 0x06005EC8 RID: 24264 RVA: 0x001E07A4 File Offset: 0x001DE9A4
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

		// Token: 0x06005EC9 RID: 24265 RVA: 0x0004198A File Offset: 0x0003FB8A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plant, "plant");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.harvested, "harvested", 0, false);
		}

		// Token: 0x06005ECA RID: 24266 RVA: 0x000419C6 File Offset: 0x0003FBC6
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.plant = ThingDefOf.RawPotatoes;
			this.count = 10;
		}

		// Token: 0x04003F5E RID: 16222
		public ThingDef plant;

		// Token: 0x04003F5F RID: 16223
		public int count;

		// Token: 0x04003F60 RID: 16224
		private int harvested;
	}
}
