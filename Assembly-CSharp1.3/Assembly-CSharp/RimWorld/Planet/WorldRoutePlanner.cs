using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x0200181B RID: 6171
	[StaticConstructorOnStartup]
	public class WorldRoutePlanner
	{
		// Token: 0x170017CB RID: 6091
		// (get) Token: 0x0600909D RID: 37021 RVA: 0x0033DDDF File Offset: 0x0033BFDF
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x170017CC RID: 6092
		// (get) Token: 0x0600909E RID: 37022 RVA: 0x0033DDE7 File Offset: 0x0033BFE7
		public bool FormingCaravan
		{
			get
			{
				return this.Active && this.currentFormCaravanDialog != null;
			}
		}

		// Token: 0x170017CD RID: 6093
		// (get) Token: 0x0600909F RID: 37023 RVA: 0x0033DDFC File Offset: 0x0033BFFC
		private bool ShouldStop
		{
			get
			{
				return !this.active || !WorldRendererUtility.WorldRenderedNow || (Current.ProgramState == ProgramState.Playing && Find.TickManager.CurTimeSpeed != TimeSpeed.Paused);
			}
		}

		// Token: 0x170017CE RID: 6094
		// (get) Token: 0x060090A0 RID: 37024 RVA: 0x0033DE28 File Offset: 0x0033C028
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

		// Token: 0x170017CF RID: 6095
		// (get) Token: 0x060090A1 RID: 37025 RVA: 0x0033DE6C File Offset: 0x0033C06C
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

		// Token: 0x170017D0 RID: 6096
		// (get) Token: 0x060090A2 RID: 37026 RVA: 0x0033DEA7 File Offset: 0x0033C0A7
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

		// Token: 0x060090A3 RID: 37027 RVA: 0x0033DED3 File Offset: 0x0033C0D3
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

		// Token: 0x060090A4 RID: 37028 RVA: 0x0033DF0C File Offset: 0x0033C10C
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

		// Token: 0x060090A5 RID: 37029 RVA: 0x0033DF70 File Offset: 0x0033C170
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

		// Token: 0x060090A6 RID: 37030 RVA: 0x0033DFF4 File Offset: 0x0033C1F4
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

		// Token: 0x060090A7 RID: 37031 RVA: 0x0033E048 File Offset: 0x0033C248
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
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							list.Add(new FloatMenuOption("RemoveWaypoint".Translate(), delegate()
							{
								this.TryRemoveWaypoint(waypoint, true);
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
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

		// Token: 0x060090A8 RID: 37032 RVA: 0x0033E224 File Offset: 0x0033C424
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
			}, true, false, 1f, null);
		}

		// Token: 0x060090A9 RID: 37033 RVA: 0x0033E2D4 File Offset: 0x0033C4D4
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

		// Token: 0x060090AA RID: 37034 RVA: 0x0033E3AC File Offset: 0x0033C5AC
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
					}, true, false, 1f, null);
					return;
				}
			}
		}

		// Token: 0x060090AB RID: 37035 RVA: 0x0033E498 File Offset: 0x0033C698
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

		// Token: 0x060090AC RID: 37036 RVA: 0x0033E6C4 File Offset: 0x0033C8C4
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

		// Token: 0x060090AD RID: 37037 RVA: 0x0033E765 File Offset: 0x0033C965
		public int GetTicksToWaypoint(int index)
		{
			return this.cachedTicksToWaypoint[index];
		}

		// Token: 0x060090AE RID: 37038 RVA: 0x0033E774 File Offset: 0x0033C974
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

		// Token: 0x060090AF RID: 37039 RVA: 0x0033E870 File Offset: 0x0033CA70
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

		// Token: 0x060090B0 RID: 37040 RVA: 0x0033E940 File Offset: 0x0033CB40
		private void ReleasePaths()
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				this.paths[i].ReleaseToPool();
			}
			this.paths.Clear();
		}

		// Token: 0x060090B1 RID: 37041 RVA: 0x0033E980 File Offset: 0x0033CB80
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

		// Token: 0x060090B2 RID: 37042 RVA: 0x0033EA7C File Offset: 0x0033CC7C
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

		// Token: 0x04005AEA RID: 23274
		private bool active;

		// Token: 0x04005AEB RID: 23275
		private CaravanTicksPerMoveUtility.CaravanInfo? caravanInfoFromFormCaravanDialog;

		// Token: 0x04005AEC RID: 23276
		private Dialog_FormCaravan currentFormCaravanDialog;

		// Token: 0x04005AED RID: 23277
		private List<WorldPath> paths = new List<WorldPath>();

		// Token: 0x04005AEE RID: 23278
		private List<int> cachedTicksToWaypoint = new List<int>();

		// Token: 0x04005AEF RID: 23279
		public List<RoutePlannerWaypoint> waypoints = new List<RoutePlannerWaypoint>();

		// Token: 0x04005AF0 RID: 23280
		private bool cantRemoveFirstWaypoint;

		// Token: 0x04005AF1 RID: 23281
		private const int MaxCount = 25;

		// Token: 0x04005AF2 RID: 23282
		private static readonly Texture2D ButtonTex = ContentFinder<Texture2D>.Get("UI/Misc/WorldRoutePlanner", true);

		// Token: 0x04005AF3 RID: 23283
		private static readonly Texture2D MouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/WaypointMouseAttachment", true);

		// Token: 0x04005AF4 RID: 23284
		private static readonly Vector2 BottomWindowSize = new Vector2(500f, 95f);

		// Token: 0x04005AF5 RID: 23285
		private static readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04005AF6 RID: 23286
		private const float BottomWindowBotMargin = 45f;

		// Token: 0x04005AF7 RID: 23287
		private const float BottomWindowEntryExtraBotMargin = 22f;
	}
}
