using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000DC6 RID: 3526
	public abstract class LordJob_MechanoidDefendBase : LordJob
	{
		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06005064 RID: 20580 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool KeepExistingWhileHasAnyBuilding
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005065 RID: 20581 RVA: 0x000386F7 File Offset: 0x000368F7
		public override void LordJobTick()
		{
			base.LordJobTick();
			if (this.isMechCluster && !this.mechClusterDefeated && !MechClusterUtility.AnyThreatBuilding(this.things))
			{
				this.OnDefeat();
			}
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x00038722 File Offset: 0x00036922
		public override void Notify_LordDestroyed()
		{
			if (this.isMechCluster && !this.mechClusterDefeated)
			{
				this.OnDefeat();
			}
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x001B7A58 File Offset: 0x001B5C58
		private void OnDefeat()
		{
			foreach (Thing thing in this.things)
			{
				thing.SetFaction(null, null);
				CompSendSignalOnPawnProximity compSendSignalOnPawnProximity = thing.TryGetComp<CompSendSignalOnPawnProximity>();
				if (compSendSignalOnPawnProximity != null)
				{
					compSendSignalOnPawnProximity.Expire();
				}
				CompSendSignalOnCountdown compSendSignalOnCountdown = thing.TryGetComp<CompSendSignalOnCountdown>();
				if (compSendSignalOnCountdown != null)
				{
					compSendSignalOnCountdown.ticksLeft = 0;
				}
				ThingWithComps thingWithComps;
				if ((thingWithComps = (thing as ThingWithComps)) != null)
				{
					thingWithComps.BroadcastCompSignal("MechClusterDefeated");
				}
			}
			this.lord.Notify_MechClusterDefeated();
			for (int i = 0; i < this.thingsToNotifyOnDefeat.Count; i++)
			{
				this.thingsToNotifyOnDefeat[i].Notify_LordDestroyed();
			}
			this.mechClusterDefeated = true;
			foreach (Pawn pawn in base.Map.mapPawns.FreeColonistsSpawned)
			{
				Pawn_NeedsTracker needs = pawn.needs;
				if (needs != null)
				{
					Need_Mood mood = needs.mood;
					if (mood != null)
					{
						mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DefeatedMechCluster, null);
					}
				}
			}
			QuestUtility.SendQuestTargetSignals(this.lord.questTags, "AllEnemiesDefeated");
			Messages.Message("MessageMechClusterDefeated".Translate(), new LookTargets(this.defSpot, base.Map), MessageTypeDefOf.PositiveEvent, true);
			SoundDefOf.MechClusterDefeated.PlayOneShotOnCamera(base.Map);
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x0003873A File Offset: 0x0003693A
		public void AddThingToNotifyOnDefeat(Thing t)
		{
			this.thingsToNotifyOnDefeat.AddDistinct(t);
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x001B7BDC File Offset: 0x001B5DDC
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defSpot, "defSpot", default(IntVec3), false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 0f, false);
			Scribe_Values.Look<bool>(ref this.canAssaultColony, "canAssaultColony", false, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.thingsToNotifyOnDefeat, "thingsToNotifyOnDefeat", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.isMechCluster, "isMechCluster", false, false);
			Scribe_Values.Look<bool>(ref this.mechClusterDefeated, "mechClusterDefeated", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x.DestroyedOrNull());
				this.thingsToNotifyOnDefeat.RemoveAll((Thing x) => x.DestroyedOrNull());
			}
		}

		// Token: 0x040033E7 RID: 13287
		public List<Thing> things = new List<Thing>();

		// Token: 0x040033E8 RID: 13288
		protected List<Thing> thingsToNotifyOnDefeat = new List<Thing>();

		// Token: 0x040033E9 RID: 13289
		protected IntVec3 defSpot;

		// Token: 0x040033EA RID: 13290
		protected Faction faction;

		// Token: 0x040033EB RID: 13291
		protected float defendRadius;

		// Token: 0x040033EC RID: 13292
		protected bool canAssaultColony;

		// Token: 0x040033ED RID: 13293
		protected bool isMechCluster;

		// Token: 0x040033EE RID: 13294
		protected bool mechClusterDefeated;
	}
}
