using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x0200181D RID: 6173
	public class WorldSelector
	{
		// Token: 0x170017D2 RID: 6098
		// (get) Token: 0x060090BC RID: 37052 RVA: 0x002A561B File Offset: 0x002A381B
		private bool ShiftIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			}
		}

		// Token: 0x170017D3 RID: 6099
		// (get) Token: 0x060090BD RID: 37053 RVA: 0x0033ECEB File Offset: 0x0033CEEB
		public List<WorldObject> SelectedObjects
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x170017D4 RID: 6100
		// (get) Token: 0x060090BE RID: 37054 RVA: 0x0033ECF3 File Offset: 0x0033CEF3
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

		// Token: 0x170017D5 RID: 6101
		// (get) Token: 0x060090BF RID: 37055 RVA: 0x0033ED11 File Offset: 0x0033CF11
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

		// Token: 0x170017D6 RID: 6102
		// (get) Token: 0x060090C0 RID: 37056 RVA: 0x0033ED2E File Offset: 0x0033CF2E
		public int NumSelectedObjects
		{
			get
			{
				return this.selected.Count;
			}
		}

		// Token: 0x170017D7 RID: 6103
		// (get) Token: 0x060090C1 RID: 37057 RVA: 0x0033ED3B File Offset: 0x0033CF3B
		public bool AnyObjectOrTileSelected
		{
			get
			{
				return this.NumSelectedObjects != 0 || this.selectedTile >= 0;
			}
		}

		// Token: 0x060090C2 RID: 37058 RVA: 0x0033ED53 File Offset: 0x0033CF53
		public void WorldSelectorOnGUI()
		{
			this.HandleWorldClicks();
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.selected.Count > 0)
			{
				this.ClearSelection();
				Event.current.Use();
			}
		}

		// Token: 0x060090C3 RID: 37059 RVA: 0x0033ED88 File Offset: 0x0033CF88
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

		// Token: 0x060090C4 RID: 37060 RVA: 0x0033EF0B File Offset: 0x0033D10B
		public bool IsSelected(WorldObject obj)
		{
			return this.selected.Contains(obj);
		}

		// Token: 0x060090C5 RID: 37061 RVA: 0x0033EF19 File Offset: 0x0033D119
		public void ClearSelection()
		{
			WorldSelectionDrawer.Clear();
			this.selected.Clear();
			this.selectedTile = -1;
		}

		// Token: 0x060090C6 RID: 37062 RVA: 0x0033EF32 File Offset: 0x0033D132
		public void Deselect(WorldObject obj)
		{
			if (this.selected.Contains(obj))
			{
				this.selected.Remove(obj);
			}
		}

		// Token: 0x060090C7 RID: 37063 RVA: 0x0033EF50 File Offset: 0x0033D150
		public void Select(WorldObject obj, bool playSound = true)
		{
			if (obj == null)
			{
				Log.Error("Cannot select null.");
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

		// Token: 0x060090C8 RID: 37064 RVA: 0x0033EFA7 File Offset: 0x0033D1A7
		public void Notify_DialogOpened()
		{
			this.dragBox.active = false;
		}

		// Token: 0x060090C9 RID: 37065 RVA: 0x0033EFB5 File Offset: 0x0033D1B5
		private void PlaySelectionSoundFor(WorldObject obj)
		{
			SoundDefOf.ThingSelected.PlayOneShotOnCamera(null);
		}

		// Token: 0x060090CA RID: 37066 RVA: 0x0033EFC4 File Offset: 0x0033D1C4
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

		// Token: 0x060090CB RID: 37067 RVA: 0x0033F188 File Offset: 0x0033D388
		public IEnumerable<WorldObject> SelectableObjectsUnderMouse()
		{
			bool flag;
			bool flag2;
			return this.SelectableObjectsUnderMouse(out flag, out flag2);
		}

		// Token: 0x060090CC RID: 37068 RVA: 0x0033F1A0 File Offset: 0x0033D3A0
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

		// Token: 0x060090CD RID: 37069 RVA: 0x0033F25C File Offset: 0x0033D45C
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

		// Token: 0x060090CE RID: 37070 RVA: 0x0033F26C File Offset: 0x0033D46C
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
					int num = this.selectedTile;
					this.ClearSelection();
					if (canSelectTile)
					{
						this.selectedTile = GenWorld.MouseTile(false);
						if (num != this.selectedTile && this.selectedTile != -1)
						{
							SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
							return;
						}
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

		// Token: 0x060090CF RID: 37071 RVA: 0x0033F3EC File Offset: 0x0033D5EC
		public void SelectFirstOrNextAt(int tileID)
		{
			this.SelectFirstOrNextFrom(WorldSelector.SelectableObjectsAt(tileID).ToList<WorldObject>(), tileID);
		}

		// Token: 0x060090D0 RID: 37072 RVA: 0x0033F400 File Offset: 0x0033D600
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

		// Token: 0x060090D1 RID: 37073 RVA: 0x0033F4A0 File Offset: 0x0033D6A0
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

		// Token: 0x060090D2 RID: 37074 RVA: 0x0033F518 File Offset: 0x0033D718
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

		// Token: 0x060090D3 RID: 37075 RVA: 0x0033F574 File Offset: 0x0033D774
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

		// Token: 0x04005AFE RID: 23294
		public WorldDragBox dragBox = new WorldDragBox();

		// Token: 0x04005AFF RID: 23295
		private List<WorldObject> selected = new List<WorldObject>();

		// Token: 0x04005B00 RID: 23296
		public int selectedTile = -1;

		// Token: 0x04005B01 RID: 23297
		private const int MaxNumSelected = 80;

		// Token: 0x04005B02 RID: 23298
		private const float MaxDragBoxDiagonalToSelectTile = 30f;
	}
}
