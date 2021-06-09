using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001135 RID: 4405
	public class QuestPart_DropPods : QuestPart
	{
		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x06006076 RID: 24694 RVA: 0x000428CB File Offset: 0x00040ACB
		// (set) Token: 0x06006077 RID: 24695 RVA: 0x001E49AC File Offset: 0x001E2BAC
		public IEnumerable<Thing> Things
		{
			get
			{
				return this.items.Concat(this.pawns.Cast<Thing>());
			}
			set
			{
				this.items.Clear();
				this.pawns.Clear();
				if (value != null)
				{
					foreach (Thing thing in value)
					{
						if (thing.Destroyed)
						{
							Log.Error("Tried to add a destroyed thing to QuestPart_DropPods: " + thing.ToStringSafe<Thing>(), false);
						}
						else
						{
							Pawn pawn = thing as Pawn;
							if (pawn != null)
							{
								this.pawns.Add(pawn);
							}
							else
							{
								this.items.Add(thing);
							}
						}
					}
				}
			}
		}

		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x06006078 RID: 24696 RVA: 0x000428E3 File Offset: 0x00040AE3
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
				if (this.importantLookTarget != null)
				{
					yield return this.importantLookTarget;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x06006079 RID: 24697 RVA: 0x000428F3 File Offset: 0x00040AF3
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, this.makePrisoners);
			}
		}

		// Token: 0x0600607A RID: 24698 RVA: 0x001E4A4C File Offset: 0x001E2C4C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				this.items.RemoveAll((Thing x) => x.Destroyed);
				Thing thing = (from x in this.Things
				where x is Pawn
				select x).MaxByWithFallback((Thing x) => x.MarketValue, null);
				Thing thing2 = this.Things.MaxByWithFallback((Thing x) => x.MarketValue * (float)x.stackCount, null);
				if (this.mapParent != null && this.mapParent.HasMap && this.Things.Any<Thing>())
				{
					Map map = this.mapParent.Map;
					IntVec3 intVec = this.dropSpot.IsValid ? this.dropSpot : this.GetRandomDropSpot();
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
							taggedString = "LetterQuestDropPodsArrived".Translate(GenLabel.ThingsLabel(this.Things, "  - "));
							taggedString2 = "LetterLabelQuestDropPodsArrived".Translate();
							PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(this.pawns, ref taggedString2, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
						}
						taggedString2 = (this.customLetterLabel.NullOrEmpty() ? taggedString2 : this.customLetterLabel.Formatted(taggedString2.Named("BASELABEL")));
						taggedString = (this.customLetterText.NullOrEmpty() ? taggedString : this.customLetterText.Formatted(taggedString.Named("BASETEXT")));
						Find.LetterStack.ReceiveLetter(taggedString2, taggedString, this.customLetterDef ?? LetterDefOf.PositiveEvent, new TargetInfo(intVec, map, false), null, this.quest, null, null);
					}
					if (this.joinPlayer)
					{
						for (int i = 0; i < this.pawns.Count; i++)
						{
							if (this.pawns[i].Faction != Faction.OfPlayer)
							{
								this.pawns[i].SetFaction(Faction.OfPlayer, null);
							}
						}
					}
					else if (this.makePrisoners)
					{
						for (int j = 0; j < this.pawns.Count; j++)
						{
							if (this.pawns[j].RaceProps.Humanlike)
							{
								if (!this.pawns[j].IsPrisonerOfColony)
								{
									this.pawns[j].guest.SetGuestStatus(Faction.OfPlayer, true);
								}
								HealthUtility.TryAnesthetize(this.pawns[j]);
							}
						}
					}
					for (int k = 0; k < this.pawns.Count; k++)
					{
						this.pawns[k].needs.SetInitialLevels();
					}
					DropPodUtility.DropThingsNear(intVec, map, this.Things, 110, false, false, !this.useTradeDropSpot, false);
					this.importantLookTarget = this.items.Find((Thing x) => x.GetInnerIfMinified() is MonumentMarker).GetInnerIfMinified();
					this.items.Clear();
				}
				if (!this.outSignalResult.NullOrEmpty())
				{
					if (thing != null)
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalResult, thing.Named("SUBJECT")));
						return;
					}
					if (thing2 != null)
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalResult, thing2.Named("SUBJECT")));
						return;
					}
					Find.SignalManager.SendSignal(new Signal(this.outSignalResult));
				}
			}
		}

		// Token: 0x0600607B RID: 24699 RVA: 0x0004290C File Offset: 0x00040B0C
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x0600607C RID: 24700 RVA: 0x0004291A File Offset: 0x00040B1A
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x0600607D RID: 24701 RVA: 0x001E4EF0 File Offset: 0x001E30F0
		public override void PostQuestAdded()
		{
			base.PostQuestAdded();
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].def == ThingDefOf.PsychicAmplifier)
				{
					Find.History.Notify_PsylinkAvailable();
					return;
				}
			}
		}

		// Token: 0x0600607E RID: 24702 RVA: 0x001E4F3C File Offset: 0x001E313C
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.destroyItemsOnCleanup)
			{
				for (int i = 0; i < this.items.Count; i++)
				{
					if (!this.items[i].Destroyed)
					{
						this.items[i].Destroy(DestroyMode.Vanish);
					}
				}
				this.items.Clear();
			}
		}

		// Token: 0x0600607F RID: 24703 RVA: 0x001E4FA0 File Offset: 0x001E31A0
		private IntVec3 GetRandomDropSpot()
		{
			Map map = this.mapParent.Map;
			if (this.useTradeDropSpot)
			{
				return DropCellFinder.TradeDropSpot(map);
			}
			IntVec3 result;
			if (CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => x.Standable(map) && !x.Roofed(map) && !x.Fogged(map) && map.reachability.CanReachColony(x), map, 1000, out result))
			{
				return result;
			}
			return DropCellFinder.RandomDropSpot(map);
		}

		// Token: 0x06006080 RID: 24704 RVA: 0x001E5008 File Offset: 0x001E3208
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_Values.Look<IntVec3>(ref this.dropSpot, "dropSpot", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.useTradeDropSpot, "useTradeDropSpot", false, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.joinPlayer, "joinPlayer", false, false);
			Scribe_Values.Look<bool>(ref this.makePrisoners, "makePrisoners", false, false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_References.Look<Thing>(ref this.importantLookTarget, "importantLookTarget", false);
			Scribe_Collections.Look<ThingDef>(ref this.thingsToExcludeFromHyperlinks, "thingsToExcludeFromHyperlinks", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.destroyItemsOnCleanup, "destroyItemsOnCleanup", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.thingsToExcludeFromHyperlinks == null)
				{
					this.thingsToExcludeFromHyperlinks = new List<ThingDef>();
				}
				this.items.RemoveAll((Thing x) => x == null);
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006081 RID: 24705 RVA: 0x001E51BC File Offset: 0x001E33BC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				List<Thing> list = ThingSetMakerDefOf.DebugQuestDropPodsContents.root.Generate();
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i] as Pawn;
					if (pawn != null)
					{
						pawn.relations.everSeenByPlayer = true;
						if (!pawn.IsWorldPawn())
						{
							Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						}
					}
				}
				this.Things = list;
			}
		}

		// Token: 0x0400406D RID: 16493
		public string inSignal;

		// Token: 0x0400406E RID: 16494
		public string outSignalResult;

		// Token: 0x0400406F RID: 16495
		public IntVec3 dropSpot = IntVec3.Invalid;

		// Token: 0x04004070 RID: 16496
		public bool useTradeDropSpot;

		// Token: 0x04004071 RID: 16497
		public MapParent mapParent;

		// Token: 0x04004072 RID: 16498
		private List<Thing> items = new List<Thing>();

		// Token: 0x04004073 RID: 16499
		private List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04004074 RID: 16500
		public List<ThingDef> thingsToExcludeFromHyperlinks = new List<ThingDef>();

		// Token: 0x04004075 RID: 16501
		public bool joinPlayer;

		// Token: 0x04004076 RID: 16502
		public bool makePrisoners;

		// Token: 0x04004077 RID: 16503
		public bool destroyItemsOnCleanup = true;

		// Token: 0x04004078 RID: 16504
		public string customLetterText;

		// Token: 0x04004079 RID: 16505
		public string customLetterLabel;

		// Token: 0x0400407A RID: 16506
		public LetterDef customLetterDef;

		// Token: 0x0400407B RID: 16507
		public bool sendStandardLetter = true;

		// Token: 0x0400407C RID: 16508
		private Thing importantLookTarget;
	}
}
