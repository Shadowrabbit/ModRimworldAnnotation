using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013E2 RID: 5090
	[StaticConstructorOnStartup]
	public class WorldTargeter
	{
		// Token: 0x170015AD RID: 5549
		// (get) Token: 0x06007BCC RID: 31692 RVA: 0x002BA8B5 File Offset: 0x002B8AB5
		public bool IsTargeting
		{
			get
			{
				return this.action != null;
			}
		}

		// Token: 0x06007BCD RID: 31693 RVA: 0x002BA8C0 File Offset: 0x002B8AC0
		public void BeginTargeting(Func<GlobalTargetInfo, bool> action, bool canTargetTiles, Texture2D mouseAttachment = null, bool closeWorldTabWhenFinished = false, Action onUpdate = null, Func<GlobalTargetInfo, string> extraLabelGetter = null, Func<GlobalTargetInfo, bool> canSelectTarget = null)
		{
			this.action = action;
			this.canTargetTiles = canTargetTiles;
			this.mouseAttachment = mouseAttachment;
			this.closeWorldTabWhenFinished = closeWorldTabWhenFinished;
			this.onUpdate = onUpdate;
			this.extraLabelGetter = extraLabelGetter;
			this.canSelectTarget = canSelectTarget;
		}

		// Token: 0x06007BCE RID: 31694 RVA: 0x002BA8F7 File Offset: 0x002B8AF7
		public void StopTargeting()
		{
			if (this.closeWorldTabWhenFinished)
			{
				CameraJumper.TryHideWorld();
			}
			this.action = null;
			this.canTargetTiles = false;
			this.mouseAttachment = null;
			this.closeWorldTabWhenFinished = false;
			this.onUpdate = null;
			this.extraLabelGetter = null;
		}

		// Token: 0x06007BCF RID: 31695 RVA: 0x002BA934 File Offset: 0x002B8B34
		public void ProcessInputEvents()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0 && this.IsTargeting)
				{
					GlobalTargetInfo arg = this.CurrentTargetUnderMouse();
					if ((this.canSelectTarget == null || this.canSelectTarget(arg)) && this.action(arg))
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.StopTargeting();
					}
					Event.current.Use();
				}
				if (Event.current.button == 1 && this.IsTargeting)
				{
					SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
					this.StopTargeting();
					Event.current.Use();
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.IsTargeting)
			{
				SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
				this.StopTargeting();
				Event.current.Use();
			}
		}

		// Token: 0x06007BD0 RID: 31696 RVA: 0x002BAA0C File Offset: 0x002B8C0C
		public void TargeterOnGUI()
		{
			if (this.IsTargeting && !Mouse.IsInputBlockedNow)
			{
				Vector2 mousePosition = Event.current.mousePosition;
				Texture2D image = this.mouseAttachment ?? TexCommand.Attack;
				Rect position = new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f);
				GUI.DrawTexture(position, image);
				if (this.extraLabelGetter != null)
				{
					GUI.color = Color.white;
					string text = this.extraLabelGetter(this.CurrentTargetUnderMouse());
					if (!text.NullOrEmpty())
					{
						Color color = GUI.color;
						GUI.color = Color.white;
						Rect rect = new Rect(position.xMax, position.y, 9999f, 100f);
						Vector2 vector = Text.CalcSize(text);
						GUI.DrawTexture(new Rect(rect.x - vector.x * 0.1f, rect.y, vector.x * 1.2f, vector.y), TexUI.GrayTextBG);
						GUI.color = color;
						Widgets.Label(rect, text);
					}
					GUI.color = Color.white;
				}
			}
		}

		// Token: 0x06007BD1 RID: 31697 RVA: 0x002BAB38 File Offset: 0x002B8D38
		public void TargeterUpdate()
		{
			if (this.IsTargeting)
			{
				Vector3 pos = Vector3.zero;
				GlobalTargetInfo arg = this.CurrentTargetUnderMouse();
				if (arg.HasWorldObject)
				{
					pos = arg.WorldObject.DrawPos;
				}
				else if (arg.Tile >= 0)
				{
					pos = Find.WorldGrid.GetTileCenter(arg.Tile);
				}
				if (arg.IsValid && !Mouse.IsInputBlockedNow && (this.canSelectTarget == null || this.canSelectTarget(arg)))
				{
					WorldRendererUtility.DrawQuadTangentialToPlanet(pos, 0.8f * Find.WorldGrid.averageTileSize, 0.018f, WorldMaterials.CurTargetingMat, false, false, null);
				}
				if (this.onUpdate != null)
				{
					this.onUpdate();
				}
			}
		}

		// Token: 0x06007BD2 RID: 31698 RVA: 0x002BABED File Offset: 0x002B8DED
		public bool IsTargetedNow(WorldObject o, List<WorldObject> worldObjectsUnderMouse = null)
		{
			if (!this.IsTargeting)
			{
				return false;
			}
			if (worldObjectsUnderMouse == null)
			{
				worldObjectsUnderMouse = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			}
			return worldObjectsUnderMouse.Any<WorldObject>() && o == worldObjectsUnderMouse[0];
		}

		// Token: 0x06007BD3 RID: 31699 RVA: 0x002BAC1C File Offset: 0x002B8E1C
		private GlobalTargetInfo CurrentTargetUnderMouse()
		{
			if (!this.IsTargeting)
			{
				return GlobalTargetInfo.Invalid;
			}
			List<WorldObject> list = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			if (list.Any<WorldObject>())
			{
				return list[0];
			}
			if (!this.canTargetTiles)
			{
				return GlobalTargetInfo.Invalid;
			}
			int num = GenWorld.MouseTile(false);
			if (num >= 0)
			{
				return new GlobalTargetInfo(num);
			}
			return GlobalTargetInfo.Invalid;
		}

		// Token: 0x0400448B RID: 17547
		private Func<GlobalTargetInfo, bool> action;

		// Token: 0x0400448C RID: 17548
		private bool canTargetTiles;

		// Token: 0x0400448D RID: 17549
		private Texture2D mouseAttachment;

		// Token: 0x0400448E RID: 17550
		public bool closeWorldTabWhenFinished;

		// Token: 0x0400448F RID: 17551
		private Action onUpdate;

		// Token: 0x04004490 RID: 17552
		private Func<GlobalTargetInfo, string> extraLabelGetter;

		// Token: 0x04004491 RID: 17553
		private Func<GlobalTargetInfo, bool> canSelectTarget;

		// Token: 0x04004492 RID: 17554
		private const float BaseFeedbackTexSize = 0.8f;
	}
}
