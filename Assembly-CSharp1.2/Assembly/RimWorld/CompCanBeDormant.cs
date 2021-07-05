using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020017AB RID: 6059
	public class CompCanBeDormant : ThingComp
	{
		// Token: 0x170014B6 RID: 5302
		// (get) Token: 0x060085E1 RID: 34273 RVA: 0x00059C20 File Offset: 0x00057E20
		private CompProperties_CanBeDormant Props
		{
			get
			{
				return (CompProperties_CanBeDormant)this.props;
			}
		}

		// Token: 0x170014B7 RID: 5303
		// (get) Token: 0x060085E2 RID: 34274 RVA: 0x00059C2D File Offset: 0x00057E2D
		private bool WaitingToWakeUp
		{
			get
			{
				return this.wakeUpOnTick != int.MinValue;
			}
		}

		// Token: 0x170014B8 RID: 5304
		// (get) Token: 0x060085E3 RID: 34275 RVA: 0x00059C3F File Offset: 0x00057E3F
		public bool Awake
		{
			get
			{
				return this.wokeUpTick != int.MinValue && this.wokeUpTick <= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060085E4 RID: 34276 RVA: 0x00059C65 File Offset: 0x00057E65
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.makeTick = GenTicks.TicksGame;
			if (!this.Props.startsDormant)
			{
				this.WakeUp();
			}
		}

		// Token: 0x060085E5 RID: 34277 RVA: 0x00059C8B File Offset: 0x00057E8B
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.wakeUpSignalTag = this.Props.wakeUpSignalTag;
		}

		// Token: 0x060085E6 RID: 34278 RVA: 0x00059CA5 File Offset: 0x00057EA5
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Wake Up",
				action = delegate()
				{
					this.WakeUp();
				}
			};
			yield break;
		}

		// Token: 0x060085E7 RID: 34279 RVA: 0x002770B0 File Offset: 0x002752B0
		public override string CompInspectStringExtra()
		{
			if (!this.Awake)
			{
				return this.Props.dormantStateLabelKey.Translate();
			}
			if (this.makeTick != this.wokeUpTick)
			{
				return this.Props.awakeStateLabelKey.Translate((GenTicks.TicksGame - this.wokeUpTick).TicksToDays().ToString("0.#"));
			}
			return null;
		}

		// Token: 0x060085E8 RID: 34280 RVA: 0x00059CB5 File Offset: 0x00057EB5
		public void WakeUpWithDelay()
		{
			if (!this.Awake)
			{
				this.wakeUpOnTick = Find.TickManager.TicksGame + Rand.Range(60, 300);
			}
		}

		// Token: 0x060085E9 RID: 34281 RVA: 0x00277124 File Offset: 0x00275324
		public void WakeUp()
		{
			if (this.Awake)
			{
				return;
			}
			this.wokeUpTick = GenTicks.TicksGame;
			this.wakeUpOnTick = int.MinValue;
			Pawn pawn = this.parent as Pawn;
			Building building = this.parent as Building;
			Lord lord = ((pawn != null) ? pawn.GetLord() : null) ?? ((building != null) ? building.GetLord() : null);
			if (lord != null)
			{
				lord.Notify_DormancyWakeup();
			}
			if (this.parent.Spawned)
			{
				IAttackTarget attackTarget = this.parent as IAttackTarget;
				if (attackTarget != null)
				{
					this.parent.Map.attackTargetsCache.UpdateTarget(attackTarget);
				}
			}
		}

		// Token: 0x060085EA RID: 34282 RVA: 0x002771C0 File Offset: 0x002753C0
		public void ToSleep()
		{
			if (!this.Awake)
			{
				return;
			}
			this.wokeUpTick = int.MinValue;
			if (this.parent.Spawned)
			{
				IAttackTarget attackTarget = this.parent as IAttackTarget;
				if (attackTarget != null)
				{
					this.parent.Map.attackTargetsCache.UpdateTarget(attackTarget);
				}
			}
		}

		// Token: 0x060085EB RID: 34283 RVA: 0x00059CDC File Offset: 0x00057EDC
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.wakeUpOnTick != -2147483648 && Find.TickManager.TicksGame >= this.wakeUpOnTick)
			{
				this.WakeUp();
			}
			this.TickRareWorker();
		}

		// Token: 0x060085EC RID: 34284 RVA: 0x00277214 File Offset: 0x00275414
		public override void CompTick()
		{
			base.CompTick();
			if (this.wakeUpOnTick != -2147483648 && Find.TickManager.TicksGame >= this.wakeUpOnTick)
			{
				this.WakeUp();
			}
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		// Token: 0x060085ED RID: 34285 RVA: 0x00277264 File Offset: 0x00275464
		public void TickRareWorker()
		{
			if (!this.parent.Spawned || this.Awake)
			{
				return;
			}
			if (!(this.parent is Pawn) && !this.parent.Position.Fogged(this.parent.Map))
			{
				MoteMaker.ThrowMetaIcon(this.parent.Position, this.parent.Map, ThingDefOf.Mote_SleepZ);
			}
		}

		// Token: 0x060085EE RID: 34286 RVA: 0x002772D4 File Offset: 0x002754D4
		public override void Notify_SignalReceived(Signal signal)
		{
			if (string.IsNullOrEmpty(this.wakeUpSignalTag) || this.Awake)
			{
				return;
			}
			Thing thing;
			Faction faction;
			if ((signal.tag == this.wakeUpSignalTag || (this.wakeUpSignalTags != null && this.wakeUpSignalTags.Contains(signal.tag))) && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= this.Props.maxDistAwakenByOther && (!signal.args.TryGetArg<Faction>("FACTION", out faction) || faction == null || faction == this.parent.Faction) && (this.Props.canWakeUpFogged || !this.parent.Fogged()) && !this.WaitingToWakeUp)
			{
				this.WakeUpWithDelay();
			}
		}

		// Token: 0x060085EF RID: 34287 RVA: 0x002773DC File Offset: 0x002755DC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.wokeUpTick, "wokeUpTick", int.MinValue, false);
			Scribe_Values.Look<int>(ref this.wakeUpOnTick, "wakeUpOnTick", int.MinValue, false);
			Scribe_Values.Look<string>(ref this.wakeUpSignalTag, "wakeUpSignalTag", null, false);
			Scribe_Collections.Look<string>(ref this.wakeUpSignalTags, "wakeUpSignalTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.makeTick, "makeTick", 0, false);
		}

		// Token: 0x04005658 RID: 22104
		public int makeTick;

		// Token: 0x04005659 RID: 22105
		public int wokeUpTick = int.MinValue;

		// Token: 0x0400565A RID: 22106
		public int wakeUpOnTick = int.MinValue;

		// Token: 0x0400565B RID: 22107
		public string wakeUpSignalTag;

		// Token: 0x0400565C RID: 22108
		public List<string> wakeUpSignalTags;

		// Token: 0x0400565D RID: 22109
		public const string DefaultWakeUpSignal = "CompCanBeDormant.WakeUp";
	}
}
