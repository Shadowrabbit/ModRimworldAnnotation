using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BA2 RID: 2978
	public class JobDriver_VisitGrave : JobDriver_VisitJoyThing
	{
		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x060045F5 RID: 17909 RVA: 0x00193F60 File Offset: 0x00192160
		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x00193F88 File Offset: 0x00192188
		protected override void WaitTickAction()
		{
			float num = 1f;
			Room room = this.pawn.GetRoom(RegionType.Set_Passable);
			if (room != null)
			{
				num *= room.GetStat(RoomStatDefOf.GraveVisitingJoyGainFactor);
			}
			this.pawn.GainComfortFromCellIfPossible(false);
			JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, num, this.Grave);
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x00033443 File Offset: 0x00031643
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
