using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BC3 RID: 3011
	public class QuestPart_GiveToCaravan : QuestPart
	{
		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x0600467B RID: 18043 RVA: 0x00174C85 File Offset: 0x00172E85
		// (set) Token: 0x0600467C RID: 18044 RVA: 0x00174CA0 File Offset: 0x00172EA0
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

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x0600467D RID: 18045 RVA: 0x00174D20 File Offset: 0x00172F20
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

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x0600467E RID: 18046 RVA: 0x00174D30 File Offset: 0x00172F30
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, true, false);
			}
		}

		// Token: 0x0600467F RID: 18047 RVA: 0x00174D40 File Offset: 0x00172F40
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

		// Token: 0x06004680 RID: 18048 RVA: 0x00174E54 File Offset: 0x00173054
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

		// Token: 0x06004681 RID: 18049 RVA: 0x00174EA0 File Offset: 0x001730A0
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06004682 RID: 18050 RVA: 0x00174EB0 File Offset: 0x001730B0
		public override void Cleanup()
		{
			base.Cleanup();
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].Destroy(DestroyMode.Vanish);
			}
			this.items.Clear();
		}

		// Token: 0x06004683 RID: 18051 RVA: 0x00174EF8 File Offset: 0x001730F8
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

		// Token: 0x06004684 RID: 18052 RVA: 0x00174FB8 File Offset: 0x001731B8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x06004685 RID: 18053 RVA: 0x00174FDA File Offset: 0x001731DA
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04002AFF RID: 11007
		public string inSignal;

		// Token: 0x04002B00 RID: 11008
		public Caravan caravan;

		// Token: 0x04002B01 RID: 11009
		private List<Thing> items = new List<Thing>();

		// Token: 0x04002B02 RID: 11010
		private List<Pawn> pawns = new List<Pawn>();
	}
}
