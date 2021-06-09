using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001149 RID: 4425
	public class QuestPart_PawnsArrive : QuestPart
	{
		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x06006119 RID: 24857 RVA: 0x00042E93 File Offset: 0x00041093
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

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x0600611A RID: 24858 RVA: 0x00042EA3 File Offset: 0x000410A3
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, false);
			}
		}

		// Token: 0x0600611B RID: 24859 RVA: 0x001E6A34 File Offset: 0x001E4C34
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

		// Token: 0x0600611C RID: 24860 RVA: 0x001E6D18 File Offset: 0x001E4F18
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

		// Token: 0x0600611D RID: 24861 RVA: 0x001E6E1C File Offset: 0x001E501C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, null, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
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

		// Token: 0x0600611E RID: 24862 RVA: 0x00042EB7 File Offset: 0x000410B7
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x0600611F RID: 24863 RVA: 0x00042EC5 File Offset: 0x000410C5
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040040E0 RID: 16608
		public string inSignal;

		// Token: 0x040040E1 RID: 16609
		public MapParent mapParent;

		// Token: 0x040040E2 RID: 16610
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040040E3 RID: 16611
		public PawnsArrivalModeDef arrivalMode;

		// Token: 0x040040E4 RID: 16612
		public IntVec3 spawnNear = IntVec3.Invalid;

		// Token: 0x040040E5 RID: 16613
		public bool joinPlayer;

		// Token: 0x040040E6 RID: 16614
		public string customLetterText;

		// Token: 0x040040E7 RID: 16615
		public string customLetterLabel;

		// Token: 0x040040E8 RID: 16616
		public LetterDef customLetterDef;

		// Token: 0x040040E9 RID: 16617
		public bool sendStandardLetter = true;
	}
}
