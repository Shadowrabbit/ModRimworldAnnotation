using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001026 RID: 4134
	public class HistoryAutoRecorder : IExposable
	{
		// Token: 0x06005A3B RID: 23099 RVA: 0x001D4B2C File Offset: 0x001D2D2C
		public void Tick()
		{
			if (Find.TickManager.TicksGame % this.def.recordTicksFrequency == 0 || !this.records.Any<float>())
			{
				float item = this.def.Worker.PullRecord();
				this.records.Add(item);
			}
		}

		// Token: 0x06005A3C RID: 23100 RVA: 0x001D4B7C File Offset: 0x001D2D7C
		public void ExposeData()
		{
			Scribe_Defs.Look<HistoryAutoRecorderDef>(ref this.def, "def");
			byte[] recordsFromBytes = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				recordsFromBytes = this.RecordsToBytes();
			}
			DataExposeUtility.ByteArray(ref recordsFromBytes, "records");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.SetRecordsFromBytes(recordsFromBytes);
			}
		}

		// Token: 0x06005A3D RID: 23101 RVA: 0x001D4BC8 File Offset: 0x001D2DC8
		private byte[] RecordsToBytes()
		{
			byte[] array = new byte[this.records.Count * 4];
			for (int i = 0; i < this.records.Count; i++)
			{
				byte[] bytes = BitConverter.GetBytes(this.records[i]);
				for (int j = 0; j < 4; j++)
				{
					array[i * 4 + j] = bytes[j];
				}
			}
			return array;
		}

		// Token: 0x06005A3E RID: 23102 RVA: 0x001D4C28 File Offset: 0x001D2E28
		private void SetRecordsFromBytes(byte[] bytes)
		{
			int num = bytes.Length / 4;
			this.records.Clear();
			for (int i = 0; i < num; i++)
			{
				float item = BitConverter.ToSingle(bytes, i * 4);
				this.records.Add(item);
			}
		}

		// Token: 0x04003CB7 RID: 15543
		public HistoryAutoRecorderDef def;

		// Token: 0x04003CB8 RID: 15544
		public List<float> records = new List<float>();
	}
}
