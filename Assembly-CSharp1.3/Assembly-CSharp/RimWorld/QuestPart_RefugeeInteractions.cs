using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B92 RID: 2962
	public class QuestPart_RefugeeInteractions : QuestPartActivable
	{
		// Token: 0x0600453D RID: 17725 RVA: 0x0016EFE0 File Offset: 0x0016D1E0
		protected override void ProcessQuestSignal(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRecruited && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
				if (this.pawns.Count == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalLast_Recruited, signal.args));
				}
			}
			Pawn item2;
			if (signal.tag == this.inSignalKidnapped && signal.args.TryGetArg<Pawn>("SUBJECT", out item2) && this.pawns.Contains(item2))
			{
				this.pawns.Remove(item2);
				if (this.pawns.Count == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalLast_Kidnapped, signal.args));
				}
			}
			Pawn item3;
			if (signal.tag == this.inSignalBanished && signal.args.TryGetArg<Pawn>("SUBJECT", out item3) && this.pawns.Contains(item3))
			{
				this.pawns.Remove(item3);
				if (this.pawns.Count == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalLast_Banished, signal.args));
				}
			}
			Pawn pawn;
			if (signal.tag == this.inSignalLeftMap && signal.args.TryGetArg<Pawn>("SUBJECT", out pawn) && this.pawns.Contains(pawn))
			{
				this.pawns.Remove(pawn);
				if (pawn.Destroyed || pawn.InMentalState || pawn.health.hediffSet.BleedRateTotal > 0.001f)
				{
					this.pawnsLeftUnhealthy++;
				}
				int num = this.pawns.Count((Pawn p) => p.Downed);
				if (this.pawns.Count - num <= 0)
				{
					if (this.pawnsLeftUnhealthy > 0 || num > 0)
					{
						this.pawns.Clear();
						this.pawnsLeftUnhealthy += num;
						Find.SignalManager.SendSignal(new Signal(this.outSignalLast_LeftMapAllNotHealthy, signal.args));
					}
					else
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalLast_LeftMapAllHealthy, signal.args));
					}
				}
			}
			Pawn pawn2;
			if (signal.tag == this.inSignalDestroyed && signal.args.TryGetArg<Pawn>("SUBJECT", out pawn2) && this.pawns.Contains(pawn2))
			{
				this.pawns.Remove(pawn2);
				pawn2.SetFaction(this.faction, null);
				if (this.pawns.Count == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalLast_Destroyed, signal.args));
				}
				else
				{
					signal.args.Add(this.pawns.Count.Named("PAWNSALIVECOUNT"));
					switch (this.ChooseRandomInteraction())
					{
					case QuestPart_RefugeeInteractions.InteractionResponseType.AssaultColony:
						this.AssaultColony(HistoryEventDefOf.QuestPawnLost);
						Find.SignalManager.SendSignal(new Signal(this.outSignalDestroyed_AssaultColony, signal.args));
						break;
					case QuestPart_RefugeeInteractions.InteractionResponseType.Leave:
						this.LeavePlayer();
						Find.SignalManager.SendSignal(new Signal(this.outSignalDestroyed_LeaveColony, signal.args));
						break;
					case QuestPart_RefugeeInteractions.InteractionResponseType.BadThought:
						Find.SignalManager.SendSignal(new Signal(this.outSignalDestroyed_BadThought, signal.args));
						break;
					}
				}
			}
			Pawn pawn3;
			if (signal.tag == this.inSignalArrested && signal.args.TryGetArg<Pawn>("SUBJECT", out pawn3) && this.pawns.Contains(pawn3))
			{
				this.pawns.Remove(pawn3);
				bool inAggroMentalState = pawn3.InAggroMentalState;
				pawn3.SetFaction(null, null);
				if (this.pawns.Count == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalLast_Arrested, signal.args));
				}
				else if (!inAggroMentalState)
				{
					signal.args.Add(this.pawns.Count.Named("PAWNSALIVECOUNT"));
					switch (this.ChooseRandomInteraction())
					{
					case QuestPart_RefugeeInteractions.InteractionResponseType.AssaultColony:
						this.AssaultColony(HistoryEventDefOf.QuestPawnArrested);
						Find.SignalManager.SendSignal(new Signal(this.outSignalArrested_AssaultColony, signal.args));
						break;
					case QuestPart_RefugeeInteractions.InteractionResponseType.Leave:
						this.LeavePlayer();
						Find.SignalManager.SendSignal(new Signal(this.outSignalArrested_LeaveColony, signal.args));
						break;
					case QuestPart_RefugeeInteractions.InteractionResponseType.BadThought:
						Find.SignalManager.SendSignal(new Signal(this.outSignalArrested_BadThought, signal.args));
						break;
					}
				}
			}
			Pawn item4;
			if (signal.tag == this.inSignalSurgeryViolation && signal.args.TryGetArg<Pawn>("SUBJECT", out item4) && this.pawns.Contains(item4))
			{
				signal.args.Add(this.pawns.Count.Named("PAWNSALIVECOUNT"));
				switch (this.ChooseRandomInteraction())
				{
				case QuestPart_RefugeeInteractions.InteractionResponseType.AssaultColony:
					this.AssaultColony(HistoryEventDefOf.PerformedHarmfulSurgery);
					Find.SignalManager.SendSignal(new Signal(this.outSignalSurgeryViolation_AssaultColony, signal.args));
					break;
				case QuestPart_RefugeeInteractions.InteractionResponseType.Leave:
					this.LeavePlayer();
					Find.SignalManager.SendSignal(new Signal(this.outSignalSurgeryViolation_LeaveColony, signal.args));
					break;
				case QuestPart_RefugeeInteractions.InteractionResponseType.BadThought:
					Find.SignalManager.SendSignal(new Signal(this.outSignalSurgeryViolation_BadThought, signal.args));
					break;
				}
			}
			if (this.inSignalAssaultColony != null && signal.tag == this.inSignalAssaultColony)
			{
				this.AssaultColony(null);
			}
		}

		// Token: 0x0600453E RID: 17726 RVA: 0x0016F5C8 File Offset: 0x0016D7C8
		private void LeavePlayer()
		{
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.faction != this.pawns[i].Faction)
				{
					this.pawns[i].SetFaction(this.faction, null);
				}
			}
			LeaveQuestPartUtility.MakePawnsLeave(this.pawns, false, this.quest, false);
			base.Complete();
		}

		// Token: 0x0600453F RID: 17727 RVA: 0x0016F638 File Offset: 0x0016D838
		private void AssaultColony(HistoryEventDef reason)
		{
			if (this.faction.HasGoodwill)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this.faction, Faction.OfPlayer.GoodwillToMakeHostile(this.faction), true, false, reason, null);
			}
			else
			{
				this.faction.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, false, null, null);
			}
			for (int i = 0; i < this.pawns.Count; i++)
			{
				Lord lord = this.pawns[i].GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this.pawns[i], PawnLostCondition.ForcedByQuest, null);
				}
			}
			for (int j = 0; j < this.pawns.Count; j++)
			{
				this.pawns[j].SetFaction(this.faction, null);
				if (!this.pawns[j].Awake())
				{
					RestUtility.WakeUp(this.pawns[j]);
				}
			}
			Lord lord2 = LordMaker.MakeNewLord(this.faction, new LordJob_AssaultColony(this.faction, true, true, false, false, true, false, true), this.mapParent.Map, null);
			for (int k = 0; k < this.pawns.Count; k++)
			{
				if (!this.pawns[k].Dead)
				{
					lord2.AddPawn(this.pawns[k]);
				}
			}
			base.Complete();
		}

		// Token: 0x06004540 RID: 17728 RVA: 0x0016F7B0 File Offset: 0x0016D9B0
		private QuestPart_RefugeeInteractions.InteractionResponseType ChooseRandomInteraction()
		{
			return Gen.RandomEnumValue<QuestPart_RefugeeInteractions.InteractionResponseType>(false);
		}

		// Token: 0x06004541 RID: 17729 RVA: 0x0016F7B8 File Offset: 0x0016D9B8
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06004542 RID: 17730 RVA: 0x0016F7CC File Offset: 0x0016D9CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignalDestroyed, "inSignalDestroyed", null, false);
			Scribe_Values.Look<string>(ref this.inSignalArrested, "inSignalArrested", null, false);
			Scribe_Values.Look<string>(ref this.inSignalSurgeryViolation, "inSignalSurgeryViolation", null, false);
			Scribe_Values.Look<string>(ref this.inSignalRecruited, "inSignalRecruited", null, false);
			Scribe_Values.Look<string>(ref this.inSignalKidnapped, "inSignalKidnapped", null, false);
			Scribe_Values.Look<string>(ref this.inSignalAssaultColony, "inSignalAssaultColony", null, false);
			Scribe_Values.Look<string>(ref this.inSignalLeftMap, "inSignalLeftMap", null, false);
			Scribe_Values.Look<string>(ref this.inSignalBanished, "inSignalBanished", null, false);
			Scribe_Values.Look<string>(ref this.outSignalDestroyed_AssaultColony, "outSignalDestroyed_AssaultColony", null, false);
			Scribe_Values.Look<string>(ref this.outSignalDestroyed_LeaveColony, "outSignalDestroyed_LeaveColony", null, false);
			Scribe_Values.Look<string>(ref this.outSignalDestroyed_BadThought, "outSignalDestroyed_BadThought", null, false);
			Scribe_Values.Look<string>(ref this.outSignalArrested_AssaultColony, "outSignalArrested_AssaultColony", null, false);
			Scribe_Values.Look<string>(ref this.outSignalArrested_LeaveColony, "outSignalArrested_LeaveColony", null, false);
			Scribe_Values.Look<string>(ref this.outSignalArrested_BadThought, "outSignalArrested_BadThought", null, false);
			Scribe_Values.Look<string>(ref this.outSignalSurgeryViolation_AssaultColony, "outSignalSurgeryViolation_AssaultColony", null, false);
			Scribe_Values.Look<string>(ref this.outSignalSurgeryViolation_LeaveColony, "outSignalSurgeryViolation_LeaveColony", null, false);
			Scribe_Values.Look<string>(ref this.outSignalSurgeryViolation_BadThought, "outSignalSurgeryViolation_BadThought", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLast_Arrested, "outSignalLastArrested", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLast_Destroyed, "outSignalLastDestroyed", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLast_Kidnapped, "outSignalLastKidnapped", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLast_Recruited, "outSignalLastRecruited", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLast_LeftMapAllHealthy, "outSignalLastLeftMapAllHealthy", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLast_LeftMapAllNotHealthy, "outSignalLastLeftMapAllNotHealthy", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLast_Banished, "outSignalLast_Banished", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.pawnsLeftUnhealthy, "pawnsLeftUnhealthy", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04002A1C RID: 10780
		public string inSignalDestroyed;

		// Token: 0x04002A1D RID: 10781
		public string inSignalArrested;

		// Token: 0x04002A1E RID: 10782
		public string inSignalSurgeryViolation;

		// Token: 0x04002A1F RID: 10783
		public string inSignalRecruited;

		// Token: 0x04002A20 RID: 10784
		public string inSignalKidnapped;

		// Token: 0x04002A21 RID: 10785
		public string inSignalAssaultColony;

		// Token: 0x04002A22 RID: 10786
		public string inSignalLeftMap;

		// Token: 0x04002A23 RID: 10787
		public string inSignalBanished;

		// Token: 0x04002A24 RID: 10788
		public string outSignalDestroyed_AssaultColony;

		// Token: 0x04002A25 RID: 10789
		public string outSignalDestroyed_LeaveColony;

		// Token: 0x04002A26 RID: 10790
		public string outSignalDestroyed_BadThought;

		// Token: 0x04002A27 RID: 10791
		public string outSignalArrested_AssaultColony;

		// Token: 0x04002A28 RID: 10792
		public string outSignalArrested_LeaveColony;

		// Token: 0x04002A29 RID: 10793
		public string outSignalArrested_BadThought;

		// Token: 0x04002A2A RID: 10794
		public string outSignalSurgeryViolation_AssaultColony;

		// Token: 0x04002A2B RID: 10795
		public string outSignalSurgeryViolation_LeaveColony;

		// Token: 0x04002A2C RID: 10796
		public string outSignalSurgeryViolation_BadThought;

		// Token: 0x04002A2D RID: 10797
		public string outSignalLast_Arrested;

		// Token: 0x04002A2E RID: 10798
		public string outSignalLast_Destroyed;

		// Token: 0x04002A2F RID: 10799
		public string outSignalLast_Kidnapped;

		// Token: 0x04002A30 RID: 10800
		public string outSignalLast_Recruited;

		// Token: 0x04002A31 RID: 10801
		public string outSignalLast_LeftMapAllHealthy;

		// Token: 0x04002A32 RID: 10802
		public string outSignalLast_LeftMapAllNotHealthy;

		// Token: 0x04002A33 RID: 10803
		public string outSignalLast_Banished;

		// Token: 0x04002A34 RID: 10804
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04002A35 RID: 10805
		public Faction faction;

		// Token: 0x04002A36 RID: 10806
		public MapParent mapParent;

		// Token: 0x04002A37 RID: 10807
		public int pawnsLeftUnhealthy;

		// Token: 0x020020B7 RID: 8375
		private enum InteractionResponseType
		{
			// Token: 0x04007D63 RID: 32099
			AssaultColony,
			// Token: 0x04007D64 RID: 32100
			Leave,
			// Token: 0x04007D65 RID: 32101
			BadThought
		}
	}
}
