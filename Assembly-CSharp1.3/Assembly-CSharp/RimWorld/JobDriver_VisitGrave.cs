using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000702 RID: 1794
	public class JobDriver_VisitGrave : JobDriver_VisitJoyThing
	{
		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060031DA RID: 12762 RVA: 0x00121528 File Offset: 0x0011F728
		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060031DB RID: 12763 RVA: 0x00121550 File Offset: 0x0011F750
		protected override void WaitTickAction()
		{
			float num = 1f;
			Room room = this.pawn.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				num *= room.GetStat(RoomStatDefOf.GraveVisitingJoyGainFactor);
			}
			this.pawn.GainComfortFromCellIfPossible(false);
			JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, num, this.Grave);
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x001215A2 File Offset: 0x0011F7A2
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				(this.Grave.Corpse != null) ? this.Grave.Corpse.InnerPawn : null
			};
		}
	}
}
