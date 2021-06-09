using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017AF RID: 6063
	public class CompDeepDrill : ThingComp
	{
		// Token: 0x170014C3 RID: 5315
		// (get) Token: 0x0600860D RID: 34317 RVA: 0x00059EC1 File Offset: 0x000580C1
		[Obsolete("Use WorkPerPortionBase constant directly.")]
		public static float WorkPerPortionCurrentDifficulty
		{
			get
			{
				return 10000f;
			}
		}

		// Token: 0x170014C4 RID: 5316
		// (get) Token: 0x0600860E RID: 34318 RVA: 0x00059EC8 File Offset: 0x000580C8
		public float ProgressToNextPortionPercent
		{
			get
			{
				return this.portionProgress / 10000f;
			}
		}

		// Token: 0x0600860F RID: 34319 RVA: 0x00059ED7 File Offset: 0x000580D7
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x06008610 RID: 34320 RVA: 0x00059EEA File Offset: 0x000580EA
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.portionProgress, "portionProgress", 0f, false);
			Scribe_Values.Look<float>(ref this.portionYieldPct, "portionYieldPct", 0f, false);
			Scribe_Values.Look<int>(ref this.lastUsedTick, "lastUsedTick", 0, false);
		}

		// Token: 0x06008611 RID: 34321 RVA: 0x00277630 File Offset: 0x00275830
		public void DrillWorkDone(Pawn driller)
		{
			float statValue = driller.GetStatValue(StatDefOf.DeepDrillingSpeed, true);
			this.portionProgress += statValue;
			this.portionYieldPct += statValue * driller.GetStatValue(StatDefOf.MiningYield, true) / 10000f;
			this.lastUsedTick = Find.TickManager.TicksGame;
			if (this.portionProgress > 10000f)
			{
				this.TryProducePortion(this.portionYieldPct);
				this.portionProgress = 0f;
				this.portionYieldPct = 0f;
			}
		}

		// Token: 0x06008612 RID: 34322 RVA: 0x00059F2A File Offset: 0x0005812A
		public override void PostDeSpawn(Map map)
		{
			this.portionProgress = 0f;
			this.portionYieldPct = 0f;
			this.lastUsedTick = -99999;
		}

		// Token: 0x06008613 RID: 34323 RVA: 0x002776B8 File Offset: 0x002758B8
		private void TryProducePortion(float yieldPct)
		{
			ThingDef thingDef;
			int num;
			IntVec3 intVec;
			bool nextResource = this.GetNextResource(out thingDef, out num, out intVec);
			if (thingDef == null)
			{
				return;
			}
			int num2 = Mathf.Min(num, thingDef.deepCountPerPortion);
			if (nextResource)
			{
				this.parent.Map.deepResourceGrid.SetAt(intVec, thingDef, num - num2);
			}
			int stackCount = Mathf.Max(1, GenMath.RoundRandom((float)num2 * yieldPct));
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			thing.stackCount = stackCount;
			GenPlace.TryPlaceThing(thing, this.parent.InteractionCell, this.parent.Map, ThingPlaceMode.Near, null, null, default(Rot4));
			if (nextResource && !this.ValuableResourcesPresent())
			{
				if (DeepDrillUtility.GetBaseResource(this.parent.Map, this.parent.Position) == null)
				{
					Messages.Message("DeepDrillExhaustedNoFallback".Translate(), this.parent, MessageTypeDefOf.TaskCompletion, true);
					return;
				}
				Messages.Message("DeepDrillExhausted".Translate(Find.ActiveLanguageWorker.Pluralize(DeepDrillUtility.GetBaseResource(this.parent.Map, this.parent.Position).label, -1)), this.parent, MessageTypeDefOf.TaskCompletion, true);
				for (int i = 0; i < 21; i++)
				{
					IntVec3 c = intVec + GenRadial.RadialPattern[i];
					if (c.InBounds(this.parent.Map))
					{
						ThingWithComps firstThingWithComp = c.GetFirstThingWithComp(this.parent.Map);
						if (firstThingWithComp != null && !firstThingWithComp.GetComp<CompDeepDrill>().ValuableResourcesPresent())
						{
							firstThingWithComp.SetForbidden(true, true);
						}
					}
				}
			}
		}

		// Token: 0x06008614 RID: 34324 RVA: 0x00059F4D File Offset: 0x0005814D
		private bool GetNextResource(out ThingDef resDef, out int countPresent, out IntVec3 cell)
		{
			return DeepDrillUtility.GetNextResource(this.parent.Position, this.parent.Map, out resDef, out countPresent, out cell);
		}

		// Token: 0x06008615 RID: 34325 RVA: 0x00059F6D File Offset: 0x0005816D
		public bool CanDrillNow()
		{
			return (this.powerComp == null || this.powerComp.PowerOn) && (DeepDrillUtility.GetBaseResource(this.parent.Map, this.parent.Position) != null || this.ValuableResourcesPresent());
		}

		// Token: 0x06008616 RID: 34326 RVA: 0x00277860 File Offset: 0x00275A60
		public bool ValuableResourcesPresent()
		{
			ThingDef thingDef;
			int num;
			IntVec3 intVec;
			return this.GetNextResource(out thingDef, out num, out intVec);
		}

		// Token: 0x06008617 RID: 34327 RVA: 0x00059FAB File Offset: 0x000581AB
		public bool UsedLastTick()
		{
			return this.lastUsedTick >= Find.TickManager.TicksGame - 1;
		}

		// Token: 0x06008618 RID: 34328 RVA: 0x0027787C File Offset: 0x00275A7C
		public override string CompInspectStringExtra()
		{
			if (!this.parent.Spawned)
			{
				return null;
			}
			ThingDef thingDef;
			int num;
			IntVec3 intVec;
			this.GetNextResource(out thingDef, out num, out intVec);
			if (thingDef == null)
			{
				return "DeepDrillNoResources".Translate();
			}
			return "ResourceBelow".Translate() + ": " + thingDef.LabelCap + "\n" + "ProgressToNextPortion".Translate() + ": " + this.ProgressToNextPortionPercent.ToStringPercent("F0");
		}

		// Token: 0x04005668 RID: 22120
		private CompPowerTrader powerComp;

		// Token: 0x04005669 RID: 22121
		private float portionProgress;

		// Token: 0x0400566A RID: 22122
		private float portionYieldPct;

		// Token: 0x0400566B RID: 22123
		private int lastUsedTick = -99999;

		// Token: 0x0400566C RID: 22124
		private const float WorkPerPortionBase = 10000f;
	}
}
