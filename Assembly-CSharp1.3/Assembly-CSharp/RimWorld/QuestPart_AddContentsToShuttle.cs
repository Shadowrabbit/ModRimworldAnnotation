using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B5E RID: 2910
	public class QuestPart_AddContentsToShuttle : QuestPart
	{
		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x06004404 RID: 17412 RVA: 0x001699F2 File Offset: 0x00167BF2
		// (set) Token: 0x06004405 RID: 17413 RVA: 0x00169A0C File Offset: 0x00167C0C
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
							Log.Error("Tried to add a destroyed thing to QuestPart_AddContentsToShuttle: " + thing.ToStringSafe<Thing>());
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

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x06004406 RID: 17414 RVA: 0x00169AAC File Offset: 0x00167CAC
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

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x06004407 RID: 17415 RVA: 0x00169ABC File Offset: 0x00167CBC
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
				yield break;
				yield break;
			}
		}

		// Token: 0x06004408 RID: 17416 RVA: 0x00169ACC File Offset: 0x00167CCC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.shuttle != null)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				this.items.RemoveAll((Thing x) => x.Destroyed);
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.pawns[i].IsWorldPawn())
					{
						Find.WorldPawns.RemovePawn(this.pawns[i]);
					}
				}
				CompTransporter compTransporter = this.shuttle.TryGetComp<CompTransporter>();
				compTransporter.innerContainer.TryAddRangeOrTransfer(this.pawns, true, false);
				compTransporter.innerContainer.TryAddRangeOrTransfer(this.items, true, false);
				this.items.Clear();
			}
		}

		// Token: 0x06004409 RID: 17417 RVA: 0x00169BD1 File Offset: 0x00167DD1
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x00169BDF File Offset: 0x00167DDF
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x00169BF0 File Offset: 0x00167DF0
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

		// Token: 0x0600440C RID: 17420 RVA: 0x00169C4C File Offset: 0x00167E4C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.items.RemoveAll((Thing x) => x == null);
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x00169D0C File Offset: 0x00167F0C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04002949 RID: 10569
		public string inSignal;

		// Token: 0x0400294A RID: 10570
		public Thing shuttle;

		// Token: 0x0400294B RID: 10571
		private List<Thing> items = new List<Thing>();

		// Token: 0x0400294C RID: 10572
		private List<Pawn> pawns = new List<Pawn>();
	}
}
