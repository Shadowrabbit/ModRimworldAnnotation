using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001357 RID: 4951
	public static class InspectGizmoGrid
	{
		// Token: 0x060077E8 RID: 30696 RVA: 0x002A3BBC File Offset: 0x002A1DBC
		public static void DrawInspectGizmoGridFor(IEnumerable<object> selectedObjects, out Gizmo mouseoverGizmo)
		{
			mouseoverGizmo = null;
			try
			{
				InspectGizmoGrid.objList.Clear();
				InspectGizmoGrid.objList.AddRange(selectedObjects);
				InspectGizmoGrid.gizmoList.Clear();
				for (int i = 0; i < InspectGizmoGrid.objList.Count; i++)
				{
					ISelectable selectable = InspectGizmoGrid.objList[i] as ISelectable;
					if (selectable != null)
					{
						InspectGizmoGrid.gizmoList.AddRange(selectable.GetGizmos());
					}
				}
				for (int j = 0; j < InspectGizmoGrid.objList.Count; j++)
				{
					Thing t = InspectGizmoGrid.objList[j] as Thing;
					if (t != null)
					{
						List<Designator> allDesignators = Find.ReverseDesignatorDatabase.AllDesignators;
						for (int k = 0; k < allDesignators.Count; k++)
						{
							Designator des = allDesignators[k];
							if (des.CanDesignateThing(t).Accepted)
							{
								Command_Action command_Action = new Command_Action();
								command_Action.defaultLabel = des.LabelCapReverseDesignating(t);
								float iconAngle;
								Vector2 iconOffset;
								command_Action.icon = des.IconReverseDesignating(t, out iconAngle, out iconOffset);
								command_Action.iconAngle = iconAngle;
								command_Action.iconOffset = iconOffset;
								command_Action.defaultDesc = des.DescReverseDesignating(t);
								command_Action.order = ((des is Designator_Uninstall) ? -11f : -20f);
								command_Action.action = delegate()
								{
									if (!TutorSystem.AllowAction(des.TutorTagDesignate))
									{
										return;
									}
									des.DesignateThing(t);
									des.Finalize(true);
								};
								command_Action.hotKey = des.hotKey;
								command_Action.groupKey = des.groupKey;
								InspectGizmoGrid.gizmoList.Add(command_Action);
							}
						}
					}
				}
				InspectGizmoGrid.objList.Clear();
				GizmoGridDrawer.DrawGizmoGrid(InspectGizmoGrid.gizmoList, InspectPaneUtility.PaneWidthFor(Find.WindowStack.WindowOfType<IInspectPane>()) + GizmoGridDrawer.GizmoSpacing.y, out mouseoverGizmo, null, null, null);
				InspectGizmoGrid.gizmoList.Clear();
			}
			catch (Exception ex)
			{
				Log.ErrorOnce(ex.ToString(), 3427734);
			}
		}

		// Token: 0x040042A7 RID: 17063
		private static List<object> objList = new List<object>();

		// Token: 0x040042A8 RID: 17064
		private static List<Gizmo> gizmoList = new List<Gizmo>();
	}
}
