using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001806 RID: 6150
	public class CompPlantHarmRadius : ThingComp
	{
		// Token: 0x17001532 RID: 5426
		// (get) Token: 0x0600880F RID: 34831 RVA: 0x0005B51C File Offset: 0x0005971C
		protected CompProperties_PlantHarmRadius PropsPlantHarmRadius
		{
			get
			{
				return (CompProperties_PlantHarmRadius)this.props;
			}
		}

		// Token: 0x17001533 RID: 5427
		// (get) Token: 0x06008810 RID: 34832 RVA: 0x0005B529 File Offset: 0x00059729
		public float AgeDays
		{
			get
			{
				return (float)this.plantHarmAge / 60000f;
			}
		}

		// Token: 0x17001534 RID: 5428
		// (get) Token: 0x06008811 RID: 34833 RVA: 0x0005B538 File Offset: 0x00059738
		public float CurrentRadius
		{
			get
			{
				return this.PropsPlantHarmRadius.radiusPerDayCurve.Evaluate(this.AgeDays);
			}
		}

		// Token: 0x06008812 RID: 34834 RVA: 0x0005B550 File Offset: 0x00059750
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.plantHarmAge, "plantHarmAge", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToPlantHarm, "ticksToPlantHarm", 0, false);
		}

		// Token: 0x06008813 RID: 34835 RVA: 0x0005B576 File Offset: 0x00059776
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostPostMake();
			this.initiatableComp = this.parent.GetComp<CompInitiatable>();
		}

		// Token: 0x06008814 RID: 34836 RVA: 0x0027D5C0 File Offset: 0x0027B7C0
		public override string CompInspectStringExtra()
		{
			return "FoliageKillRadius".Translate() + ": " + this.CurrentRadius.ToString("0.0") + "\n" + "RadiusExpandRate".Translate() + ": " + Math.Round((double)(this.PropsPlantHarmRadius.radiusPerDayCurve.Evaluate(this.AgeDays + 1f) - this.PropsPlantHarmRadius.radiusPerDayCurve.Evaluate(this.AgeDays))) + "/" + "day".Translate();
		}

		// Token: 0x06008815 RID: 34837 RVA: 0x0027D680 File Offset: 0x0027B880
		public override void CompTick()
		{
			if (!this.parent.Spawned || (this.initiatableComp != null && !this.initiatableComp.Initiated))
			{
				return;
			}
			this.plantHarmAge++;
			this.ticksToPlantHarm--;
			if (this.ticksToPlantHarm <= 0)
			{
				float currentRadius = this.CurrentRadius;
				float num = 3.1415927f * currentRadius * currentRadius * this.PropsPlantHarmRadius.harmFrequencyPerArea;
				float num2 = 60f / num;
				int num3;
				if (num2 >= 1f)
				{
					this.ticksToPlantHarm = GenMath.RoundRandom(num2);
					num3 = 1;
				}
				else
				{
					this.ticksToPlantHarm = 1;
					num3 = GenMath.RoundRandom(1f / num2);
				}
				for (int i = 0; i < num3; i++)
				{
					this.HarmRandomPlantInRadius(currentRadius);
				}
			}
		}

		// Token: 0x06008816 RID: 34838 RVA: 0x0027D740 File Offset: 0x0027B940
		private void HarmRandomPlantInRadius(float radius)
		{
			IntVec3 c = this.parent.Position + (Rand.InsideUnitCircleVec3 * radius).ToIntVec3();
			if (!c.InBounds(this.parent.Map))
			{
				return;
			}
			Plant plant = c.GetPlant(this.parent.Map);
			if (plant != null)
			{
				if (plant.LeaflessNow)
				{
					if (Rand.Value < this.PropsPlantHarmRadius.leaflessPlantKillChance)
					{
						plant.Kill(null, null);
						return;
					}
				}
				else
				{
					plant.MakeLeafless(Plant.LeaflessCause.Poison);
				}
			}
		}

		// Token: 0x0400574C RID: 22348
		private int plantHarmAge;

		// Token: 0x0400574D RID: 22349
		private int ticksToPlantHarm;

		// Token: 0x0400574E RID: 22350
		protected CompInitiatable initiatableComp;
	}
}
