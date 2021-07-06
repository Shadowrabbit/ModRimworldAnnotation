using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001BF1 RID: 7153
	[StaticConstructorOnStartup]
	public class WorldTargeter
	{
		// Token: 0x170018B5 RID: 6325
		// (get) Token: 0x06009D6C RID: 40300 RVA: 0x00068CD6 File Offset: 0x00066ED6
		public bool IsTargeting
		{
			get
			{
				return this.action != null;
			}
		}

		// Token: 0x06009D6D RID: 40301 RVA: 0x00068CE1 File Offset: 0x00066EE1
		public void BeginTargeting_NewTemp(Func<GlobalTargetInfo, bool> action, bool canTargetTiles, Texture2D mouseAttachment = null, bool closeWorldTabWhenFinished = false, Action onUpdate = null, Func<GlobalTargetInfo, string> extraLabelGetter = null, Func<GlobalTargetInfo, bool> canSelectTarget = null)
		{
			this.action = action;
			this.canTargetTiles = canTargetTiles;
			this.mouseAttachment = mouseAttachment;
			this.closeWorldTabWhenFinished = closeWorldTabWhenFinished;
			this.onUpdate = onUpdate;
			this.extraLabelGetter = extraLabelGetter;
			this.canSelectTarget = canSelectTarget;
		}

		// Token: 0x06009D6E RID: 40302 RVA: 0x00068D18 File Offset: 0x00066F18
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public void BeginTargeting(Func<GlobalTargetInfo, bool> action, bool canTargetTiles, Texture2D mouseAttachment = null, bool closeWorldTabWhenFinished = false, Action onUpdate = null, Func<GlobalTargetInfo, string> extraLabelGetter = null)
		{
			this.BeginTargeting_NewTemp(action, canTargetTiles, mouseAttachment, closeWorldTabWhenFinished, onUpdate, extraLabelGetter, null);
		}

		// Token: 0x06009D6F RID: 40303 RVA: 0x00068D2A File Offset: 0x00066F2A
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

		// Token: 0x06009D70 RID: 40304 RVA: 0x002E0F1C File Offset: 0x002DF11C
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

		// Token: 0x06009D71 RID: 40305 RVA: 0x002E0FF4 File Offset: 0x002DF1F4
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

		// Token: 0x06009D72 RID: 40306 RVA: 0x002E1120 File Offset: 0x002DF320
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

		// Token: 0x06009D73 RID: 40307 RVA: 0x00068D64 File Offset: 0x00066F64
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

		// Token: 0x06009D74 RID: 40308 RVA: 0x002E11D8 File Offset: 0x002DF3D8
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

		// Token: 0x04006432 RID: 25650
		private Func<GlobalTargetInfo, bool> action;

		// Token: 0x04006433 RID: 25651
		private bool canTargetTiles;

		// Token: 0x04006434 RID: 25652
		private Texture2D mouseAttachment;

		// Token: 0x04006435 RID: 25653
		public bool closeWorldTabWhenFinished;

		// Token: 0x04006436 RID: 25654
		private Action onUpdate;

		// Token: 0x04006437 RID: 25655
		private Func<GlobalTargetInfo, string> extraLabelGetter;

		// Token: 0x04006438 RID: 25656
		private Func<GlobalTargetInfo, bool> canSelectTarget;

		// Token: 0x04006439 RID: 25657
		private const float BaseFeedbackTexSize = 0.8f;
	}
}
