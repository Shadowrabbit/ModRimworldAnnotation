using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006ED RID: 1773
	public class JobDriver_MarryAdjacentPawn : JobDriver
	{
		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06003163 RID: 12643 RVA: 0x0011FD7C File Offset: 0x0011DF7C
		private Pawn OtherFiance
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06003164 RID: 12644 RVA: 0x0011FDA2 File Offset: 0x0011DFA2
		public int TicksLeftToMarry
		{
			get
			{
				return this.ticksLeftToMarry;
			}
		}

		// Token: 0x06003165 RID: 12645 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003166 RID: 12646 RVA: 0x0011FDAA File Offset: 0x0011DFAA
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

		// Token: 0x06003167 RID: 12647 RVA: 0x0011FDBA File Offset: 0x0011DFBA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToMarry, "ticksLeftToMarry", 0, false);
		}

		// Token: 0x04001D7D RID: 7549
		private int ticksLeftToMarry = 2500;

		// Token: 0x04001D7E RID: 7550
		private const TargetIndex OtherFianceInd = TargetIndex.A;

		// Token: 0x04001D7F RID: 7551
		private const int Duration = 2500;
	}
}
