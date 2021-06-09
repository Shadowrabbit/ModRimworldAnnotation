using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001174 RID: 4468
	public class GameCondition_Flashstorm : GameCondition
	{
		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x06006268 RID: 25192 RVA: 0x00043B9A File Offset: 0x00041D9A
		public int AreaRadius
		{
			get
			{
				return this.areaRadius;
			}
		}

		// Token: 0x06006269 RID: 25193 RVA: 0x001EBD04 File Offset: 0x001E9F04
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

		// Token: 0x0600626A RID: 25194 RVA: 0x001EBD9C File Offset: 0x001E9F9C
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

		// Token: 0x0600626B RID: 25195 RVA: 0x001EBE10 File Offset: 0x001EA010
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

		// Token: 0x0600626C RID: 25196 RVA: 0x00043BA2 File Offset: 0x00041DA2
		public override void End()
		{
			base.SingleMap.weatherDecider.DisableRainFor(30000);
			base.End();
		}

		// Token: 0x0600626D RID: 25197 RVA: 0x001EBF28 File Offset: 0x001EA128
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

		// Token: 0x0600626E RID: 25198 RVA: 0x00043BBF File Offset: 0x00041DBF
		private bool IsGoodLocationForStrike(IntVec3 loc)
		{
			return loc.InBounds(base.SingleMap) && !loc.Roofed(base.SingleMap) && loc.Standable(base.SingleMap);
		}

		// Token: 0x0600626F RID: 25199 RVA: 0x001EBFC0 File Offset: 0x001EA1C0
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

		// Token: 0x06006270 RID: 25200 RVA: 0x00043BEB File Offset: 0x00041DEB
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

		// Token: 0x040041F2 RID: 16882
		private static readonly IntRange AreaRadiusRange = new IntRange(45, 60);

		// Token: 0x040041F3 RID: 16883
		private static readonly IntRange TicksBetweenStrikes = new IntRange(320, 800);

		// Token: 0x040041F4 RID: 16884
		private const int RainDisableTicksAfterConditionEnds = 30000;

		// Token: 0x040041F5 RID: 16885
		public IntVec2 centerLocation = IntVec2.Invalid;

		// Token: 0x040041F6 RID: 16886
		public IntRange areaRadiusOverride = IntRange.zero;

		// Token: 0x040041F7 RID: 16887
		public IntRange initialStrikeDelay = IntRange.zero;

		// Token: 0x040041F8 RID: 16888
		public bool ambientSound;

		// Token: 0x040041F9 RID: 16889
		private int areaRadius;

		// Token: 0x040041FA RID: 16890
		private int nextLightningTicks;

		// Token: 0x040041FB RID: 16891
		private Sustainer soundSustainer;
	}
}
