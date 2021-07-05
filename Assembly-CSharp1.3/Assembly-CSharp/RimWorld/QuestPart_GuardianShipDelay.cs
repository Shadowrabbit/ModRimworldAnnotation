using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B7B RID: 2939
	public class QuestPart_GuardianShipDelay : QuestPartActivable
	{
		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x060044B3 RID: 17587 RVA: 0x0016C39E File Offset: 0x0016A59E
		public int TicksLeft
		{
			get
			{
				if (base.State != QuestPartState.Enabled)
				{
					return 0;
				}
				return this.delayTicks - this.ticksPassed;
			}
		}

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x060044B4 RID: 17588 RVA: 0x0016C3B8 File Offset: 0x0016A5B8
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.pawn != null)
				{
					yield return this.pawn;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x0016C3C8 File Offset: 0x0016A5C8
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.ticksPassed = 0;
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x0016C3D8 File Offset: 0x0016A5D8
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (!this.pawn.DestroyedOrNull() && !this.pawn.Suspended)
			{
				this.ticksPassed++;
				if (this.ticksPassed >= this.delayTicks)
				{
					base.Complete();
				}
			}
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x0016C427 File Offset: 0x0016A627
		public override IEnumerable<Gizmo> ExtraGizmos(ISelectable target)
		{
			if (target == this.pawn)
			{
				if (this.gizmo == null)
				{
					this.gizmo = new GuardianShipGizmo(this);
				}
				yield return this.gizmo;
			}
			yield break;
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x0016C43E File Offset: 0x0016A63E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x0016C47B File Offset: 0x0016A67B
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.pawn = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.RandomElement<Pawn>();
			this.delayTicks = Rand.RangeInclusive(833, 2500);
		}

		// Token: 0x040029B2 RID: 10674
		public Pawn pawn;

		// Token: 0x040029B3 RID: 10675
		public int delayTicks;

		// Token: 0x040029B4 RID: 10676
		public int ticksPassed;

		// Token: 0x040029B5 RID: 10677
		private Gizmo gizmo;
	}
}
