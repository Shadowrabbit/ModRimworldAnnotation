using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007F1 RID: 2033
	public class JoyGiver_InPrivateRoom : JoyGiver
	{
		// Token: 0x06003679 RID: 13945 RVA: 0x00134E90 File Offset: 0x00133090
		public override Job TryGiveJob(Pawn pawn)
		{
			Room room = null;
			if (ModsConfig.IdeologyActive)
			{
				room = MeditationUtility.UsableWorshipRooms(pawn).RandomElementWithFallback(null);
			}
			if (room == null)
			{
				Pawn_Ownership ownership = pawn.ownership;
				room = ((ownership != null) ? ownership.OwnedRoom : null);
			}
			if (room == null)
			{
				return null;
			}
			IntVec3 c2;
			if (!(from c in room.Cells
			where c.Standable(pawn.Map) && !c.IsForbidden(pawn) && pawn.CanReserveAndReach(c, PathEndMode.OnCell, Danger.None, 1, -1, null, false)
			select c).TryRandomElement(out c2))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, c2);
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x00134F1D File Offset: 0x0013311D
		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return JobMaker.MakeJob(this.def.jobDef, pawn.CurrentBed());
		}
	}
}
