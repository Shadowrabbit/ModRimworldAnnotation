using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000983 RID: 2435
	public class ThoughtWorker_NotBondedAnimalMaster : ThoughtWorker_BondedAnimalMaster
	{
		// Token: 0x06003D8E RID: 15758 RVA: 0x001527C5 File Offset: 0x001509C5
		protected override bool AnimalMasterCheck(Pawn p, Pawn animal)
		{
			return animal.playerSettings.RespectedMaster != p;
		}
	}
}
