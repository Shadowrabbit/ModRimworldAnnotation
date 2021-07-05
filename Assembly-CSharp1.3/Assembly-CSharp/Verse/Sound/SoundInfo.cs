using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x02000570 RID: 1392
	public struct SoundInfo
	{
		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x0600290D RID: 10509 RVA: 0x000F8AD2 File Offset: 0x000F6CD2
		// (set) Token: 0x0600290E RID: 10510 RVA: 0x000F8ADA File Offset: 0x000F6CDA
		public bool IsOnCamera { get; private set; }

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x0600290F RID: 10511 RVA: 0x000F8AE3 File Offset: 0x000F6CE3
		// (set) Token: 0x06002910 RID: 10512 RVA: 0x000F8AEB File Offset: 0x000F6CEB
		public TargetInfo Maker { get; private set; }

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06002911 RID: 10513 RVA: 0x000F8AF4 File Offset: 0x000F6CF4
		// (set) Token: 0x06002912 RID: 10514 RVA: 0x000F8AFC File Offset: 0x000F6CFC
		public MaintenanceType Maintenance { get; private set; }

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06002913 RID: 10515 RVA: 0x000F8B05 File Offset: 0x000F6D05
		public IEnumerable<KeyValuePair<string, float>> DefinedParameters
		{
			get
			{
				if (this.parameters == null)
				{
					yield break;
				}
				foreach (KeyValuePair<string, float> keyValuePair in this.parameters)
				{
					yield return keyValuePair;
				}
				Dictionary<string, float>.Enumerator enumerator = default(Dictionary<string, float>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06002914 RID: 10516 RVA: 0x000F8B1C File Offset: 0x000F6D1C
		public static SoundInfo OnCamera(MaintenanceType maint = MaintenanceType.None)
		{
			SoundInfo result = default(SoundInfo);
			result.IsOnCamera = true;
			result.Maintenance = maint;
			result.Maker = TargetInfo.Invalid;
			result.testPlay = false;
			result.volumeFactor = (result.pitchFactor = 1f);
			return result;
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x000F8B6C File Offset: 0x000F6D6C
		public static SoundInfo InMap(TargetInfo maker, MaintenanceType maint = MaintenanceType.None)
		{
			SoundInfo result = default(SoundInfo);
			result.IsOnCamera = false;
			result.Maintenance = maint;
			result.Maker = maker;
			result.testPlay = false;
			result.volumeFactor = (result.pitchFactor = 1f);
			return result;
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x000F8BB8 File Offset: 0x000F6DB8
		public void SetParameter(string key, float value)
		{
			if (this.parameters == null)
			{
				this.parameters = new Dictionary<string, float>();
			}
			this.parameters[key] = value;
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x000F8BDA File Offset: 0x000F6DDA
		public static implicit operator SoundInfo(TargetInfo source)
		{
			return SoundInfo.InMap(source, MaintenanceType.None);
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x000F8BE3 File Offset: 0x000F6DE3
		public static implicit operator SoundInfo(Thing sourceThing)
		{
			return SoundInfo.InMap(sourceThing, MaintenanceType.None);
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x000F8BF4 File Offset: 0x000F6DF4
		public override string ToString()
		{
			string text = null;
			if (this.parameters != null && this.parameters.Count > 0)
			{
				text = "parameters=";
				foreach (KeyValuePair<string, float> keyValuePair in this.parameters)
				{
					text = string.Concat(new string[]
					{
						text,
						keyValuePair.Key.ToString(),
						"-",
						keyValuePair.Value.ToString(),
						" "
					});
				}
			}
			string text2 = null;
			if (this.Maker.HasThing || this.Maker.Cell.IsValid)
			{
				text2 = this.Maker.ToString();
			}
			string text3 = null;
			if (this.Maintenance != MaintenanceType.None)
			{
				text3 = ", Maint=" + this.Maintenance;
			}
			return string.Concat(new string[]
			{
				"(",
				this.IsOnCamera ? "Camera" : "World from ",
				text2,
				text,
				text3,
				")"
			});
		}

		// Token: 0x04001970 RID: 6512
		private Dictionary<string, float> parameters;

		// Token: 0x04001971 RID: 6513
		public float volumeFactor;

		// Token: 0x04001972 RID: 6514
		public float pitchFactor;

		// Token: 0x04001973 RID: 6515
		public bool testPlay;

		// Token: 0x04001974 RID: 6516
		public bool forcedPlayOnCamera;
	}
}
