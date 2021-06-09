using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020021F1 RID: 8689
	public class WorldSelector
	{
		// Token: 0x17001BB4 RID: 7092
		// (get) Token: 0x0600BA1D RID: 47645 RVA: 0x00065ECD File Offset: 0x000640CD
		private bool ShiftIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			}
		}

		// Token: 0x17001BB5 RID: 7093
		// (get) Token: 0x0600BA1E RID: 47646 RVA: 0x00078907 File Offset: 0x00076B07
		public List<WorldObject> SelectedObjects
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x17001BB6 RID: 7094
		// (get) Token: 0x0600BA1F RID: 47647 RVA: 0x0007890F File Offset: 0x00076B0F
		public WorldObject SingleSelectedObject
		{
			get
			{
				if (this.selected.Count != 1)
				{
					return null;
				}
				return this.selected[0];
			}
		}

		// Token: 0x17001BB7 RID: 7095
		// (get) Token: 0x0600BA20 RID: 47648 RVA: 0x0007892D File Offset: 0x00076B2D
		public WorldObject FirstSelectedObject
		{
			get
			{
				if (this.selected.Count == 0)
				{
					return null;
				}
				return this.selected[0];
			}
		}

		// Token: 0x17001BB8 RID: 7096
		// (get) Token: 0x0600BA21 RID: 47649 RVA: 0x0007894A File Offset: 0x00076B4A
		public int NumSelectedObjects
		{
			get
			{
				return this.selected.Count;
			}
		}

		// Token: 0x17001BB9 RID: 7097
		// (get) Token: 0x0600BA22 RID: 47650 RVA: 0x00078957 File Offset: 0x00076B57
		public bool AnyObjectOrTileSelected
		{
			get
			{
				return this.NumSelectedObjects != 0 || this.selectedTile >= 0;
			}
		}

		// Token: 0x0600BA23 RID: 47651 RVA: 0x0007896F File Offset: 0x00076B6F
		public void WorldSelectorOnGUI()
		{
			this.HandleWorldClicks();
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.selected.Count > 0)
			{
				this.ClearSelection();
				Event.current.Use();
			}
		}

		// Token: 0x0600BA24 RID: 47652 RVA: 0x00358BFC File Offset: 0x00356DFC
		private void HandleWorldClicks()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					if (Event.current.clickCount == 1)
					{
						this.dragBox.active = true;
						this.dragBox.start = UI.MousePositionOnUIInverted;
					}
					if (Event.current.clickCount == 2)
					{
						this.SelectAllMatchingObjectUnderMouseOnScreen();
					}
					Event.current.Use();
				}
				if (Event.current.button == 1 && this.selected.Count > 0)
				{
					if (this.selected.Count == 1 && this.selected[0] is Caravan)
					{
						Caravan caravan = (Caravan)this.selected[0];
						if (caravan.IsPlayerControlled && !FloatMenuMakerWorld.TryMakeFloatMenu(caravan))
						{
							this.AutoOrderToTile(caravan, GenWorld.MouseTile(false));
						}
					}
					else
					{
						for (int i = 0; i < this.selected.Count; i++)
						{
							Caravan caravan2 = this.selected[i] as Caravan;
							if (caravan2 != null && caravan2.IsPlayerControlled)
							{
								this.AutoOrderToTile(caravan2, GenWorld.MouseTile(false));
							}
						}
					}
					Event.current.Use();
				}
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				if (Event.current.button == 0 && this.dragBox.active)
				{
					this.dragBox.active = false;
					if (!this.dragBox.IsValid)
					{
						this.SelectUnderMouse(true);
					}
					else
					{
						this.SelectInsideDragBox();
					}
				}
				Event.current.Use();
			}
		}

		// Token: 0x0600BA25 RID: 47653 RVA: 0x000789A1 File Offset: 0x00076BA1
		public bool IsSelected(WorldObject obj)
		{
			return this.selected.Contains(obj);
		}

		// Token: 0x0600BA26 RID: 47654 RVA: 0x000789AF File Offset: 0x00076BAF
		public void ClearSelection()
		{
			WorldSelectionDrawer.Clear();
			this.selected.Clear();
			this.selectedTile = -1;
		}

		// Token: 0x0600BA27 RID: 47655 RVA: 0x000789C8 File Offset: 0x00076BC8
		public void Deselect(WorldObject obj)
		{
			if (this.selected.Contains(obj))
			{
				this.selected.Remove(obj);
			}
		}

		// Token: 0x0600BA28 RID: 47656 RVA: 0x00358D80 File Offset: 0x00356F80
		public void Select(WorldObject obj, bool playSound = true)
		{
			if (obj == null)
			{
				Log.Error("Cannot select null.", false);
				return;
			}
			this.selectedTile = -1;
			if (this.selected.Count >= 80)
			{
				return;
			}
			if (!this.IsSelected(obj))
			{
				if (playSound)
				{
					this.PlaySelectionSoundFor(obj);
				}
				this.selected.Add(obj);
				WorldSelectionDrawer.Notify_Selected(obj);
			}
		}

		// Token: 0x0600BA29 RID: 47657 RVA: 0x000789E5 File Offset: 0x00076BE5
		public void Notify_DialogOpened()
		{
			this.dragBox.active = false;
		}

		// Token: 0x0600BA2A RID: 47658 RVA: 0x000789F3 File Offset: 0x00076BF3
		private void PlaySelectionSoundFor(WorldObject obj)
		{
			SoundDefOf.ThingSelected.PlayOneShotOnCamera(null);
		}

		// Token: 0x0600BA2B RID: 47659 RVA: 0x00358DD8 File Offset: 0x00356FD8
		private void SelectInsideDragBox()
		{
			if (!this.ShiftIsHeld)
			{
				this.ClearSelection();
			}
			bool flag = false;
			if (Current.ProgramState == ProgramState.Playing)
			{
				List<Caravan> list = Find.ColonistBar.CaravanMembersCaravansInScreenRect(this.dragBox.ScreenRect);
				for (int i = 0; i < list.Count; i++)
				{
					flag = true;
					this.Select(list[i], true);
				}
			}
			if (!flag && Current.ProgramState == ProgramState.Playing)
			{
				List<Thing> list2 = Find.ColonistBar.MapColonistsOrCorpsesInScreenRect(this.dragBox.ScreenRect);
				for (int j = 0; j < list2.Count; j++)
				{
					if (!flag)
					{
						CameraJumper.TryJumpAndSelect(list2[j]);
						flag = true;
					}
					else
					{
						Find.Selector.Select(list2[j], true, true);
					}
				}
			}
			if (!flag)
			{
				List<WorldObject> list3 = WorldObjectSelectionUtility.MultiSelectableWorldObjectsInScreenRectDistinct(this.dragBox.ScreenRect).ToList<WorldObject>();
				if (list3.Any((WorldObject x) => x is Caravan))
				{
					list3.RemoveAll((WorldObject x) => !(x is Caravan));
					if (list3.Any((WorldObject x) => x.Faction == Faction.OfPlayer))
					{
						list3.RemoveAll((WorldObject x) => x.Faction != Faction.OfPlayer);
					}
				}
				for (int k = 0; k < list3.Count; k++)
				{
					flag = true;
					this.Select(list3[k], true);
				}
			}
			if (!flag)
			{
				bool canSelectTile = this.dragBox.Diagonal < 30f;
				this.SelectUnderMouse(canSelectTile);
			}
		}

		// Token: 0x0600BA2C RID: 47660 RVA: 0x00358F9C File Offset: 0x0035719C
		public IEnumerable<WorldObject> SelectableObjectsUnderMouse()
		{
			bool flag;
			bool flag2;
			return this.SelectableObjectsUnderMouse(out flag, out flag2);
		}

		// Token: 0x0600BA2D RID: 47661 RVA: 0x00358FB4 File Offset: 0x003571B4
		public IEnumerable<WorldObject> SelectableObjectsUnderMouse(out bool clickedDirectlyOnCaravan, out bool usedColonistBar)
		{
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			if (Current.ProgramState == ProgramState.Playing)
			{
				Caravan caravan = Find.ColonistBar.CaravanMemberCaravanAt(mousePositionOnUIInverted);
				if (caravan != null)
				{
					clickedDirectlyOnCaravan = true;
					usedColonistBar = true;
					return Gen.YieldSingle<WorldObject>(caravan);
				}
			}
			List<WorldObject> list = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			clickedDirectlyOnCaravan = false;
			if (list.Count > 0 && list[0] is Caravan && list[0].DistanceToMouse(UI.MousePositionOnUI) < GenWorldUI.CaravanDirectClickRadius)
			{
				clickedDirectlyOnCaravan = true;
				for (int i = list.Count - 1; i >= 0; i--)
				{
					WorldObject worldObject = list[i];
					if (worldObject is Caravan && worldObject.DistanceToMouse(UI.MousePositionOnUI) > GenWorldUI.CaravanDirectClickRadius)
					{
						list.Remove(worldObject);
					}
				}
			}
			usedColonistBar = false;
			return list;
		}

		// Token: 0x0600BA2E RID: 47662 RVA: 0x00078A00 File Offset: 0x00076C00
		public static IEnumerable<WorldObject> SelectableObjectsAt(int tileID)
		{
			foreach (WorldObject worldObject in Find.WorldObjects.ObjectsAt(tileID))
			{
				if (worldObject.SelectableNow)
				{
					yield return worldObject;
				}
			}
			IEnumerator<WorldObject> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BA2F RID: 47663 RVA: 0x00359070 File Offset: 0x00357270
		private void SelectUnderMouse(bool canSelectTile = true)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Thing thing = Find.ColonistBar.ColonistOrCorpseAt(UI.MousePositionOnUIInverted);
				Pawn pawn = thing as Pawn;
				if (thing != null && (pawn == null || !pawn.IsCaravanMember()))
				{
					if (thing.Spawned)
					{
						CameraJumper.TryJumpAndSelect(thing);
						return;
					}
					CameraJumper.TryJump(thing);
					return;
				}
			}
			bool flag;
			bool flag2;
			List<WorldObject> list = this.SelectableObjectsUnderMouse(out flag, out flag2).ToList<WorldObject>();
			if (flag2 || (flag && list.Count >= 2))
			{
				canSelectTile = false;
			}
			if (list.Count == 0)
			{
				if (!this.ShiftIsHeld)
				{
					this.ClearSelection();
					if (canSelectTile)
					{
						this.selectedTile = GenWorld.MouseTile(false);
						return;
					}
				}
			}
			else
			{
				if ((from obj in list
				where this.selected.Contains(obj)
				select obj).FirstOrDefault<WorldObject>() != null)
				{
					if (!this.ShiftIsHeld)
					{
						int tile = canSelectTile ? GenWorld.MouseTile(false) : -1;
						this.SelectFirstOrNextFrom(list, tile);
						return;
					}
					using (List<WorldObject>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							WorldObject worldObject = enumerator.Current;
							if (this.selected.Contains(worldObject))
							{
								this.Deselect(worldObject);
							}
						}
						return;
					}
				}
				if (!this.ShiftIsHeld)
				{
					this.ClearSelection();
				}
				this.Select(list[0], true);
			}
		}

		// Token: 0x0600BA30 RID: 47664 RVA: 0x00078A10 File Offset: 0x00076C10
		public void SelectFirstOrNextAt(int tileID)
		{
			this.SelectFirstOrNextFrom(WorldSelector.SelectableObjectsAt(tileID).ToList<WorldObject>(), tileID);
		}

		// Token: 0x0600BA31 RID: 47665 RVA: 0x003591C4 File Offset: 0x003573C4
		private void SelectAllMatchingObjectUnderMouseOnScreen()
		{
			List<WorldObject> list = this.SelectableObjectsUnderMouse().ToList<WorldObject>();
			if (list.Count == 0)
			{
				return;
			}
			Type type = list[0].GetType();
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				if (!(type != allWorldObjects[i].GetType()) && (allWorldObjects[i] == list[0] || allWorldObjects[i].AllMatchingObjectsOnScreenMatchesWith(list[0])) && allWorldObjects[i].VisibleToCameraNow())
				{
					this.Select(allWorldObjects[i], true);
				}
			}
		}

		// Token: 0x0600BA32 RID: 47666 RVA: 0x00359264 File Offset: 0x00357464
		private void AutoOrderToTile(Caravan c, int tile)
		{
			if (tile < 0)
			{
				return;
			}
			if (c.autoJoinable && CaravanExitMapUtility.AnyoneTryingToJoinCaravan(c))
			{
				CaravanExitMapUtility.OpenSomeoneTryingToJoinCaravanDialog(c, delegate
				{
					this.AutoOrderToTileNow(c, tile);
				});
				return;
			}
			this.AutoOrderToTileNow(c, tile);
		}

		// Token: 0x0600BA33 RID: 47667 RVA: 0x003592DC File Offset: 0x003574DC
		private void AutoOrderToTileNow(Caravan c, int tile)
		{
			if (tile < 0 || (tile == c.Tile && !c.pather.Moving))
			{
				return;
			}
			int num = CaravanUtility.BestGotoDestNear(tile, c);
			if (num >= 0)
			{
				c.pather.StartPath(num, null, true, true);
				c.gotoMote.OrderedToTile(num);
				SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600BA34 RID: 47668 RVA: 0x00359338 File Offset: 0x00357538
		private void SelectFirstOrNextFrom(List<WorldObject> objects, int tile)
		{
			int num = objects.FindIndex((WorldObject x) => this.selected.Contains(x));
			int num2 = -1;
			int num3 = -1;
			if (num != -1)
			{
				if (num == objects.Count - 1 || this.selected.Count >= 2)
				{
					if (this.selected.Count >= 2)
					{
						num3 = 0;
					}
					else if (tile >= 0)
					{
						num2 = tile;
					}
					else
					{
						num3 = 0;
					}
				}
				else
				{
					num3 = num + 1;
				}
			}
			else if (objects.Count == 0)
			{
				num2 = tile;
			}
			else
			{
				num3 = 0;
			}
			this.ClearSelection();
			if (num3 >= 0)
			{
				this.Select(objects[num3], true);
			}
			this.selectedTile = num2;
		}

		// Token: 0x04007F1B RID: 32539
		public WorldDragBox dragBox = new WorldDragBox();

		// Token: 0x04007F1C RID: 32540
		private List<WorldObject> selected = new List<WorldObject>();

		// Token: 0x04007F1D RID: 32541
		public int selectedTile = -1;

		// Token: 0x04007F1E RID: 32542
		private const int MaxNumSelected = 80;

		// Token: 0x04007F1F RID: 32543
		private const float MaxDragBoxDiagonalToSelectTile = 30f;
	}
}
