using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001239 RID: 4665
	public static class TraderCaravanUtility
	{
		// Token: 0x06006FFE RID: 28670 RVA: 0x00255250 File Offset: 0x00253450
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

		// Token: 0x06006FFF RID: 28671 RVA: 0x002552B4 File Offset: 0x002534B4
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

		// Token: 0x06007000 RID: 28672 RVA: 0x002552F9 File Offset: 0x002534F9
		public static float GenerateGuardPoints()
		{
			return (float)Rand.Range(550, 1000);
		}
	}
}
