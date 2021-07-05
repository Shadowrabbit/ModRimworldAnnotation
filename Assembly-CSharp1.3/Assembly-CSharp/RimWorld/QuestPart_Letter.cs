using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B85 RID: 2949
	public class QuestPart_Letter : QuestPart
	{
		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x060044F3 RID: 17651 RVA: 0x0016D5E7 File Offset: 0x0016B7E7
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				GlobalTargetInfo globalTargetInfo2 = this.letter.lookTargets.TryGetPrimaryTarget();
				if (globalTargetInfo2.IsValid)
				{
					yield return globalTargetInfo2;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x060044F4 RID: 17652 RVA: 0x0016D5F7 File Offset: 0x0016B7F7
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.letter.relatedFaction != null)
				{
					yield return this.letter.relatedFaction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x0016D608 File Offset: 0x0016B808
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (!string.IsNullOrEmpty(this.getColonistsFromSignal) && signal.tag == this.getColonistsFromSignal)
			{
				NamedArgument namedArgument;
				if (signal.args.TryGetArg("SUBJECT", out namedArgument))
				{
					this.<Notify_QuestSignalReceived>g__ReadPawns|15_0(namedArgument.arg);
				}
				NamedArgument namedArgument2;
				if (signal.args.TryGetArg("SENT", out namedArgument2))
				{
					this.<Notify_QuestSignalReceived>g__ReadPawns|15_0(namedArgument2.arg);
				}
			}
			if (signal.tag == this.inSignal)
			{
				Letter letter = Gen.MemberwiseClone<Letter>(this.letter);
				letter.ID = Find.UniqueIDsManager.GetNextLetterID();
				ChoiceLetter choiceLetter;
				if ((choiceLetter = (letter as ChoiceLetter)) != null)
				{
					choiceLetter.quest = this.quest;
				}
				ChoiceLetter_ChoosePawn choiceLetter_ChoosePawn;
				if ((choiceLetter_ChoosePawn = (letter as ChoiceLetter_ChoosePawn)) != null)
				{
					if (this.useColonistsOnMap != null && this.useColonistsOnMap.HasMap)
					{
						choiceLetter_ChoosePawn.pawns.Clear();
						choiceLetter_ChoosePawn.pawns.AddRange(this.useColonistsOnMap.Map.mapPawns.FreeColonists);
						choiceLetter_ChoosePawn.chosenPawnSignal = this.chosenPawnSignal;
					}
					Caravan caravan;
					if (this.useColonistsFromCaravanArg && signal.args.TryGetArg<Caravan>("CARAVAN", out caravan) && caravan != null)
					{
						choiceLetter_ChoosePawn.pawns.Clear();
						choiceLetter_ChoosePawn.pawns.AddRange(from x in caravan.PawnsListForReading
						where x.IsFreeColonist
						select x);
						choiceLetter_ChoosePawn.chosenPawnSignal = this.chosenPawnSignal;
					}
					if (!string.IsNullOrEmpty(this.getColonistsFromSignal))
					{
						this.colonistsFromSignal.RemoveAll((Pawn x) => x.Dead);
						choiceLetter_ChoosePawn.pawns.Clear();
						choiceLetter_ChoosePawn.pawns.AddRange(this.colonistsFromSignal);
						choiceLetter_ChoosePawn.chosenPawnSignal = this.chosenPawnSignal;
					}
				}
				ChoiceLetter_AcceptVisitors choiceLetter_AcceptVisitors;
				if ((choiceLetter_AcceptVisitors = (letter as ChoiceLetter_AcceptVisitors)) != null)
				{
					choiceLetter_AcceptVisitors.acceptedSignal = this.acceptedVisitorsSignal;
					if (this.visitors != null)
					{
						choiceLetter_AcceptVisitors.pawns.AddRange(this.visitors);
					}
				}
				LookTargets lookTargets;
				if (this.getLookTargetsFromSignal && !letter.lookTargets.IsValid() && SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets))
				{
					letter.lookTargets = lookTargets;
				}
				letter.label = signal.args.GetFormattedText(letter.label);
				ChoiceLetter choiceLetter2 = letter as ChoiceLetter;
				bool flag = true;
				if (choiceLetter2 != null)
				{
					choiceLetter2.title = signal.args.GetFormattedText(choiceLetter2.title);
					choiceLetter2.text = signal.args.GetFormattedText(choiceLetter2.text);
					if (choiceLetter2.text.NullOrEmpty())
					{
						flag = false;
					}
				}
				if (this.filterDeadPawnsFromLookTargets)
				{
					for (int i = letter.lookTargets.targets.Count - 1; i >= 0; i--)
					{
						Thing thing = letter.lookTargets.targets[i].Thing;
						Pawn pawn = thing as Pawn;
						if (pawn != null && pawn.Dead)
						{
							letter.lookTargets.targets.Remove(thing);
						}
					}
				}
				if (flag)
				{
					Find.LetterStack.ReceiveLetter(letter, null);
				}
			}
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x0016D954 File Offset: 0x0016BB54
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Deep.Look<Letter>(ref this.letter, "letter", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", true, false);
			Scribe_References.Look<MapParent>(ref this.useColonistsOnMap, "useColonistsOnMap", false);
			Scribe_Values.Look<bool>(ref this.useColonistsFromCaravanArg, "useColonistsFromCaravanArg", false, false);
			Scribe_Values.Look<string>(ref this.chosenPawnSignal, "chosenPawnSignal", null, false);
			Scribe_Values.Look<bool>(ref this.filterDeadPawnsFromLookTargets, "filterDeadPawnsFromLookTargets", false, false);
			Scribe_Values.Look<string>(ref this.getColonistsFromSignal, "getColonistsFromSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.colonistsFromSignal, "colonistsFromSignal", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.colonistsFromSignal.RemoveAll((Pawn x) => x == null);
			}
			Scribe_Values.Look<string>(ref this.acceptedVisitorsSignal, "acceptedVisitorsSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.visitors, "visitors", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.visitors != null)
			{
				this.visitors.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x0016DAA8 File Offset: 0x0016BCA8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.letter = LetterMaker.MakeLetter("Dev: Test", "Test text", LetterDefOf.PositiveEvent, null, null);
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x0016DB18 File Offset: 0x0016BD18
		[CompilerGenerated]
		private void <Notify_QuestSignalReceived>g__ReadPawns|15_0(object obj)
		{
			Pawn item;
			if ((item = (obj as Pawn)) != null && !this.colonistsFromSignal.Contains(item))
			{
				this.colonistsFromSignal.Add(item);
			}
			List<Pawn> source;
			if ((source = (obj as List<Pawn>)) != null)
			{
				this.colonistsFromSignal.AddRange(from p in source
				where !this.colonistsFromSignal.Contains(p)
				select p);
			}
			List<Thing> source2;
			if ((source2 = (obj as List<Thing>)) != null)
			{
				this.colonistsFromSignal.AddRange(from Pawn p in 
					from t in source2
					where t is Pawn
					select t
				where !this.colonistsFromSignal.Contains(p)
				select p);
			}
		}

		// Token: 0x040029D6 RID: 10710
		public string inSignal;

		// Token: 0x040029D7 RID: 10711
		public Letter letter;

		// Token: 0x040029D8 RID: 10712
		public bool getLookTargetsFromSignal = true;

		// Token: 0x040029D9 RID: 10713
		public MapParent useColonistsOnMap;

		// Token: 0x040029DA RID: 10714
		public string getColonistsFromSignal;

		// Token: 0x040029DB RID: 10715
		public bool useColonistsFromCaravanArg;

		// Token: 0x040029DC RID: 10716
		public string chosenPawnSignal;

		// Token: 0x040029DD RID: 10717
		public string acceptedVisitorsSignal;

		// Token: 0x040029DE RID: 10718
		public List<Pawn> visitors;

		// Token: 0x040029DF RID: 10719
		public bool filterDeadPawnsFromLookTargets;

		// Token: 0x040029E0 RID: 10720
		private List<Pawn> colonistsFromSignal = new List<Pawn>();
	}
}
