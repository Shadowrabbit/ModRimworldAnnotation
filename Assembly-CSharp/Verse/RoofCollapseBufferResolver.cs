using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x020002F8 RID: 760
	public class RoofCollapseBufferResolver
	{
		// Token: 0x0600138D RID: 5005 RVA: 0x00013FED File Offset: 0x000121ED
		public RoofCollapseBufferResolver(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x000CAC7C File Offset: 0x000C8E7C
		public void CollapseRoofsMarkedToCollapse()
		{
			RoofCollapseBuffer roofCollapseBuffer = this.map.roofCollapseBuffer;
			if (roofCollapseBuffer.CellsMarkedToCollapse.Any<IntVec3>())
			{
				this.tmpCrushedThings.Clear();
				RoofCollapserImmediate.DropRoofInCells(roofCollapseBuffer.CellsMarkedToCollapse, this.map, this.tmpCrushedThings);
				if (this.tmpCrushedThings.Any<Thing>())
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("RoofCollapsed".Translate());
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("TheseThingsCrushed".Translate());
					this.tmpCrushedNames.Clear();
					for (int i = 0; i < this.tmpCrushedThings.Count; i++)
					{
						Thing thing = this.tmpCrushedThings[i];
						Corpse corpse;
						if ((corpse = (thing as Corpse)) == null || !corpse.Bugged)
						{
							string item = thing.LabelShortCap;
							if (thing.def.category == ThingCategory.Pawn)
							{
								item = thing.LabelCap;
							}
							if (!this.tmpCrushedNames.Contains(item))
							{
								this.tmpCrushedNames.Add(item);
							}
						}
					}
					foreach (string str in this.tmpCrushedNames)
					{
						stringBuilder.AppendLine("    -" + str);
					}
					Find.LetterStack.ReceiveLetter("LetterLabelRoofCollapsed".Translate(), stringBuilder.ToString().TrimEndNewlines(), LetterDefOf.NegativeEvent, new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), null, null, null, null);
				}
				else
				{
					Messages.Message("RoofCollapsed".Translate(), new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), MessageTypeDefOf.SilentInput, true);
				}
				this.tmpCrushedThings.Clear();
				roofCollapseBuffer.Clear();
			}
		}

		// Token: 0x04000F7C RID: 3964
		private Map map;

		// Token: 0x04000F7D RID: 3965
		private List<Thing> tmpCrushedThings = new List<Thing>();

		// Token: 0x04000F7E RID: 3966
		private HashSet<string> tmpCrushedNames = new HashSet<string>();
	}
}
