using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000710 RID: 1808
	public class JobDriver_EnterTransporter : JobDriver
	{
		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x0600322E RID: 12846 RVA: 0x001220CC File Offset: 0x001202CC
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

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x0600322F RID: 12847 RVA: 0x00122100 File Offset: 0x00120300
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

		// Token: 0x06003230 RID: 12848 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x00122132 File Offset: 0x00120332
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.TransporterInd);
			this.FailOn(() => this.Shuttle != null && !this.Shuttle.IsAllowed(this.pawn));
			yield return Toils_Goto.GotoThing(this.TransporterInd, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = delegate()
				{
					if (!this.Transporter.LoadingInProgressOrReadyToLaunch)
					{
						TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
					}
					CompTransporter transporter = this.Transporter;
					this.pawn.DeSpawn(DestroyMode.Vanish);
					transporter.GetDirectlyHeldThings().TryAdd(this.pawn, true);
				}
			};
			yield break;
		}

		// Token: 0x04001DB7 RID: 7607
		private TargetIndex TransporterInd = TargetIndex.A;
	}
}
