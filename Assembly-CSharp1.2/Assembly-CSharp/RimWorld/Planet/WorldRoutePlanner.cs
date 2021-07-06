using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020021EB RID: 8683
	[StaticConstructorOnStartup]
	public class WorldRoutePlanner
	{
		// Token: 0x17001BAD RID: 7085
		// (get) Token: 0x0600B9F6 RID: 47606 RVA: 0x00078773 File Offset: 0x00076973
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x17001BAE RID: 7086
		// (get) Token: 0x0600B9F7 RID: 47607 RVA: 0x0007877B File Offset: 0x0007697B
		public bool FormingCaravan
		{
			get
			{
				return this.Active && this.currentFormCaravanDialog != null;
			}
		}

		// Token: 0x17001BAF RID: 7087
		// (get) Token: 0x0600B9F8 RID: 47608 RVA: 0x00078790 File Offset: 0x00076990
		private bool ShouldStop
		{
			get
			{
				return !this.active || !WorldRendererUtility.WorldRenderedNow || (Current.ProgramState == ProgramState.Playing && Find.TickManager.CurTimeSpeed != TimeSpeed.Paused);
			}
		}

		// Token: 0x17001BB0 RID: 7088
		// (get) Token: 0x0600B9F9 RID: 47609 RVA: 0x00357B84 File Offset: 0x00355D84
		private int CaravanTicksPerMove
		{
			get
			{
				CaravanTicksPerMoveUtility.CaravanInfo? caravanInfo = this.CaravanInfo;
				if (caravanInfo != null && caravanInfo.Value.pawns.Any<Pawn>())
				{
					return CaravanTicksPerMoveUtility.GetTicksPerMove(caravanInfo.Value, null);
				}
				return 3464;
			}
		}

		// Token: 0x17001BB1 RID: 7089
		// (get) Token: 0x0600B9FA RID: 47610 RVA: 0x00357BC8 File Offset: 0x00355DC8
		private CaravanTicksPerMoveUtility.CaravanInfo? CaravanInfo
		{
			get
			{
				if (this.currentFormCaravanDialog != null)
				{
					return this.caravanInfoFromFormCaravanDialog;
				}
				Caravan caravanAtTheFirstWaypoint = this.CaravanAtTheFirstWaypoint;
				if (caravanAtTheFirstWaypoint != null)
				{
					return new CaravanTicksPerMoveUtility.CaravanInfo?(new CaravanTicksPerMoveUtility.CaravanInfo(caravanAtTheFirstWaypoint));
				}
				return null;
			}
		}

		// Token: 0x17001BB2 RID: 7090
		// (get) Token: 0x0600B9FB RID: 47611 RVA: 0x000787BC File Offset: 0x000769BC
		private Caravan CaravanAtTheFirstWaypoint
		{
			get
			{
				if (!this.waypoints.Any<RoutePlannerWaypoint>())
				{
					return null;
				}
				return Find.WorldObjects.PlayerControlledCaravanAt(this.waypoints[0].Tile);
			}
		}

		// Token: 0x0600B9FC RID: 47612 RVA: 0x000787E8 File Offset: 0x000769E8
		public void Start()
		{
			if (this.active)
			{
				this.Stop();
			}
			this.active = true;
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				Find.TickManager.Pause();
			}
		}

		// Token: 0x0600B9FD RID: 47613 RVA: 0x00357C04 File Offset: 0x00355E04
		public void Start(Dialog_FormCaravan formCaravanDialog)
		{
			if (this.active)
			{
				this.Stop();
			}
			this.currentFormCaravanDialog = formCaravanDialog;
			this.caravanInfoFromFormCaravanDialog = new CaravanTicksPerMoveUtility.CaravanInfo?(new CaravanTicksPerMoveUtility.CaravanInfo(formCaravanDialog));
			formCaravanDialog.choosingRoute = true;
			Find.WindowStack.TryRemove(formCaravanDialog, false);
			this.Start();
			this.TryAddWaypoint(formCaravanDialog.CurrentTile, true);
			this.cantRemoveFirstWaypoint = true;
		}

		// Token: 0x0600B9FE RID: 47614 RVA: 0x00357C68 File Offset: 0x00355E68
		public void Stop()
		{
			this.active = false;
			for (int i = 0; i < this.waypoints.Count; i++)
			{
				this.waypoints[i].Destroy();
			}
			this.waypoints.Clear();
			this.cachedTicksToWaypoint.Clear();
			if (this.currentFormCaravanDialog != null)
			{
				this.currentFormCaravanDialog.Notify_NoLongerChoosingRoute();
			}
			this.caravanInfoFromFormCaravanDialog = null;
			this.currentFormCaravanDialog = null;
			this.cantRemoveFirstWaypoint = false;
			this.ReleasePaths();
		}

		// Token: 0x0600B9FF RID: 47615 RVA: 0x00357CEC File Offset: 0x00355EEC
		public void WorldRoutePlannerUpdate()
		{
			if (this.active && this.ShouldStop)
			{
				this.Stop();
			}
			if (!this.active)
			{
				return;
			}
			for (int i = 0; i < this.paths.Count; i++)
			{
				this.paths[i].DrawPath(null);
			}
		}

		// Token: 0x0600BA00 RID: 47616 RVA: 0x00357D40 File Offset: 0x00355F40
		public void WorldRoutePlannerOnGUI()
		{
			if (!this.active)
			{
				return;
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				if (this.currentFormCaravanDialog != null)
				{
					Find.WindowStack.Add(this.currentFormCaravanDialog);
				}
				else
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				this.Stop();
				Event.current.Use();
				return;
			}
			GenUI.DrawMouseAttachment(WorldRoutePlanner.MouseAttachment);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
			{
				Caravan caravan = Find.WorldSelector.SelectableObjectsUnderMouse().FirstOrDefault<WorldObject>() as Caravan;
				int tile = (caravan != null) ? caravan.Tile : GenWorld.MouseTile(true);
				if (tile >= 0)
				{
					RoutePlannerWaypoint waypoint = this.MostRecentWaypointAt(tile);
					if (waypoint != null)
					{
						if (waypoint == this.waypoints[this.waypoints.Count - 1])
						{
							this.TryRemoveWaypoint(waypoint, true);
						}
						else
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							list.Add(new FloatMenuOption("AddWaypoint".Translate(), delegate()
							{
								this.TryAddWaypoint(tile, true);
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
							list.Add(new FloatMenuOption("RemoveWaypoint".Translate(), delegate()
							{
								this.TryRemoveWaypoint(waypoint, true);
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
							Find.WindowStack.Add(new FloatMenu(list));
						}
					}
					else
					{
						this.TryAddWaypoint(tile, true);
					}
					Event.current.Use();
				}
			}
			this.DoRouteDetailsBox();
			if (this.DoChooseRouteButton())
			{
				return;
			}
			this.DoTileTooltips();
		}

		// Token: 0x0600BA01 RID: 47617 RVA: 0x00357F18 File Offset: 0x00356118
		private void DoRouteDetailsBox()
		{
			WorldRoutePlanner.<>c__DisplayClass31_0 CS$<>8__locals1 = new WorldRoutePlanner.<>c__DisplayClass31_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.rect = new Rect(((float)UI.screenWidth - WorldRoutePlanner.BottomWindowSize.x) / 2f, (float)UI.screenHeight - WorldRoutePlanner.BottomWindowSize.y - 45f, WorldRoutePlanner.BottomWindowSize.x, WorldRoutePlanner.BottomWindowSize.y);
			if (Current.ProgramState == ProgramState.Entry)
			{
				WorldRoutePlanner.<>c__DisplayClass31_0 CS$<>8__locals2 = CS$<>8__locals1;
				CS$<>8__locals2.rect.y = CS$<>8__locals2.rect.y - 22f;
			}
			Find.WindowStack.ImmediateWindow(1373514241, CS$<>8__locals1.rect, WindowLayer.Dialog, delegate
			{
				if (!CS$<>8__locals1.<>4__this.active)
				{
					return;
				}
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperCenter;
				Text.Font = GameFont.Small;
				float num = 6f;
				if (CS$<>8__locals1.<>4__this.waypoints.Count >= 2)
				{
					Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), "RoutePlannerEstTimeToFinalDest".Translate(CS$<>8__locals1.<>4__this.GetTicksToWaypoint(CS$<>8__locals1.<>4__this.waypoints.Count - 1).ToStringTicksToDays("0.#")));
				}
				else if (CS$<>8__locals1.<>4__this.cantRemoveFirstWaypoint)
				{
					Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), "RoutePlannerAddOneOrMoreWaypoints".Translate());
				}
				else
				{
					Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), "RoutePlannerAddTwoOrMoreWaypoints".Translate());
				}
				num += 20f;
				if (CS$<>8__locals1.<>4__this.CaravanInfo == null || !CS$<>8__locals1.<>4__this.CaravanInfo.Value.pawns.Any<Pawn>())
				{
					GUI.color = new Color(0.8f, 0.6f, 0.6f);
					Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), "RoutePlannerUsingAverageTicksPerMoveWarning".Translate());
				}
				else if (CS$<>8__locals1.<>4__this.currentFormCaravanDialog == null && CS$<>8__locals1.<>4__this.CaravanAtTheFirstWaypoint != null)
				{
					GUI.color = Color.gray;
					Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), "RoutePlannerUsingTicksPerMoveOfCaravan".Translate(CS$<>8__locals1.<>4__this.CaravanAtTheFirstWaypoint.LabelCap));
				}
				num += 20f;
				GUI.color = Color.gray;
				Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), "RoutePlannerPressRMBToAddAndRemoveWaypoints".Translate());
				num += 20f;
				if (CS$<>8__locals1.<>4__this.currentFormCaravanDialog != null)
				{
					Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), "RoutePlannerPressEscapeToReturnToCaravanFormationDialog".Translate());
				}
				else
				{
					Widgets.Label(new Rect(0f, num, CS$<>8__locals1.rect.width, 25f), "RoutePlannerPressEscapeToExit".Translate());
				}
				num += 20f;
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}, true, false, 1f);
		}

		// Token: 0x0600BA02 RID: 47618 RVA: 0x00357FC8 File Offset: 0x003561C8
		private bool DoChooseRouteButton()
		{
			if (this.currentFormCaravanDialog == null || this.waypoints.Count < 2)
			{
				return false;
			}
			if (Widgets.ButtonText(new Rect(((float)UI.screenWidth - WorldRoutePlanner.BottomButtonSize.x) / 2f, (float)UI.screenHeight - WorldRoutePlanner.BottomWindowSize.y - 45f - 10f - WorldRoutePlanner.BottomButtonSize.y, WorldRoutePlanner.BottomButtonSize.x, WorldRoutePlanner.BottomButtonSize.y), "Accept".Translate(), true, true, true))
			{
				Find.WindowStack.Add(this.currentFormCaravanDialog);
				this.currentFormCaravanDialog.Notify_ChoseRoute(this.waypoints[1].Tile);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				this.Stop();
				return true;
			}
			return false;
		}

		// Token: 0x0600BA03 RID: 47619 RVA: 0x003580A0 File Offset: 0x003562A0
		private void DoTileTooltips()
		{
			if (Mouse.IsInputBlockedNow)
			{
				return;
			}
			int num = GenWorld.MouseTile(true);
			if (num == -1)
			{
				return;
			}
			for (int i = 0; i < this.paths.Count; i++)
			{
				if (this.paths[i].NodesReversed.Contains(num))
				{
					string str = this.GetTileTip(num, i);
					Text.Font = GameFont.Small;
					Vector2 vector = Text.CalcSize(str);
					vector.x += 20f;
					vector.y += 20f;
					Vector2 mouseAttachedWindowPos = GenUI.GetMouseAttachedWindowPos(vector.x, vector.y);
					Rect rect = new Rect(mouseAttachedWindowPos, vector);
					Find.WindowStack.ImmediateWindow(1859615246, rect, WindowLayer.Super, delegate
					{
						Text.Font = GameFont.Small;
						Widgets.Label(rect.AtZero().ContractedBy(10f), str);
					}, true, false, 1f);
					return;
				}
			}
		}

		// Token: 0x0600BA04 RID: 47620 RVA: 0x0035818C File Offset: 0x0035638C
		private string GetTileTip(int tile, int pathIndex)
		{
			int num = this.paths[pathIndex].NodesReversed.IndexOf(tile);
			int num2;
			if (num > 0)
			{
				num2 = this.paths[pathIndex].NodesReversed[num - 1];
			}
			else if (pathIndex < this.paths.Count - 1 && this.paths[pathIndex + 1].NodesReversed.Count >= 2)
			{
				num2 = this.paths[pathIndex + 1].NodesReversed[this.paths[pathIndex + 1].NodesReversed.Count - 2];
			}
			else
			{
				num2 = -1;
			}
			int num3 = this.cachedTicksToWaypoint[pathIndex] + CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.paths[pathIndex].FirstNode, tile, this.paths[pathIndex], 0f, this.CaravanTicksPerMove, GenTicks.TicksAbs + this.cachedTicksToWaypoint[pathIndex]);
			int num4 = GenTicks.TicksAbs + num3;
			StringBuilder stringBuilder = new StringBuilder();
			if (num3 != 0)
			{
				stringBuilder.AppendLine("EstimatedTimeToTile".Translate(num3.ToStringTicksToDays("0.##")));
			}
			stringBuilder.AppendLine("ForagedFoodAmount".Translate() + ": " + Find.WorldGrid[tile].biome.forageability.ToStringPercent());
			stringBuilder.Append(VirtualPlantsUtility.GetVirtualPlantsStatusExplanationAt(tile, num4));
			if (num2 != -1)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				StringBuilder stringBuilder2 = new StringBuilder();
				float num5 = WorldPathGrid.CalculatedMovementDifficultyAt(num2, false, new int?(num4), stringBuilder2);
				float roadMovementDifficultyMultiplier = Find.WorldGrid.GetRoadMovementDifficultyMultiplier(tile, num2, stringBuilder2);
				stringBuilder.Append("TileMovementDifficulty".Translate() + ":\n" + stringBuilder2.ToString().Indented("  "));
				stringBuilder.AppendLine();
				stringBuilder.Append("  = ");
				stringBuilder.Append((num5 * roadMovementDifficultyMultiplier).ToString("0.#"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600BA05 RID: 47621 RVA: 0x003583B8 File Offset: 0x003565B8
		public void DoRoutePlannerButton(ref float curBaseY)
		{
			float num = 57f;
			float num2 = 33f;
			Rect rect = new Rect((float)UI.screenWidth - 10f - num, curBaseY - 10f - num2, num, num2);
			if (Widgets.ButtonImage(rect, WorldRoutePlanner.ButtonTex, Color.white, new Color(0.8f, 0.8f, 0.8f), true))
			{
				if (this.active)
				{
					this.Stop();
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				else
				{
					this.Start();
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
			}
			TooltipHandler.TipRegionByKey(rect, "RoutePlannerButtonTip");
			curBaseY -= num2 + 20f;
		}

		// Token: 0x0600BA06 RID: 47622 RVA: 0x00078821 File Offset: 0x00076A21
		public int GetTicksToWaypoint(int index)
		{
			return this.cachedTicksToWaypoint[index];
		}

		// Token: 0x0600BA07 RID: 47623 RVA: 0x0035845C File Offset: 0x0035665C
		private void TryAddWaypoint(int tile, bool playSound = true)
		{
			if (Find.World.Impassable(tile))
			{
				Messages.Message("MessageCantAddWaypointBecauseImpassable".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (this.waypoints.Any<RoutePlannerWaypoint>() && !Find.WorldReachability.CanReach(this.waypoints[this.waypoints.Count - 1].Tile, tile))
			{
				Messages.Message("MessageCantAddWaypointBecauseUnreachable".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (this.waypoints.Count >= 25)
			{
				Messages.Message("MessageCantAddWaypointBecauseLimit".Translate(25), MessageTypeDefOf.RejectInput, false);
				return;
			}
			RoutePlannerWaypoint routePlannerWaypoint = (RoutePlannerWaypoint)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.RoutePlannerWaypoint);
			routePlannerWaypoint.Tile = tile;
			Find.WorldObjects.Add(routePlannerWaypoint);
			this.waypoints.Add(routePlannerWaypoint);
			this.RecreatePaths();
			if (playSound)
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600BA08 RID: 47624 RVA: 0x00358558 File Offset: 0x00356758
		public void TryRemoveWaypoint(RoutePlannerWaypoint point, bool playSound = true)
		{
			if (this.cantRemoveFirstWaypoint && this.waypoints.Any<RoutePlannerWaypoint>() && point == this.waypoints[0])
			{
				Messages.Message("MessageCantRemoveWaypointBecauseFirst".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			point.Destroy();
			this.waypoints.Remove(point);
			for (int i = this.waypoints.Count - 1; i >= 1; i--)
			{
				if (this.waypoints[i].Tile == this.waypoints[i - 1].Tile)
				{
					this.waypoints[i].Destroy();
					this.waypoints.RemoveAt(i);
				}
			}
			this.RecreatePaths();
			if (playSound)
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600BA09 RID: 47625 RVA: 0x00358628 File Offset: 0x00356828
		private void ReleasePaths()
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				this.paths[i].ReleaseToPool();
			}
			this.paths.Clear();
		}

		// Token: 0x0600BA0A RID: 47626 RVA: 0x00358668 File Offset: 0x00356868
		private void RecreatePaths()
		{
			this.ReleasePaths();
			WorldPathFinder worldPathFinder = Find.WorldPathFinder;
			for (int i = 1; i < this.waypoints.Count; i++)
			{
				this.paths.Add(worldPathFinder.FindPath(this.waypoints[i - 1].Tile, this.waypoints[i].Tile, null, null));
			}
			this.cachedTicksToWaypoint.Clear();
			int num = 0;
			int caravanTicksPerMove = this.CaravanTicksPerMove;
			for (int j = 0; j < this.waypoints.Count; j++)
			{
				if (j == 0)
				{
					this.cachedTicksToWaypoint.Add(0);
				}
				else
				{
					num += CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.waypoints[j - 1].Tile, this.waypoints[j].Tile, this.paths[j - 1], 0f, caravanTicksPerMove, GenTicks.TicksAbs + num);
					this.cachedTicksToWaypoint.Add(num);
				}
			}
		}

		// Token: 0x0600BA0B RID: 47627 RVA: 0x00358764 File Offset: 0x00356964
		private RoutePlannerWaypoint MostRecentWaypointAt(int tile)
		{
			for (int i = this.waypoints.Count - 1; i >= 0; i--)
			{
				if (this.waypoints[i].Tile == tile)
				{
					return this.waypoints[i];
				}
			}
			return null;
		}

		// Token: 0x04007EFF RID: 32511
		private bool active;

		// Token: 0x04007F00 RID: 32512
		private CaravanTicksPerMoveUtility.CaravanInfo? caravanInfoFromFormCaravanDialog;

		// Token: 0x04007F01 RID: 32513
		private Dialog_FormCaravan currentFormCaravanDialog;

		// Token: 0x04007F02 RID: 32514
		private List<WorldPath> paths = new List<WorldPath>();

		// Token: 0x04007F03 RID: 32515
		private List<int> cachedTicksToWaypoint = new List<int>();

		// Token: 0x04007F04 RID: 32516
		public List<RoutePlannerWaypoint> waypoints = new List<RoutePlannerWaypoint>();

		// Token: 0x04007F05 RID: 32517
		private bool cantRemoveFirstWaypoint;

		// Token: 0x04007F06 RID: 32518
		private const int MaxCount = 25;

		// Token: 0x04007F07 RID: 32519
		private static readonly Texture2D ButtonTex = ContentFinder<Texture2D>.Get("UI/Misc/WorldRoutePlanner", true);

		// Token: 0x04007F08 RID: 32520
		private static readonly Texture2D MouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/WaypointMouseAttachment", true);

		// Token: 0x04007F09 RID: 32521
		private static readonly Vector2 BottomWindowSize = new Vector2(500f, 95f);

		// Token: 0x04007F0A RID: 32522
		private static readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04007F0B RID: 32523
		private const float BottomWindowBotMargin = 45f;

		// Token: 0x04007F0C RID: 32524
		private const float BottomWindowEntryExtraBotMargin = 22f;
	}
}
