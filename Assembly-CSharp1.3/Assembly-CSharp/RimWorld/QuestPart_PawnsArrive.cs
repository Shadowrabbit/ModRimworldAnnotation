using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BC5 RID: 3013
	public class QuestPart_PawnsArrive : QuestPart
	{
		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x06004690 RID: 18064 RVA: 0x00175234 File Offset: 0x00173434
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				foreach (Pawn t in PawnsArriveQuestPartUtility.GetQuestLookTargets(this.pawns))
				{
					yield return t;
				}
				IEnumerator<Pawn> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x06004691 RID: 18065 RVA: 0x00175244 File Offset: 0x00173444
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, false);
			}
		}

		// Token: 0x06004692 RID: 18066 RVA: 0x00175258 File Offset: 0x00173458
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				if (this.mapParent != null && this.mapParent.HasMap && this.pawns.Any<Pawn>())
				{
					for (int i = 0; i < this.pawns.Count; i++)
					{
						if (this.joinPlayer && this.pawns[i].Faction != Faction.OfPlayer)
						{
							this.pawns[i].SetFaction(Faction.OfPlayer, null);
						}
					}
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = this.mapParent.Map;
					incidentParms.spawnCenter = this.spawnNear;
					PawnsArrivalModeDef pawnsArrivalModeDef = this.arrivalMode ?? PawnsArrivalModeDefOf.EdgeWalkIn;
					pawnsArrivalModeDef.Worker.TryResolveRaidSpawnCenter(incidentParms);
					pawnsArrivalModeDef.Worker.Arrive(this.pawns, incidentParms);
					if (this.sendStandardLetter)
					{
						TaggedString taggedString;
						TaggedString taggedString2;
						if (this.joinPlayer && this.pawns.Count == 1 && this.pawns[0].RaceProps.Humanlike)
						{
							taggedString = "LetterRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
							taggedString2 = "LetterLabelRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
							PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref taggedString2, this.pawns[0]);
						}
						else
						{
							if (this.joinPlayer)
							{
								taggedString = "LetterPawnsArriveAndJoin".Translate(GenLabel.ThingsLabel(this.pawns.Cast<Thing>(), "  - "));
								taggedString2 = "LetterLabelPawnsArriveAndJoin".Translate();
							}
							else
							{
								taggedString = "LetterPawnsArrive".Translate(GenLabel.ThingsLabel(this.pawns.Cast<Thing>(), "  - "));
								taggedString2 = "LetterLabelPawnsArrive".Translate();
							}
							PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(this.pawns, ref taggedString2, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
						}
						taggedString2 = (this.customLetterLabel.NullOrEmpty() ? taggedString2 : this.customLetterLabel.Formatted(taggedString2.Named("BASELABEL")));
						taggedString = (this.customLetterText.NullOrEmpty() ? taggedString : this.customLetterText.Formatted(taggedString.Named("BASETEXT")));
						Find.LetterStack.ReceiveLetter(taggedString2, taggedString, this.customLetterDef ?? LetterDefOf.PositiveEvent, this.pawns[0], null, this.quest, null, null);
					}
				}
			}
		}

		// Token: 0x06004693 RID: 18067 RVA: 0x0017553C File Offset: 0x0017373C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.spawnNear, "spawnNear", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.joinPlayer, "joinPlayer", false, false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06004694 RID: 18068 RVA: 0x00175640 File Offset: 0x00173840
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, null, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false));
				pawn.relations.everSeenByPlayer = true;
				if (!pawn.IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
				this.pawns.Add(pawn);
				this.arrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.joinPlayer = true;
			}
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x00175732 File Offset: 0x00173932
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x00175740 File Offset: 0x00173940
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04002B09 RID: 11017
		public string inSignal;

		// Token: 0x04002B0A RID: 11018
		public MapParent mapParent;

		// Token: 0x04002B0B RID: 11019
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04002B0C RID: 11020
		public PawnsArrivalModeDef arrivalMode;

		// Token: 0x04002B0D RID: 11021
		public IntVec3 spawnNear = IntVec3.Invalid;

		// Token: 0x04002B0E RID: 11022
		public bool joinPlayer;

		// Token: 0x04002B0F RID: 11023
		public string customLetterText;

		// Token: 0x04002B10 RID: 11024
		public string customLetterLabel;

		// Token: 0x04002B11 RID: 11025
		public LetterDef customLetterDef;

		// Token: 0x04002B12 RID: 11026
		public bool sendStandardLetter = true;
	}
}
