using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017CB RID: 6091
	public class CompGiveThoughtToAllMapPawnsOnDestroy : ThingComp
	{
		// Token: 0x170014E0 RID: 5344
		// (get) Token: 0x060086B5 RID: 34485 RVA: 0x0005A607 File Offset: 0x00058807
		private CompProperties_GiveThoughtToAllMapPawnsOnDestroy Props
		{
			get
			{
				return (CompProperties_GiveThoughtToAllMapPawnsOnDestroy)this.props;
			}
		}

		// Token: 0x060086B6 RID: 34486 RVA: 0x00279840 File Offset: 0x00277A40
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
							mood.thoughts.memories.TryGainMemory(this.Props.thought, null);
						}
					}
				}
			}
		}
	}
}
