using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010DF RID: 4319
	public class ThingSetMaker_Count : ThingSetMaker
	{
		// Token: 0x0600674D RID: 26445 RVA: 0x0022E758 File Offset: 0x0022C958
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

		// Token: 0x0600674E RID: 26446 RVA: 0x0022E824 File Offset: 0x0022CA24
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
			int num4 = 0;
			ThingStuffPair thingStuffPair;
			while (num4 < num2 && ThingSetMakerUtility.TryGetRandomThingWhichCanWeighNoMoreThan(enumerable, stuffTechLevel, (num == 3.4028235E+38f) ? 3.4028235E+38f : (num - num3), parms.qualityGenerator, out thingStuffPair))
			{
				Thing thing = ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
				ThingSetMakerUtility.AssignQuality(thing, parms.qualityGenerator);
				outThings.Add(thing);
				if (!(thing is Pawn))
				{
					num3 += thing.GetStatValue(StatDefOf.Mass, true) * (float)thing.stackCount;
				}
				num4++;
			}
		}

		// Token: 0x0600674F RID: 26447 RVA: 0x0022E937 File Offset: 0x0022CB37
		protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
		{
			return ThingSetMakerUtility.GetAllowedThingDefs(parms);
		}

		// Token: 0x06006750 RID: 26448 RVA: 0x0022E93F File Offset: 0x0022CB3F
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
