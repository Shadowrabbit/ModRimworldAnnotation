using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001558 RID: 5464
	public sealed class MemoryThoughtHandler : IExposable
	{
		// Token: 0x17001256 RID: 4694
		// (get) Token: 0x06007675 RID: 30325 RVA: 0x0004FE5B File Offset: 0x0004E05B
		public List<Thought_Memory> Memories
		{
			get
			{
				return this.memories;
			}
		}

		// Token: 0x06007676 RID: 30326 RVA: 0x0004FE63 File Offset: 0x0004E063
		public MemoryThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06007677 RID: 30327 RVA: 0x00241734 File Offset: 0x0023F934
		public void ExposeData()
		{
			Scribe_Collections.Look<Thought_Memory>(ref this.memories, "memories", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = this.memories.Count - 1; i >= 0; i--)
				{
					if (this.memories[i].def == null)
					{
						this.memories.RemoveAt(i);
					}
					else
					{
						this.memories[i].pawn = this.pawn;
					}
				}
			}
		}

		// Token: 0x06007678 RID: 30328 RVA: 0x002417B0 File Offset: 0x0023F9B0
		public void MemoryThoughtInterval()
		{
			for (int i = 0; i < this.memories.Count; i++)
			{
				this.memories[i].ThoughtInterval();
			}
			this.RemoveExpiredMemories();
		}

		// Token: 0x06007679 RID: 30329 RVA: 0x002417EC File Offset: 0x0023F9EC
		private void RemoveExpiredMemories()
		{
			for (int i = this.memories.Count - 1; i >= 0; i--)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.ShouldDiscard)
				{
					this.RemoveMemory(thought_Memory);
					if (thought_Memory.def.nextThought != null)
					{
						this.TryGainMemory(thought_Memory.def.nextThought, null);
					}
				}
			}
		}

		// Token: 0x0600767A RID: 30330 RVA: 0x0024184C File Offset: 0x0023FA4C
		public void TryGainMemoryFast(ThoughtDef mem)
		{
			Thought_Memory firstMemoryOfDef = this.GetFirstMemoryOfDef(mem);
			if (firstMemoryOfDef != null)
			{
				firstMemoryOfDef.Renew();
				return;
			}
			this.TryGainMemory(mem, null);
		}

		// Token: 0x0600767B RID: 30331 RVA: 0x0004FE7D File Offset: 0x0004E07D
		public void TryGainMemory(ThoughtDef def, Pawn otherPawn = null)
		{
			if (!def.IsMemory)
			{
				Log.Error(def + " is not a memory thought.", false);
				return;
			}
			this.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(def), otherPawn);
		}

		// Token: 0x0600767C RID: 30332 RVA: 0x00241874 File Offset: 0x0023FA74
		public void TryGainMemory(Thought_Memory newThought, Pawn otherPawn = null)
		{
			if (!ThoughtUtility.CanGetThought_NewTemp(this.pawn, newThought.def, false))
			{
				return;
			}
			if (newThought is Thought_MemorySocial && newThought.otherPawn == null && otherPawn == null)
			{
				Log.Error("Can't gain social thought " + newThought.def + " because its otherPawn is null and otherPawn passed to this method is also null. Social thoughts must have otherPawn.", false);
				return;
			}
			newThought.pawn = this.pawn;
			newThought.otherPawn = otherPawn;
			bool flag;
			if (!newThought.TryMergeWithExistingMemory(out flag))
			{
				this.memories.Add(newThought);
			}
			if (newThought.def.stackLimitForSameOtherPawn >= 0)
			{
				while (this.NumMemoriesInGroup(newThought) > newThought.def.stackLimitForSameOtherPawn)
				{
					this.RemoveMemory(this.OldestMemoryInGroup(newThought));
				}
			}
			if (newThought.def.stackLimit >= 0)
			{
				while (this.NumMemoriesOfDef(newThought.def) > newThought.def.stackLimit)
				{
					this.RemoveMemory(this.OldestMemoryOfDef(newThought.def));
				}
			}
			if (newThought.def.thoughtToMake != null)
			{
				this.TryGainMemory(newThought.def.thoughtToMake, newThought.otherPawn);
			}
			if (flag && newThought.def.showBubble && this.pawn.Spawned && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				MoteMaker.MakeMoodThoughtBubble(this.pawn, newThought);
			}
		}

		// Token: 0x0600767D RID: 30333 RVA: 0x002419B8 File Offset: 0x0023FBB8
		public Thought_Memory OldestMemoryInGroup(Thought_Memory group)
		{
			Thought_Memory result = null;
			int num = -9999;
			for (int i = 0; i < this.memories.Count; i++)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.GroupsWith(group) && thought_Memory.age > num)
				{
					result = thought_Memory;
					num = thought_Memory.age;
				}
			}
			return result;
		}

		// Token: 0x0600767E RID: 30334 RVA: 0x00241A0C File Offset: 0x0023FC0C
		public Thought_Memory OldestMemoryOfDef(ThoughtDef def)
		{
			Thought_Memory result = null;
			int num = -9999;
			for (int i = 0; i < this.memories.Count; i++)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.def == def && thought_Memory.age > num)
				{
					result = thought_Memory;
					num = thought_Memory.age;
				}
			}
			return result;
		}

		// Token: 0x0600767F RID: 30335 RVA: 0x0004FEAB File Offset: 0x0004E0AB
		public void RemoveMemory(Thought_Memory th)
		{
			if (!this.memories.Remove(th))
			{
				Log.Warning("Tried to remove memory thought of def " + th.def.defName + " but it's not here.", false);
			}
		}

		// Token: 0x06007680 RID: 30336 RVA: 0x00241A60 File Offset: 0x0023FC60
		public int NumMemoriesInGroup(Thought_Memory group)
		{
			int num = 0;
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].GroupsWith(group))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06007681 RID: 30337 RVA: 0x00241AA0 File Offset: 0x0023FCA0
		public int NumMemoriesOfDef(ThoughtDef def)
		{
			int num = 0;
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].def == def)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06007682 RID: 30338 RVA: 0x00241AE0 File Offset: 0x0023FCE0
		public Thought_Memory GetFirstMemoryOfDef(ThoughtDef def)
		{
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].def == def)
				{
					return this.memories[i];
				}
			}
			return null;
		}

		// Token: 0x06007683 RID: 30339 RVA: 0x00241B28 File Offset: 0x0023FD28
		public void RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef def, Pawn otherPawn)
		{
			Predicate<Thought_Memory> <>9__0;
			for (;;)
			{
				List<Thought_Memory> list = this.memories;
				Predicate<Thought_Memory> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Thought_Memory x) => x.def == def && x.otherPawn == otherPawn));
				}
				Thought_Memory thought_Memory = list.Find(match);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x06007684 RID: 30340 RVA: 0x00241B80 File Offset: 0x0023FD80
		public void RemoveMemoriesWhereOtherPawnIs(Pawn otherPawn)
		{
			Predicate<Thought_Memory> <>9__0;
			for (;;)
			{
				List<Thought_Memory> list = this.memories;
				Predicate<Thought_Memory> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Thought_Memory x) => x.otherPawn == otherPawn));
				}
				Thought_Memory thought_Memory = list.Find(match);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x06007685 RID: 30341 RVA: 0x00241BD4 File Offset: 0x0023FDD4
		public void RemoveMemoriesOfDef(ThoughtDef def)
		{
			if (!def.IsMemory)
			{
				Log.Warning(def + " is not a memory thought.", false);
				return;
			}
			Predicate<Thought_Memory> <>9__0;
			for (;;)
			{
				List<Thought_Memory> list = this.memories;
				Predicate<Thought_Memory> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Thought_Memory x) => x.def == def));
				}
				Thought_Memory thought_Memory = list.Find(match);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x06007686 RID: 30342 RVA: 0x00241C4C File Offset: 0x0023FE4C
		public void RemoveMemoriesOfDefIf(ThoughtDef def, Func<Thought_Memory, bool> predicate)
		{
			if (!def.IsMemory)
			{
				Log.Warning(def + " is not a memory thought.", false);
				return;
			}
			Predicate<Thought_Memory> <>9__0;
			for (;;)
			{
				List<Thought_Memory> list = this.memories;
				Predicate<Thought_Memory> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Thought_Memory x) => x.def == def && predicate(x)));
				}
				Thought_Memory thought_Memory = list.Find(match);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x06007687 RID: 30343 RVA: 0x00241CC8 File Offset: 0x0023FEC8
		public bool AnyMemoryConcerns(Pawn otherPawn)
		{
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].otherPawn == otherPawn)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007688 RID: 30344 RVA: 0x0004FEDB File Offset: 0x0004E0DB
		public void Notify_PawnDiscarded(Pawn discarded)
		{
			this.RemoveMemoriesWhereOtherPawnIs(discarded);
		}

		// Token: 0x04004E35 RID: 20021
		public Pawn pawn;

		// Token: 0x04004E36 RID: 20022
		private List<Thought_Memory> memories = new List<Thought_Memory>();
	}
}
