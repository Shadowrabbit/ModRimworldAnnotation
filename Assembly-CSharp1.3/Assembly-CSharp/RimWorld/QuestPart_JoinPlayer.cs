using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BC4 RID: 3012
	public class QuestPart_JoinPlayer : QuestPart
	{
		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x06004688 RID: 18056 RVA: 0x00175008 File Offset: 0x00173208
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

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x06004689 RID: 18057 RVA: 0x00175018 File Offset: 0x00173218
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, this.makePrisoners);
			}
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x00175034 File Offset: 0x00173234
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				if (this.joinPlayer)
				{
					for (int i = 0; i < this.pawns.Count; i++)
					{
						if (this.pawns[i].Faction != Faction.OfPlayer)
						{
							this.pawns[i].SetFaction(Faction.OfPlayer, null);
						}
					}
					return;
				}
				if (this.makePrisoners)
				{
					for (int j = 0; j < this.pawns.Count; j++)
					{
						if (this.pawns[j].RaceProps.Humanlike)
						{
							if (!this.pawns[j].IsPrisonerOfColony)
							{
								this.pawns[j].guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Prisoner);
							}
							HealthUtility.TryAnesthetize(this.pawns[j]);
						}
					}
				}
			}
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x0017514E File Offset: 0x0017334E
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x0017515C File Offset: 0x0017335C
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x0017516C File Offset: 0x0017336C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.joinPlayer, "joinPlayer", false, false);
			Scribe_Values.Look<bool>(ref this.makePrisoners, "makePrisoners", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04002B03 RID: 11011
		public string inSignal;

		// Token: 0x04002B04 RID: 11012
		public string outSignalResult;

		// Token: 0x04002B05 RID: 11013
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04002B06 RID: 11014
		public bool joinPlayer;

		// Token: 0x04002B07 RID: 11015
		public bool makePrisoners;

		// Token: 0x04002B08 RID: 11016
		public MapParent mapParent;
	}
}
