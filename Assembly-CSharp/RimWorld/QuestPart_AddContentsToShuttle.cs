using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001094 RID: 4244
	public class QuestPart_AddContentsToShuttle : QuestPart
	{
		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06005C71 RID: 23665 RVA: 0x0004028B File Offset: 0x0003E48B
		// (set) Token: 0x06005C72 RID: 23666 RVA: 0x001DA728 File Offset: 0x001D8928
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
							Log.Error("Tried to add a destroyed thing to QuestPart_AddContentsToShuttle: " + thing.ToStringSafe<Thing>(), false);
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

		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06005C73 RID: 23667 RVA: 0x000402A3 File Offset: 0x0003E4A3
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

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06005C74 RID: 23668 RVA: 0x000402B3 File Offset: 0x0003E4B3
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

		// Token: 0x06005C75 RID: 23669 RVA: 0x001DA7C8 File Offset: 0x001D89C8
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

		// Token: 0x06005C76 RID: 23670 RVA: 0x000402C3 File Offset: 0x0003E4C3
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06005C77 RID: 23671 RVA: 0x000402D1 File Offset: 0x0003E4D1
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x06005C78 RID: 23672 RVA: 0x001DA8D0 File Offset: 0x001D8AD0
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

		// Token: 0x06005C79 RID: 23673 RVA: 0x001DA92C File Offset: 0x001D8B2C
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

		// Token: 0x06005C7A RID: 23674 RVA: 0x000402E1 File Offset: 0x0003E4E1
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04003DEB RID: 15851
		public string inSignal;

		// Token: 0x04003DEC RID: 15852
		public Thing shuttle;

		// Token: 0x04003DED RID: 15853
		private List<Thing> items = new List<Thing>();

		// Token: 0x04003DEE RID: 15854
		private List<Pawn> pawns = new List<Pawn>();
	}
}
