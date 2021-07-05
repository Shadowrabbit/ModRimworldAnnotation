using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BBD RID: 3005
	public class QuestPart_DropPods : QuestPart
	{
		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x06004644 RID: 17988 RVA: 0x00172FBC File Offset: 0x001711BC
		// (set) Token: 0x06004645 RID: 17989 RVA: 0x00172FD4 File Offset: 0x001711D4
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
							Log.Error("Tried to add a destroyed thing to QuestPart_DropPods: " + thing.ToStringSafe<Thing>());
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
								ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
								if (thingOwner != null)
								{
									for (int i = 0; i < thingOwner.Count; i++)
									{
										Pawn item;
										if ((item = (thingOwner[i] as Pawn)) != null)
										{
											this.pawnsInContainers.Add(item);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06004646 RID: 17990 RVA: 0x001730B8 File Offset: 0x001712B8
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
				foreach (Pawn t in PawnsArriveQuestPartUtility.GetQuestLookTargets(this.pawns.Concat(this.pawnsInContainers)))
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

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06004647 RID: 17991 RVA: 0x001730C8 File Offset: 0x001712C8
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, this.makePrisoners);
			}
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x001730E4 File Offset: 0x001712E4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.mapParent != null && this.mapParent.HasMap)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				this.pawnsInContainers.RemoveAll((Pawn x) => x.Destroyed);
				this.items.RemoveAll((Thing x) => x.Destroyed);
				this.tmpThingsToDrop.Clear();
				this.tmpThingsToDrop.AddRange(this.Things);
				for (int i = 0; i < this.thingDefs.Count; i++)
				{
					Thing thing = ThingMaker.MakeThing(this.thingDefs[i].thingDef, GenStuff.RandomStuffByCommonalityFor(this.thingDefs[i].thingDef, TechLevel.Undefined));
					thing.stackCount = this.thingDefs[i].count;
					this.tmpThingsToDrop.Add(thing);
				}
				this.tmpThingsToDrop.RemoveAll((Thing x) => x.Spawned);
				Thing thing2 = (from x in this.tmpThingsToDrop
				where x is Pawn
				select x).MaxByWithFallback((Thing x) => x.MarketValue, null);
				Thing thing3 = this.tmpThingsToDrop.MaxByWithFallback((Thing x) => x.MarketValue * (float)x.stackCount, null);
				if (this.tmpThingsToDrop.Any<Thing>())
				{
					Map map = this.mapParent.Map;
					IntVec3 intVec = this.dropSpot.IsValid ? this.dropSpot : this.GetRandomDropSpot();
					TaggedString taggedString = null;
					TaggedString taggedString2 = null;
					if (this.sendStandardLetter)
					{
						if (this.joinPlayer && this.pawns.Count == 1 && this.pawns[0].RaceProps.Humanlike)
						{
							taggedString = "LetterRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
							taggedString2 = "LetterLabelRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
							PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref taggedString2, this.pawns[0]);
						}
						else
						{
							taggedString = "LetterQuestDropPodsArrived".Translate(GenLabel.ThingsLabel(this.tmpThingsToDrop, "  - "));
							taggedString2 = "LetterLabelQuestDropPodsArrived".Translate();
							PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(this.pawns, ref taggedString2, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
						}
						taggedString2 = (this.customLetterLabel.NullOrEmpty() ? taggedString2 : this.customLetterLabel.Formatted(taggedString2.Named("BASELABEL")));
						taggedString = (this.customLetterText.NullOrEmpty() ? taggedString : this.customLetterText.Formatted(taggedString.Named("BASETEXT")));
					}
					if (this.joinPlayer)
					{
						for (int j = 0; j < this.pawns.Count; j++)
						{
							if (this.pawns[j].Faction != Faction.OfPlayer)
							{
								this.pawns[j].SetFaction(Faction.OfPlayer, null);
							}
						}
					}
					else if (this.makePrisoners)
					{
						for (int k = 0; k < this.pawns.Count; k++)
						{
							if (this.pawns[k].RaceProps.Humanlike)
							{
								if (!this.pawns[k].IsPrisonerOfColony)
								{
									this.pawns[k].guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Prisoner);
								}
								HealthUtility.TryAnesthetize(this.pawns[k]);
							}
						}
					}
					DropPodUtility.DropThingsNear(intVec, map, this.tmpThingsToDrop, 110, false, false, !this.useTradeDropSpot, false);
					for (int l = 0; l < this.pawns.Count; l++)
					{
						this.pawns[l].needs.SetInitialLevels();
						Pawn_MindState mindState = this.pawns[l].mindState;
						if (mindState != null)
						{
							mindState.SetupLastHumanMeatTick();
						}
					}
					if (this.sendStandardLetter)
					{
						IntVec3 cell = intVec;
						for (int m = 0; m < this.tmpThingsToDrop.Count; m++)
						{
							if (this.tmpThingsToDrop[m].SpawnedOrAnyParentSpawned)
							{
								cell = this.tmpThingsToDrop[m].PositionHeld;
								break;
							}
						}
						Find.LetterStack.ReceiveLetter(taggedString2, taggedString, this.customLetterDef ?? LetterDefOf.PositiveEvent, new TargetInfo(cell, map, false), null, this.quest, null, null);
					}
					this.importantLookTarget = this.items.Find((Thing x) => x.GetInnerIfMinified() is MonumentMarker).GetInnerIfMinified();
					this.items.Clear();
					if (!this.outSignalResult.NullOrEmpty())
					{
						if (thing2 != null)
						{
							Find.SignalManager.SendSignal(new Signal(this.outSignalResult, thing2.Named("SUBJECT")));
							return;
						}
						if (thing3 != null)
						{
							Find.SignalManager.SendSignal(new Signal(this.outSignalResult, thing3.Named("SUBJECT")));
							return;
						}
						Find.SignalManager.SendSignal(new Signal(this.outSignalResult));
					}
				}
			}
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x001736E3 File Offset: 0x001718E3
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p) || this.pawnsInContainers.Contains(p);
		}

		// Token: 0x0600464A RID: 17994 RVA: 0x00173701 File Offset: 0x00171901
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x00173714 File Offset: 0x00171914
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

		// Token: 0x0600464C RID: 17996 RVA: 0x00173760 File Offset: 0x00171960
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

		// Token: 0x0600464D RID: 17997 RVA: 0x001737C4 File Offset: 0x001719C4
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
			return DropCellFinder.RandomDropSpot(map, true);
		}

		// Token: 0x0600464E RID: 17998 RVA: 0x0017382C File Offset: 0x00171A2C
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
			Scribe_Collections.Look<Pawn>(ref this.pawnsInContainers, "pawnsInContainers", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<ThingDefCountClass>(ref this.thingDefs, "thingDefs", LookMode.Deep, Array.Empty<object>());
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
				this.pawnsInContainers.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600464F RID: 17999 RVA: 0x00173A3C File Offset: 0x00171C3C
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

		// Token: 0x04002AC0 RID: 10944
		public string inSignal;

		// Token: 0x04002AC1 RID: 10945
		public string outSignalResult;

		// Token: 0x04002AC2 RID: 10946
		public IntVec3 dropSpot = IntVec3.Invalid;

		// Token: 0x04002AC3 RID: 10947
		public bool useTradeDropSpot;

		// Token: 0x04002AC4 RID: 10948
		public MapParent mapParent;

		// Token: 0x04002AC5 RID: 10949
		private List<Thing> items = new List<Thing>();

		// Token: 0x04002AC6 RID: 10950
		private List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04002AC7 RID: 10951
		private List<Pawn> pawnsInContainers = new List<Pawn>();

		// Token: 0x04002AC8 RID: 10952
		public List<ThingDefCountClass> thingDefs = new List<ThingDefCountClass>();

		// Token: 0x04002AC9 RID: 10953
		public List<ThingDef> thingsToExcludeFromHyperlinks = new List<ThingDef>();

		// Token: 0x04002ACA RID: 10954
		public bool joinPlayer;

		// Token: 0x04002ACB RID: 10955
		public bool makePrisoners;

		// Token: 0x04002ACC RID: 10956
		public bool destroyItemsOnCleanup = true;

		// Token: 0x04002ACD RID: 10957
		public string customLetterText;

		// Token: 0x04002ACE RID: 10958
		public string customLetterLabel;

		// Token: 0x04002ACF RID: 10959
		public LetterDef customLetterDef;

		// Token: 0x04002AD0 RID: 10960
		public bool sendStandardLetter = true;

		// Token: 0x04002AD1 RID: 10961
		private Thing importantLookTarget;

		// Token: 0x04002AD2 RID: 10962
		private List<Thing> tmpThingsToDrop = new List<Thing>();
	}
}
