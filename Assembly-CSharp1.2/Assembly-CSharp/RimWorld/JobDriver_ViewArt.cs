using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BA1 RID: 2977
	public class JobDriver_ViewArt : JobDriver_VisitJoyThing
	{
		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x060045F2 RID: 17906 RVA: 0x0018EA98 File Offset: 0x0018CC98
		private Thing ArtThing
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060045F3 RID: 17907 RVA: 0x00193EF4 File Offset: 0x001920F4
		protected override void WaitTickAction()
		{
			float num = this.ArtThing.GetStatValue(StatDefOf.Beauty, true) / this.ArtThing.def.GetStatValueAbstract(StatDefOf.Beauty, null);
			float extraJoyGainFactor = (num > 0f) ? num : 0f;
			this.pawn.GainComfortFromCellIfPossible(false);
			JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, (Building)this.ArtThing);
		}
	}
}
