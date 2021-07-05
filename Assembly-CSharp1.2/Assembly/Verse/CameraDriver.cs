using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000B8 RID: 184
	public class CameraDriver : MonoBehaviour
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x0000AED5 File Offset: 0x000090D5
		private Camera MyCamera
		{
			get
			{
				if (this.cachedCamera == null)
				{
					this.cachedCamera = base.GetComponent<Camera>();
				}
				return this.cachedCamera;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0000AEF7 File Offset: 0x000090F7
		private float ScreenDollyEdgeWidthBottom
		{
			get
			{
				if (Screen.fullScreen)
				{
					return 6f;
				}
				return 20f;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0008C8BC File Offset: 0x0008AABC
		public CameraZoomRange CurrentZoom
		{
			get
			{
				if (this.rootSize < this.config.minSize + 1f)
				{
					return CameraZoomRange.Closest;
				}
				if (this.rootSize < 13.8f)
				{
					return CameraZoomRange.Close;
				}
				if (this.rootSize < 42f)
				{
					return CameraZoomRange.Middle;
				}
				if (this.rootSize < 57f)
				{
					return CameraZoomRange.Far;
				}
				return CameraZoomRange.Furthest;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x0000AF0B File Offset: 0x0000910B
		private Vector3 CurrentRealPosition
		{
			get
			{
				return this.MyCamera.transform.position;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x0000AF1D File Offset: 0x0000911D
		private bool AnythingPreventsCameraMotion
		{
			get
			{
				return Find.WindowStack.WindowsPreventCameraMotion || WorldRendererUtility.WorldRenderedNow;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x0008C914 File Offset: 0x0008AB14
		public IntVec3 MapPosition
		{
			get
			{
				IntVec3 result = this.CurrentRealPosition.ToIntVec3();
				result.y = 0;
				return result;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x0008C938 File Offset: 0x0008AB38
		public CellRect CurrentViewRect
		{
			get
			{
				if (Time.frameCount != CameraDriver.lastViewRectGetFrame)
				{
					CameraDriver.lastViewRect = default(CellRect);
					float num = (float)UI.screenWidth / (float)UI.screenHeight;
					Vector3 currentRealPosition = this.CurrentRealPosition;
					CameraDriver.lastViewRect.minX = Mathf.FloorToInt(currentRealPosition.x - this.rootSize * num - 1f);
					CameraDriver.lastViewRect.maxX = Mathf.CeilToInt(currentRealPosition.x + this.rootSize * num);
					CameraDriver.lastViewRect.minZ = Mathf.FloorToInt(currentRealPosition.z - this.rootSize - 1f);
					CameraDriver.lastViewRect.maxZ = Mathf.CeilToInt(currentRealPosition.z + this.rootSize);
					CameraDriver.lastViewRectGetFrame = Time.frameCount;
				}
				return CameraDriver.lastViewRect;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x0008CA04 File Offset: 0x0008AC04
		public static float HitchReduceFactor
		{
			get
			{
				float result = 1f;
				if (Time.deltaTime > 0.1f)
				{
					result = 0.1f / Time.deltaTime;
				}
				return result;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x0000AF32 File Offset: 0x00009132
		public float CellSizePixels
		{
			get
			{
				return (float)UI.screenHeight / (this.rootSize * 2f);
			}
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0000AF47 File Offset: 0x00009147
		public void Awake()
		{
			this.ResetSize();
			this.reverbDummy = GameObject.Find("ReverbZoneDummy");
			this.ApplyPositionToGameObject();
			this.MyCamera.farClipPlane = 71.5f;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0000AF75 File Offset: 0x00009175
		public void OnPreRender()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			Map currentMap = Find.CurrentMap;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0000AF85 File Offset: 0x00009185
		public void OnPreCull()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				Find.CurrentMap.weatherManager.DrawAllWeather();
			}
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0008CA30 File Offset: 0x0008AC30
		public void CameraDriverOnGUI()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			this.mouseCoveredByUI = false;
			if (Find.WindowStack.GetWindowAt(UI.MousePositionOnUIInverted) != null)
			{
				this.mouseCoveredByUI = true;
			}
			if (!this.AnythingPreventsCameraMotion)
			{
				if (Event.current.type == EventType.MouseDrag && Event.current.button == 2)
				{
					Vector2 currentEventDelta = UnityGUIBugsFixer.CurrentEventDelta;
					Event.current.Use();
					if (currentEventDelta != Vector2.zero)
					{
						currentEventDelta.x *= -1f;
						this.desiredDollyRaw += currentEventDelta / UI.CurUICellSize() * Prefs.MapDragSensitivity;
					}
				}
				float num = 0f;
				if (Event.current.type == EventType.ScrollWheel)
				{
					num -= Event.current.delta.y * 0.35f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.TinyInteraction);
				}
				if (KeyBindingDefOf.MapZoom_In.KeyDownEvent)
				{
					num += 4f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.SmallInteraction);
				}
				if (KeyBindingDefOf.MapZoom_Out.KeyDownEvent)
				{
					num -= 4f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.SmallInteraction);
				}
				this.desiredSize -= num * this.config.zoomSpeed * this.rootSize / 35f;
				this.desiredSize = Mathf.Clamp(this.desiredSize, this.config.minSize, 60f);
				this.desiredDolly = Vector3.zero;
				if (KeyBindingDefOf.MapDolly_Left.IsDown)
				{
					this.desiredDolly.x = -this.config.dollyRateKeys;
				}
				if (KeyBindingDefOf.MapDolly_Right.IsDown)
				{
					this.desiredDolly.x = this.config.dollyRateKeys;
				}
				if (KeyBindingDefOf.MapDolly_Up.IsDown)
				{
					this.desiredDolly.y = this.config.dollyRateKeys;
				}
				if (KeyBindingDefOf.MapDolly_Down.IsDown)
				{
					this.desiredDolly.y = -this.config.dollyRateKeys;
				}
				this.config.ConfigOnGUI();
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0008CC40 File Offset: 0x0008AE40
		public void Update()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				if (Current.SubcameraDriver != null)
				{
					Current.SubcameraDriver.UpdatePositions(this.MyCamera);
				}
				return;
			}
			if (Find.CurrentMap == null)
			{
				return;
			}
			Vector2 vector = this.CalculateCurInputDollyVect();
			if (vector != Vector2.zero)
			{
				float d = (this.rootSize - this.config.minSize) / (60f - this.config.minSize) * 0.7f + 0.3f;
				this.velocity = new Vector3(vector.x, 0f, vector.y) * d;
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraDolly, KnowledgeAmount.FrameInteraction);
			}
			if (!Input.GetMouseButton(2) && this.dragTimeStamps.Any<CameraDriver.DragTimeStamp>())
			{
				Vector2 extraVelocityFromReleasingDragButton = CameraDriver.GetExtraVelocityFromReleasingDragButton(this.dragTimeStamps, 0.75f);
				this.velocity += new Vector3(extraVelocityFromReleasingDragButton.x, 0f, extraVelocityFromReleasingDragButton.y);
				this.dragTimeStamps.Clear();
			}
			if (!this.AnythingPreventsCameraMotion)
			{
				float d2 = Time.deltaTime * CameraDriver.HitchReduceFactor;
				this.rootPos += this.velocity * d2 * this.config.moveSpeedScale;
				this.rootPos += new Vector3(this.desiredDollyRaw.x, 0f, this.desiredDollyRaw.y);
				this.dragTimeStamps.Add(new CameraDriver.DragTimeStamp
				{
					posDelta = this.desiredDollyRaw,
					time = Time.time
				});
				this.rootPos.x = Mathf.Clamp(this.rootPos.x, 2f, (float)Find.CurrentMap.Size.x + -2f);
				this.rootPos.z = Mathf.Clamp(this.rootPos.z, 2f, (float)Find.CurrentMap.Size.z + -2f);
			}
			this.desiredDollyRaw = Vector2.zero;
			int num = Gen.FixedTimeStepUpdate(ref this.fixedTimeStepBuffer, 60f);
			for (int i = 0; i < num; i++)
			{
				if (this.velocity != Vector3.zero)
				{
					this.velocity *= this.config.camSpeedDecayFactor;
					if (this.velocity.magnitude < 0.1f)
					{
						this.velocity = Vector3.zero;
					}
				}
				if (this.config.smoothZoom)
				{
					float num2 = Mathf.Lerp(this.rootSize, this.desiredSize, 0.05f);
					this.desiredSize += (num2 - this.rootSize) * this.config.zoomPreserveFactor;
					this.rootSize = num2;
				}
				else
				{
					float num3 = (this.desiredSize - this.rootSize) * 0.4f;
					this.desiredSize += this.config.zoomPreserveFactor * num3;
					this.rootSize += num3;
				}
				this.config.ConfigFixedUpdate_60(ref this.velocity);
			}
			this.shaker.Update();
			this.ApplyPositionToGameObject();
			Current.SubcameraDriver.UpdatePositions(this.MyCamera);
			if (Find.CurrentMap != null)
			{
				RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
				rememberedCameraPos.rootPos = this.rootPos;
				rememberedCameraPos.rootSize = this.rootSize;
			}
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0008CFBC File Offset: 0x0008B1BC
		private void ApplyPositionToGameObject()
		{
			this.rootPos.y = 15f + (this.rootSize - this.config.minSize) / (60f - this.config.minSize) * 50f;
			this.MyCamera.orthographicSize = this.rootSize;
			this.MyCamera.transform.position = this.rootPos + this.shaker.ShakeOffset;
			Vector3 position = base.transform.position;
			position.y = 65f;
			this.reverbDummy.transform.position = position;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0008D064 File Offset: 0x0008B264
		private Vector2 CalculateCurInputDollyVect()
		{
			Vector2 vector = this.desiredDolly;
			bool flag = false;
			if ((UnityData.isEditor || Screen.fullScreen) && Prefs.EdgeScreenScroll && !this.mouseCoveredByUI)
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				Vector2 vector2 = mousePositionOnUI;
				vector2.y = (float)UI.screenHeight - vector2.y;
				Rect rect = new Rect(0f, 0f, 200f, 200f);
				Rect rect2 = new Rect((float)(UI.screenWidth - 250), 0f, 255f, 255f);
				Rect rect3 = new Rect(0f, (float)(UI.screenHeight - 250), 225f, 255f);
				Rect rect4 = new Rect((float)(UI.screenWidth - 250), (float)(UI.screenHeight - 250), 255f, 255f);
				MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
				if (Find.MainTabsRoot.OpenTab == MainButtonDefOf.Inspect && mainTabWindow_Inspect.RecentHeight > rect3.height)
				{
					rect3.yMin = (float)UI.screenHeight - mainTabWindow_Inspect.RecentHeight;
				}
				if (!rect.Contains(vector2) && !rect3.Contains(vector2) && !rect2.Contains(vector2) && !rect4.Contains(vector2))
				{
					Vector2 b = new Vector2(0f, 0f);
					if (mousePositionOnUI.x >= 0f && mousePositionOnUI.x < 20f)
					{
						b.x -= this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.x <= (float)UI.screenWidth && mousePositionOnUI.x > (float)UI.screenWidth - 20f)
					{
						b.x += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y <= (float)UI.screenHeight && mousePositionOnUI.y > (float)UI.screenHeight - 20f)
					{
						b.y += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y >= 0f && mousePositionOnUI.y < this.ScreenDollyEdgeWidthBottom)
					{
						if (this.mouseTouchingScreenBottomEdgeStartTime < 0f)
						{
							this.mouseTouchingScreenBottomEdgeStartTime = Time.realtimeSinceStartup;
						}
						if (Time.realtimeSinceStartup - this.mouseTouchingScreenBottomEdgeStartTime >= 0.28f)
						{
							b.y -= this.config.dollyRateScreenEdge;
						}
						flag = true;
					}
					vector += b;
				}
			}
			if (!flag)
			{
				this.mouseTouchingScreenBottomEdgeStartTime = -1f;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				vector *= 2.4f;
			}
			return vector;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0008D300 File Offset: 0x0008B500
		public void Expose()
		{
			if (Scribe.EnterNode("cameraMap"))
			{
				try
				{
					Scribe_Values.Look<Vector3>(ref this.rootPos, "camRootPos", default(Vector3), false);
					Scribe_Values.Look<float>(ref this.desiredSize, "desiredSize", 0f, false);
					this.rootSize = this.desiredSize;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0000AFAD File Offset: 0x000091AD
		public void ResetSize()
		{
			this.desiredSize = 24f;
			this.rootSize = this.desiredSize;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0000AFC6 File Offset: 0x000091C6
		public void JumpToCurrentMapLoc(IntVec3 cell)
		{
			this.JumpToCurrentMapLoc(cell.ToVector3Shifted());
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0000AFD5 File Offset: 0x000091D5
		public void JumpToCurrentMapLoc(Vector3 loc)
		{
			this.rootPos = new Vector3(loc.x, this.rootPos.y, loc.z);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0008D370 File Offset: 0x0008B570
		public void SetRootPosAndSize(Vector3 rootPos, float rootSize)
		{
			this.rootPos = rootPos;
			this.rootSize = rootSize;
			this.desiredDolly = Vector2.zero;
			this.desiredDollyRaw = Vector2.zero;
			this.desiredSize = rootSize;
			this.dragTimeStamps.Clear();
			LongEventHandler.ExecuteWhenFinished(new Action(this.ApplyPositionToGameObject));
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0008D3C4 File Offset: 0x0008B5C4
		public static Vector2 GetExtraVelocityFromReleasingDragButton(List<CameraDriver.DragTimeStamp> dragTimeStamps, float velocityFromMouseDragInitialFactor)
		{
			float num = 0f;
			Vector2 vector = Vector2.zero;
			for (int i = 0; i < dragTimeStamps.Count; i++)
			{
				if (dragTimeStamps[i].time < Time.time - 0.05f)
				{
					num = 0.05f;
				}
				else
				{
					num = Mathf.Max(num, Time.time - dragTimeStamps[i].time);
					vector += dragTimeStamps[i].posDelta;
				}
			}
			if (vector != Vector2.zero && num > 0f)
			{
				return vector / num * velocityFromMouseDragInitialFactor;
			}
			return Vector2.zero;
		}

		// Token: 0x040002D3 RID: 723
		public CameraShaker shaker = new CameraShaker();

		// Token: 0x040002D4 RID: 724
		private Camera cachedCamera;

		// Token: 0x040002D5 RID: 725
		private GameObject reverbDummy;

		// Token: 0x040002D6 RID: 726
		public CameraMapConfig config = new CameraMapConfig_Normal();

		// Token: 0x040002D7 RID: 727
		private Vector3 velocity;

		// Token: 0x040002D8 RID: 728
		private Vector3 rootPos;

		// Token: 0x040002D9 RID: 729
		private float rootSize;

		// Token: 0x040002DA RID: 730
		private float desiredSize;

		// Token: 0x040002DB RID: 731
		private Vector2 desiredDolly = Vector2.zero;

		// Token: 0x040002DC RID: 732
		private Vector2 desiredDollyRaw = Vector2.zero;

		// Token: 0x040002DD RID: 733
		private List<CameraDriver.DragTimeStamp> dragTimeStamps = new List<CameraDriver.DragTimeStamp>();

		// Token: 0x040002DE RID: 734
		private bool mouseCoveredByUI;

		// Token: 0x040002DF RID: 735
		private float mouseTouchingScreenBottomEdgeStartTime = -1f;

		// Token: 0x040002E0 RID: 736
		private float fixedTimeStepBuffer;

		// Token: 0x040002E1 RID: 737
		private static int lastViewRectGetFrame = -1;

		// Token: 0x040002E2 RID: 738
		private static CellRect lastViewRect;

		// Token: 0x040002E3 RID: 739
		public const float MaxDeltaTime = 0.1f;

		// Token: 0x040002E4 RID: 740
		private const float ScreenDollyEdgeWidth = 20f;

		// Token: 0x040002E5 RID: 741
		private const float ScreenDollyEdgeWidth_BottomFullscreen = 6f;

		// Token: 0x040002E6 RID: 742
		private const float MinDurationForMouseToTouchScreenBottomEdgeToDolly = 0.28f;

		// Token: 0x040002E7 RID: 743
		private const float DragTimeStampExpireSeconds = 0.05f;

		// Token: 0x040002E8 RID: 744
		private const float VelocityFromMouseDragInitialFactor = 0.75f;

		// Token: 0x040002E9 RID: 745
		private const float MapEdgeClampMarginCells = -2f;

		// Token: 0x040002EA RID: 746
		public const float StartingSize = 24f;

		// Token: 0x040002EB RID: 747
		private const float MaxSize = 60f;

		// Token: 0x040002EC RID: 748
		private const float ZoomTightness = 0.4f;

		// Token: 0x040002ED RID: 749
		private const float ZoomScaleFromAltDenominator = 35f;

		// Token: 0x040002EE RID: 750
		private const float PageKeyZoomRate = 4f;

		// Token: 0x040002EF RID: 751
		private const float ScrollWheelZoomRate = 0.35f;

		// Token: 0x040002F0 RID: 752
		public const float MinAltitude = 15f;

		// Token: 0x040002F1 RID: 753
		private const float MaxAltitude = 65f;

		// Token: 0x040002F2 RID: 754
		private const float ReverbDummyAltitude = 65f;

		// Token: 0x020000B9 RID: 185
		public struct DragTimeStamp
		{
			// Token: 0x040002F3 RID: 755
			public Vector2 posDelta;

			// Token: 0x040002F4 RID: 756
			public float time;
		}
	}
}
