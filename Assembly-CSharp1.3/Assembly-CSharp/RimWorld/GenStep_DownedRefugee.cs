using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CB7 RID: 3255
	public class GenStep_DownedRefugee : GenStep_Scatterer
	{
		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06004BDA RID: 19418 RVA: 0x00194146 File Offset: 0x00192346
		public override int SeedPart
		{
			get
			{
				return 931842770;
			}
		}

		// Token: 0x06004BDB RID: 19419 RVA: 0x0019414D File Offset: 0x0019234D
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return base.CanScatterAt(c, map) && c.Standable(map);
		}

		// Token: 0x06004BDC RID: 19420 RVA: 0x00194164 File Offset: 0x00192364
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
					pawn = DownedRefugeeQuestUtility.GenerateRefugee(map.Tile, null, 0.6f);
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
