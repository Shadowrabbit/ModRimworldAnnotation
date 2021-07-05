using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000885 RID: 2181
	public class LordJob_SleepThenMechanoidsDefend : LordJob_MechanoidDefendBase
	{
		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x0600399D RID: 14749 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x00141788 File Offset: 0x0013F988
		public LordJob_SleepThenMechanoidsDefend()
		{
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x001425DF File Offset: 0x001407DF
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

		// Token: 0x060039A0 RID: 14752 RVA: 0x0014261C File Offset: 0x0014081C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Sleep lordToil_Sleep = new LordToil_Sleep();
			stateGraph.StartingToil = lordToil_Sleep;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_MechanoidsDefend(this.things, this.faction, this.defendRadius, this.defSpot, this.canAssaultColony, this.isMechCluster).CreateGraph()).StartingToil;
			Transition transition = new Transition(lordToil_Sleep, startingToil, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.DormancyWakeup));
			transition.AddTrigger(new Trigger_OnHumanlikeHarmAnyThing(this.things));
			transition.AddPreAction(new TransitionAction_Message("MessageSleepingPawnsWokenUp".Translate(this.faction.def.pawnsPlural).CapitalizeFirst(), MessageTypeDefOf.ThreatBig, null, 1f, null));
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
