using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001269 RID: 4713
	public class Alert_TimedMakeFactionHostile : Alert
	{
		// Token: 0x170013AE RID: 5038
		// (get) Token: 0x060070DA RID: 28890 RVA: 0x002599F8 File Offset: 0x00257BF8
		private List<GlobalTargetInfo> WorldObjects
		{
			get
			{
				this.worldObjectsResult.Clear();
				foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjects)
				{
					TimedMakeFactionHostile component = worldObject.GetComponent<TimedMakeFactionHostile>();
					if (component != null && component.TicksLeft != null)
					{
						this.worldObjectsResult.Add(worldObject);
					}
				}
				return this.worldObjectsResult;
			}
		}

		// Token: 0x060070DB RID: 28891 RVA: 0x00259A84 File Offset: 0x00257C84
		public override string GetLabel()
		{
			return "FactionWillBecomeHostileIfNotLeavingWithin".Translate();
		}

		// Token: 0x060070DC RID: 28892 RVA: 0x00259A98 File Offset: 0x00257C98
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (GlobalTargetInfo globalTargetInfo in this.WorldObjects)
			{
				stringBuilder.Append("- ");
				stringBuilder.Append(globalTargetInfo.Label);
				stringBuilder.Append(" (");
				stringBuilder.Append(globalTargetInfo.WorldObject.GetComponent<TimedMakeFactionHostile>().TicksLeft.Value.ToStringTicksToPeriod(true, false, true, true));
				stringBuilder.AppendLine(")");
			}
			return "FactionWillBecomeHostileIfNotLeavingWithinDesc".Translate(stringBuilder.ToString().TrimEndNewlines());
		}

		// Token: 0x060070DD RID: 28893 RVA: 0x00259B60 File Offset: 0x00257D60
		public override AlertReport GetReport()
		{
			List<GlobalTargetInfo> worldObjects = this.WorldObjects;
			Map currentMap = Find.CurrentMap;
			List<Pawn> culprits;
			if (!WorldRendererUtility.WorldRenderedNow && currentMap != null && worldObjects.Contains(currentMap.Parent) && !(culprits = currentMap.mapPawns.FreeHumanlikesSpawnedOfFaction(currentMap.ParentFaction)).NullOrEmpty<Pawn>())
			{
				return AlertReport.CulpritsAre(culprits);
			}
			if (worldObjects.Count > 0)
			{
				return AlertReport.CulpritsAre(worldObjects);
			}
			return false;
		}

		// Token: 0x04003E2A RID: 15914
		private List<GlobalTargetInfo> worldObjectsResult = new List<GlobalTargetInfo>();
	}
}
