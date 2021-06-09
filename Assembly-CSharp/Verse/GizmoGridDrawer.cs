using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200071C RID: 1820
	public static class GizmoGridDrawer
	{
		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06002DFE RID: 11774 RVA: 0x00024331 File Offset: 0x00022531
		public static float HeightDrawnRecently
		{
			get
			{
				if (Time.frameCount > GizmoGridDrawer.heightDrawnFrame + 2)
				{
					return 0f;
				}
				return GizmoGridDrawer.heightDrawn;
			}
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x00135BDC File Offset: 0x00133DDC
		public static void DrawGizmoGrid(IEnumerable<Gizmo> gizmos, float startX, out Gizmo mouseoverGizmo)
		{
			if (Event.current.type == EventType.Layout)
			{
				mouseoverGizmo = null;
				return;
			}
			GizmoGridDrawer.tmpAllGizmos.Clear();
			GizmoGridDrawer.tmpAllGizmos.AddRange(gizmos);
			GizmoGridDrawer.tmpAllGizmos.SortStable(GizmoGridDrawer.SortByOrder);
			GizmoGridDrawer.gizmoGroups.Clear();
			for (int i = 0; i < GizmoGridDrawer.tmpAllGizmos.Count; i++)
			{
				Gizmo gizmo = GizmoGridDrawer.tmpAllGizmos[i];
				bool flag = false;
				for (int j = 0; j < GizmoGridDrawer.gizmoGroups.Count; j++)
				{
					if (GizmoGridDrawer.gizmoGroups[j][0].GroupsWith(gizmo))
					{
						flag = true;
						GizmoGridDrawer.gizmoGroups[j].Add(gizmo);
						GizmoGridDrawer.gizmoGroups[j][0].MergeWith(gizmo);
						break;
					}
				}
				if (!flag)
				{
					List<Gizmo> list = SimplePool<List<Gizmo>>.Get();
					list.Add(gizmo);
					GizmoGridDrawer.gizmoGroups.Add(list);
				}
			}
			GizmoGridDrawer.firstGizmos.Clear();
			GizmoGridDrawer.shrinkableCommands.Clear();
			float num = (float)(UI.screenWidth - 147);
			Vector2 vector = new Vector2(startX, (float)(UI.screenHeight - 35) - GizmoGridDrawer.GizmoSpacing.y - 75f);
			float maxWidth = num - startX;
			int num2 = 0;
			for (int k = 0; k < GizmoGridDrawer.gizmoGroups.Count; k++)
			{
				List<Gizmo> list2 = GizmoGridDrawer.gizmoGroups[k];
				Gizmo gizmo2 = null;
				for (int l = 0; l < list2.Count; l++)
				{
					if (!list2[l].disabled)
					{
						gizmo2 = list2[l];
						break;
					}
				}
				if (gizmo2 == null)
				{
					gizmo2 = list2.FirstOrDefault<Gizmo>();
				}
				else
				{
					Command_Toggle command_Toggle = gizmo2 as Command_Toggle;
					if (command_Toggle != null)
					{
						if (!command_Toggle.activateIfAmbiguous && !command_Toggle.isActive())
						{
							for (int m = 0; m < list2.Count; m++)
							{
								Command_Toggle command_Toggle2 = list2[m] as Command_Toggle;
								if (command_Toggle2 != null && !command_Toggle2.disabled && command_Toggle2.isActive())
								{
									gizmo2 = list2[m];
									break;
								}
							}
						}
						if (command_Toggle.activateIfAmbiguous && command_Toggle.isActive())
						{
							for (int n = 0; n < list2.Count; n++)
							{
								Command_Toggle command_Toggle3 = list2[n] as Command_Toggle;
								if (command_Toggle3 != null && !command_Toggle3.disabled && !command_Toggle3.isActive())
								{
									gizmo2 = list2[n];
									break;
								}
							}
						}
					}
				}
				if (gizmo2 != null)
				{
					Command command;
					if ((command = (gizmo2 as Command)) != null && command.shrinkable && command.Visible)
					{
						GizmoGridDrawer.shrinkableCommands.Add(command);
					}
					if (vector.x + gizmo2.GetWidth(maxWidth) > num)
					{
						vector.x = startX;
						vector.y -= 75f + GizmoGridDrawer.GizmoSpacing.x;
						num2++;
					}
					vector.x += gizmo2.GetWidth(maxWidth) + GizmoGridDrawer.GizmoSpacing.x;
					GizmoGridDrawer.firstGizmos.Add(gizmo2);
				}
			}
			if (num2 > 1 && GizmoGridDrawer.shrinkableCommands.Count > 1)
			{
				for (int num3 = 0; num3 < GizmoGridDrawer.shrinkableCommands.Count; num3++)
				{
					GizmoGridDrawer.firstGizmos.Remove(GizmoGridDrawer.shrinkableCommands[num3]);
				}
			}
			else
			{
				GizmoGridDrawer.shrinkableCommands.Clear();
			}
			GizmoGridDrawer.drawnHotKeys.Clear();
			Text.Font = GameFont.Tiny;
			Vector2 vector2 = new Vector2(startX, (float)(UI.screenHeight - 35) - GizmoGridDrawer.GizmoSpacing.y - 75f);
			mouseoverGizmo = null;
			GizmoGridDrawer.<>c__DisplayClass11_0 CS$<>8__locals1;
			CS$<>8__locals1.interactedGiz = null;
			CS$<>8__locals1.interactedEvent = null;
			CS$<>8__locals1.floatMenuGiz = null;
			for (int num4 = 0; num4 < GizmoGridDrawer.firstGizmos.Count; num4++)
			{
				Gizmo gizmo3 = GizmoGridDrawer.firstGizmos[num4];
				if (gizmo3.Visible)
				{
					if (vector2.x + gizmo3.GetWidth(maxWidth) > num)
					{
						vector2.x = startX;
						vector2.y -= 75f + GizmoGridDrawer.GizmoSpacing.x;
					}
					GizmoGridDrawer.heightDrawnFrame = Time.frameCount;
					GizmoGridDrawer.heightDrawn = (float)UI.screenHeight - vector2.y;
					GizmoResult result = gizmo3.GizmoOnGUI(vector2, maxWidth);
					GizmoGridDrawer.<DrawGizmoGrid>g__ProcessGizmoState|11_0(gizmo3, result, ref mouseoverGizmo, ref CS$<>8__locals1);
					GenUI.AbsorbClicksInRect(new Rect(vector2.x, vector2.y, gizmo3.GetWidth(maxWidth), 75f + GizmoGridDrawer.GizmoSpacing.y).ContractedBy(-12f));
					vector2.x += gizmo3.GetWidth(maxWidth) + GizmoGridDrawer.GizmoSpacing.x;
				}
			}
			float x = vector2.x;
			int num5 = 0;
			for (int num6 = 0; num6 < GizmoGridDrawer.shrinkableCommands.Count; num6++)
			{
				Command command2 = GizmoGridDrawer.shrinkableCommands[num6];
				float getShrunkSize = command2.GetShrunkSize;
				if (vector2.x + getShrunkSize > num)
				{
					num5++;
					if (num5 > 1)
					{
						x = startX;
					}
					vector2.x = x;
					vector2.y -= getShrunkSize + 3f;
				}
				Vector2 vector3 = vector2;
				vector3.y += getShrunkSize + 3f;
				GizmoGridDrawer.heightDrawnFrame = Time.frameCount;
				GizmoGridDrawer.heightDrawn = Mathf.Min(GizmoGridDrawer.heightDrawn, (float)UI.screenHeight - vector3.y);
				GizmoResult result2 = command2.GizmoOnGUIShrunk(vector3, getShrunkSize);
				GizmoGridDrawer.<DrawGizmoGrid>g__ProcessGizmoState|11_0(command2, result2, ref mouseoverGizmo, ref CS$<>8__locals1);
				GenUI.AbsorbClicksInRect(new Rect(vector3.x, vector3.y, getShrunkSize, getShrunkSize + 3f).ExpandedBy(3f));
				vector2.x += getShrunkSize + 3f;
			}
			if (CS$<>8__locals1.interactedGiz != null)
			{
				List<Gizmo> list3 = GizmoGridDrawer.<DrawGizmoGrid>g__FindMatchingGroup|11_1(CS$<>8__locals1.interactedGiz);
				for (int num7 = 0; num7 < list3.Count; num7++)
				{
					Gizmo gizmo4 = list3[num7];
					if (gizmo4 != CS$<>8__locals1.interactedGiz && !gizmo4.disabled && CS$<>8__locals1.interactedGiz.InheritInteractionsFrom(gizmo4))
					{
						gizmo4.ProcessInput(CS$<>8__locals1.interactedEvent);
					}
				}
				CS$<>8__locals1.interactedGiz.ProcessInput(CS$<>8__locals1.interactedEvent);
				Event.current.Use();
			}
			else if (CS$<>8__locals1.floatMenuGiz != null)
			{
				List<FloatMenuOption> list4 = new List<FloatMenuOption>();
				foreach (FloatMenuOption item in CS$<>8__locals1.floatMenuGiz.RightClickFloatMenuOptions)
				{
					list4.Add(item);
				}
				List<Gizmo> list5 = GizmoGridDrawer.<DrawGizmoGrid>g__FindMatchingGroup|11_1(CS$<>8__locals1.floatMenuGiz);
				for (int num8 = 0; num8 < list5.Count; num8++)
				{
					Gizmo gizmo5 = list5[num8];
					if (gizmo5 != CS$<>8__locals1.floatMenuGiz && !gizmo5.disabled && CS$<>8__locals1.floatMenuGiz.InheritFloatMenuInteractionsFrom(gizmo5))
					{
						foreach (FloatMenuOption floatMenuOption in gizmo5.RightClickFloatMenuOptions)
						{
							FloatMenuOption floatMenuOption2 = null;
							for (int num9 = 0; num9 < list4.Count; num9++)
							{
								if (list4[num9].Label == floatMenuOption.Label)
								{
									floatMenuOption2 = list4[num9];
									break;
								}
							}
							if (floatMenuOption2 == null)
							{
								list4.Add(floatMenuOption);
							}
							else if (!floatMenuOption.Disabled)
							{
								if (!floatMenuOption2.Disabled)
								{
									Action prevAction = floatMenuOption2.action;
									Action localOptionAction = floatMenuOption.action;
									floatMenuOption2.action = delegate()
									{
										prevAction();
										localOptionAction();
									};
								}
								else if (floatMenuOption2.Disabled)
								{
									list4[list4.IndexOf(floatMenuOption2)] = floatMenuOption;
								}
							}
						}
					}
				}
				Event.current.Use();
				if (list4.Any<FloatMenuOption>())
				{
					Find.WindowStack.Add(new FloatMenu(list4));
				}
			}
			for (int num10 = 0; num10 < GizmoGridDrawer.gizmoGroups.Count; num10++)
			{
				GizmoGridDrawer.gizmoGroups[num10].Clear();
				SimplePool<List<Gizmo>>.Return(GizmoGridDrawer.gizmoGroups[num10]);
			}
			GizmoGridDrawer.gizmoGroups.Clear();
			GizmoGridDrawer.firstGizmos.Clear();
			GizmoGridDrawer.tmpAllGizmos.Clear();
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x001364D0 File Offset: 0x001346D0
		[CompilerGenerated]
		internal static void <DrawGizmoGrid>g__ProcessGizmoState|11_0(Gizmo giz, GizmoResult result, ref Gizmo mouseoverGiz, ref GizmoGridDrawer.<>c__DisplayClass11_0 A_3)
		{
			if (result.State == GizmoState.Interacted || (result.State == GizmoState.OpenedFloatMenu && giz.RightClickFloatMenuOptions.FirstOrDefault<FloatMenuOption>() == null))
			{
				A_3.interactedEvent = result.InteractEvent;
				A_3.interactedGiz = giz;
			}
			else if (result.State == GizmoState.OpenedFloatMenu)
			{
				A_3.floatMenuGiz = giz;
			}
			if (result.State >= GizmoState.Mouseover)
			{
				mouseoverGiz = giz;
			}
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x00136534 File Offset: 0x00134734
		[CompilerGenerated]
		internal static List<Gizmo> <DrawGizmoGrid>g__FindMatchingGroup|11_1(Gizmo toMatch)
		{
			for (int i = 0; i < GizmoGridDrawer.gizmoGroups.Count; i++)
			{
				if (GizmoGridDrawer.gizmoGroups[i].Contains(toMatch))
				{
					return GizmoGridDrawer.gizmoGroups[i];
				}
			}
			return null;
		}

		// Token: 0x04001F56 RID: 8022
		public static HashSet<KeyCode> drawnHotKeys = new HashSet<KeyCode>();

		// Token: 0x04001F57 RID: 8023
		private static float heightDrawn;

		// Token: 0x04001F58 RID: 8024
		private static int heightDrawnFrame;

		// Token: 0x04001F59 RID: 8025
		public static readonly Vector2 GizmoSpacing = new Vector2(5f, 14f);

		// Token: 0x04001F5A RID: 8026
		private static List<List<Gizmo>> gizmoGroups = new List<List<Gizmo>>();

		// Token: 0x04001F5B RID: 8027
		private static List<Gizmo> firstGizmos = new List<Gizmo>();

		// Token: 0x04001F5C RID: 8028
		private static List<Command> shrinkableCommands = new List<Command>();

		// Token: 0x04001F5D RID: 8029
		private static List<Gizmo> tmpAllGizmos = new List<Gizmo>();

		// Token: 0x04001F5E RID: 8030
		private static readonly Func<Gizmo, Gizmo, int> SortByOrder = (Gizmo lhs, Gizmo rhs) => lhs.order.CompareTo(rhs.order);
	}
}
