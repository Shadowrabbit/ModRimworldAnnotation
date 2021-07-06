using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C3 RID: 4803
	public class GenStep_DownedRefugee : GenStep_Scatterer
	{
		// Token: 0x1700100A RID: 4106
		// (get) Token: 0x06006824 RID: 26660 RVA: 0x00046EBD File Offset: 0x000450BD
		public override int SeedPart
		{
			get
			{
				return 931842770;
			}
		}

		// Token: 0x06006825 RID: 26661 RVA: 0x00046EC4 File Offset: 0x000450C4
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return base.CanScatterAt(c, map) && c.Standable(map);
		}

		// Token: 0x06006826 RID: 26662 RVA: 0x00202154 File Offset: 0x00200354
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Pawn pawn;
			if (parms.sitePart != null && parms.sitePart.things != null && parms.sitePart.things.Any)
			{
				pawn = (Pawn)parms.sitePart.things.Take(parms.sitePart.things[0]);
			}
			else
			{
				DownedRefugeeComp component = map.Parent.GetComponent<DownedRefugeeComp>();
				if (component != null && component.pawn.Any)
				{
					pawn = component.pawn.Take(component.pawn[0]);
				}
				else
				{
					pawn = DownedRefugeeQuestUtility.GenerateRefugee(map.Tile);
				}
			}
			HealthUtility.DamageUntilDowned(pawn, false);
			HealthUtility.DamageLegsUntilIncapableOfMoving(pawn, false);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			pawn.mindState.WillJoinColonyIfRescued = true;
			MapGenerator.rootsToUnfog.Add(loc);
			MapGenerator.SetVar<CellRect>("RectOfInterest", CellRect.CenteredOn(loc, 1, 1));
		}
	}
}
