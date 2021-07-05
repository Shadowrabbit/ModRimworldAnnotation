using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200087E RID: 2174
	public abstract class LordJob_MechanoidDefendBase : LordJob
	{
		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06003967 RID: 14695 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool KeepExistingWhileHasAnyBuilding
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x00141460 File Offset: 0x0013F660
		public override void LordJobTick()
		{
			base.LordJobTick();
			if (this.isMechCluster && !this.mechClusterDefeated && !MechClusterUtility.AnyThreatBuilding(this.things))
			{
				this.OnDefeat();
			}
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x0014148B File Offset: 0x0013F68B
		public override void Notify_LordDestroyed()
		{
			if (this.isMechCluster && !this.mechClusterDefeated)
			{
				this.OnDefeat();
			}
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x001414A4 File Offset: 0x0013F6A4
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
			if (!base.Map.IsPlayerHome)
			{
				IdeoUtility.Notify_PlayerRaidedSomeone(base.Map.mapPawns.FreeColonistsSpawned);
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
						mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DefeatedMechCluster, null, null);
					}
				}
			}
			QuestUtility.SendQuestTargetSignals(this.lord.questTags, "AllEnemiesDefeated");
			Messages.Message("MessageMechClusterDefeated".Translate(), new LookTargets(this.defSpot, base.Map), MessageTypeDefOf.PositiveEvent, true);
			SoundDefOf.MechClusterDefeated.PlayOneShotOnCamera(base.Map);
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x0014164C File Offset: 0x0013F84C
		public void AddThingToNotifyOnDefeat(Thing t)
		{
			this.thingsToNotifyOnDefeat.AddDistinct(t);
		}

		// Token: 0x0600396C RID: 14700 RVA: 0x0014165C File Offset: 0x0013F85C
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

		// Token: 0x04001F8A RID: 8074
		public List<Thing> things = new List<Thing>();

		// Token: 0x04001F8B RID: 8075
		protected List<Thing> thingsToNotifyOnDefeat = new List<Thing>();

		// Token: 0x04001F8C RID: 8076
		protected IntVec3 defSpot;

		// Token: 0x04001F8D RID: 8077
		protected Faction faction;

		// Token: 0x04001F8E RID: 8078
		protected float defendRadius;

		// Token: 0x04001F8F RID: 8079
		protected bool canAssaultColony;

		// Token: 0x04001F90 RID: 8080
		protected bool isMechCluster;

		// Token: 0x04001F91 RID: 8081
		protected bool mechClusterDefeated;
	}
}
