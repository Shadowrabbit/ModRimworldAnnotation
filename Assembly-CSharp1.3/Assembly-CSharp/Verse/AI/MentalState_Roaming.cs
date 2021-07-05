using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005F0 RID: 1520
	public class MentalState_Roaming : MentalState
	{
		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06002BC2 RID: 11202 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x00104800 File Offset: 0x00102A00
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.exitDest, "exitDest", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.waitAtEdgeUntilTick, "waitAtEdgeUntilTick", 0, false);
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x00104840 File Offset: 0x00102A40
		public override void PreStart()
		{
			base.PreStart();
			Pawn_CallTracker caller = this.pawn.caller;
			if (caller != null)
			{
				caller.DoCall();
			}
			Messages.Message("MessageRoamerLeaving".Translate(this.pawn.Named("PAWN")).CapitalizeFirst(), this.pawn, MessageTypeDefOf.ThreatSmall, true);
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x001048A8 File Offset: 0x00102AA8
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (!this.exitDest.IsValid || !this.pawn.CanReach(this.exitDest, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				this.waitAtEdgeUntilTick = -1;
				this.exitDest = IntVec3.Invalid;
				if (!RCellFinder.TryFindRandomExitSpot(this.pawn, out this.exitDest, TraverseMode.ByPawn))
				{
					base.RecoverFromState();
					return;
				}
			}
			if (this.waitAtEdgeUntilTick < 0 && this.pawn.Position.InHorDistOf(this.exitDest, 12f))
			{
				this.waitAtEdgeUntilTick = Find.TickManager.TicksGame + MentalState_Roaming.WaitAtEdgeBeforeExitingTicks.RandomInRange;
			}
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x00104959 File Offset: 0x00102B59
		public override void PostEnd()
		{
			base.PostEnd();
			this.pawn.mindState.lastStartRoamCooldownTick = new int?(Find.TickManager.TicksGame);
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x00104980 File Offset: 0x00102B80
		public bool ShouldExitMapNow()
		{
			return (this.waitAtEdgeUntilTick > 0 && Find.TickManager.TicksGame > this.waitAtEdgeUntilTick) || base.Age > 60000;
		}

		// Token: 0x04001A9B RID: 6811
		public const int WanderDistance = 12;

		// Token: 0x04001A9C RID: 6812
		private const int MaxTicksToRoamBeforeExit = 60000;

		// Token: 0x04001A9D RID: 6813
		private static readonly IntRange WaitAtEdgeBeforeExitingTicks = new IntRange(7000, 8000);

		// Token: 0x04001A9E RID: 6814
		public IntVec3 exitDest = IntVec3.Invalid;

		// Token: 0x04001A9F RID: 6815
		public int waitAtEdgeUntilTick = -1;
	}
}
