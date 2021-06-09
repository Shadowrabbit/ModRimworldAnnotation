using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001146 RID: 4422
	public class QuestPart_JoinPlayer : QuestPart
	{
		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x06006103 RID: 24835 RVA: 0x00042DBE File Offset: 0x00040FBE
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

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x06006104 RID: 24836 RVA: 0x00042DCE File Offset: 0x00040FCE
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, this.makePrisoners);
			}
		}

		// Token: 0x06006105 RID: 24837 RVA: 0x001E6664 File Offset: 0x001E4864
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
								this.pawns[j].guest.SetGuestStatus(Faction.OfPlayer, true);
							}
							HealthUtility.TryAnesthetize(this.pawns[j]);
						}
					}
				}
			}
		}

		// Token: 0x06006106 RID: 24838 RVA: 0x00042DE7 File Offset: 0x00040FE7
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06006107 RID: 24839 RVA: 0x00042DF5 File Offset: 0x00040FF5
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x06006108 RID: 24840 RVA: 0x001E6780 File Offset: 0x001E4980
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

		// Token: 0x040040D1 RID: 16593
		public string inSignal;

		// Token: 0x040040D2 RID: 16594
		public string outSignalResult;

		// Token: 0x040040D3 RID: 16595
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040040D4 RID: 16596
		public bool joinPlayer;

		// Token: 0x040040D5 RID: 16597
		public bool makePrisoners;

		// Token: 0x040040D6 RID: 16598
		public MapParent mapParent;
	}
}
