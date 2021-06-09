using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8D RID: 3725
	public class ThoughtWorker_NotBondedAnimalMaster : ThoughtWorker_BondedAnimalMaster
	{
		// Token: 0x06005368 RID: 21352 RVA: 0x0003A305 File Offset: 0x00038505
		protected override bool AnimalMasterCheck(Pawn p, Pawn animal)
		{
			return animal.playerSettings.RespectedMaster != p;
		}
	}
}
