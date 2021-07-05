using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BC0 RID: 3008
	public class QuestPart_GiveNearPawn : QuestPart
	{
		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x06004663 RID: 18019 RVA: 0x00174015 File Offset: 0x00172215
		// (set) Token: 0x06004664 RID: 18020 RVA: 0x00174030 File Offset: 0x00172230
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
						Pawn item;
						if ((item = (thing as Pawn)) != null)
						{
							this.pawns.Add(item);
						}
						else
						{
							this.items.Add(thing);
						}
					}
				}
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x06004665 RID: 18021 RVA: 0x001740B0 File Offset: 0x001722B0
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in base.Hyperlinks)
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				foreach (Thing outerThing in this.items)
				{
					ThingDef def = outerThing.GetInnerIfMinified().def;
					yield return new Dialog_InfoCard.Hyperlink(def, -1);
				}
				List<Thing>.Enumerator enumerator2 = default(List<Thing>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x06004666 RID: 18022 RVA: 0x001740C0 File Offset: 0x001722C0
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
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

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x06004667 RID: 18023 RVA: 0x001740D0 File Offset: 0x001722D0
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, this.makePrisoners);
			}
		}

		// Token: 0x06004668 RID: 18024 RVA: 0x001740EC File Offset: 0x001722EC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.nearPawn != null && (this.nearPawn.SpawnedOrAnyParentSpawned || this.nearPawn.IsCaravanMember()))
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				this.items.RemoveAll((Thing x) => x.Destroyed);
				this.tmpThingsToGive.Clear();
				this.tmpThingsToGive.AddRange(this.Things);
				for (int i = 0; i < this.thingDefs.Count; i++)
				{
					Thing thing = ThingMaker.MakeThing(this.thingDefs[i].thingDef, GenStuff.RandomStuffByCommonalityFor(this.thingDefs[i].thingDef, TechLevel.Undefined));
					thing.stackCount = this.thingDefs[i].count;
					this.tmpThingsToGive.Add(thing);
				}
				this.tmpThingsToGive.RemoveAll((Thing x) => x.Spawned);
				Thing thing2 = (from x in this.tmpThingsToGive
				where x is Pawn
				select x).MaxByWithFallback((Thing x) => x.MarketValue, null);
				Thing thing3 = this.tmpThingsToGive.MaxByWithFallback((Thing x) => x.MarketValue * (float)x.stackCount, null);
				if (this.tmpThingsToGive.Any<Thing>())
				{
					TaggedString taggedString = null;
					TaggedString taggedString2 = null;
					if (this.sendStandardLetter)
					{
						if (this.nearPawn.SpawnedOrAnyParentSpawned)
						{
							if (this.joinPlayer && this.pawns.Count == 1 && this.pawns[0].RaceProps.Humanlike)
							{
								taggedString = "LetterRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
								taggedString2 = "LetterLabelRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
								PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref taggedString2, this.pawns[0]);
							}
							else
							{
								taggedString = "LetterQuestDropPodsArrived".Translate(GenLabel.ThingsLabel(this.tmpThingsToGive, "  - "));
								taggedString2 = "LetterLabelQuestDropPodsArrived".Translate();
								PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(this.pawns, ref taggedString2, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
							}
							taggedString2 = (this.customDropPodsLetterLabel.NullOrEmpty() ? taggedString2 : this.customDropPodsLetterLabel.Formatted(taggedString2.Named("BASELABEL")));
							taggedString = (this.customDropPodsLetterText.NullOrEmpty() ? taggedString : this.customDropPodsLetterText.Formatted(taggedString.Named("BASETEXT")));
						}
						else if (this.nearPawn.IsCaravanMember())
						{
							taggedString = "LetterQuestItemsAddedToCaravanInventory".Translate(this.nearPawn.GetCaravan().Named("CARAVAN"), GenLabel.ThingsLabel(this.tmpThingsToGive, "  - ").Named("THINGS"));
							taggedString2 = "LetterLabelQuestItemsAddedToCaravanInventory".Translate(this.nearPawn.GetCaravan().Named("CARAVAN"));
							taggedString2 = (this.customCaravanInventoryLetterLabel.NullOrEmpty() ? taggedString2 : this.customCaravanInventoryLetterLabel.Formatted(taggedString2.Named("BASELABEL")));
							taggedString = (this.customCaravanInventoryLetterText.NullOrEmpty() ? taggedString : this.customCaravanInventoryLetterText.Formatted(taggedString.Named("BASETEXT")));
						}
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
					GlobalTargetInfo target = GlobalTargetInfo.Invalid;
					if (this.nearPawn.SpawnedOrAnyParentSpawned)
					{
						IntVec3 intVec = DropCellFinder.TradeDropSpot(this.nearPawn.MapHeld);
						DropPodUtility.DropThingsNear(intVec, this.nearPawn.MapHeld, this.tmpThingsToGive, 110, false, false, false, false);
						target = new GlobalTargetInfo(intVec, this.nearPawn.MapHeld, false);
						for (int l = 0; l < this.tmpThingsToGive.Count; l++)
						{
							if (this.tmpThingsToGive[l].SpawnedOrAnyParentSpawned)
							{
								target = new GlobalTargetInfo(this.tmpThingsToGive[l].PositionHeld, this.nearPawn.MapHeld, false);
								break;
							}
						}
					}
					else if (this.nearPawn.IsCaravanMember())
					{
						for (int m = 0; m < this.tmpThingsToGive.Count; m++)
						{
							Pawn pawn;
							if ((pawn = (this.tmpThingsToGive[m] as Pawn)) != null)
							{
								if (pawn.Faction != Faction.OfPlayer)
								{
									pawn.SetFaction(Faction.OfPlayer, null);
								}
								this.nearPawn.GetCaravan().AddPawn(pawn, true);
							}
							else
							{
								CaravanInventoryUtility.GiveThing(this.nearPawn.GetCaravan(), this.tmpThingsToGive[m]);
							}
						}
						target = this.nearPawn.GetCaravan();
					}
					if (this.sendStandardLetter)
					{
						Find.LetterStack.ReceiveLetter(taggedString2, taggedString, this.customLetterDef ?? LetterDefOf.PositiveEvent, target, null, this.quest, null, null);
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

		// Token: 0x06004669 RID: 18025 RVA: 0x00174820 File Offset: 0x00172A20
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x0600466A RID: 18026 RVA: 0x00174830 File Offset: 0x00172A30
		public override void Cleanup()
		{
			base.Cleanup();
			for (int i = 0; i < this.items.Count; i++)
			{
				if (!this.items[i].Destroyed)
				{
					this.items[i].Destroy(DestroyMode.Vanish);
				}
			}
			this.items.Clear();
		}

		// Token: 0x0600466B RID: 18027 RVA: 0x0017488C File Offset: 0x00172A8C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_References.Look<Pawn>(ref this.nearPawn, "nearPawn", false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<ThingDefCountClass>(ref this.thingDefs, "thingDefs", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.joinPlayer, "joinPlayer", false, false);
			Scribe_Values.Look<bool>(ref this.makePrisoners, "makePrisoners", false, false);
			Scribe_Values.Look<string>(ref this.customDropPodsLetterLabel, "customDropPodsLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customDropPodsLetterText, "customDropPodsLetterText", null, false);
			Scribe_Values.Look<string>(ref this.customCaravanInventoryLetterLabel, "customCaravanInventoryLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customCaravanInventoryLetterText, "customCaravanInventoryLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_References.Look<Thing>(ref this.importantLookTarget, "importantLookTarget", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.items.RemoveAll((Thing x) => x == null);
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600466C RID: 18028 RVA: 0x00174A13 File Offset: 0x00172C13
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04002AE5 RID: 10981
		public string inSignal;

		// Token: 0x04002AE6 RID: 10982
		public string outSignalResult;

		// Token: 0x04002AE7 RID: 10983
		public Pawn nearPawn;

		// Token: 0x04002AE8 RID: 10984
		private List<Thing> items = new List<Thing>();

		// Token: 0x04002AE9 RID: 10985
		private List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04002AEA RID: 10986
		public List<ThingDefCountClass> thingDefs = new List<ThingDefCountClass>();

		// Token: 0x04002AEB RID: 10987
		public bool joinPlayer;

		// Token: 0x04002AEC RID: 10988
		public bool makePrisoners;

		// Token: 0x04002AED RID: 10989
		public string customDropPodsLetterText;

		// Token: 0x04002AEE RID: 10990
		public string customDropPodsLetterLabel;

		// Token: 0x04002AEF RID: 10991
		public string customCaravanInventoryLetterText;

		// Token: 0x04002AF0 RID: 10992
		public string customCaravanInventoryLetterLabel;

		// Token: 0x04002AF1 RID: 10993
		public LetterDef customLetterDef;

		// Token: 0x04002AF2 RID: 10994
		public bool sendStandardLetter = true;

		// Token: 0x04002AF3 RID: 10995
		private Thing importantLookTarget;

		// Token: 0x04002AF4 RID: 10996
		private List<Thing> tmpThingsToGive = new List<Thing>();
	}
}
