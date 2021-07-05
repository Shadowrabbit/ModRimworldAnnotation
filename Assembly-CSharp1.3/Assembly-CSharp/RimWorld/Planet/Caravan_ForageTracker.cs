using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017C3 RID: 6083
	public class Caravan_ForageTracker : IExposable
	{
		// Token: 0x170016F8 RID: 5880
		// (get) Token: 0x06008D2C RID: 36140 RVA: 0x0032CED9 File Offset: 0x0032B0D9
		public Pair<ThingDef, float> ForagedFoodPerDay
		{
			get
			{
				return ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.caravan, null);
			}
		}

		// Token: 0x170016F9 RID: 5881
		// (get) Token: 0x06008D2D RID: 36141 RVA: 0x0032CEE8 File Offset: 0x0032B0E8
		public string ForagedFoodPerDayExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.caravan, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06008D2E RID: 36142 RVA: 0x0032CF0E File Offset: 0x0032B10E
		public Caravan_ForageTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06008D2F RID: 36143 RVA: 0x0032CF1D File Offset: 0x0032B11D
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.progress, "progress", 0f, false);
		}

		// Token: 0x06008D30 RID: 36144 RVA: 0x0032CF35 File Offset: 0x0032B135
		public void ForageTrackerTick()
		{
			if (this.caravan.IsHashIntervalTick(10))
			{
				this.UpdateProgressInterval();
			}
		}

		// Token: 0x06008D31 RID: 36145 RVA: 0x0032CF4C File Offset: 0x0032B14C
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Forage",
					action = new Action(this.Forage)
				};
			}
			yield break;
		}

		// Token: 0x06008D32 RID: 36146 RVA: 0x0032CF5C File Offset: 0x0032B15C
		private void UpdateProgressInterval()
		{
			float num = 10f * ForagedFoodPerDayCalculator.GetProgressPerTick(this.caravan, null);
			this.progress += num;
			if (this.progress >= 1f)
			{
				this.Forage();
				this.progress = 0f;
			}
		}

		// Token: 0x06008D33 RID: 36147 RVA: 0x0032CFA8 File Offset: 0x0032B1A8
		private void Forage()
		{
			ThingDef foragedFood = this.caravan.Biome.foragedFood;
			if (foragedFood == null)
			{
				return;
			}
			int i = GenMath.RoundRandom(ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(this.caravan, null));
			int b = Mathf.FloorToInt((this.caravan.MassCapacity - this.caravan.MassUsage) / foragedFood.GetStatValueAbstract(StatDefOf.Mass, null));
			i = Mathf.Min(i, b);
			while (i > 0)
			{
				Thing thing = ThingMaker.MakeThing(foragedFood, null);
				thing.stackCount = Mathf.Min(i, foragedFood.stackLimit);
				i -= thing.stackCount;
				CaravanInventoryUtility.GiveThing(this.caravan, thing);
			}
		}

		// Token: 0x0400597E RID: 22910
		private Caravan caravan;

		// Token: 0x0400597F RID: 22911
		private float progress;

		// Token: 0x04005980 RID: 22912
		private const int UpdateProgressIntervalTicks = 10;
	}
}
