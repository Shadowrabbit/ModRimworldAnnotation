using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200113F RID: 4415
	public class CompGiveHediffSeverity : ThingComp
	{
		// Token: 0x17001226 RID: 4646
		// (get) Token: 0x06006A0B RID: 27147 RVA: 0x0023B15F File Offset: 0x0023935F
		private CompProperties_GiveHediffSeverity Props
		{
			get
			{
				return (CompProperties_GiveHediffSeverity)this.props;
			}
		}

		// Token: 0x06006A0C RID: 27148 RVA: 0x0023B16C File Offset: 0x0023936C
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (Find.TickManager.TicksGame % 87 == 0)
			{
				CompRefuelable compRefuelable = this.parent.TryGetComp<CompRefuelable>();
				CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
				if (compRefuelable != null && !compRefuelable.HasFuel)
				{
					return;
				}
				if (compPowerTrader != null && !compPowerTrader.PowerOn)
				{
					return;
				}
				int num = GenRadial.NumCellsInRadius(this.Props.range);
				for (int i = 0; i < num; i++)
				{
					List<Thing> thingList = (this.parent.Position + GenRadial.RadialPattern[i]).GetThingList(this.parent.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Thing thing = thingList[j];
						Pawn pawn;
						if ((pawn = (thing as Pawn)) != null && thing.GetRoom(RegionType.Set_All) == this.parent.GetRoom(RegionType.Set_All))
						{
							Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.hediff, false);
							float num2 = this.Props.severityPerSecond * 1.45f;
							if (firstHediffOfDef != null)
							{
								firstHediffOfDef.Severity += num2;
							}
							else
							{
								pawn.health.AddHediff(this.Props.hediff, null, null, null).Severity = num2;
							}
							if (this.Props.drugExposure)
							{
								pawn.mindState.lastTakeRecreationalDrugTick = Find.TickManager.TicksGame;
							}
						}
					}
				}
			}
		}

		// Token: 0x04003B2A RID: 15146
		private const int TickInterval = 87;
	}
}
