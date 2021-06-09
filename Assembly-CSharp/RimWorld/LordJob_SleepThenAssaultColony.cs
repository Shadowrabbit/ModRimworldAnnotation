using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000DCC RID: 3532
	public class LordJob_SleepThenAssaultColony : LordJob
	{
		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x06005089 RID: 20617 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600508A RID: 20618 RVA: 0x00030B4B File Offset: 0x0002ED4B
		public LordJob_SleepThenAssaultColony()
		{
		}

		// Token: 0x0600508B RID: 20619 RVA: 0x00038824 File Offset: 0x00036A24
		public LordJob_SleepThenAssaultColony(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x0600508C RID: 20620 RVA: 0x001B841C File Offset: 0x001B661C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Sleep lordToil_Sleep = new LordToil_Sleep();
			stateGraph.StartingToil = lordToil_Sleep;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true).CreateGraph()).StartingToil;
			Transition transition = new Transition(lordToil_Sleep, startingToil, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.DormancyWakeup));
			transition.AddPreAction(new TransitionAction_Message("MessageSleepingPawnsWokenUp".Translate(this.faction.def.pawnsPlural).CapitalizeFirst(), MessageTypeDefOf.ThreatBig, null, 1f));
			transition.AddPostAction(new TransitionAction_WakeAll());
			transition.AddPostAction(new TransitionAction_Custom(delegate()
			{
				Vector3 vector = Vector3.zero;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					vector += this.lord.ownedPawns[i].Position.ToVector3();
				}
				vector /= (float)this.lord.ownedPawns.Count;
				SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(vector.ToIntVec3(), base.Map, false));
			}));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x0600508D RID: 20621 RVA: 0x00038833 File Offset: 0x00036A33
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x040033FC RID: 13308
		private Faction faction;
	}
}
