using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BBD RID: 3005
	public class JobDriver_EnterTransporter : JobDriver
	{
		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06004698 RID: 18072 RVA: 0x00195C20 File Offset: 0x00193E20
		public CompTransporter Transporter
		{
			get
			{
				Thing thing = this.job.GetTarget(this.TransporterInd).Thing;
				if (thing == null)
				{
					return null;
				}
				return thing.TryGetComp<CompTransporter>();
			}
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06004699 RID: 18073 RVA: 0x00195C54 File Offset: 0x00193E54
		public CompShuttle Shuttle
		{
			get
			{
				Thing thing = this.job.GetTarget(this.TransporterInd).Thing;
				if (thing == null)
				{
					return null;
				}
				return thing.TryGetComp<CompShuttle>();
			}
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600469B RID: 18075 RVA: 0x00033894 File Offset: 0x00031A94
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.TransporterInd);
			this.FailOn(() => !this.Transporter.LoadingInProgressOrReadyToLaunch);
			this.FailOn(() => this.Shuttle != null && !this.Shuttle.IsAllowedNow(this.pawn));
			yield return Toils_Goto.GotoThing(this.TransporterInd, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = delegate()
				{
					CompTransporter transporter = this.Transporter;
					this.pawn.DeSpawn(DestroyMode.Vanish);
					transporter.GetDirectlyHeldThings().TryAdd(this.pawn, true);
				}
			};
			yield break;
		}

		// Token: 0x04002F71 RID: 12145
		private TargetIndex TransporterInd = TargetIndex.A;
	}
}
