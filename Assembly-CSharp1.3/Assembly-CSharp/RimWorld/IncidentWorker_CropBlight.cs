using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BFF RID: 3071
	public class IncidentWorker_CropBlight : IncidentWorker
	{
		// Token: 0x06004846 RID: 18502 RVA: 0x0017E260 File Offset: 0x0017C460
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Plant plant;
			return this.TryFindRandomBlightablePlant((Map)parms.target, out plant);
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x0017E280 File Offset: 0x0017C480
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			float num = IncidentWorker_CropBlight.RadiusFactorPerPointsCurve.Evaluate(parms.points);
			Plant plant;
			if (!this.TryFindRandomBlightablePlant(map, out plant))
			{
				return false;
			}
			Room room = plant.GetRoom(RegionType.Set_All);
			int i = 0;
			int num2 = GenRadial.NumCellsInRadius(11f * num);
			while (i < num2)
			{
				IntVec3 intVec = plant.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && intVec.GetRoom(map) == room)
				{
					Plant firstBlightableNowPlant = BlightUtility.GetFirstBlightableNowPlant(intVec, map);
					if (firstBlightableNowPlant != null && firstBlightableNowPlant.def == plant.def && Rand.Chance(this.BlightChance(firstBlightableNowPlant.Position, plant.Position, num)))
					{
						firstBlightableNowPlant.CropBlighted();
					}
				}
				i++;
			}
			base.SendStandardLetter("LetterLabelCropBlight".Translate(new NamedArgument(plant.def, "PLANTDEF")), "LetterCropBlight".Translate(new NamedArgument(plant.def, "PLANTDEF")), LetterDefOf.NegativeEvent, parms, new TargetInfo(plant.Position, map, false), Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x0017E3A4 File Offset: 0x0017C5A4
		private bool TryFindRandomBlightablePlant(Map map, out Plant plant)
		{
			Thing thing;
			bool result = (from x in map.listerThings.ThingsInGroup(ThingRequestGroup.Plant)
			where ((Plant)x).BlightableNow
			select x).TryRandomElement(out thing);
			plant = (Plant)thing;
			return result;
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x0017E3F4 File Offset: 0x0017C5F4
		private float BlightChance(IntVec3 c, IntVec3 root, float radiusFactor)
		{
			float x = c.DistanceTo(root) / radiusFactor;
			return IncidentWorker_CropBlight.BlightChancePerRadius.Evaluate(x);
		}

		// Token: 0x04002C54 RID: 11348
		private const float Radius = 11f;

		// Token: 0x04002C55 RID: 11349
		private static readonly SimpleCurve BlightChancePerRadius = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(8f, 1f),
				true
			},
			{
				new CurvePoint(11f, 0.3f),
				true
			}
		};

		// Token: 0x04002C56 RID: 11350
		private static readonly SimpleCurve RadiusFactorPerPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 0.6f),
				true
			},
			{
				new CurvePoint(500f, 1f),
				true
			},
			{
				new CurvePoint(2000f, 2f),
				true
			}
		};
	}
}
