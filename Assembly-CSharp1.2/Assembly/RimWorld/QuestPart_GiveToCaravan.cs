using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001143 RID: 4419
	public class QuestPart_GiveToCaravan : QuestPart
	{
		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x060060E7 RID: 24807 RVA: 0x00042CAE File Offset: 0x00040EAE
		// (set) Token: 0x060060E8 RID: 24808 RVA: 0x001E6180 File Offset: 0x001E4380
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

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x060060E9 RID: 24809 RVA: 0x00042CC6 File Offset: 0x00040EC6
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.caravan != null)
				{
					yield return this.caravan;
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

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x060060EA RID: 24810 RVA: 0x00042CD6 File Offset: 0x00040ED6
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, true, false);
			}
		}

		// Token: 0x060060EB RID: 24811 RVA: 0x001E6200 File Offset: 0x001E4400
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				Caravan caravan = this.caravan;
				if (caravan == null)
				{
					signal.args.TryGetArg<Caravan>("CARAVAN", out caravan);
				}
				if (caravan != null && this.Things.Any<Thing>())
				{
					for (int i = 0; i < this.pawns.Count; i++)
					{
						if (this.pawns[i].Faction != Faction.OfPlayer)
						{
							this.pawns[i].SetFaction(Faction.OfPlayer, null);
						}
						caravan.AddPawn(this.pawns[i], true);
					}
					for (int j = 0; j < this.items.Count; j++)
					{
						CaravanInventoryUtility.GiveThing(caravan, this.items[j]);
					}
					this.items.Clear();
				}
			}
		}

		// Token: 0x060060EC RID: 24812 RVA: 0x001E6314 File Offset: 0x001E4514
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

		// Token: 0x060060ED RID: 24813 RVA: 0x00042CE5 File Offset: 0x00040EE5
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x060060EE RID: 24814 RVA: 0x001E6360 File Offset: 0x001E4560
		public override void Cleanup()
		{
			base.Cleanup();
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].Destroy(DestroyMode.Vanish);
			}
			this.items.Clear();
		}

		// Token: 0x060060EF RID: 24815 RVA: 0x001E63A8 File Offset: 0x001E45A8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Caravan>(ref this.caravan, "caravan", false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.items.RemoveAll((Thing x) => x == null);
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060060F0 RID: 24816 RVA: 0x00042CF3 File Offset: 0x00040EF3
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x060060F1 RID: 24817 RVA: 0x00042D15 File Offset: 0x00040F15
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x040040C3 RID: 16579
		public string inSignal;

		// Token: 0x040040C4 RID: 16580
		public Caravan caravan;

		// Token: 0x040040C5 RID: 16581
		private List<Thing> items = new List<Thing>();

		// Token: 0x040040C6 RID: 16582
		private List<Pawn> pawns = new List<Pawn>();
	}
}
