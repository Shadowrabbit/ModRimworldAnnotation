using System;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020011CF RID: 4559
	public class CompWakeUpDormant : ThingComp
	{
		// Token: 0x1700131D RID: 4893
		// (get) Token: 0x06006DFE RID: 28158 RVA: 0x0024DFAC File Offset: 0x0024C1AC
		private CompProperties_WakeUpDormant Props
		{
			get
			{
				return (CompProperties_WakeUpDormant)this.props;
			}
		}

		// Token: 0x06006DFF RID: 28159 RVA: 0x0024DFB9 File Offset: 0x0024C1B9
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.wakeUpIfColonistClose = this.Props.wakeUpIfAnyColonistClose;
		}

		// Token: 0x06006E00 RID: 28160 RVA: 0x0024DFD3 File Offset: 0x0024C1D3
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		// Token: 0x06006E01 RID: 28161 RVA: 0x0024DFF4 File Offset: 0x0024C1F4
		public void TickRareWorker()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.wakeUpIfColonistClose)
			{
				int num = GenRadial.NumCellsInRadius(this.Props.anyColonistCloseCheckRadius);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
					if (intVec.InBounds(this.parent.Map) && GenSight.LineOfSight(this.parent.Position, intVec, this.parent.Map, false, null, 0, 0))
					{
						foreach (Thing thing in intVec.GetThingList(this.parent.Map))
						{
							Pawn pawn = thing as Pawn;
							if (pawn != null && pawn.IsColonist)
							{
								this.Activate(true, false);
								return;
							}
						}
					}
				}
			}
			if (this.Props.wakeUpOnThingConstructedRadius > 0f)
			{
				if (GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false), this.Props.wakeUpOnThingConstructedRadius, (Thing t) => t.Faction == Faction.OfPlayer, null, 0, -1, false, RegionType.Set_Passable, false) != null)
				{
					this.Activate(true, false);
				}
			}
		}

		// Token: 0x06006E02 RID: 28162 RVA: 0x0024E174 File Offset: 0x0024C374
		public void Activate(bool sendSignal = true, bool silent = false)
		{
			if (sendSignal && !this.sentSignal)
			{
				if (!string.IsNullOrEmpty(this.Props.wakeUpSignalTag))
				{
					if (this.Props.onlyWakeUpSameFaction)
					{
						Find.SignalManager.SendSignal(new Signal(this.Props.wakeUpSignalTag, this.parent.Named("SUBJECT"), this.parent.Faction.Named("FACTION")));
					}
					else
					{
						Find.SignalManager.SendSignal(new Signal(this.Props.wakeUpSignalTag, this.parent.Named("SUBJECT")));
					}
				}
				if (!silent && this.parent.Spawned && this.Props.wakeUpSound != null)
				{
					this.Props.wakeUpSound.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
				}
				this.sentSignal = true;
			}
			CompCanBeDormant compCanBeDormant = this.parent.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant != null)
			{
				compCanBeDormant.WakeUp();
			}
		}

		// Token: 0x06006E03 RID: 28163 RVA: 0x0024E286 File Offset: 0x0024C486
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Props.wakeUpOnDamage && totalDamageDealt > 0f && dinfo.Def.ExternalViolenceFor(this.parent))
			{
				this.Activate(true, false);
			}
		}

		// Token: 0x06006E04 RID: 28164 RVA: 0x0024E2B9 File Offset: 0x0024C4B9
		public override void Notify_SignalReceived(Signal signal)
		{
			if (string.IsNullOrEmpty(this.Props.wakeUpSignalTag))
			{
				return;
			}
			this.sentSignal = true;
		}

		// Token: 0x06006E05 RID: 28165 RVA: 0x0024E2D5 File Offset: 0x0024C4D5
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.wakeUpIfColonistClose, "wakeUpIfColonistClose", false, false);
			Scribe_Values.Look<bool>(ref this.sentSignal, "sentSignal", false, false);
		}

		// Token: 0x04003D17 RID: 15639
		public bool wakeUpIfColonistClose;

		// Token: 0x04003D18 RID: 15640
		private bool sentSignal;
	}
}
