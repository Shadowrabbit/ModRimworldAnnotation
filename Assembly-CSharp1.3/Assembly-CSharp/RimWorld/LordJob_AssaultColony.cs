using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200086C RID: 2156
	public class LordJob_AssaultColony : LordJob
	{
		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x060038E8 RID: 14568 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060038E9 RID: 14569 RVA: 0x0013E754 File Offset: 0x0013C954
		public LordJob_AssaultColony()
		{
		}

		// Token: 0x060038EA RID: 14570 RVA: 0x0013E774 File Offset: 0x0013C974
		public LordJob_AssaultColony(SpawnedPawnParams parms)
		{
			this.assaulterFaction = parms.spawnerThing.Faction;
			this.canKidnap = false;
			this.canTimeoutOrFlee = false;
			this.canSteal = false;
		}

		// Token: 0x060038EB RID: 14571 RVA: 0x0013E7C4 File Offset: 0x0013C9C4
		public LordJob_AssaultColony(Faction assaulterFaction, bool canKidnap = true, bool canTimeoutOrFlee = true, bool sappers = false, bool useAvoidGridSmart = false, bool canSteal = true, bool breachers = false, bool canPickUpOpportunisticWeapons = false)
		{
			this.assaulterFaction = assaulterFaction;
			this.canKidnap = canKidnap;
			this.canTimeoutOrFlee = canTimeoutOrFlee;
			this.sappers = sappers;
			this.useAvoidGridSmart = useAvoidGridSmart;
			this.canSteal = canSteal;
			this.breachers = breachers;
			this.canPickUpOpportunisticWeapons = canPickUpOpportunisticWeapons;
		}

		// Token: 0x060038EC RID: 14572 RVA: 0x0013E82C File Offset: 0x0013CA2C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			List<LordToil> list = new List<LordToil>();
			LordToil lordToil = null;
			if (this.sappers)
			{
				lordToil = new LordToil_AssaultColonySappers();
				if (this.useAvoidGridSmart)
				{
					lordToil.useAvoidGrid = true;
				}
				stateGraph.AddToil(lordToil);
				list.Add(lordToil);
				Transition transition = new Transition(lordToil, lordToil, true, true);
				transition.AddTrigger(new Trigger_PawnLost(PawnLostCondition.Undefined, null));
				stateGraph.AddTransition(transition, false);
			}
			if (this.breachers && ModLister.CheckIdeology("Breach raid"))
			{
				LordToil lordToil2 = new LordToil_AssaultColonyBreaching();
				if (this.useAvoidGridSmart)
				{
					lordToil2.useAvoidGrid = this.useAvoidGridSmart;
				}
				stateGraph.AddToil(lordToil2);
				list.Add(lordToil2);
			}
			LordToil lordToil3 = new LordToil_AssaultColony(false, this.canPickUpOpportunisticWeapons);
			if (this.useAvoidGridSmart)
			{
				lordToil3.useAvoidGrid = true;
			}
			stateGraph.AddToil(lordToil3);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, false, true);
			lordToil_ExitMap.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap);
			if (this.sappers)
			{
				Transition transition2 = new Transition(lordToil, lordToil3, false, true);
				transition2.AddTrigger(new Trigger_NoFightingSappers());
				stateGraph.AddTransition(transition2, false);
			}
			if (this.assaulterFaction != null && this.assaulterFaction.def.humanlikeFaction)
			{
				if (this.canTimeoutOrFlee)
				{
					Transition transition3 = new Transition(lordToil3, lordToil_ExitMap, false, true);
					transition3.AddSources(list);
					IntRange intRange;
					if (this.sappers)
					{
						intRange = LordJob_AssaultColony.SapTimeBeforeGiveUp;
					}
					else if (this.breachers)
					{
						intRange = LordJob_AssaultColony.BreachTimeBeforeGiveUp;
					}
					else
					{
						intRange = LordJob_AssaultColony.AssaultTimeBeforeGiveUp;
					}
					transition3.AddTrigger(new Trigger_TicksPassed(intRange.RandomInRange));
					transition3.AddPreAction(new TransitionAction_Message("MessageRaidersGivenUpLeaving".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					stateGraph.AddTransition(transition3, false);
					Transition transition4 = new Transition(lordToil3, lordToil_ExitMap, false, true);
					transition4.AddSources(list);
					FloatRange floatRange = new FloatRange(0.25f, 0.35f);
					float randomInRange = floatRange.RandomInRange;
					transition4.AddTrigger(new Trigger_FractionColonyDamageTaken(randomInRange, 900f));
					transition4.AddPreAction(new TransitionAction_Message("MessageRaidersSatisfiedLeaving".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					stateGraph.AddTransition(transition4, false);
				}
				if (this.canKidnap)
				{
					LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Kidnap().CreateGraph()).StartingToil;
					Transition transition5 = new Transition(lordToil3, startingToil, false, true);
					transition5.AddSources(list);
					transition5.AddPreAction(new TransitionAction_Message("MessageRaidersKidnapping".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					transition5.AddTrigger(new Trigger_KidnapVictimPresent());
					stateGraph.AddTransition(transition5, false);
				}
				if (this.canSteal)
				{
					LordToil startingToil2 = stateGraph.AttachSubgraph(new LordJob_Steal().CreateGraph()).StartingToil;
					Transition transition6 = new Transition(lordToil3, startingToil2, false, true);
					transition6.AddSources(list);
					transition6.AddPreAction(new TransitionAction_Message("MessageRaidersStealing".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					transition6.AddTrigger(new Trigger_HighValueThingsAround());
					stateGraph.AddTransition(transition6, false);
				}
			}
			if (this.assaulterFaction != null)
			{
				Transition transition7 = new Transition(lordToil3, lordToil_ExitMap, false, true);
				transition7.AddSources(list);
				transition7.AddTrigger(new Trigger_BecameNonHostileToPlayer());
				transition7.AddPreAction(new TransitionAction_Message("MessageRaidersLeaving".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
				stateGraph.AddTransition(transition7, false);
			}
			return stateGraph;
		}

		// Token: 0x060038ED RID: 14573 RVA: 0x0013EC50 File Offset: 0x0013CE50
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.assaulterFaction, "assaulterFaction", false);
			Scribe_Values.Look<bool>(ref this.canKidnap, "canKidnap", true, false);
			Scribe_Values.Look<bool>(ref this.canTimeoutOrFlee, "canTimeoutOrFlee", true, false);
			Scribe_Values.Look<bool>(ref this.sappers, "sappers", false, false);
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
			Scribe_Values.Look<bool>(ref this.canSteal, "canSteal", true, false);
			Scribe_Values.Look<bool>(ref this.breachers, "breaching", false, false);
			Scribe_Values.Look<bool>(ref this.canPickUpOpportunisticWeapons, "canPickUpOpportunisticWeapons", false, false);
		}

		// Token: 0x04001F38 RID: 7992
		private Faction assaulterFaction;

		// Token: 0x04001F39 RID: 7993
		private bool canKidnap = true;

		// Token: 0x04001F3A RID: 7994
		private bool canTimeoutOrFlee = true;

		// Token: 0x04001F3B RID: 7995
		private bool sappers;

		// Token: 0x04001F3C RID: 7996
		private bool useAvoidGridSmart;

		// Token: 0x04001F3D RID: 7997
		private bool canSteal = true;

		// Token: 0x04001F3E RID: 7998
		private bool breachers;

		// Token: 0x04001F3F RID: 7999
		private bool canPickUpOpportunisticWeapons;

		// Token: 0x04001F40 RID: 8000
		private static readonly IntRange AssaultTimeBeforeGiveUp = new IntRange(26000, 38000);

		// Token: 0x04001F41 RID: 8001
		private static readonly IntRange SapTimeBeforeGiveUp = new IntRange(33000, 38000);

		// Token: 0x04001F42 RID: 8002
		private static readonly IntRange BreachTimeBeforeGiveUp = new IntRange(33000, 38000);
	}
}
