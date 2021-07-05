using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001141 RID: 4417
	public class CompGiveThoughtToAllMapPawnsOnDestroy : ThingComp
	{
		// Token: 0x17001227 RID: 4647
		// (get) Token: 0x06006A0F RID: 27151 RVA: 0x0023B317 File Offset: 0x00239517
		private CompProperties_GiveThoughtToAllMapPawnsOnDestroy Props
		{
			get
			{
				return (CompProperties_GiveThoughtToAllMapPawnsOnDestroy)this.props;
			}
		}

		// Token: 0x06006A10 RID: 27152 RVA: 0x0023B324 File Offset: 0x00239524
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (previousMap != null)
			{
				if (!this.Props.message.NullOrEmpty())
				{
					Messages.Message(this.Props.message, new TargetInfo(this.parent.Position, previousMap, false), MessageTypeDefOf.NegativeEvent, true);
				}
				foreach (Pawn pawn in previousMap.mapPawns.AllPawnsSpawned)
				{
					Pawn_NeedsTracker needs = pawn.needs;
					if (needs != null)
					{
						Need_Mood mood = needs.mood;
						if (mood != null)
						{
							mood.thoughts.memories.TryGainMemory(this.Props.thought, null, null);
						}
					}
				}
			}
		}
	}
}
