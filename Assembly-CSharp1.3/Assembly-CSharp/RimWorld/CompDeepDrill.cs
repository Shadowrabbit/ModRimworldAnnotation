using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200111E RID: 4382
	public class CompDeepDrill : ThingComp
	{
		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x0600693E RID: 26942 RVA: 0x002379CA File Offset: 0x00235BCA
		[Obsolete("Use WorkPerPortionBase constant directly.")]
		public static float WorkPerPortionCurrentDifficulty
		{
			get
			{
				return 10000f;
			}
		}

		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x0600693F RID: 26943 RVA: 0x002379D1 File Offset: 0x00235BD1
		public float ProgressToNextPortionPercent
		{
			get
			{
				return this.portionProgress / 10000f;
			}
		}

		// Token: 0x06006940 RID: 26944 RVA: 0x002379E0 File Offset: 0x00235BE0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x06006941 RID: 26945 RVA: 0x002379F3 File Offset: 0x00235BF3
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.portionProgress, "portionProgress", 0f, false);
			Scribe_Values.Look<float>(ref this.portionYieldPct, "portionYieldPct", 0f, false);
			Scribe_Values.Look<int>(ref this.lastUsedTick, "lastUsedTick", 0, false);
		}

		// Token: 0x06006942 RID: 26946 RVA: 0x00237A34 File Offset: 0x00235C34
		public void DrillWorkDone(Pawn driller)
		{
			float statValue = driller.GetStatValue(StatDefOf.DeepDrillingSpeed, true);
			this.portionProgress += statValue;
			this.portionYieldPct += statValue * driller.GetStatValue(StatDefOf.MiningYield, true) / 10000f;
			this.lastUsedTick = Find.TickManager.TicksGame;
			if (this.portionProgress > 10000f)
			{
				this.TryProducePortion(this.portionYieldPct, driller);
				this.portionProgress = 0f;
				this.portionYieldPct = 0f;
			}
		}

		// Token: 0x06006943 RID: 26947 RVA: 0x00237ABD File Offset: 0x00235CBD
		public override void PostDeSpawn(Map map)
		{
			this.portionProgress = 0f;
			this.portionYieldPct = 0f;
			this.lastUsedTick = -99999;
		}

		// Token: 0x06006944 RID: 26948 RVA: 0x00237AE0 File Offset: 0x00235CE0
		private void TryProducePortion(float yieldPct, Pawn driller)
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
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.Mined, driller.Named(HistoryEventArgsNames.Doer)), true);
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

		// Token: 0x06006945 RID: 26949 RVA: 0x00237CA5 File Offset: 0x00235EA5
		private bool GetNextResource(out ThingDef resDef, out int countPresent, out IntVec3 cell)
		{
			return DeepDrillUtility.GetNextResource(this.parent.Position, this.parent.Map, out resDef, out countPresent, out cell);
		}

		// Token: 0x06006946 RID: 26950 RVA: 0x00237CC5 File Offset: 0x00235EC5
		public bool CanDrillNow()
		{
			return (this.powerComp == null || this.powerComp.PowerOn) && (DeepDrillUtility.GetBaseResource(this.parent.Map, this.parent.Position) != null || this.ValuableResourcesPresent());
		}

		// Token: 0x06006947 RID: 26951 RVA: 0x00237D04 File Offset: 0x00235F04
		public bool ValuableResourcesPresent()
		{
			ThingDef thingDef;
			int num;
			IntVec3 intVec;
			return this.GetNextResource(out thingDef, out num, out intVec);
		}

		// Token: 0x06006948 RID: 26952 RVA: 0x00237D1D File Offset: 0x00235F1D
		public bool UsedLastTick()
		{
			return this.lastUsedTick >= Find.TickManager.TicksGame - 1;
		}

		// Token: 0x06006949 RID: 26953 RVA: 0x00237D38 File Offset: 0x00235F38
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

		// Token: 0x04003AE5 RID: 15077
		private CompPowerTrader powerComp;

		// Token: 0x04003AE6 RID: 15078
		private float portionProgress;

		// Token: 0x04003AE7 RID: 15079
		private float portionYieldPct;

		// Token: 0x04003AE8 RID: 15080
		private int lastUsedTick = -99999;

		// Token: 0x04003AE9 RID: 15081
		private const float WorkPerPortionBase = 10000f;
	}
}
