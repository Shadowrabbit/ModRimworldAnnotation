using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020011AD RID: 4525
	public class IncidentWorker_CropBlight : IncidentWorker
	{
		// Token: 0x06006395 RID: 25493 RVA: 0x001EFE4C File Offset: 0x001EE04C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Plant plant;
			return this.TryFindRandomBlightablePlant((Map)parms.target, out plant);
		}

		// Token: 0x06006396 RID: 25494 RVA: 0x001EFE6C File Offset: 0x001EE06C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			float num = IncidentWorker_CropBlight.RadiusFactorPerPointsCurve.Evaluate(parms.points);
			Plant plant;
			if (!this.TryFindRandomBlightablePlant(map, out plant))
			{
				return false;
			}
			Room room = plant.GetRoom(RegionType.Set_Passable);
			int i = 0;
			int num2 = GenRadial.NumCellsInRadius(11f * num);
			while (i < num2)
			{
				IntVec3 intVec = plant.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && intVec.GetRoom(map, RegionType.Set_Passable) == room)
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

		// Token: 0x06006397 RID: 25495 RVA: 0x001EFF90 File Offset: 0x001EE190
		private bool TryFindRandomBlightablePlant(Map map, out Plant plant)
		{
			Thing thing;
			bool result = (from x in map.listerThings.ThingsInGroup(ThingRequestGroup.Plant)
			where ((Plant)x).BlightableNow
			select x).TryRandomElement(out thing);
			plant = (Plant)thing;
			return result;
		}

		// Token: 0x06006398 RID: 25496 RVA: 0x001EFFE0 File Offset: 0x001EE1E0
		private float BlightChance(IntVec3 c, IntVec3 root, float radiusFactor)
		{
			float x = c.DistanceTo(root) / radiusFactor;
			return IncidentWorker_CropBlight.BlightChancePerRadius.Evaluate(x);
		}

		// Token: 0x040042A3 RID: 17059
		private const float Radius = 11f;

		// Token: 0x040042A4 RID: 17060
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

		// Token: 0x040042A5 RID: 17061
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
