using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200116D RID: 4461
	public class CompPlantHarmRadius : ThingComp
	{
		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x06006B1D RID: 27421 RVA: 0x0023F4AA File Offset: 0x0023D6AA
		protected CompProperties_PlantHarmRadius PropsPlantHarmRadius
		{
			get
			{
				return (CompProperties_PlantHarmRadius)this.props;
			}
		}

		// Token: 0x1700126E RID: 4718
		// (get) Token: 0x06006B1E RID: 27422 RVA: 0x0023F4B7 File Offset: 0x0023D6B7
		public float AgeDays
		{
			get
			{
				return (float)this.plantHarmAge / 60000f;
			}
		}

		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x06006B1F RID: 27423 RVA: 0x0023F4C6 File Offset: 0x0023D6C6
		public float CurrentRadius
		{
			get
			{
				return this.PropsPlantHarmRadius.radiusPerDayCurve.Evaluate(this.AgeDays);
			}
		}

		// Token: 0x06006B20 RID: 27424 RVA: 0x0023F4DE File Offset: 0x0023D6DE
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.plantHarmAge, "plantHarmAge", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToPlantHarm, "ticksToPlantHarm", 0, false);
		}

		// Token: 0x06006B21 RID: 27425 RVA: 0x0023F504 File Offset: 0x0023D704
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostPostMake();
			this.initiatableComp = this.parent.GetComp<CompInitiatable>();
		}

		// Token: 0x06006B22 RID: 27426 RVA: 0x0023F520 File Offset: 0x0023D720
		public override string CompInspectStringExtra()
		{
			return "FoliageKillRadius".Translate() + ": " + this.CurrentRadius.ToString("0.0") + "\n" + "RadiusExpandRate".Translate() + ": " + Math.Round((double)(this.PropsPlantHarmRadius.radiusPerDayCurve.Evaluate(this.AgeDays + 1f) - this.PropsPlantHarmRadius.radiusPerDayCurve.Evaluate(this.AgeDays))) + "/" + "day".Translate();
		}

		// Token: 0x06006B23 RID: 27427 RVA: 0x0023F5E0 File Offset: 0x0023D7E0
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

		// Token: 0x06006B24 RID: 27428 RVA: 0x0023F6A0 File Offset: 0x0023D8A0
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

		// Token: 0x04003B98 RID: 15256
		private int plantHarmAge;

		// Token: 0x04003B99 RID: 15257
		private int ticksToPlantHarm;

		// Token: 0x04003B9A RID: 15258
		protected CompInitiatable initiatableComp;
	}
}
