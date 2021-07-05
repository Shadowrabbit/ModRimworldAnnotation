using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200057D RID: 1405
	public class Sustainer
	{
		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x0600293D RID: 10557 RVA: 0x000F977D File Offset: 0x000F797D
		public bool Ended
		{
			get
			{
				return this.endRealTime >= 0f;
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x0600293E RID: 10558 RVA: 0x000F978F File Offset: 0x000F798F
		public float TimeSinceEnd
		{
			get
			{
				return Time.realtimeSinceStartup - this.endRealTime;
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x0600293F RID: 10559 RVA: 0x000F97A0 File Offset: 0x000F79A0
		public float CameraDistanceSquared
		{
			get
			{
				if (this.info.IsOnCamera)
				{
					return 0f;
				}
				if (this.worldRootObject == null)
				{
					if (Prefs.DevMode)
					{
						Log.Error(string.Concat(new object[]
						{
							"Sustainer ",
							this.def,
							" info is ",
							this.info,
							" but its worldRootObject is null"
						}));
					}
					return 0f;
				}
				return (float)(Find.CameraDriver.MapPosition - this.worldRootObject.transform.position.ToIntVec3()).LengthHorizontalSquared;
			}
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x000F9848 File Offset: 0x000F7A48
		public Sustainer(SoundDef def, SoundInfo info)
		{
			this.def = def;
			this.info = info;
			if (def.subSounds.Count > 0)
			{
				foreach (KeyValuePair<string, float> keyValuePair in info.DefinedParameters)
				{
					this.externalParams[keyValuePair.Key] = keyValuePair.Value;
				}
				if (def.HasSubSoundsInWorld)
				{
					if (!info.forcedPlayOnCamera && info.IsOnCamera)
					{
						Log.Error("Playing sound " + def.ToString() + " on camera, but it has sub-sounds in the world.");
					}
					this.worldRootObject = new GameObject("SustainerRootObject_" + def.defName);
					this.UpdateRootObjectPosition();
				}
				else if (!info.IsOnCamera)
				{
					info = SoundInfo.OnCamera(info.Maintenance);
				}
				Find.SoundRoot.sustainerManager.RegisterSustainer(this);
				if (!info.IsOnCamera)
				{
					Find.SoundRoot.sustainerManager.UpdateAllSustainerScopes();
				}
				for (int i = 0; i < def.subSounds.Count; i++)
				{
					this.subSustainers.Add(new SubSustainer(this, def.subSounds[i]));
				}
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
				this.lastMaintainFrame = Time.frameCount;
			});
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x000F99D4 File Offset: 0x000F7BD4
		public void SustainerUpdate()
		{
			if (!this.Ended)
			{
				if (this.info.Maintenance == MaintenanceType.PerTick)
				{
					if (Find.TickManager.TicksGame > this.lastMaintainTick + 1)
					{
						this.End();
						return;
					}
				}
				else if (this.info.Maintenance == MaintenanceType.PerFrame && Time.frameCount > this.lastMaintainFrame + 1)
				{
					this.End();
					return;
				}
			}
			else if (this.TimeSinceEnd > this.def.sustainFadeoutTime)
			{
				this.Cleanup();
			}
			if (this.def.subSounds.Count > 0)
			{
				if (!this.info.IsOnCamera && this.info.Maker.HasThing)
				{
					this.UpdateRootObjectPosition();
				}
				this.scopeFader.SustainerScopeUpdate();
				for (int i = 0; i < this.subSustainers.Count; i++)
				{
					this.subSustainers[i].SubSustainerUpdate();
				}
			}
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x000F9ABC File Offset: 0x000F7CBC
		private void UpdateRootObjectPosition()
		{
			Vector3 position = Vector3.zero;
			if (this.info.forcedPlayOnCamera)
			{
				position = Find.Camera.gameObject.transform.position;
			}
			else
			{
				position = this.info.Maker.Cell.ToVector3ShiftedWithAltitude(0f);
				Thing thing = this.info.Maker.Thing;
				if (thing != null)
				{
					position = thing.DrawPos.Yto0();
				}
			}
			if (this.worldRootObject != null)
			{
				this.worldRootObject.transform.position = position;
			}
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x000F9B58 File Offset: 0x000F7D58
		public void Maintain()
		{
			if (this.Ended)
			{
				Log.Error("Tried to maintain ended sustainer: " + this.def);
				return;
			}
			if (this.info.Maintenance == MaintenanceType.PerTick)
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
				return;
			}
			if (this.info.Maintenance == MaintenanceType.PerFrame)
			{
				this.lastMaintainFrame = Time.frameCount;
			}
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x000F9BBB File Offset: 0x000F7DBB
		public void End()
		{
			this.endRealTime = Time.realtimeSinceStartup;
			if (this.def.sustainFadeoutTime < 0.001f)
			{
				this.Cleanup();
			}
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x000F9BE0 File Offset: 0x000F7DE0
		private void Cleanup()
		{
			if (this.def.subSounds.Count > 0)
			{
				Find.SoundRoot.sustainerManager.DeregisterSustainer(this);
				for (int i = 0; i < this.subSustainers.Count; i++)
				{
					this.subSustainers[i].Cleanup();
				}
			}
			if (this.def.sustainStopSound != null)
			{
				if (this.worldRootObject != null)
				{
					Map map = this.info.Maker.Map;
					if (map != null)
					{
						SoundInfo soundInfo = SoundInfo.InMap(new TargetInfo(this.worldRootObject.transform.position.ToIntVec3(), map, false), MaintenanceType.None);
						this.def.sustainStopSound.PlayOneShot(soundInfo);
					}
				}
				else
				{
					this.def.sustainStopSound.PlayOneShot(SoundInfo.OnCamera(MaintenanceType.None));
				}
			}
			if (this.worldRootObject != null)
			{
				UnityEngine.Object.Destroy(this.worldRootObject);
			}
			DebugSoundEventsLog.Notify_SustainerEnded(this, this.info);
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x000F9CDC File Offset: 0x000F7EDC
		public string DebugString()
		{
			string text = this.def.defName;
			text = text + "\n  inScopePercent=" + this.scopeFader.inScopePercent;
			text = text + "\n  CameraDistanceSquared=" + this.CameraDistanceSquared;
			foreach (SubSustainer arg in this.subSustainers)
			{
				text = text + "\n  sub: " + arg;
			}
			return text;
		}

		// Token: 0x04001991 RID: 6545
		public SoundDef def;

		// Token: 0x04001992 RID: 6546
		public SoundInfo info;

		// Token: 0x04001993 RID: 6547
		internal GameObject worldRootObject;

		// Token: 0x04001994 RID: 6548
		private int lastMaintainTick;

		// Token: 0x04001995 RID: 6549
		private int lastMaintainFrame;

		// Token: 0x04001996 RID: 6550
		private float endRealTime = -1f;

		// Token: 0x04001997 RID: 6551
		private List<SubSustainer> subSustainers = new List<SubSustainer>();

		// Token: 0x04001998 RID: 6552
		public SoundParams externalParams = new SoundParams();

		// Token: 0x04001999 RID: 6553
		public SustainerScopeFader scopeFader = new SustainerScopeFader();
	}
}
