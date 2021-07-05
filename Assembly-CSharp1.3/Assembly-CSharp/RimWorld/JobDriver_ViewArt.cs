using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000701 RID: 1793
	public class JobDriver_ViewArt : JobDriver_VisitJoyThing
	{
		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060031D7 RID: 12759 RVA: 0x00121490 File Offset: 0x0011F690
		private Thing ArtThing
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x001214B4 File Offset: 0x0011F6B4
		protected override void WaitTickAction()
		{
			float num = this.ArtThing.GetStatValue(StatDefOf.Beauty, true) / this.ArtThing.def.GetStatValueAbstract(StatDefOf.Beauty, null);
			float extraJoyGainFactor = (num > 0f) ? num : 0f;
			this.pawn.GainComfortFromCellIfPossible(false);
			JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, (Building)this.ArtThing);
		}
	}
}
