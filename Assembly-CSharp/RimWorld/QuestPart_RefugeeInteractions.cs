using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020010F3 RID: 4339
	public class QuestPart_RefugeeInteractions : QuestPartActivable
	{
		// Token: 0x06005ED6 RID: 24278 RVA: 0x001E0958 File Offset: 0x001DEB58
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
				if (this.pawns.Count == 0)
				{
					if (this.pawnsLeftUnhealthy > 0)
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalLast_LeftMapAllNotHealthy, signal.args));
					}
					else
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalLast_LeftMapAllHealthy, signal.args));
					}
				}
			}
			Pawn item4;
			if (signal.tag == this.inSignalDestroyed && signal.args.TryGetArg<Pawn>("SUBJECT", out item4) && this.pawns.Contains(item4))
			{
				this.pawns.Remove(item4);
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
						this.AssaultColony();
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
			Pawn pawn2;
			if (signal.tag == this.inSignalArrested && signal.args.TryGetArg<Pawn>("SUBJECT", out pawn2) && this.pawns.Contains(pawn2) && !pawn2.InAggroMentalState)
			{
				this.pawns.Remove(pawn2);
				pawn2.SetFaction(this.faction, null);
				if (this.pawns.Count == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalLast_Arrested, signal.args));
				}
				else
				{
					signal.args.Add(this.pawns.Count.Named("PAWNSALIVECOUNT"));
					switch (this.ChooseRandomInteraction())
					{
					case QuestPart_RefugeeInteractions.InteractionResponseType.AssaultColony:
						this.AssaultColony();
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
			Pawn item5;
			if (signal.tag == this.inSignalSurgeryViolation && signal.args.TryGetArg<Pawn>("SUBJECT", out item5) && this.pawns.Contains(item5))
			{
				signal.args.Add(this.pawns.Count.Named("PAWNSALIVECOUNT"));
				switch (this.ChooseRandomInteraction())
				{
				case QuestPart_RefugeeInteractions.InteractionResponseType.AssaultColony:
					this.AssaultColony();
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
				this.AssaultColony();
			}
		}

		// Token: 0x06005ED7 RID: 24279 RVA: 0x001E0ED4 File Offset: 0x001DF0D4
		private void LeavePlayer()
		{
			for (int i = 0; i < this.pawns.Count; i++)
			{
				this.pawns[i].SetFaction(this.faction, null);
			}
			LeaveQuestPartUtility.MakePawnsLeave(this.pawns, false, this.quest);
			base.Complete();
		}

		// Token: 0x06005ED8 RID: 24280 RVA: 0x001E0F28 File Offset: 0x001DF128
		private void AssaultColony()
		{
			this.faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Hostile, false, null, null);
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
			Lord lord2 = LordMaker.MakeNewLord(this.faction, new LordJob_AssaultColony(this.faction, true, true, false, false, true), this.mapParent.Map, null);
			for (int k = 0; k < this.pawns.Count; k++)
			{
				if (!this.pawns[k].Dead)
				{
					lord2.AddPawn(this.pawns[k]);
				}
			}
			base.Complete();
		}

		// Token: 0x06005ED9 RID: 24281 RVA: 0x001E1064 File Offset: 0x001DF264
		private QuestPart_RefugeeInteractions.InteractionResponseType ChooseRandomInteraction()
		{
			Array values = Enum.GetValues(typeof(QuestPart_RefugeeInteractions.InteractionResponseType));
			return (QuestPart_RefugeeInteractions.InteractionResponseType)values.GetValue(Rand.Range(0, values.Length));
		}

		// Token: 0x06005EDA RID: 24282 RVA: 0x00041A34 File Offset: 0x0003FC34
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06005EDB RID: 24283 RVA: 0x001E1098 File Offset: 0x001DF298
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

		// Token: 0x04003F66 RID: 16230
		public string inSignalDestroyed;

		// Token: 0x04003F67 RID: 16231
		public string inSignalArrested;

		// Token: 0x04003F68 RID: 16232
		public string inSignalSurgeryViolation;

		// Token: 0x04003F69 RID: 16233
		public string inSignalRecruited;

		// Token: 0x04003F6A RID: 16234
		public string inSignalKidnapped;

		// Token: 0x04003F6B RID: 16235
		public string inSignalAssaultColony;

		// Token: 0x04003F6C RID: 16236
		public string inSignalLeftMap;

		// Token: 0x04003F6D RID: 16237
		public string inSignalBanished;

		// Token: 0x04003F6E RID: 16238
		public string outSignalDestroyed_AssaultColony;

		// Token: 0x04003F6F RID: 16239
		public string outSignalDestroyed_LeaveColony;

		// Token: 0x04003F70 RID: 16240
		public string outSignalDestroyed_BadThought;

		// Token: 0x04003F71 RID: 16241
		public string outSignalArrested_AssaultColony;

		// Token: 0x04003F72 RID: 16242
		public string outSignalArrested_LeaveColony;

		// Token: 0x04003F73 RID: 16243
		public string outSignalArrested_BadThought;

		// Token: 0x04003F74 RID: 16244
		public string outSignalSurgeryViolation_AssaultColony;

		// Token: 0x04003F75 RID: 16245
		public string outSignalSurgeryViolation_LeaveColony;

		// Token: 0x04003F76 RID: 16246
		public string outSignalSurgeryViolation_BadThought;

		// Token: 0x04003F77 RID: 16247
		public string outSignalLast_Arrested;

		// Token: 0x04003F78 RID: 16248
		public string outSignalLast_Destroyed;

		// Token: 0x04003F79 RID: 16249
		public string outSignalLast_Kidnapped;

		// Token: 0x04003F7A RID: 16250
		public string outSignalLast_Recruited;

		// Token: 0x04003F7B RID: 16251
		public string outSignalLast_LeftMapAllHealthy;

		// Token: 0x04003F7C RID: 16252
		public string outSignalLast_LeftMapAllNotHealthy;

		// Token: 0x04003F7D RID: 16253
		public string outSignalLast_Banished;

		// Token: 0x04003F7E RID: 16254
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04003F7F RID: 16255
		public Faction faction;

		// Token: 0x04003F80 RID: 16256
		public MapParent mapParent;

		// Token: 0x04003F81 RID: 16257
		public int pawnsLeftUnhealthy;

		// Token: 0x020010F4 RID: 4340
		private enum InteractionResponseType
		{
			// Token: 0x04003F83 RID: 16259
			AssaultColony,
			// Token: 0x04003F84 RID: 16260
			Leave,
			// Token: 0x04003F85 RID: 16261
			BadThought
		}
	}
}
