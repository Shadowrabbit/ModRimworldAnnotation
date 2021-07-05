using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000884 RID: 2180
	public class LordJob_SleepThenAssaultColony : LordJob
	{
		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06003995 RID: 14741 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_SleepThenAssaultColony()
		{
		}

		// Token: 0x06003997 RID: 14743 RVA: 0x00142360 File Offset: 0x00140560
		public LordJob_SleepThenAssaultColony(Faction faction, bool sendWokenUpMessage = true)
		{
			this.faction = faction;
			this.sendWokenUpMessage = sendWokenUpMessage;
		}

		// Token: 0x06003998 RID: 14744 RVA: 0x00142378 File Offset: 0x00140578
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Sleep lordToil_Sleep = new LordToil_Sleep();
			stateGraph.StartingToil = lordToil_Sleep;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true, false, false).CreateGraph()).StartingToil;
			Transition transition = new Transition(lordToil_Sleep, startingToil, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.DormancyWakeup));
			if (this.sendWokenUpMessage)
			{
				transition.AddPreAction(new TransitionAction_Message("MessageSleepingPawnsWokenUp".Translate(this.faction.def.pawnsPlural).CapitalizeFirst(), MessageTypeDefOf.ThreatBig, null, 1f, () => this.AnyAsleep()));
			}
			transition.AddPostAction(new TransitionAction_WakeAll());
			transition.AddPostAction(new TransitionAction_Custom(delegate()
			{
				Vector3 vector = Vector3.zero;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					vector += this.lord.ownedPawns[i].Position.ToVector3();
				}
				vector /= (float)this.lord.ownedPawns.Count;
				if (this.faction == Faction.OfMechanoids)
				{
					SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(vector.ToIntVec3(), base.Map, false));
					return;
				}
				if (ModsConfig.IdeologyActive && this.faction == Faction.OfInsects)
				{
					SoundDefOf.InsectsWakeUp.PlayOneShot(new TargetInfo(vector.ToIntVec3(), base.Map, false));
				}
			}));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x00142470 File Offset: 0x00140670
		private bool AnyAsleep()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (this.lord.ownedPawns[i].Spawned && !this.lord.ownedPawns[i].Dead && !this.lord.ownedPawns[i].Awake())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x001424E3 File Offset: 0x001406E3
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.sendWokenUpMessage, "sendWokenUpMessage", true, false);
		}

		// Token: 0x04001FA2 RID: 8098
		private Faction faction;

		// Token: 0x04001FA3 RID: 8099
		private bool sendWokenUpMessage;
	}
}
