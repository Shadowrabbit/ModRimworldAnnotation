using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200135D RID: 4957
	public class Selector
	{
		// Token: 0x17001518 RID: 5400
		// (get) Token: 0x0600781A RID: 30746 RVA: 0x002A561B File Offset: 0x002A381B
		private bool ShiftIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			}
		}

		// Token: 0x17001519 RID: 5401
		// (get) Token: 0x0600781B RID: 30747 RVA: 0x002A5635 File Offset: 0x002A3835
		public List<object> SelectedObjects
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x1700151A RID: 5402
		// (get) Token: 0x0600781C RID: 30748 RVA: 0x002A5635 File Offset: 0x002A3835
		public List<object> SelectedObjectsListForReading
		{
			get
			{
				return this.selected;
			}
		}

		// Token: 0x1700151B RID: 5403
		// (get) Token: 0x0600781D RID: 30749 RVA: 0x002A563D File Offset: 0x002A383D
		public Thing SingleSelectedThing
		{
			get
			{
				if (this.selected.Count != 1)
				{
					return null;
				}
				if (this.selected[0] is Thing)
				{
					return (Thing)this.selected[0];
				}
				return null;
			}
		}

		// Token: 0x1700151C RID: 5404
		// (get) Token: 0x0600781E RID: 30750 RVA: 0x002A5675 File Offset: 0x002A3875
		public object FirstSelectedObject
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

		// Token: 0x1700151D RID: 5405
		// (get) Token: 0x0600781F RID: 30751 RVA: 0x002A5692 File Offset: 0x002A3892
		public object SingleSelectedObject
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

		// Token: 0x1700151E RID: 5406
		// (get) Token: 0x06007820 RID: 30752 RVA: 0x002A56B0 File Offset: 0x002A38B0
		public List<Pawn> SelectedPawns
		{
			get
			{
				Selector.tmpSelectedPawns.Clear();
				for (int i = 0; i < this.selected.Count; i++)
				{
					Pawn item;
					if ((item = (this.selected[i] as Pawn)) != null)
					{
						Selector.tmpSelectedPawns.Add(item);
					}
				}
				return Selector.tmpSelectedPawns;
			}
		}

		// Token: 0x1700151F RID: 5407
		// (get) Token: 0x06007821 RID: 30753 RVA: 0x002A5702 File Offset: 0x002A3902
		public int NumSelected
		{
			get
			{
				return this.selected.Count;
			}
		}

		// Token: 0x17001520 RID: 5408
		// (get) Token: 0x06007822 RID: 30754 RVA: 0x002A570F File Offset: 0x002A390F
		// (set) Token: 0x06007823 RID: 30755 RVA: 0x002A5731 File Offset: 0x002A3931
		public Zone SelectedZone
		{
			get
			{
				if (this.selected.Count == 0)
				{
					return null;
				}
				return this.selected[0] as Zone;
			}
			set
			{
				this.ClearSelection();
				if (value != null)
				{
					this.Select(value, true, true);
				}
			}
		}

		// Token: 0x06007824 RID: 30756 RVA: 0x002A5748 File Offset: 0x002A3948
		public void SelectorOnGUI()
		{
			this.HandleMapClicks();
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.selected.Count > 0)
			{
				this.ClearSelection();
				Event.current.Use();
			}
			if (this.NumSelected > 0 && Find.MainTabsRoot.OpenTab == null && !WorldRendererUtility.WorldRenderedNow)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Inspect, false);
			}
		}

		// Token: 0x06007825 RID: 30757 RVA: 0x002A57B1 File Offset: 0x002A39B1
		public void SelectorOnGUI_BeforeMainTabs()
		{
			if (this.gotoController.Active)
			{
				this.gotoController.OnGUI();
			}
		}

		// Token: 0x06007826 RID: 30758 RVA: 0x002A57CC File Offset: 0x002A39CC
		private void HandleMapClicks()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					if (Event.current.clickCount == 1)
					{
						this.dragBox.active = true;
						this.dragBox.start = UI.MouseMapPosition();
					}
					if (Event.current.clickCount == 2)
					{
						this.SelectAllMatchingObjectUnderMouseOnScreen();
					}
					Event.current.Use();
				}
				if (Event.current.button == 1 && this.selected.Count > 0)
				{
					if (this.selected.Count == 1 && this.selected[0] is Pawn)
					{
						FloatMenuMakerMap.TryMakeFloatMenu((Pawn)this.selected[0]);
					}
					else if (!FloatMenuMakerMap.TryMakeMultiSelectFloatMenu(this.SelectedPawns))
					{
						this.MassTakeFirstAutoTakeableOptionOrGoto();
					}
					Event.current.Use();
				}
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				if (Event.current.button == 0)
				{
					if (this.dragBox.active)
					{
						this.dragBox.active = false;
						if (!this.dragBox.IsValid)
						{
							this.SelectUnderMouse();
						}
						else
						{
							this.SelectInsideDragBox();
						}
					}
				}
				else if (Event.current.button == 1 && this.gotoController.Active)
				{
					this.gotoController.FinalizeInteraction();
				}
				Event.current.Use();
			}
			if (this.gotoController.Active)
			{
				this.gotoController.ProcessInputEvents();
			}
		}

		// Token: 0x06007827 RID: 30759 RVA: 0x002A5941 File Offset: 0x002A3B41
		public bool IsSelected(object obj)
		{
			return this.selected.Contains(obj);
		}

		// Token: 0x06007828 RID: 30760 RVA: 0x002A594F File Offset: 0x002A3B4F
		public void ClearSelection()
		{
			SelectionDrawer.Clear();
			this.selected.Clear();
			this.shelved.Clear();
			this.gotoController.Deactivate();
		}

		// Token: 0x06007829 RID: 30761 RVA: 0x002A5977 File Offset: 0x002A3B77
		public void Deselect(object obj)
		{
			this.DeselectInternal(obj, true);
		}

		// Token: 0x0600782A RID: 30762 RVA: 0x002A5981 File Offset: 0x002A3B81
		private void DeselectInternal(object obj, bool clearShelfOnRemove = true)
		{
			if (this.selected.Contains(obj))
			{
				this.selected.Remove(obj);
				if (clearShelfOnRemove)
				{
					this.shelved.Clear();
				}
			}
		}

		// Token: 0x0600782B RID: 30763 RVA: 0x002A59AC File Offset: 0x002A3BAC
		public void ShelveSelected(object obj)
		{
			if (!this.IsSelected(obj))
			{
				return;
			}
			this.DeselectInternal(obj, false);
			this.shelved.Add(obj);
		}

		// Token: 0x0600782C RID: 30764 RVA: 0x002A59CC File Offset: 0x002A3BCC
		public void Unshelve(object obj, bool playSound = true, bool forceDesignatorDeselect = true)
		{
			if (this.shelved.Contains(obj))
			{
				this.shelved.Remove(obj);
				this.SelectInternal(obj, playSound, forceDesignatorDeselect, false);
			}
		}

		// Token: 0x0600782D RID: 30765 RVA: 0x002A59F3 File Offset: 0x002A3BF3
		public void Select(object obj, bool playSound = true, bool forceDesignatorDeselect = true)
		{
			this.SelectInternal(obj, playSound, forceDesignatorDeselect, true);
		}

		// Token: 0x0600782E RID: 30766 RVA: 0x002A5A00 File Offset: 0x002A3C00
		private void SelectInternal(object obj, bool playSound = true, bool forceDesignatorDeselect = true, bool clearShelfOnAdd = true)
		{
			if (obj == null)
			{
				Log.Error("Cannot select null.");
				return;
			}
			Thing thing = obj as Thing;
			if (thing == null && !(obj is Zone))
			{
				Log.Error("Tried to select " + obj + " which is neither a Thing nor a Zone.");
				return;
			}
			if (thing != null && thing.Destroyed)
			{
				Log.Error("Cannot select destroyed thing.");
				return;
			}
			Pawn pawn = obj as Pawn;
			if (pawn != null && pawn.IsWorldPawn())
			{
				Log.Error("Cannot select world pawns.");
				return;
			}
			if (forceDesignatorDeselect)
			{
				Find.DesignatorManager.Deselect();
			}
			if (this.SelectedZone != null && !(obj is Zone))
			{
				this.ClearSelection();
			}
			if (obj is Zone && this.SelectedZone == null)
			{
				this.ClearSelection();
			}
			Map map = (thing != null) ? thing.Map : ((Zone)obj).Map;
			for (int i = this.selected.Count - 1; i >= 0; i--)
			{
				Thing thing2 = this.selected[i] as Thing;
				if (((thing2 != null) ? thing2.Map : ((Zone)this.selected[i]).Map) != map)
				{
					this.Deselect(this.selected[i]);
				}
			}
			if (this.selected.Count >= 200)
			{
				return;
			}
			if (!this.IsSelected(obj))
			{
				if (map != Find.CurrentMap)
				{
					Current.Game.CurrentMap = map;
					SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
					IntVec3 cell = (thing != null) ? thing.Position : ((Zone)obj).Cells[0];
					Find.CameraDriver.JumpToCurrentMapLoc(cell);
				}
				if (playSound)
				{
					this.PlaySelectionSoundFor(obj);
				}
				this.selected.Add(obj);
				if (clearShelfOnAdd)
				{
					this.shelved.Clear();
				}
				SelectionDrawer.Notify_Selected(obj);
			}
		}

		// Token: 0x0600782F RID: 30767 RVA: 0x002A5BB8 File Offset: 0x002A3DB8
		public void Notify_DialogOpened()
		{
			this.dragBox.active = false;
			this.gotoController.Deactivate();
		}

		// Token: 0x06007830 RID: 30768 RVA: 0x002A5BD4 File Offset: 0x002A3DD4
		private void PlaySelectionSoundFor(object obj)
		{
			if (obj is Pawn && ((Pawn)obj).Faction == Faction.OfPlayer && ((Pawn)obj).RaceProps.Humanlike)
			{
				SoundDefOf.ColonistSelected.PlayOneShotOnCamera(null);
				return;
			}
			if (obj is Thing || obj is Zone)
			{
				SoundDefOf.ThingSelected.PlayOneShotOnCamera(null);
				return;
			}
			Log.Warning("Can't determine selection sound for " + obj);
		}

		// Token: 0x06007831 RID: 30769 RVA: 0x002A5C48 File Offset: 0x002A3E48
		private void SelectInsideDragBox()
		{
			Selector.<>c__DisplayClass40_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			if (!this.ShiftIsHeld)
			{
				this.ClearSelection();
			}
			CS$<>8__locals1.selectedSomething = false;
			List<Thing> list = Find.ColonistBar.MapColonistsOrCorpsesInScreenRect(this.dragBox.ScreenRect);
			for (int i = 0; i < list.Count; i++)
			{
				CS$<>8__locals1.selectedSomething = true;
				this.Select(list[i], true, true);
			}
			if (CS$<>8__locals1.selectedSomething)
			{
				return;
			}
			List<Caravan> list2 = Find.ColonistBar.CaravanMembersCaravansInScreenRect(this.dragBox.ScreenRect);
			for (int j = 0; j < list2.Count; j++)
			{
				if (!CS$<>8__locals1.selectedSomething)
				{
					CameraJumper.TryJumpAndSelect(list2[j]);
					CS$<>8__locals1.selectedSomething = true;
				}
				else
				{
					Find.WorldSelector.Select(list2[j], true);
				}
			}
			if (CS$<>8__locals1.selectedSomething)
			{
				return;
			}
			CS$<>8__locals1.boxThings = ThingSelectionUtility.MultiSelectableThingsInScreenRectDistinct(this.dragBox.ScreenRect).ToList<Thing>();
			Predicate<Thing> predicate = (Thing t) => t.def.category == ThingCategory.Pawn && ((Pawn)t).RaceProps.Humanlike && t.Faction == Faction.OfPlayer;
			if (this.<SelectInsideDragBox>g__SelectWhere|40_0(predicate, new Action<List<Thing>>(SelectorUtility.SortInColonistBarOrder), ref CS$<>8__locals1))
			{
				return;
			}
			Predicate<Thing> predicate2 = (Thing t) => t.def.category == ThingCategory.Pawn && ((Pawn)t).RaceProps.Humanlike;
			if (this.<SelectInsideDragBox>g__SelectWhere|40_0(predicate2, null, ref CS$<>8__locals1))
			{
				return;
			}
			Predicate<Thing> predicate3 = (Thing t) => t.def.CountAsResource;
			if (this.<SelectInsideDragBox>g__SelectWhere|40_0(predicate3, null, ref CS$<>8__locals1))
			{
				return;
			}
			Predicate<Thing> predicate4 = (Thing t) => t.def.category == ThingCategory.Pawn;
			if (this.<SelectInsideDragBox>g__SelectWhere|40_0(predicate4, null, ref CS$<>8__locals1))
			{
				return;
			}
			if (this.<SelectInsideDragBox>g__SelectWhere|40_0((Thing t) => t.def.selectable, null, ref CS$<>8__locals1))
			{
				return;
			}
			foreach (Zone obj in ThingSelectionUtility.MultiSelectableZonesInScreenRectDistinct(this.dragBox.ScreenRect).ToList<Zone>())
			{
				CS$<>8__locals1.selectedSomething = true;
				this.Select(obj, true, true);
			}
			if (CS$<>8__locals1.selectedSomething)
			{
				return;
			}
			this.SelectUnderMouse();
		}

		// Token: 0x06007832 RID: 30770 RVA: 0x002A5EA8 File Offset: 0x002A40A8
		private IEnumerable<object> SelectableObjectsUnderMouse()
		{
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			Thing thing = Find.ColonistBar.ColonistOrCorpseAt(mousePositionOnUIInverted);
			if (thing != null && thing.Spawned)
			{
				yield return thing;
				yield break;
			}
			if (!UI.MouseCell().InBounds(Find.CurrentMap))
			{
				yield break;
			}
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.mustBeSelectable = true;
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = true;
			targetingParameters.canTargetItems = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			List<Thing> selectableList = GenUI.ThingsUnderMouse(UI.MouseMapPosition(), 1f, targetingParameters, null);
			if (selectableList.Count > 0 && selectableList[0] is Pawn && (selectableList[0].DrawPos - UI.MouseMapPosition()).MagnitudeHorizontal() < 0.4f)
			{
				for (int j = selectableList.Count - 1; j >= 0; j--)
				{
					Thing thing2 = selectableList[j];
					if (thing2.def.category == ThingCategory.Pawn && (thing2.DrawPos - UI.MouseMapPosition()).MagnitudeHorizontal() > 0.4f)
					{
						selectableList.Remove(thing2);
					}
				}
			}
			int num;
			for (int i = 0; i < selectableList.Count; i = num + 1)
			{
				yield return selectableList[i];
				num = i;
			}
			Zone zone = Find.CurrentMap.zoneManager.ZoneAt(UI.MouseCell());
			if (zone != null)
			{
				yield return zone;
			}
			yield break;
		}

		// Token: 0x06007833 RID: 30771 RVA: 0x002A5EB1 File Offset: 0x002A40B1
		public static IEnumerable<object> SelectableObjectsAt(IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num;
			for (int i = 0; i < thingList.Count; i = num + 1)
			{
				Thing thing = thingList[i];
				if (ThingSelectionUtility.SelectableByMapClick(thing))
				{
					yield return thing;
				}
				num = i;
			}
			Zone zone = map.zoneManager.ZoneAt(c);
			if (zone != null)
			{
				yield return zone;
			}
			yield break;
		}

		// Token: 0x06007834 RID: 30772 RVA: 0x002A5EC8 File Offset: 0x002A40C8
		private void SelectUnderMouse()
		{
			Caravan caravan = Find.ColonistBar.CaravanMemberCaravanAt(UI.MousePositionOnUIInverted);
			if (caravan != null)
			{
				CameraJumper.TryJumpAndSelect(caravan);
				return;
			}
			Thing thing = Find.ColonistBar.ColonistOrCorpseAt(UI.MousePositionOnUIInverted);
			if (thing != null && !thing.Spawned)
			{
				CameraJumper.TryJump(thing);
				return;
			}
			List<object> list = this.SelectableObjectsUnderMouse().ToList<object>();
			if (list.Count == 0)
			{
				if (!this.ShiftIsHeld)
				{
					this.ClearSelection();
					return;
				}
			}
			else if (list.Count == 1)
			{
				object obj4 = list[0];
				if (!this.ShiftIsHeld)
				{
					this.ClearSelection();
					this.Select(obj4, true, true);
					return;
				}
				if (!this.selected.Contains(obj4))
				{
					this.Select(obj4, true, true);
					return;
				}
				this.Deselect(obj4);
				return;
			}
			else if (list.Count > 1)
			{
				object obj2 = (from obj in list
				where this.selected.Contains(obj)
				select obj).FirstOrDefault<object>();
				if (obj2 != null)
				{
					if (!this.ShiftIsHeld)
					{
						int num = list.IndexOf(obj2) + 1;
						if (num >= list.Count)
						{
							num -= list.Count;
						}
						this.ClearSelection();
						this.Select(list[num], true, true);
						return;
					}
					using (List<object>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj3 = enumerator.Current;
							if (this.selected.Contains(obj3))
							{
								this.Deselect(obj3);
							}
						}
						return;
					}
				}
				if (!this.ShiftIsHeld)
				{
					this.ClearSelection();
				}
				this.Select(list[0], true, true);
			}
		}

		// Token: 0x06007835 RID: 30773 RVA: 0x002A6068 File Offset: 0x002A4268
		public void SelectNextAt(IntVec3 c, Map map)
		{
			if (this.SelectedObjects.Count<object>() != 1)
			{
				Log.Error("Cannot select next at with < or > 1 selected.");
				return;
			}
			List<object> list = Selector.SelectableObjectsAt(c, map).ToList<object>();
			int num = list.IndexOf(this.SingleSelectedThing) + 1;
			if (num >= list.Count)
			{
				num -= list.Count;
			}
			this.ClearSelection();
			this.Select(list[num], true, true);
		}

		// Token: 0x06007836 RID: 30774 RVA: 0x002A60D4 File Offset: 0x002A42D4
		private void SelectAllMatchingObjectUnderMouseOnScreen()
		{
			List<object> list = this.SelectableObjectsUnderMouse().ToList<object>();
			if (list.Count == 0)
			{
				return;
			}
			Thing clickedThing = list.FirstOrDefault((object o) => o is Pawn && ((Pawn)o).Faction == Faction.OfPlayer && !((Pawn)o).IsPrisoner) as Thing;
			clickedThing = (list.FirstOrDefault((object o) => o is Pawn) as Thing);
			if (clickedThing == null)
			{
				clickedThing = ((from o in list
				where o is Thing && !((Thing)o).def.neverMultiSelect
				select o).FirstOrDefault<object>() as Thing);
			}
			Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
			if (clickedThing != null)
			{
				IEnumerable enumerable = ThingSelectionUtility.MultiSelectableThingsInScreenRectDistinct(rect);
				Predicate<Thing> predicate = delegate(Thing t)
				{
					Thing innerIfMinified = clickedThing.GetInnerIfMinified();
					if (t.def != innerIfMinified.def || t.Faction != innerIfMinified.Faction || this.IsSelected(t))
					{
						return false;
					}
					Pawn pawn = innerIfMinified as Pawn;
					if (pawn != null)
					{
						Pawn pawn2 = t as Pawn;
						if (pawn2.RaceProps != pawn.RaceProps)
						{
							return false;
						}
						if (pawn2.HostFaction != pawn.HostFaction)
						{
							return false;
						}
					}
					return true;
				};
				foreach (object obj in enumerable)
				{
					Thing thing = (Thing)obj;
					if (predicate(thing.GetInnerIfMinified()))
					{
						this.Select(thing, true, true);
					}
				}
				return;
			}
			if (list.FirstOrDefault((object o) => o is Zone && ((Zone)o).IsMultiselectable) == null)
			{
				return;
			}
			foreach (Zone obj2 in ThingSelectionUtility.MultiSelectableZonesInScreenRectDistinct(rect))
			{
				if (!this.IsSelected(obj2))
				{
					this.Select(obj2, true, true);
				}
			}
		}

		// Token: 0x06007837 RID: 30775 RVA: 0x002A62B0 File Offset: 0x002A44B0
		private void MassTakeFirstAutoTakeableOptionOrGoto()
		{
			List<Pawn> selectedPawns = this.SelectedPawns;
			if (!selectedPawns.Any<Pawn>())
			{
				return;
			}
			Map map = selectedPawns[0].Map;
			IntVec3 intVec = UI.MouseCell();
			if (!intVec.InBounds(map))
			{
				return;
			}
			IntVec3 mouseCell = CellFinder.StandableCellNear(intVec, map, 2.9f);
			if (mouseCell.IsValid)
			{
				this.gotoController.StartInteraction(mouseCell);
			}
			foreach (Pawn pawn in selectedPawns)
			{
				if (!FloatMenuMakerMap.InvalidPawnForMultiSelectOption(pawn) && !Selector.TakeFirstNonGotoAutoTakeableOption(pawn, intVec) && mouseCell.IsValid && pawn.Drafted)
				{
					this.gotoController.AddPawn(pawn);
				}
			}
		}

		// Token: 0x06007838 RID: 30776 RVA: 0x002A637C File Offset: 0x002A457C
		private static bool TakeFirstNonGotoAutoTakeableOption(Pawn pawn, IntVec3 dest)
		{
			FloatMenuOption floatMenuOption = null;
			foreach (FloatMenuOption floatMenuOption2 in FloatMenuMakerMap.ChoicesAtFor(dest.ToVector3Shifted(), pawn, true))
			{
				if (!floatMenuOption2.Disabled && floatMenuOption2.autoTakeable && (floatMenuOption == null || floatMenuOption2.autoTakeablePriority > floatMenuOption.autoTakeablePriority))
				{
					floatMenuOption = floatMenuOption2;
				}
			}
			if (floatMenuOption != null)
			{
				floatMenuOption.Chosen(true, null);
				return true;
			}
			return false;
		}

		// Token: 0x0600783B RID: 30779 RVA: 0x002A6450 File Offset: 0x002A4650
		[CompilerGenerated]
		private bool <SelectInsideDragBox>g__SelectWhere|40_0(Predicate<Thing> predicate, Action<List<Thing>> postProcessor, ref Selector.<>c__DisplayClass40_0 A_3)
		{
			Selector.tmp_selectedBoxThings.Clear();
			IEnumerable<Thing> boxThings = A_3.boxThings;
			Func<Thing, bool> <>9__6;
			Func<Thing, bool> predicate2;
			if ((predicate2 = <>9__6) == null)
			{
				predicate2 = (<>9__6 = ((Thing t) => predicate(t)));
			}
			foreach (Thing item in boxThings.Where(predicate2))
			{
				Selector.tmp_selectedBoxThings.Add(item);
			}
			if (Selector.tmp_selectedBoxThings.Any<Thing>())
			{
				if (postProcessor != null)
				{
					postProcessor(Selector.tmp_selectedBoxThings);
				}
				foreach (Thing obj in Selector.tmp_selectedBoxThings)
				{
					this.Select(obj, true, true);
					A_3.selectedSomething = true;
				}
			}
			Selector.tmp_selectedBoxThings.Clear();
			return A_3.selectedSomething;
		}

		// Token: 0x040042CF RID: 17103
		public DragBox dragBox = new DragBox();

		// Token: 0x040042D0 RID: 17104
		public MultiPawnGotoController gotoController = new MultiPawnGotoController();

		// Token: 0x040042D1 RID: 17105
		private List<object> selected = new List<object>();

		// Token: 0x040042D2 RID: 17106
		private List<object> shelved = new List<object>();

		// Token: 0x040042D3 RID: 17107
		private const float PawnSelectRadius = 1f;

		// Token: 0x040042D4 RID: 17108
		private const int MaxNumSelected = 200;

		// Token: 0x040042D5 RID: 17109
		private static List<Pawn> tmpSelectedPawns = new List<Pawn>();

		// Token: 0x040042D6 RID: 17110
		private static List<Thing> tmp_selectedBoxThings = new List<Thing>();
	}
}
