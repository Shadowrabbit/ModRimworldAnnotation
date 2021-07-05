using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001752 RID: 5970
	public class WorldCameraDriver : MonoBehaviour
	{
		// Token: 0x1700166A RID: 5738
		// (get) Token: 0x060089C1 RID: 35265 RVA: 0x003176EA File Offset: 0x003158EA
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

		// Token: 0x1700166B RID: 5739
		// (get) Token: 0x060089C2 RID: 35266 RVA: 0x0031770C File Offset: 0x0031590C
		public WorldCameraZoomRange CurrentZoom
		{
			get
			{
				float altitudePercent = this.AltitudePercent;
				if (altitudePercent < 0.025f)
				{
					return WorldCameraZoomRange.VeryClose;
				}
				if (altitudePercent < 0.042f)
				{
					return WorldCameraZoomRange.Close;
				}
				if (altitudePercent < 0.125f)
				{
					return WorldCameraZoomRange.Far;
				}
				return WorldCameraZoomRange.VeryFar;
			}
		}

		// Token: 0x1700166C RID: 5740
		// (get) Token: 0x060089C3 RID: 35267 RVA: 0x00016DF9 File Offset: 0x00014FF9
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

		// Token: 0x1700166D RID: 5741
		// (get) Token: 0x060089C4 RID: 35268 RVA: 0x0031773F File Offset: 0x0031593F
		private Vector3 CurrentRealPosition
		{
			get
			{
				return this.MyCamera.transform.position;
			}
		}

		// Token: 0x1700166E RID: 5742
		// (get) Token: 0x060089C5 RID: 35269 RVA: 0x00317751 File Offset: 0x00315951
		public float AltitudePercent
		{
			get
			{
				return Mathf.InverseLerp(125f, 1100f, this.altitude);
			}
		}

		// Token: 0x1700166F RID: 5743
		// (get) Token: 0x060089C6 RID: 35270 RVA: 0x00317768 File Offset: 0x00315968
		public Vector3 CurrentlyLookingAtPointOnSphere
		{
			get
			{
				return -(Quaternion.Inverse(this.sphereRotation) * Vector3.forward);
			}
		}

		// Token: 0x17001670 RID: 5744
		// (get) Token: 0x060089C7 RID: 35271 RVA: 0x00317784 File Offset: 0x00315984
		private bool AnythingPreventsCameraMotion
		{
			get
			{
				return Find.WindowStack.WindowsPreventCameraMotion || !WorldRendererUtility.WorldRenderedNow;
			}
		}

		// Token: 0x060089C8 RID: 35272 RVA: 0x0031779C File Offset: 0x0031599C
		public void Awake()
		{
			this.ResetAltitude();
			this.ApplyPositionToGameObject();
		}

		// Token: 0x060089C9 RID: 35273 RVA: 0x003177AC File Offset: 0x003159AC
		public void WorldCameraDriverOnGUI()
		{
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
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.FrameInteraction);
						currentEventDelta.x *= -1f;
						this.desiredRotationRaw += currentEventDelta / GenWorldUI.CurUITileSize() * 0.273f * Prefs.MapDragSensitivity;
					}
				}
				float num = 0f;
				if (Event.current.type == EventType.ScrollWheel)
				{
					num -= Event.current.delta.y * 0.1f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapZoom_In.KeyDownEvent)
				{
					num += 2f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapZoom_Out.KeyDownEvent)
				{
					num -= 2f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				this.desiredAltitude -= num * this.config.zoomSpeed * this.altitude / 12f;
				this.desiredAltitude = Mathf.Clamp(this.desiredAltitude, 125f, 1100f);
				this.desiredRotation = Vector2.zero;
				if (KeyBindingDefOf.MapDolly_Left.IsDown)
				{
					this.desiredRotation.x = -this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapDolly_Right.IsDown)
				{
					this.desiredRotation.x = this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapDolly_Up.IsDown)
				{
					this.desiredRotation.y = this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapDolly_Down.IsDown)
				{
					this.desiredRotation.y = -this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				this.config.ConfigOnGUI();
			}
		}

		// Token: 0x060089CA RID: 35274 RVA: 0x003179E8 File Offset: 0x00315BE8
		public void Update()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			if (Find.World == null)
			{
				this.MyCamera.gameObject.SetActive(false);
				return;
			}
			if (!Find.WorldInterface.everReset)
			{
				Find.WorldInterface.Reset();
			}
			Vector2 vector = this.CalculateCurInputDollyVect();
			if (vector != Vector2.zero)
			{
				float d = (this.altitude - 125f) / 975f * 0.85f + 0.15f;
				this.rotationVelocity = new Vector2(vector.x, vector.y) * d;
			}
			if (!Input.GetMouseButton(2) && this.dragTimeStamps.Any<CameraDriver.DragTimeStamp>())
			{
				this.rotationVelocity += CameraDriver.GetExtraVelocityFromReleasingDragButton(this.dragTimeStamps, 5f);
				this.dragTimeStamps.Clear();
			}
			if (!this.AnythingPreventsCameraMotion)
			{
				float num = Time.deltaTime * CameraDriver.HitchReduceFactor;
				this.sphereRotation *= Quaternion.AngleAxis(this.rotationVelocity.x * num * this.config.rotationSpeedScale, this.MyCamera.transform.up);
				this.sphereRotation *= Quaternion.AngleAxis(-this.rotationVelocity.y * num * this.config.rotationSpeedScale, this.MyCamera.transform.right);
				if (this.desiredRotationRaw != Vector2.zero)
				{
					this.sphereRotation *= Quaternion.AngleAxis(this.desiredRotationRaw.x, this.MyCamera.transform.up);
					this.sphereRotation *= Quaternion.AngleAxis(-this.desiredRotationRaw.y, this.MyCamera.transform.right);
				}
				this.dragTimeStamps.Add(new CameraDriver.DragTimeStamp
				{
					posDelta = this.desiredRotationRaw,
					time = Time.time
				});
			}
			this.desiredRotationRaw = Vector2.zero;
			int num2 = Gen.FixedTimeStepUpdate(ref this.fixedTimeStepBuffer, 60f);
			for (int i = 0; i < num2; i++)
			{
				if (this.rotationVelocity != Vector2.zero)
				{
					this.rotationVelocity *= this.config.camRotationDecayFactor;
					if (this.rotationVelocity.magnitude < 0.05f)
					{
						this.rotationVelocity = Vector2.zero;
					}
				}
				if (this.config.smoothZoom)
				{
					float num3 = Mathf.Lerp(this.altitude, this.desiredAltitude, 0.05f);
					this.desiredAltitude += (num3 - this.altitude) * this.config.zoomPreserveFactor;
					this.altitude = num3;
				}
				else
				{
					float num4 = (this.desiredAltitude - this.altitude) * 0.4f;
					this.desiredAltitude += this.config.zoomPreserveFactor * num4;
					this.altitude += num4;
				}
			}
			this.rotationAnimation_lerpFactor += Time.deltaTime * 8f;
			if (Find.PlaySettings.lockNorthUp)
			{
				this.RotateSoNorthIsUp(false);
				this.ClampXRotation(ref this.sphereRotation);
			}
			for (int j = 0; j < num2; j++)
			{
				this.config.ConfigFixedUpdate_60(ref this.rotationVelocity);
			}
			this.ApplyPositionToGameObject();
		}

		// Token: 0x060089CB RID: 35275 RVA: 0x00317D64 File Offset: 0x00315F64
		private void ApplyPositionToGameObject()
		{
			Quaternion rotation;
			if (this.rotationAnimation_lerpFactor < 1f)
			{
				rotation = Quaternion.Lerp(this.rotationAnimation_prevSphereRotation, this.sphereRotation, this.rotationAnimation_lerpFactor);
			}
			else
			{
				rotation = this.sphereRotation;
			}
			if (Find.PlaySettings.lockNorthUp)
			{
				this.ClampXRotation(ref rotation);
			}
			this.MyCamera.transform.rotation = Quaternion.Inverse(rotation);
			Vector3 a = this.MyCamera.transform.rotation * Vector3.forward;
			this.MyCamera.transform.position = -a * this.altitude;
		}

		// Token: 0x060089CC RID: 35276 RVA: 0x00317E08 File Offset: 0x00316008
		private Vector2 CalculateCurInputDollyVect()
		{
			Vector2 vector = this.desiredRotation;
			bool flag = false;
			if ((UnityData.isEditor || Screen.fullScreen) && Prefs.EdgeScreenScroll && !this.mouseCoveredByUI)
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
				Rect rect = new Rect((float)(UI.screenWidth - 250), 0f, 255f, 255f);
				Rect rect2 = new Rect(0f, (float)(UI.screenHeight - 250), 225f, 255f);
				Rect rect3 = new Rect((float)(UI.screenWidth - 250), (float)(UI.screenHeight - 250), 255f, 255f);
				WorldInspectPane inspectPane = Find.World.UI.inspectPane;
				if (Find.WindowStack.IsOpen<WorldInspectPane>() && inspectPane.RecentHeight > rect2.height)
				{
					rect2.yMin = (float)UI.screenHeight - inspectPane.RecentHeight;
				}
				if (!rect2.Contains(mousePositionOnUIInverted) && !rect3.Contains(mousePositionOnUIInverted) && !rect.Contains(mousePositionOnUIInverted))
				{
					Vector2 zero = Vector2.zero;
					if (mousePositionOnUI.x >= 0f && mousePositionOnUI.x < 20f)
					{
						zero.x -= this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.x <= (float)UI.screenWidth && mousePositionOnUI.x > (float)UI.screenWidth - 20f)
					{
						zero.x += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y <= (float)UI.screenHeight && mousePositionOnUI.y > (float)UI.screenHeight - 20f)
					{
						zero.y += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y >= 0f && mousePositionOnUI.y < this.ScreenDollyEdgeWidthBottom)
					{
						if (this.mouseTouchingScreenBottomEdgeStartTime < 0f)
						{
							this.mouseTouchingScreenBottomEdgeStartTime = Time.realtimeSinceStartup;
						}
						if (Time.realtimeSinceStartup - this.mouseTouchingScreenBottomEdgeStartTime >= 0.28f)
						{
							zero.y -= this.config.dollyRateScreenEdge;
						}
						flag = true;
					}
					vector += zero;
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

		// Token: 0x060089CD RID: 35277 RVA: 0x0031805B File Offset: 0x0031625B
		public void ResetAltitude()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.altitude = 160f;
			}
			else
			{
				this.altitude = 550f;
			}
			this.desiredAltitude = this.altitude;
		}

		// Token: 0x060089CE RID: 35278 RVA: 0x00318089 File Offset: 0x00316289
		public void JumpTo(Vector3 newLookAt)
		{
			if (!Find.WorldInterface.everReset)
			{
				Find.WorldInterface.Reset();
			}
			this.sphereRotation = Quaternion.Inverse(Quaternion.LookRotation(-newLookAt.normalized));
		}

		// Token: 0x060089CF RID: 35279 RVA: 0x003180BD File Offset: 0x003162BD
		public void JumpTo(int tile)
		{
			this.JumpTo(Find.WorldGrid.GetTileCenter(tile));
		}

		// Token: 0x060089D0 RID: 35280 RVA: 0x003180D0 File Offset: 0x003162D0
		public void RotateSoNorthIsUp(bool interpolate = true)
		{
			if (interpolate)
			{
				this.rotationAnimation_prevSphereRotation = this.sphereRotation;
			}
			this.sphereRotation = Quaternion.Inverse(Quaternion.LookRotation(Quaternion.Inverse(this.sphereRotation) * Vector3.forward));
			if (interpolate)
			{
				this.rotationAnimation_lerpFactor = 0f;
			}
		}

		// Token: 0x060089D1 RID: 35281 RVA: 0x00318120 File Offset: 0x00316320
		private void ClampXRotation(ref Quaternion invRot)
		{
			Vector3 eulerAngles = Quaternion.Inverse(invRot).eulerAngles;
			float altitudePercent = this.AltitudePercent;
			float num = Mathf.Lerp(88.6f, 78f, altitudePercent);
			bool flag = false;
			if (eulerAngles.x <= 90f)
			{
				if (eulerAngles.x > num)
				{
					eulerAngles.x = num;
					flag = true;
				}
			}
			else if (eulerAngles.x < 360f - num)
			{
				eulerAngles.x = 360f - num;
				flag = true;
			}
			if (flag)
			{
				invRot = Quaternion.Inverse(Quaternion.Euler(eulerAngles));
			}
		}

		// Token: 0x04005778 RID: 22392
		public WorldCameraConfig config = new WorldCameraConfig_Normal();

		// Token: 0x04005779 RID: 22393
		public Quaternion sphereRotation = Quaternion.identity;

		// Token: 0x0400577A RID: 22394
		private Vector2 rotationVelocity;

		// Token: 0x0400577B RID: 22395
		private Vector2 desiredRotation;

		// Token: 0x0400577C RID: 22396
		private Vector2 desiredRotationRaw;

		// Token: 0x0400577D RID: 22397
		private float desiredAltitude;

		// Token: 0x0400577E RID: 22398
		public float altitude;

		// Token: 0x0400577F RID: 22399
		private List<CameraDriver.DragTimeStamp> dragTimeStamps = new List<CameraDriver.DragTimeStamp>();

		// Token: 0x04005780 RID: 22400
		private Camera cachedCamera;

		// Token: 0x04005781 RID: 22401
		private bool mouseCoveredByUI;

		// Token: 0x04005782 RID: 22402
		private float mouseTouchingScreenBottomEdgeStartTime = -1f;

		// Token: 0x04005783 RID: 22403
		private float fixedTimeStepBuffer;

		// Token: 0x04005784 RID: 22404
		private Quaternion rotationAnimation_prevSphereRotation = Quaternion.identity;

		// Token: 0x04005785 RID: 22405
		private float rotationAnimation_lerpFactor = 1f;

		// Token: 0x04005786 RID: 22406
		private const float SphereRadius = 100f;

		// Token: 0x04005787 RID: 22407
		private const float ScreenDollyEdgeWidth = 20f;

		// Token: 0x04005788 RID: 22408
		private const float ScreenDollyEdgeWidth_BottomFullscreen = 6f;

		// Token: 0x04005789 RID: 22409
		private const float MinDurationForMouseToTouchScreenBottomEdgeToDolly = 0.28f;

		// Token: 0x0400578A RID: 22410
		private const float MaxXRotationAtMinAltitude = 88.6f;

		// Token: 0x0400578B RID: 22411
		private const float MaxXRotationAtMaxAltitude = 78f;

		// Token: 0x0400578C RID: 22412
		private const float TileSizeToRotationSpeed = 0.273f;

		// Token: 0x0400578D RID: 22413
		private const float VelocityFromMouseDragInitialFactor = 5f;

		// Token: 0x0400578E RID: 22414
		private const float StartingAltitude_Playing = 160f;

		// Token: 0x0400578F RID: 22415
		private const float StartingAltitude_Entry = 550f;

		// Token: 0x04005790 RID: 22416
		public const float MinAltitude = 125f;

		// Token: 0x04005791 RID: 22417
		private const float MaxAltitude = 1100f;

		// Token: 0x04005792 RID: 22418
		private const float ZoomTightness = 0.4f;

		// Token: 0x04005793 RID: 22419
		private const float ZoomScaleFromAltDenominator = 12f;

		// Token: 0x04005794 RID: 22420
		private const float PageKeyZoomRate = 2f;

		// Token: 0x04005795 RID: 22421
		private const float ScrollWheelZoomRate = 0.1f;
	}
}
