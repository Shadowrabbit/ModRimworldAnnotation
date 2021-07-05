using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E2 RID: 4322
	public class ThingSetMaker_StackCount : ThingSetMaker
	{
		// Token: 0x06006763 RID: 26467 RVA: 0x0022F058 File Offset: 0x0022D258
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			if (!this.AllowedThingDefs(parms).Any<ThingDef>())
			{
				return false;
			}
			if (parms.countRange != null && parms.countRange.Value.max <= 0)
			{
				return false;
			}
			if (parms.maxTotalMass != null)
			{
				float? maxTotalMass = parms.maxTotalMass;
				float maxValue = float.MaxValue;
				if (!(maxTotalMass.GetValueOrDefault() == maxValue & maxTotalMass != null) && !ThingSetMakerUtility.PossibleToWeighNoMoreThan(this.AllowedThingDefs(parms), parms.techLevel ?? TechLevel.Undefined, parms.maxTotalMass.Value, (parms.countRange != null) ? parms.countRange.Value.max : 1))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006764 RID: 26468 RVA: 0x0022F124 File Offset: 0x0022D324
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			IEnumerable<ThingDef> enumerable = this.AllowedThingDefs(parms);
			if (!enumerable.Any<ThingDef>())
			{
				return;
			}
			TechLevel stuffTechLevel = parms.techLevel ?? TechLevel.Undefined;
			IntRange intRange = parms.countRange ?? IntRange.one;
			float num = parms.maxTotalMass ?? float.MaxValue;
			int num2 = Mathf.Max(intRange.RandomInRange, 1);
			float num3 = 0f;
			int num4 = num2;
			ThingStuffPair thingStuffPair;
			while (num4 > 0 && ThingSetMakerUtility.TryGetRandomThingWhichCanWeighNoMoreThan(enumerable, stuffTechLevel, (num == 3.4028235E+38f) ? 3.4028235E+38f : (num - num3), parms.qualityGenerator, out thingStuffPair))
			{
				Thing thing = ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
				ThingSetMakerUtility.AssignQuality(thing, parms.qualityGenerator);
				int num5 = num4;
				if (num != 3.4028235E+38f && !(thing is Pawn))
				{
					num5 = Mathf.Min(num5, Mathf.FloorToInt((num - num3) / thing.GetStatValue(StatDefOf.Mass, true)));
				}
				num5 = Mathf.Clamp(num5, 1, thing.def.stackLimit);
				thing.stackCount = num5;
				num4 -= num5;
				outThings.Add(thing);
				if (!(thing is Pawn))
				{
					num3 += thing.GetStatValue(StatDefOf.Mass, true) * (float)thing.stackCount;
				}
			}
		}

		// Token: 0x06006765 RID: 26469 RVA: 0x0022E937 File Offset: 0x0022CB37
		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		// Token: 0x06006766 RID: 26470 RVA: 0x0022F291 File Offset: 0x0022D491
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			TechLevel techLevel = parms.techLevel ?? TechLevel.Undefined;
			foreach (ThingDef thingDef in this.AllowedThingDefs(parms))
			{
				if (parms.maxTotalMass != null)
				{
					float? maxTotalMass = parms.maxTotalMass;
					float maxValue = float.MaxValue;
					if (!(maxTotalMass.GetValueOrDefault() == maxValue & maxTotalMass != null))
					{
						float minMass = ThingSetMakerUtility.GetMinMass(thingDef, techLevel);
						maxTotalMass = parms.maxTotalMass;
						if (minMass > maxTotalMass.GetValueOrDefault() & maxTotalMass != null)
						{
							continue;
						}
					}
				}
				yield return thingDef;
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}
	}
}
