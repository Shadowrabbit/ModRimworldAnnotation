using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E6 RID: 4326
	public class QuestPart_Letter : QuestPart
	{
		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x06005E6B RID: 24171 RVA: 0x00041646 File Offset: 0x0003F846
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

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x06005E6C RID: 24172 RVA: 0x00041656 File Offset: 0x0003F856
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

		// Token: 0x06005E6D RID: 24173 RVA: 0x001DF4C0 File Offset: 0x001DD6C0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (!string.IsNullOrEmpty(this.getColonistsFromSignal) && signal.tag == this.getColonistsFromSignal)
			{
				NamedArgument namedArgument;
				if (signal.args.TryGetArg("SUBJECT", out namedArgument))
				{
					this.<Notify_QuestSignalReceived>g__ReadPawns|13_0(namedArgument.arg);
				}
				NamedArgument namedArgument2;
				if (signal.args.TryGetArg("SENT", out namedArgument2))
				{
					this.<Notify_QuestSignalReceived>g__ReadPawns|13_0(namedArgument2.arg);
				}
			}
			if (signal.tag == this.inSignal)
			{
				Letter letter = Gen.MemberwiseClone<Letter>(this.letter);
				letter.ID = Find.UniqueIDsManager.GetNextLetterID();
				ChoiceLetter choiceLetter = letter as ChoiceLetter;
				if (choiceLetter != null)
				{
					choiceLetter.quest = this.quest;
				}
				ChoiceLetter_ChoosePawn choiceLetter_ChoosePawn = letter as ChoiceLetter_ChoosePawn;
				if (choiceLetter_ChoosePawn != null)
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

		// Token: 0x06005E6E RID: 24174 RVA: 0x001DF7DC File Offset: 0x001DD9DC
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
		}

		// Token: 0x06005E6F RID: 24175 RVA: 0x001DF8CC File Offset: 0x001DDACC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.letter = LetterMaker.MakeLetter("Dev: Test", "Test text", LetterDefOf.PositiveEvent, null, null);
		}

		// Token: 0x06005E73 RID: 24179 RVA: 0x001DF920 File Offset: 0x001DDB20
		[CompilerGenerated]
		private void <Notify_QuestSignalReceived>g__ReadPawns|13_0(object obj)
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

		// Token: 0x04003F1C RID: 16156
		public string inSignal;

		// Token: 0x04003F1D RID: 16157
		public Letter letter;

		// Token: 0x04003F1E RID: 16158
		public bool getLookTargetsFromSignal = true;

		// Token: 0x04003F1F RID: 16159
		public MapParent useColonistsOnMap;

		// Token: 0x04003F20 RID: 16160
		public string getColonistsFromSignal;

		// Token: 0x04003F21 RID: 16161
		public bool useColonistsFromCaravanArg;

		// Token: 0x04003F22 RID: 16162
		public string chosenPawnSignal;

		// Token: 0x04003F23 RID: 16163
		public bool filterDeadPawnsFromLookTargets;

		// Token: 0x04003F24 RID: 16164
		private List<Pawn> colonistsFromSignal = new List<Pawn>();
	}
}
