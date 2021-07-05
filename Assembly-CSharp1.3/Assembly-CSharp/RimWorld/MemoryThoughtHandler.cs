using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8E RID: 3726
	public sealed class MemoryThoughtHandler : IExposable
	{
		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x0600576C RID: 22380 RVA: 0x001DC170 File Offset: 0x001DA370
		public List<Thought_Memory> Memories
		{
			get
			{
				return this.memories;
			}
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x001DC178 File Offset: 0x001DA378
		public MemoryThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600576E RID: 22382 RVA: 0x001DC1A0 File Offset: 0x001DA3A0
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpMemories.Clear();
				for (int i = this.memories.Count - 1; i >= 0; i--)
				{
					if (!this.memories[i].Save)
					{
						this.tmpMemories.Add(this.memories[i]);
						this.memories.Remove(this.memories[i]);
					}
				}
			}
			Scribe_Collections.Look<Thought_Memory>(ref this.memories, "memories", LookMode.Deep, Array.Empty<object>());
			foreach (Thought_Memory item in this.tmpMemories)
			{
				this.memories.Add(item);
			}
			this.tmpMemories.Clear();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int j = this.memories.Count - 1; j >= 0; j--)
				{
					if (this.memories[j].def == null)
					{
						this.memories.RemoveAt(j);
					}
					else
					{
						this.memories[j].pawn = this.pawn;
					}
				}
			}
		}

		// Token: 0x0600576F RID: 22383 RVA: 0x001DC2E0 File Offset: 0x001DA4E0
		public void MemoryThoughtInterval()
		{
			for (int i = 0; i < this.memories.Count; i++)
			{
				this.memories[i].ThoughtInterval();
			}
			this.RemoveExpiredMemories();
		}

		// Token: 0x06005770 RID: 22384 RVA: 0x001DC31C File Offset: 0x001DA51C
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
						this.TryGainMemory(thought_Memory.def.nextThought, null, null);
					}
				}
			}
		}

		// Token: 0x06005771 RID: 22385 RVA: 0x001DC380 File Offset: 0x001DA580
		public void TryGainMemoryFast(ThoughtDef mem, Precept sourcePrecept = null)
		{
			Thought_Memory firstMemoryOfDef = this.GetFirstMemoryOfDef(mem);
			if (firstMemoryOfDef != null)
			{
				firstMemoryOfDef.Renew();
				return;
			}
			this.TryGainMemory(mem, null, sourcePrecept);
		}

		// Token: 0x06005772 RID: 22386 RVA: 0x001DC3A8 File Offset: 0x001DA5A8
		public void TryGainMemory(ThoughtDef def, Pawn otherPawn = null, Precept sourcePrecept = null)
		{
			if (!def.IsMemory)
			{
				Log.Error(def + " is not a memory thought.");
				return;
			}
			this.TryGainMemory(ThoughtMaker.MakeThought(def, sourcePrecept), otherPawn);
		}

		// Token: 0x06005773 RID: 22387 RVA: 0x001DC3D4 File Offset: 0x001DA5D4
		public void TryGainMemory(Thought_Memory newThought, Pawn otherPawn = null)
		{
			if (!ThoughtUtility.CanGetThought(this.pawn, newThought.def, false))
			{
				return;
			}
			if (newThought is Thought_MemorySocial && newThought.otherPawn == null && otherPawn == null)
			{
				Log.Error("Can't gain social thought " + newThought.def + " because its otherPawn is null and otherPawn passed to this method is also null. Social thoughts must have otherPawn.");
				return;
			}
			newThought.pawn = this.pawn;
			newThought.otherPawn = (otherPawn ?? newThought.otherPawn);
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
				this.TryGainMemory(newThought.def.thoughtToMake, newThought.otherPawn, null);
			}
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i] != newThought && this.memories[i].GroupsWith(newThought))
				{
					this.memories[i].Notify_NewThoughtInGroupAdded(newThought);
				}
			}
			if (flag && newThought.def.showBubble && this.pawn.Spawned && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				MoteMaker.MakeMoodThoughtBubble(this.pawn, newThought);
			}
		}

		// Token: 0x06005774 RID: 22388 RVA: 0x001DC56C File Offset: 0x001DA76C
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

		// Token: 0x06005775 RID: 22389 RVA: 0x001DC5C0 File Offset: 0x001DA7C0
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

		// Token: 0x06005776 RID: 22390 RVA: 0x001DC614 File Offset: 0x001DA814
		public void RemoveMemory(Thought_Memory th)
		{
			if (!this.memories.Remove(th))
			{
				Log.Warning("Tried to remove memory thought of def " + th.def.defName + " but it's not here.");
			}
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x001DC644 File Offset: 0x001DA844
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

		// Token: 0x06005778 RID: 22392 RVA: 0x001DC684 File Offset: 0x001DA884
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

		// Token: 0x06005779 RID: 22393 RVA: 0x001DC6C4 File Offset: 0x001DA8C4
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

		// Token: 0x0600577A RID: 22394 RVA: 0x001DC70C File Offset: 0x001DA90C
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

		// Token: 0x0600577B RID: 22395 RVA: 0x001DC764 File Offset: 0x001DA964
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

		// Token: 0x0600577C RID: 22396 RVA: 0x001DC7B8 File Offset: 0x001DA9B8
		public void RemoveMemoriesOfDef(ThoughtDef def)
		{
			if (!def.IsMemory)
			{
				Log.Warning(def + " is not a memory thought.");
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

		// Token: 0x0600577D RID: 22397 RVA: 0x001DC82C File Offset: 0x001DAA2C
		public void RemoveMemoriesOfDefIf(ThoughtDef def, Func<Thought_Memory, bool> predicate)
		{
			if (!def.IsMemory)
			{
				Log.Warning(def + " is not a memory thought.");
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

		// Token: 0x0600577E RID: 22398 RVA: 0x001DC8A8 File Offset: 0x001DAAA8
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

		// Token: 0x0600577F RID: 22399 RVA: 0x001DC8E2 File Offset: 0x001DAAE2
		public void Notify_PawnDiscarded(Pawn discarded)
		{
			this.RemoveMemoriesWhereOtherPawnIs(discarded);
		}

		// Token: 0x040033AE RID: 13230
		public Pawn pawn;

		// Token: 0x040033AF RID: 13231
		private List<Thought_Memory> memories = new List<Thought_Memory>();

		// Token: 0x040033B0 RID: 13232
		private List<Thought_Memory> tmpMemories = new List<Thought_Memory>();
	}
}
