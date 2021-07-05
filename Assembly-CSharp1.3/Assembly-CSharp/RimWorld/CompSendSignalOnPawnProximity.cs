using System;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001191 RID: 4497
	[StaticConstructorOnStartup]
	public class CompSendSignalOnPawnProximity : ThingComp
	{
		// Token: 0x170012AC RID: 4780
		// (get) Token: 0x06006C28 RID: 27688 RVA: 0x002441EC File Offset: 0x002423EC
		public CompProperties_SendSignalOnPawnProximity Props
		{
			get
			{
				return (CompProperties_SendSignalOnPawnProximity)this.props;
			}
		}

		// Token: 0x170012AD RID: 4781
		// (get) Token: 0x06006C29 RID: 27689 RVA: 0x002441F9 File Offset: 0x002423F9
		public bool Sent
		{
			get
			{
				return this.sent;
			}
		}

		// Token: 0x170012AE RID: 4782
		// (get) Token: 0x06006C2A RID: 27690 RVA: 0x00244201 File Offset: 0x00242401
		public bool Enabled
		{
			get
			{
				return this.ticksUntilEnabled <= 0;
			}
		}

		// Token: 0x06006C2B RID: 27691 RVA: 0x0024420F File Offset: 0x0024240F
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.signalTag = this.Props.signalTag;
			this.ticksUntilEnabled = this.Props.enableAfterTicks;
		}

		// Token: 0x06006C2C RID: 27692 RVA: 0x0024423C File Offset: 0x0024243C
		public override void CompTick()
		{
			base.CompTick();
			if (this.sent || !this.parent.Spawned)
			{
				return;
			}
			if (this.Enabled && Find.TickManager.TicksGame % 250 == 0)
			{
				this.CompTickRare();
			}
			if (this.ticksUntilEnabled > 0)
			{
				this.ticksUntilEnabled--;
			}
		}

		// Token: 0x06006C2D RID: 27693 RVA: 0x0024429C File Offset: 0x0024249C
		public override void CompTickRare()
		{
			base.CompTickRare();
			Predicate<Thing> predicate = null;
			if (this.Props.onlyHumanlike)
			{
				predicate = delegate(Thing t)
				{
					Pawn pawn = t as Pawn;
					return pawn != null && pawn.RaceProps.Humanlike;
				};
			}
			Thing thing = null;
			if (this.Props.triggerOnPawnInRoom)
			{
				foreach (Thing thing2 in this.parent.GetRoom(RegionType.Set_All).ContainedAndAdjacentThings)
				{
					if (predicate(thing2))
					{
						thing = thing2;
						break;
					}
				}
			}
			if (thing == null && this.Props.radius > 0f)
			{
				thing = GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false), this.Props.radius, predicate, null, 0, -1, false, RegionType.Set_Passable, false);
			}
			if (thing != null)
			{
				Effecter effecter = new Effecter(EffecterDefOf.ActivatorProximityTriggered);
				effecter.Trigger(this.parent, TargetInfo.Invalid);
				effecter.Cleanup();
				Messages.Message("MessageActivatorProximityTriggered".Translate(thing), this.parent, MessageTypeDefOf.ThreatBig, true);
				Find.SignalManager.SendSignal(new Signal(this.signalTag, this.parent.Named("SUBJECT")));
				SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
				this.sent = true;
			}
		}

		// Token: 0x06006C2E RID: 27694 RVA: 0x00244448 File Offset: 0x00242648
		public void Expire()
		{
			this.sent = true;
		}

		// Token: 0x06006C2F RID: 27695 RVA: 0x00244454 File Offset: 0x00242654
		public override void Notify_SignalReceived(Signal signal)
		{
			Thing thing;
			if (signal.tag == "CompCanBeDormant.WakeUp" && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= 40f)
			{
				this.sent = true;
			}
		}

		// Token: 0x06006C30 RID: 27696 RVA: 0x002444CC File Offset: 0x002426CC
		public override string CompInspectStringExtra()
		{
			if (!this.Enabled)
			{
				return "SendSignalOnCountdownCompTime".Translate(this.ticksUntilEnabled.TicksToSeconds().ToString("0.0"));
			}
			if (!this.sent)
			{
				return "radius".Translate().CapitalizeFirst() + ": " + this.Props.radius.ToString("F0");
			}
			return "expired".Translate().CapitalizeFirst();
		}

		// Token: 0x06006C31 RID: 27697 RVA: 0x00244569 File Offset: 0x00242769
		public override void PostExposeData()
		{
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
			Scribe_Values.Look<bool>(ref this.sent, "sent", false, false);
			Scribe_Values.Look<int>(ref this.ticksUntilEnabled, "ticksUntilEnabled", 0, false);
		}

		// Token: 0x04003C21 RID: 15393
		public string signalTag;

		// Token: 0x04003C22 RID: 15394
		private bool sent;

		// Token: 0x04003C23 RID: 15395
		private int ticksUntilEnabled;

		// Token: 0x04003C24 RID: 15396
		private const float MaxDistActivationByOther = 40f;
	}
}
