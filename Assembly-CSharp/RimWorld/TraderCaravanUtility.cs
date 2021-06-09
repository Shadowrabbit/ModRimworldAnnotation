using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001924 RID: 6436
	public static class TraderCaravanUtility
	{
		// Token: 0x06008E88 RID: 36488 RVA: 0x00291BA4 File Offset: 0x0028FDA4
		public static TraderCaravanRole GetTraderCaravanRole(this Pawn p)
		{
			if (p.kindDef == PawnKindDefOf.Slave)
			{
				return TraderCaravanRole.Chattel;
			}
			if (p.kindDef.trader)
			{
				return TraderCaravanRole.Trader;
			}
			if (p.kindDef.RaceProps.packAnimal && p.inventory.innerContainer.Any)
			{
				return TraderCaravanRole.Carrier;
			}
			if (p.RaceProps.Animal)
			{
				return TraderCaravanRole.Chattel;
			}
			return TraderCaravanRole.Guard;
		}

		// Token: 0x06008E89 RID: 36489 RVA: 0x00291C08 File Offset: 0x0028FE08
		public static Pawn FindTrader(Lord lord)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].GetTraderCaravanRole() == TraderCaravanRole.Trader)
				{
					return lord.ownedPawns[i];
				}
			}
			return null;
		}

		// Token: 0x06008E8A RID: 36490 RVA: 0x0005F7E0 File Offset: 0x0005D9E0
		public static float GenerateGuardPoints()
		{
			return (float)Rand.Range(550, 1000);
		}
	}
}
