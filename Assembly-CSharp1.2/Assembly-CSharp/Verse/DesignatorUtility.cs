using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000548 RID: 1352
	[StaticConstructorOnStartup]
	public static class DesignatorUtility
	{
		// Token: 0x060022C2 RID: 8898 RVA: 0x0010AF60 File Offset: 0x00109160
		public static Designator FindAllowedDesignator<T>() where T : Designator
		{
			List<DesignationCategoryDef> allDefsListForReading = DefDatabase<DesignationCategoryDef>.AllDefsListForReading;
			GameRules rules = Current.Game.Rules;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				List<Designator> allResolvedDesignators = allDefsListForReading[i].AllResolvedDesignators;
				for (int j = 0; j < allResolvedDesignators.Count; j++)
				{
					if (rules == null || rules.DesignatorAllowed(allResolvedDesignators[j]))
					{
						T t = allResolvedDesignators[j] as T;
						if (t != null)
						{
							return t;
						}
					}
				}
			}
			Designator designator = DesignatorUtility.StandaloneDesignators.TryGetValue(typeof(T), null);
			if (designator == null)
			{
				designator = (Activator.CreateInstance(typeof(T)) as Designator);
				DesignatorUtility.StandaloneDesignators[typeof(T)] = designator;
			}
			return designator;
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x0010B034 File Offset: 0x00109234
		public static void RenderHighlightOverSelectableCells(Designator designator, List<IntVec3> dragCells)
		{
			for (int i = 0; i < dragCells.Count; i++)
			{
				Vector3 position = dragCells[i].ToVector3Shifted();
				position.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, DesignatorUtility.DragHighlightCellMat, 0);
			}
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x0010B088 File Offset: 0x00109288
		public static void RenderHighlightOverSelectableThings(Designator designator, List<IntVec3> dragCells)
		{
			DesignatorUtility.selectedThings.Clear();
			for (int i = 0; i < dragCells.Count; i++)
			{
				List<Thing> thingList = dragCells[i].GetThingList(designator.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					if (designator.CanDesignateThing(thingList[j]).Accepted && !DesignatorUtility.selectedThings.Contains(thingList[j]))
					{
						DesignatorUtility.selectedThings.Add(thingList[j]);
						Vector3 drawPos = thingList[j].DrawPos;
						drawPos.y = AltitudeLayer.MetaOverlays.AltitudeFor();
						Graphics.DrawMesh(MeshPool.plane10, drawPos, Quaternion.identity, DesignatorUtility.DragHighlightThingMat, 0);
					}
				}
			}
			DesignatorUtility.selectedThings.Clear();
		}

		// Token: 0x0400178C RID: 6028
		public static readonly Material DragHighlightCellMat = MaterialPool.MatFrom("UI/Overlays/DragHighlightCell", ShaderDatabase.MetaOverlay);

		// Token: 0x0400178D RID: 6029
		public static readonly Material DragHighlightThingMat = MaterialPool.MatFrom("UI/Overlays/DragHighlightThing", ShaderDatabase.MetaOverlay);

		// Token: 0x0400178E RID: 6030
		private static Dictionary<Type, Designator> StandaloneDesignators = new Dictionary<Type, Designator>();

		// Token: 0x0400178F RID: 6031
		private static HashSet<Thing> selectedThings = new HashSet<Thing>();
	}
}
