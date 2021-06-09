using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B7A RID: 2938
	public class JobDriver_MarryAdjacentPawn : JobDriver
	{
		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06004514 RID: 17684 RVA: 0x0016B214 File Offset: 0x00169414
		private Pawn OtherFiance
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06004515 RID: 17685 RVA: 0x00032DD0 File Offset: 0x00030FD0
		public int TicksLeftToMarry
		{
			get
			{
				return this.ticksLeftToMarry;
			}
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004517 RID: 17687 RVA: 0x00032DD8 File Offset: 0x00030FD8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => this.OtherFiance.Drafted || !this.pawn.Position.AdjacentTo8WayOrInside(this.OtherFiance));
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				this.ticksLeftToMarry = 2500;
			};
			toil.tickAction = delegate()
			{
				this.ticksLeftToMarry--;
				if (this.ticksLeftToMarry <= 0)
				{
					this.ticksLeftToMarry = 0;
					base.ReadyForNextToil();
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.FailOn(() => !this.pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, this.OtherFiance));
			yield return toil;
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Instant,
				initAction = delegate()
				{
					if (this.pawn.thingIDNumber < this.OtherFiance.thingIDNumber)
					{
						MarriageCeremonyUtility.Married(this.pawn, this.OtherFiance);
					}
				}
			};
			yield break;
		}

		// Token: 0x06004518 RID: 17688 RVA: 0x00032DE8 File Offset: 0x00030FE8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToMarry, "ticksLeftToMarry", 0, false);
		}

		// Token: 0x04002EBE RID: 11966
		private int ticksLeftToMarry = 2500;

		// Token: 0x04002EBF RID: 11967
		private const TargetIndex OtherFianceInd = TargetIndex.A;

		// Token: 0x04002EC0 RID: 11968
		private const int Duration = 2500;
	}
}
