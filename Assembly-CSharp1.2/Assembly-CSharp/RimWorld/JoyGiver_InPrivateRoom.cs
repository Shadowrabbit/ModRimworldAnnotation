using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D22 RID: 3362
	public class JoyGiver_InPrivateRoom : JoyGiver
	{
		// Token: 0x06004D17 RID: 19735 RVA: 0x001AD230 File Offset: 0x001AB430
		public override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.ownership == null)
			{
				return null;
			}
			Room ownedRoom = pawn.ownership.OwnedRoom;
			if (ownedRoom == null)
			{
				return null;
			}
			IntVec3 c2;
			if (!(from c in ownedRoom.Cells
			where c.Standable(pawn.Map) && !c.IsForbidden(pawn) && pawn.CanReserveAndReach(c, PathEndMode.OnCell, Danger.None, 1, -1, null, false)
			select c).TryRandomElement(out c2))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, c2);
		}

		// Token: 0x06004D18 RID: 19736 RVA: 0x00036A18 File Offset: 0x00034C18
		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return JobMaker.MakeJob(this.def.jobDef, pawn.CurrentBed());
		}
	}
}
