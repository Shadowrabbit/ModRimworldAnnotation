using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002121 RID: 8481
	public class Caravan_ForageTracker : IExposable
	{
		// Token: 0x17001A7E RID: 6782
		// (get) Token: 0x0600B412 RID: 46098 RVA: 0x00074FB3 File Offset: 0x000731B3
		public Pair<ThingDef, float> ForagedFoodPerDay
		{
			get
			{
				return ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.caravan, null);
			}
		}

		// Token: 0x17001A7F RID: 6783
		// (get) Token: 0x0600B413 RID: 46099 RVA: 0x003446A0 File Offset: 0x003428A0
		public string ForagedFoodPerDayExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				ForagedFoodPerDayCalculator.ForagedFoodPerDay(this.caravan, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600B414 RID: 46100 RVA: 0x00074FC1 File Offset: 0x000731C1
		public Caravan_ForageTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600B415 RID: 46101 RVA: 0x00074FD0 File Offset: 0x000731D0
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.progress, "progress", 0f, false);
		}

		// Token: 0x0600B416 RID: 46102 RVA: 0x00074FE8 File Offset: 0x000731E8
		public void ForageTrackerTick()
		{
			if (this.caravan.IsHashIntervalTick(10))
			{
				this.UpdateProgressInterval();
			}
		}

		// Token: 0x0600B417 RID: 46103 RVA: 0x00074FFF File Offset: 0x000731FF
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

		// Token: 0x0600B418 RID: 46104 RVA: 0x003446C8 File Offset: 0x003428C8
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

		// Token: 0x0600B419 RID: 46105 RVA: 0x00344714 File Offset: 0x00342914
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

		// Token: 0x04007BB8 RID: 31672
		private Caravan caravan;

		// Token: 0x04007BB9 RID: 31673
		private float progress;

		// Token: 0x04007BBA RID: 31674
		private const int UpdateProgressIntervalTicks = 10;
	}
}
