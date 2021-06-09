using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DB2 RID: 3506
	public class LordJob_AssaultColony : LordJob
	{
		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06004FEC RID: 20460 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004FED RID: 20461 RVA: 0x000381A8 File Offset: 0x000363A8
		public LordJob_AssaultColony()
		{
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x001B5B00 File Offset: 0x001B3D00
		public LordJob_AssaultColony(SpawnedPawnParams parms)
		{
			this.assaulterFaction = parms.spawnerThing.Faction;
			this.canKidnap = false;
			this.canTimeoutOrFlee = false;
			this.canSteal = false;
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x001B5B50 File Offset: 0x001B3D50
		public LordJob_AssaultColony(Faction assaulterFaction, bool canKidnap = true, bool canTimeoutOrFlee = true, bool sappers = false, bool useAvoidGridSmart = false, bool canSteal = true)
		{
			this.assaulterFaction = assaulterFaction;
			this.canKidnap = canKidnap;
			this.canTimeoutOrFlee = canTimeoutOrFlee;
			this.sappers = sappers;
			this.useAvoidGridSmart = useAvoidGridSmart;
			this.canSteal = canSteal;
		}

		// Token: 0x06004FF0 RID: 20464 RVA: 0x001B5BA8 File Offset: 0x001B3DA8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil lordToil = null;
			if (this.sappers)
			{
				lordToil = new LordToil_AssaultColonySappers();
				if (this.useAvoidGridSmart)
				{
					lordToil.useAvoidGrid = true;
				}
				stateGraph.AddToil(lordToil);
				Transition transition = new Transition(lordToil, lordToil, true, true);
				transition.AddTrigger(new Trigger_PawnLost(PawnLostCondition.Undefined, null));
				stateGraph.AddTransition(transition, false);
			}
			LordToil lordToil2 = new LordToil_AssaultColony(false);
			if (this.useAvoidGridSmart)
			{
				lordToil2.useAvoidGrid = true;
			}
			stateGraph.AddToil(lordToil2);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, false, true);
			lordToil_ExitMap.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap);
			if (this.sappers)
			{
				Transition transition2 = new Transition(lordToil, lordToil2, false, true);
				transition2.AddTrigger(new Trigger_NoFightingSappers());
				stateGraph.AddTransition(transition2, false);
			}
			if (this.assaulterFaction.def.humanlikeFaction)
			{
				if (this.canTimeoutOrFlee)
				{
					Transition transition3 = new Transition(lordToil2, lordToil_ExitMap, false, true);
					if (lordToil != null)
					{
						transition3.AddSource(lordToil);
					}
					transition3.AddTrigger(new Trigger_TicksPassed(this.sappers ? LordJob_AssaultColony.SapTimeBeforeGiveUp.RandomInRange : LordJob_AssaultColony.AssaultTimeBeforeGiveUp.RandomInRange));
					transition3.AddPreAction(new TransitionAction_Message("MessageRaidersGivenUpLeaving".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					stateGraph.AddTransition(transition3, false);
					Transition transition4 = new Transition(lordToil2, lordToil_ExitMap, false, true);
					if (lordToil != null)
					{
						transition4.AddSource(lordToil);
					}
					FloatRange floatRange = new FloatRange(0.25f, 0.35f);
					float randomInRange = floatRange.RandomInRange;
					transition4.AddTrigger(new Trigger_FractionColonyDamageTaken(randomInRange, 900f));
					transition4.AddPreAction(new TransitionAction_Message("MessageRaidersSatisfiedLeaving".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					stateGraph.AddTransition(transition4, false);
				}
				if (this.canKidnap)
				{
					LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Kidnap().CreateGraph()).StartingToil;
					Transition transition5 = new Transition(lordToil2, startingToil, false, true);
					if (lordToil != null)
					{
						transition5.AddSource(lordToil);
					}
					transition5.AddPreAction(new TransitionAction_Message("MessageRaidersKidnapping".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					transition5.AddTrigger(new Trigger_KidnapVictimPresent());
					stateGraph.AddTransition(transition5, false);
				}
				if (this.canSteal)
				{
					LordToil startingToil2 = stateGraph.AttachSubgraph(new LordJob_Steal().CreateGraph()).StartingToil;
					Transition transition6 = new Transition(lordToil2, startingToil2, false, true);
					if (lordToil != null)
					{
						transition6.AddSource(lordToil);
					}
					transition6.AddPreAction(new TransitionAction_Message("MessageRaidersStealing".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
					transition6.AddTrigger(new Trigger_HighValueThingsAround());
					stateGraph.AddTransition(transition6, false);
				}
			}
			Transition transition7 = new Transition(lordToil2, lordToil_ExitMap, false, true);
			if (lordToil != null)
			{
				transition7.AddSource(lordToil);
			}
			transition7.AddTrigger(new Trigger_BecameNonHostileToPlayer());
			transition7.AddPreAction(new TransitionAction_Message("MessageRaidersLeaving".Translate(this.assaulterFaction.def.pawnsPlural.CapitalizeFirst(), this.assaulterFaction.Name), null, 1f));
			stateGraph.AddTransition(transition7, false);
			return stateGraph;
		}

		// Token: 0x06004FF1 RID: 20465 RVA: 0x001B5F5C File Offset: 0x001B415C
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.assaulterFaction, "assaulterFaction", false);
			Scribe_Values.Look<bool>(ref this.canKidnap, "canKidnap", true, false);
			Scribe_Values.Look<bool>(ref this.canTimeoutOrFlee, "canTimeoutOrFlee", true, false);
			Scribe_Values.Look<bool>(ref this.sappers, "sappers", false, false);
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
			Scribe_Values.Look<bool>(ref this.canSteal, "canSteal", true, false);
		}

		// Token: 0x040033A2 RID: 13218
		private Faction assaulterFaction;

		// Token: 0x040033A3 RID: 13219
		private bool canKidnap = true;

		// Token: 0x040033A4 RID: 13220
		private bool canTimeoutOrFlee = true;

		// Token: 0x040033A5 RID: 13221
		private bool sappers;

		// Token: 0x040033A6 RID: 13222
		private bool useAvoidGridSmart;

		// Token: 0x040033A7 RID: 13223
		private bool canSteal = true;

		// Token: 0x040033A8 RID: 13224
		private static readonly IntRange AssaultTimeBeforeGiveUp = new IntRange(26000, 38000);

		// Token: 0x040033A9 RID: 13225
		private static readonly IntRange SapTimeBeforeGiveUp = new IntRange(33000, 38000);
	}
}
