using System;
using System.IO;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NVorbis.NAudioSupport;
using UnityEngine;

namespace RuntimeAudioClipLoader
{
	// Token: 0x02000006 RID: 6
	internal class CustomAudioFileReader : WaveStream, ISampleProvider
	{
		// Token: 0x06000027 RID: 39 RVA: 0x0007A0C4 File Offset: 0x000782C4
		public CustomAudioFileReader(Stream stream, AudioFormat format)
		{
			this.lockObject = new object();
			this.CreateReaderStream(stream, format);
			this.sourceBytesPerSample = this.readerStream.WaveFormat.BitsPerSample / 8 * this.readerStream.WaveFormat.Channels;
			this.sampleChannel = new SampleChannel(this.readerStream, false);
			this.destBytesPerSample = 4 * this.sampleChannel.WaveFormat.Channels;
			this.length = this.SourceToDest(this.readerStream.Length);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0007A154 File Offset: 0x00078354
		private void CreateReaderStream(Stream stream, AudioFormat format)
		{
			if (format == AudioFormat.wav)
			{
				this.readerStream = new WaveFileReader(stream);
				if (this.readerStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm && this.readerStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
				{
					this.readerStream = WaveFormatConversionStream.CreatePcmStream(this.readerStream);
					this.readerStream = new BlockAlignReductionStream(this.readerStream);
					return;
				}
			}
			else
			{
				if (format == AudioFormat.mp3)
				{
					this.readerStream = new Mp3FileReader(stream);
					return;
				}
				if (format == AudioFormat.aiff)
				{
					this.readerStream = new AiffFileReader(stream);
					return;
				}
				if (format == AudioFormat.ogg)
				{
					this.readerStream = new VorbisWaveReader(stream);
					return;
				}
				Debug.LogWarning("Audio format " + format + " is not supported");
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00006A07 File Offset: 0x00004C07
		public override WaveFormat WaveFormat
		{
			get
			{
				return this.sampleChannel.WaveFormat;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00006A14 File Offset: 0x00004C14
		public override long Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00006A1C File Offset: 0x00004C1C
		// (set) Token: 0x0600002C RID: 44 RVA: 0x0007A20C File Offset: 0x0007840C
		public override long Position
		{
			get
			{
				return this.SourceToDest(this.readerStream.Position);
			}
			set
			{
				object obj = this.lockObject;
				lock (obj)
				{
					this.readerStream.Position = this.DestToSource(value);
				}
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0007A258 File Offset: 0x00078458
		public override int Read(byte[] buffer, int offset, int count)
		{
			WaveBuffer waveBuffer = new WaveBuffer(buffer);
			int count2 = count / 4;
			return this.Read(waveBuffer.FloatBuffer, offset / 4, count2) * 4;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0007A284 File Offset: 0x00078484
		public int Read(float[] buffer, int offset, int count)
		{
			object obj = this.lockObject;
			int result;
			lock (obj)
			{
				result = this.sampleChannel.Read(buffer, offset, count);
			}
			return result;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00006A2F File Offset: 0x00004C2F
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00006A3C File Offset: 0x00004C3C
		public float Volume
		{
			get
			{
				return this.sampleChannel.Volume;
			}
			set
			{
				this.sampleChannel.Volume = value;
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00006A4A File Offset: 0x00004C4A
		private long SourceToDest(long sourceBytes)
		{
			return (long)this.destBytesPerSample * (sourceBytes / (long)this.sourceBytesPerSample);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00006A5D File Offset: 0x00004C5D
		private long DestToSource(long destBytes)
		{
			return (long)this.sourceBytesPerSample * (destBytes / (long)this.destBytesPerSample);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00006A70 File Offset: 0x00004C70
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.readerStream != null)
			{
				this.readerStream.Dispose();
				this.readerStream = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000010 RID: 16
		private WaveStream readerStream;

		// Token: 0x04000011 RID: 17
		private readonly SampleChannel sampleChannel;

		// Token: 0x04000012 RID: 18
		private readonly int destBytesPerSample;

		// Token: 0x04000013 RID: 19
		private readonly int sourceBytesPerSample;

		// Token: 0x04000014 RID: 20
		private readonly long length;

		// Token: 0x04000015 RID: 21
		private readonly object lockObject;
	}
}
