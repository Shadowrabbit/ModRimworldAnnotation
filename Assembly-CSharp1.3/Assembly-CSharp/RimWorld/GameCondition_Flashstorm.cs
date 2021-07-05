using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000BDE RID: 3038
	public class GameCondition_Flashstorm : GameCondition
	{
		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06004784 RID: 18308 RVA: 0x0017A672 File Offset: 0x00178872
		public int AreaRadius
		{
			get
			{
				return this.areaRadius;
			}
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x0017A67C File Offset: 0x0017887C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec2>(ref this.centerLocation, "centerLocation", default(IntVec2), false);
			Scribe_Values.Look<int>(ref this.areaRadius, "areaRadius", 0, false);
			Scribe_Values.Look<IntRange>(ref this.areaRadiusOverride, "areaRadiusOverride", default(IntRange), false);
			Scribe_Values.Look<int>(ref this.nextLightningTicks, "nextLightningTicks", 0, false);
			Scribe_Values.Look<IntRange>(ref this.initialStrikeDelay, "initialStrikeDelay", default(IntRange), false);
			Scribe_Values.Look<bool>(ref this.ambientSound, "ambientSound", false, false);
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x0017A714 File Offset: 0x00178914
		public override void Init()
		{
			base.Init();
			this.areaRadius = ((this.areaRadiusOverride == IntRange.zero) ? GameCondition_Flashstorm.AreaRadiusRange.RandomInRange : this.areaRadiusOverride.RandomInRange);
			this.nextLightningTicks = Find.TickManager.TicksGame + this.initialStrikeDelay.RandomInRange;
			if (this.centerLocation.IsInvalid)
			{
				this.FindGoodCenterLocation();
			}
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x0017A788 File Offset: 0x00178988
		public override void GameConditionTick()
		{
			if (Find.TickManager.TicksGame > this.nextLightningTicks)
			{
				Vector2 vector = Rand.UnitVector2 * Rand.Range(0f, (float)this.areaRadius);
				IntVec3 intVec = new IntVec3((int)Math.Round((double)vector.x) + this.centerLocation.x, 0, (int)Math.Round((double)vector.y) + this.centerLocation.z);
				if (this.IsGoodLocationForStrike(intVec))
				{
					base.SingleMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(base.SingleMap, intVec));
					this.nextLightningTicks = Find.TickManager.TicksGame + GameCondition_Flashstorm.TicksBetweenStrikes.RandomInRange;
				}
			}
			if (this.ambientSound)
			{
				if (this.soundSustainer == null || this.soundSustainer.Ended)
				{
					this.soundSustainer = SoundDefOf.FlashstormAmbience.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(this.centerLocation.ToIntVec3, base.SingleMap, false), MaintenanceType.PerTick));
					return;
				}
				this.soundSustainer.Maintain();
			}
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x0017A89E File Offset: 0x00178A9E
		public override void End()
		{
			base.SingleMap.weatherDecider.DisableRainFor(30000);
			base.End();
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x0017A8BC File Offset: 0x00178ABC
		private void FindGoodCenterLocation()
		{
			if (base.SingleMap.Size.x <= 16 || base.SingleMap.Size.z <= 16)
			{
				throw new Exception("Map too small for flashstorm.");
			}
			for (int i = 0; i < 10; i++)
			{
				this.centerLocation = new IntVec2(Rand.Range(8, base.SingleMap.Size.x - 8), Rand.Range(8, base.SingleMap.Size.z - 8));
				if (this.IsGoodCenterLocation(this.centerLocation))
				{
					break;
				}
			}
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x0017A952 File Offset: 0x00178B52
		private bool IsGoodLocationForStrike(IntVec3 loc)
		{
			return loc.InBounds(base.SingleMap) && !loc.Roofed(base.SingleMap) && loc.Standable(base.SingleMap);
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x0017A980 File Offset: 0x00178B80
		private bool IsGoodCenterLocation(IntVec2 loc)
		{
			int num = 0;
			int num2 = (int)(3.1415927f * (float)this.areaRadius * (float)this.areaRadius / 2f);
			foreach (IntVec3 loc2 in this.GetPotentiallyAffectedCells(loc))
			{
				if (this.IsGoodLocationForStrike(loc2))
				{
					num++;
				}
				if (num >= num2)
				{
					break;
				}
			}
			return num >= num2;
		}

		// Token: 0x0600478C RID: 18316 RVA: 0x0017AA00 File Offset: 0x00178C00
		private IEnumerable<IntVec3> GetPotentiallyAffectedCells(IntVec2 center)
		{
			int num;
			for (int x = center.x - this.areaRadius; x <= center.x + this.areaRadius; x = num)
			{
				for (int z = center.z - this.areaRadius; z <= center.z + this.areaRadius; z = num)
				{
					if ((center.x - x) * (center.x - x) + (center.z - z) * (center.z - z) <= this.areaRadius * this.areaRadius)
					{
						yield return new IntVec3(x, 0, z);
					}
					num = z + 1;
				}
				num = x + 1;
			}
			yield break;
		}

		// Token: 0x04002BE2 RID: 11234
		private static readonly IntRange AreaRadiusRange = new IntRange(45, 60);

		// Token: 0x04002BE3 RID: 11235
		private static readonly IntRange TicksBetweenStrikes = new IntRange(320, 800);

		// Token: 0x04002BE4 RID: 11236
		private const int RainDisableTicksAfterConditionEnds = 30000;

		// Token: 0x04002BE5 RID: 11237
		public IntVec2 centerLocation = IntVec2.Invalid;

		// Token: 0x04002BE6 RID: 11238
		public IntRange areaRadiusOverride = IntRange.zero;

		// Token: 0x04002BE7 RID: 11239
		public IntRange initialStrikeDelay = IntRange.zero;

		// Token: 0x04002BE8 RID: 11240
		public bool ambientSound;

		// Token: 0x04002BE9 RID: 11241
		private int areaRadius;

		// Token: 0x04002BEA RID: 11242
		private int nextLightningTicks;

		// Token: 0x04002BEB RID: 11243
		private Sustainer soundSustainer;
	}
}
