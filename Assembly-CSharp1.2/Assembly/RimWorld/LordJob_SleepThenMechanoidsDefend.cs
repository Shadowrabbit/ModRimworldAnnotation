using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000DCE RID: 3534
	public class LordJob_SleepThenMechanoidsDefend : LordJob_MechanoidDefendBase
	{
		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x06005092 RID: 20626 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005093 RID: 20627 RVA: 0x00038772 File Offset: 0x00036972
		public LordJob_SleepThenMechanoidsDefend()
		{
		}

		// Token: 0x06005094 RID: 20628 RVA: 0x0003885E File Offset: 0x00036A5E
		public LordJob_SleepThenMechanoidsDefend(List<Thing> things, Faction faction, float defendRadius, IntVec3 defSpot, bool canAssaultColony, bool isMechCluster)
		{
			if (things != null)
			{
				this.things.AddRange(things);
			}
			this.faction = faction;
			this.defendRadius = defendRadius;
			this.defSpot = defSpot;
			this.canAssaultColony = canAssaultColony;
			this.isMechCluster = isMechCluster;
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x001B8588 File Offset: 0x001B6788
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Sleep lordToil_Sleep = new LordToil_Sleep();
			stateGraph.StartingToil = lordToil_Sleep;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_MechanoidsDefend(this.things, this.faction, this.defendRadius, this.defSpot, this.canAssaultColony, this.isMechCluster).CreateGraph()).StartingToil;
			Transition transition = new Transition(lordToil_Sleep, startingToil, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.DormancyWakeup));
			transition.AddTrigger(new Trigger_OnHumanlikeHarmAnyThing(this.things));
			transition.AddPreAction(new TransitionAction_Message("MessageSleepingPawnsWokenUp".Translate(this.faction.def.pawnsPlural).CapitalizeFirst(), MessageTypeDefOf.ThreatBig, null, 1f));
			transition.AddPostAction(new TransitionAction_WakeAll());
			transition.AddPostAction(new TransitionAction_Custom(delegate()
			{
				Find.SignalManager.SendSignal(new Signal("CompCanBeDormant.WakeUp", this.things.First<Thing>().Named("SUBJECT"), Faction.OfMechanoids.Named("FACTION")));
				SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(this.defSpot, base.Map, false));
			}));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}
	}
}
