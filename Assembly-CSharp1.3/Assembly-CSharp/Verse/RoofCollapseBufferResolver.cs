using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000211 RID: 529
	public class RoofCollapseBufferResolver
	{
		// Token: 0x06000F21 RID: 3873 RVA: 0x00055B6E File Offset: 0x00053D6E
		public RoofCollapseBufferResolver(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x00055B94 File Offset: 0x00053D94
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

		// Token: 0x04000C0C RID: 3084
		private Map map;

		// Token: 0x04000C0D RID: 3085
		private List<Thing> tmpCrushedThings = new List<Thing>();

		// Token: 0x04000C0E RID: 3086
		private HashSet<string> tmpCrushedNames = new HashSet<string>();
	}
}
