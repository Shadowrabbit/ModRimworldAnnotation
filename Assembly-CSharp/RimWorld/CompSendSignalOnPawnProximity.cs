using System;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001837 RID: 6199
	[StaticConstructorOnStartup]
	public class CompSendSignalOnPawnProximity : ThingComp
	{
		// Token: 0x17001584 RID: 5508
		// (get) Token: 0x0600896C RID: 35180 RVA: 0x0005C488 File Offset: 0x0005A688
		public CompProperties_SendSignalOnPawnProximity Props
		{
			get
			{
				return (CompProperties_SendSignalOnPawnProximity)this.props;
			}
		}

		// Token: 0x17001585 RID: 5509
		// (get) Token: 0x0600896D RID: 35181 RVA: 0x0005C495 File Offset: 0x0005A695
		public bool Sent
		{
			get
			{
				return this.sent;
			}
		}

		// Token: 0x17001586 RID: 5510
		// (get) Token: 0x0600896E RID: 35182 RVA: 0x0005C49D File Offset: 0x0005A69D
		public bool Enabled
		{
			get
			{
				return this.ticksUntilEnabled <= 0;
			}
		}

		// Token: 0x0600896F RID: 35183 RVA: 0x0005C4AB File Offset: 0x0005A6AB
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.signalTag = this.Props.signalTag;
			this.ticksUntilEnabled = this.Props.enableAfterTicks;
		}

		// Token: 0x06008970 RID: 35184 RVA: 0x002822AC File Offset: 0x002804AC
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

		// Token: 0x06008971 RID: 35185 RVA: 0x0028230C File Offset: 0x0028050C
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
				foreach (Thing thing2 in this.parent.GetRoom(RegionType.Set_Passable).ContainedAndAdjacentThings)
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
				thing = GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), this.Props.radius, predicate, null, 0, -1, false, RegionType.Set_Passable, false);
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

		// Token: 0x06008972 RID: 35186 RVA: 0x0005C4D6 File Offset: 0x0005A6D6
		public void Expire()
		{
			this.sent = true;
		}

		// Token: 0x06008973 RID: 35187 RVA: 0x002824B4 File Offset: 0x002806B4
		public override void Notify_SignalReceived(Signal signal)
		{
			Thing thing;
			if (signal.tag == "CompCanBeDormant.WakeUp" && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= 40f)
			{
				this.sent = true;
			}
		}

		// Token: 0x06008974 RID: 35188 RVA: 0x0028252C File Offset: 0x0028072C
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

		// Token: 0x06008975 RID: 35189 RVA: 0x0005C4DF File Offset: 0x0005A6DF
		public override void PostExposeData()
		{
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
			Scribe_Values.Look<bool>(ref this.sent, "sent", false, false);
			Scribe_Values.Look<int>(ref this.ticksUntilEnabled, "ticksUntilEnabled", 0, false);
		}

		// Token: 0x0400581F RID: 22559
		public string signalTag;

		// Token: 0x04005820 RID: 22560
		private bool sent;

		// Token: 0x04005821 RID: 22561
		private int ticksUntilEnabled;

		// Token: 0x04005822 RID: 22562
		private const float MaxDistActivationByOther = 40f;
	}
}
