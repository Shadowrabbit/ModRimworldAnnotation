using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x0200094A RID: 2378
	public struct SoundInfo
	{
		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06003A5A RID: 14938 RVA: 0x0002CF49 File Offset: 0x0002B149
		// (set) Token: 0x06003A5B RID: 14939 RVA: 0x0002CF51 File Offset: 0x0002B151
		public bool IsOnCamera { get; private set; }

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06003A5C RID: 14940 RVA: 0x0002CF5A File Offset: 0x0002B15A
		// (set) Token: 0x06003A5D RID: 14941 RVA: 0x0002CF62 File Offset: 0x0002B162
		public TargetInfo Maker { get; private set; }

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06003A5E RID: 14942 RVA: 0x0002CF6B File Offset: 0x0002B16B
		// (set) Token: 0x06003A5F RID: 14943 RVA: 0x0002CF73 File Offset: 0x0002B173
		public MaintenanceType Maintenance { get; private set; }

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06003A60 RID: 14944 RVA: 0x0002CF7C File Offset: 0x0002B17C
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

		// Token: 0x06003A61 RID: 14945 RVA: 0x00169B88 File Offset: 0x00167D88
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

		// Token: 0x06003A62 RID: 14946 RVA: 0x00169BD8 File Offset: 0x00167DD8
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

		// Token: 0x06003A63 RID: 14947 RVA: 0x0002CF91 File Offset: 0x0002B191
		public void SetParameter(string key, float value)
		{
			if (this.parameters == null)
			{
				this.parameters = new Dictionary<string, float>();
			}
			this.parameters[key] = value;
		}

		// Token: 0x06003A64 RID: 14948 RVA: 0x0002CFB3 File Offset: 0x0002B1B3
		public static implicit operator SoundInfo(TargetInfo source)
		{
			return SoundInfo.InMap(source, MaintenanceType.None);
		}

		// Token: 0x06003A65 RID: 14949 RVA: 0x0002CFBC File Offset: 0x0002B1BC
		public static implicit operator SoundInfo(Thing sourceThing)
		{
			return SoundInfo.InMap(sourceThing, MaintenanceType.None);
		}

		// Token: 0x06003A66 RID: 14950 RVA: 0x00169C24 File Offset: 0x00167E24
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

		// Token: 0x04002873 RID: 10355
		private Dictionary<string, float> parameters;

		// Token: 0x04002874 RID: 10356
		public float volumeFactor;

		// Token: 0x04002875 RID: 10357
		public float pitchFactor;

		// Token: 0x04002876 RID: 10358
		public bool testPlay;
	}
}
