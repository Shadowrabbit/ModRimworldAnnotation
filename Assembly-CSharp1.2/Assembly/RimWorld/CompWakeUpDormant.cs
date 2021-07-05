using System;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001897 RID: 6295
	public class CompWakeUpDormant : ThingComp
	{
		// Token: 0x170015FA RID: 5626
		// (get) Token: 0x06008BB1 RID: 35761 RVA: 0x0005DA3D File Offset: 0x0005BC3D
		private CompProperties_WakeUpDormant Props
		{
			get
			{
				return (CompProperties_WakeUpDormant)this.props;
			}
		}

		// Token: 0x06008BB2 RID: 35762 RVA: 0x0005DA4A File Offset: 0x0005BC4A
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.wakeUpIfColonistClose = this.Props.wakeUpIfAnyColonistClose;
		}

		// Token: 0x06008BB3 RID: 35763 RVA: 0x0005DA64 File Offset: 0x0005BC64
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		// Token: 0x06008BB4 RID: 35764 RVA: 0x0028ACA4 File Offset: 0x00288EA4
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
				if (GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), this.Props.wakeUpOnThingConstructedRadius, (Thing t) => t.Faction == Faction.OfPlayer, null, 0, -1, false, RegionType.Set_Passable, false) != null)
				{
					this.Activate(true, false);
				}
			}
		}

		// Token: 0x06008BB5 RID: 35765 RVA: 0x0028AE20 File Offset: 0x00289020
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

		// Token: 0x06008BB6 RID: 35766 RVA: 0x0005DA84 File Offset: 0x0005BC84
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Props.wakeUpOnDamage && totalDamageDealt > 0f && dinfo.Def.ExternalViolenceFor(this.parent))
			{
				this.Activate(true, false);
			}
		}

		// Token: 0x06008BB7 RID: 35767 RVA: 0x0005DAB7 File Offset: 0x0005BCB7
		public override void Notify_SignalReceived(Signal signal)
		{
			if (string.IsNullOrEmpty(this.Props.wakeUpSignalTag))
			{
				return;
			}
			this.sentSignal = true;
		}

		// Token: 0x06008BB8 RID: 35768 RVA: 0x0005DAD3 File Offset: 0x0005BCD3
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.wakeUpIfColonistClose, "wakeUpIfColonistClose", false, false);
			Scribe_Values.Look<bool>(ref this.sentSignal, "sentSignal", false, false);
		}

		// Token: 0x0400598A RID: 22922
		public bool wakeUpIfColonistClose;

		// Token: 0x0400598B RID: 22923
		private bool sentSignal;
	}
}
