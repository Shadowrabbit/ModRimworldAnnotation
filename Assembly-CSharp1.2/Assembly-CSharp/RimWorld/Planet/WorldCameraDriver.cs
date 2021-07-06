using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200203F RID: 8255
	public class WorldCameraDriver : MonoBehaviour
	{
		// Token: 0x170019CA RID: 6602
		// (get) Token: 0x0600AEEA RID: 44778 RVA: 0x00071E40 File Offset: 0x00070040
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

		// Token: 0x170019CB RID: 6603
		// (get) Token: 0x0600AEEB RID: 44779 RVA: 0x0032D65C File Offset: 0x0032B85C
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

		// Token: 0x170019CC RID: 6604
		// (get) Token: 0x0600AEEC RID: 44780 RVA: 0x0000AEF7 File Offset: 0x000090F7
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

		// Token: 0x170019CD RID: 6605
		// (get) Token: 0x0600AEED RID: 44781 RVA: 0x00071E62 File Offset: 0x00070062
		private Vector3 CurrentRealPosition
		{
			get
			{
				return this.MyCamera.transform.position;
			}
		}

		// Token: 0x170019CE RID: 6606
		// (get) Token: 0x0600AEEE RID: 44782 RVA: 0x00071E74 File Offset: 0x00070074
		public float AltitudePercent
		{
			get
			{
				return Mathf.InverseLerp(125f, 1100f, this.altitude);
			}
		}

		// Token: 0x170019CF RID: 6607
		// (get) Token: 0x0600AEEF RID: 44783 RVA: 0x00071E8B File Offset: 0x0007008B
		public Vector3 CurrentlyLookingAtPointOnSphere
		{
			get
			{
				return -(Quaternion.Inverse(this.sphereRotation) * Vector3.forward);
			}
		}

		// Token: 0x170019D0 RID: 6608
		// (get) Token: 0x0600AEF0 RID: 44784 RVA: 0x00071EA7 File Offset: 0x000700A7
		private bool AnythingPreventsCameraMotion
		{
			get
			{
				return Find.WindowStack.WindowsPreventCameraMotion || !WorldRendererUtility.WorldRenderedNow;
			}
		}

		// Token: 0x0600AEF1 RID: 44785 RVA: 0x00071EBF File Offset: 0x000700BF
		public void Awake()
		{
			this.ResetAltitude();
			this.ApplyPositionToGameObject();
		}

		// Token: 0x0600AEF2 RID: 44786 RVA: 0x0032D690 File Offset: 0x0032B890
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

		// Token: 0x0600AEF3 RID: 44787 RVA: 0x0032D8CC File Offset: 0x0032BACC
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

		// Token: 0x0600AEF4 RID: 44788 RVA: 0x0032DC48 File Offset: 0x0032BE48
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

		// Token: 0x0600AEF5 RID: 44789 RVA: 0x0032DCEC File Offset: 0x0032BEEC
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

		// Token: 0x0600AEF6 RID: 44790 RVA: 0x00071ECD File Offset: 0x000700CD
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

		// Token: 0x0600AEF7 RID: 44791 RVA: 0x00071EFB File Offset: 0x000700FB
		public void JumpTo(Vector3 newLookAt)
		{
			if (!Find.WorldInterface.everReset)
			{
				Find.WorldInterface.Reset();
			}
			this.sphereRotation = Quaternion.Inverse(Quaternion.LookRotation(-newLookAt.normalized));
		}

		// Token: 0x0600AEF8 RID: 44792 RVA: 0x00071F2F File Offset: 0x0007012F
		public void JumpTo(int tile)
		{
			this.JumpTo(Find.WorldGrid.GetTileCenter(tile));
		}

		// Token: 0x0600AEF9 RID: 44793 RVA: 0x0032DF40 File Offset: 0x0032C140
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

		// Token: 0x0600AEFA RID: 44794 RVA: 0x0032DF90 File Offset: 0x0032C190
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

		// Token: 0x04007825 RID: 30757
		public WorldCameraConfig config = new WorldCameraConfig_Normal();

		// Token: 0x04007826 RID: 30758
		public Quaternion sphereRotation = Quaternion.identity;

		// Token: 0x04007827 RID: 30759
		private Vector2 rotationVelocity;

		// Token: 0x04007828 RID: 30760
		private Vector2 desiredRotation;

		// Token: 0x04007829 RID: 30761
		private Vector2 desiredRotationRaw;

		// Token: 0x0400782A RID: 30762
		private float desiredAltitude;

		// Token: 0x0400782B RID: 30763
		public float altitude;

		// Token: 0x0400782C RID: 30764
		private List<CameraDriver.DragTimeStamp> dragTimeStamps = new List<CameraDriver.DragTimeStamp>();

		// Token: 0x0400782D RID: 30765
		private Camera cachedCamera;

		// Token: 0x0400782E RID: 30766
		private bool mouseCoveredByUI;

		// Token: 0x0400782F RID: 30767
		private float mouseTouchingScreenBottomEdgeStartTime = -1f;

		// Token: 0x04007830 RID: 30768
		private float fixedTimeStepBuffer;

		// Token: 0x04007831 RID: 30769
		private Quaternion rotationAnimation_prevSphereRotation = Quaternion.identity;

		// Token: 0x04007832 RID: 30770
		private float rotationAnimation_lerpFactor = 1f;

		// Token: 0x04007833 RID: 30771
		private const float SphereRadius = 100f;

		// Token: 0x04007834 RID: 30772
		private const float ScreenDollyEdgeWidth = 20f;

		// Token: 0x04007835 RID: 30773
		private const float ScreenDollyEdgeWidth_BottomFullscreen = 6f;

		// Token: 0x04007836 RID: 30774
		private const float MinDurationForMouseToTouchScreenBottomEdgeToDolly = 0.28f;

		// Token: 0x04007837 RID: 30775
		private const float MaxXRotationAtMinAltitude = 88.6f;

		// Token: 0x04007838 RID: 30776
		private const float MaxXRotationAtMaxAltitude = 78f;

		// Token: 0x04007839 RID: 30777
		private const float TileSizeToRotationSpeed = 0.273f;

		// Token: 0x0400783A RID: 30778
		private const float VelocityFromMouseDragInitialFactor = 5f;

		// Token: 0x0400783B RID: 30779
		private const float StartingAltitude_Playing = 160f;

		// Token: 0x0400783C RID: 30780
		private const float StartingAltitude_Entry = 550f;

		// Token: 0x0400783D RID: 30781
		public const float MinAltitude = 125f;

		// Token: 0x0400783E RID: 30782
		private const float MaxAltitude = 1100f;

		// Token: 0x0400783F RID: 30783
		private const float ZoomTightness = 0.4f;

		// Token: 0x04007840 RID: 30784
		private const float ZoomScaleFromAltDenominator = 12f;

		// Token: 0x04007841 RID: 30785
		private const float PageKeyZoomRate = 2f;

		// Token: 0x04007842 RID: 30786
		private const float ScrollWheelZoomRate = 0.1f;
	}
}
