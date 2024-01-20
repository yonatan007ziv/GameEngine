using GameEngine.Core.API;
using SharpDX;
using SharpDX.DirectSound;
using SharpDX.Multimedia;

namespace SoundEngine.Services.Implementations;

internal class DirectSoundEngine : ISoundEngine
{
	private readonly DirectSound directSound;

	public DirectSoundEngine()
	{
		directSound = new DirectSound();
	}

	public void AttachWnd(IntPtr handle)
	{
		directSound.SetCooperativeLevel(handle, CooperativeLevel.Priority);
	}

	public void Test()
	{
		// Create PrimarySoundBuffer
		var primaryBufferDesc = new SoundBufferDescription();
		primaryBufferDesc.Flags = BufferFlags.PrimaryBuffer;
		primaryBufferDesc.AlgorithmFor3D = Guid.Empty;

		var primarySoundBuffer = new PrimarySoundBuffer(directSound, primaryBufferDesc);

		// Play the PrimarySound Buffer
		primarySoundBuffer.Play(0, PlayFlags.Looping);

		// Default WaveFormat Stereo 44100 16 bit
		WaveFormat waveFormat = new WaveFormat();

		// Create SecondarySoundBuffer
		var secondaryBufferDesc = new SoundBufferDescription();
		secondaryBufferDesc.BufferBytes = waveFormat.ConvertLatencyToByteSize(60000);
		secondaryBufferDesc.Format = waveFormat;
		secondaryBufferDesc.Flags = BufferFlags.GetCurrentPosition2 | BufferFlags.ControlPositionNotify | BufferFlags.GlobalFocus |
									BufferFlags.ControlVolume | BufferFlags.StickyFocus;
		secondaryBufferDesc.AlgorithmFor3D = Guid.Empty;
		var secondarySoundBuffer = new SecondarySoundBuffer(directSound, secondaryBufferDesc);

		// Get Capabilties from secondary sound buffer
		var capabilities = secondarySoundBuffer.Capabilities;

		// Lock the buffer
		DataStream dataPart2;
		var dataPart1 = secondarySoundBuffer.Lock(0, capabilities.BufferBytes, LockFlags.EntireBuffer, out dataPart2);

		// Fill the buffer with some sound
		int numberOfSamples = capabilities.BufferBytes / waveFormat.BlockAlign;
		for (int i = 0; i < numberOfSamples; i++)
		{
			double vibrato = Math.Cos(2 * Math.PI * 10.0 * i / waveFormat.SampleRate);
			short value = (short)(Math.Cos(2 * Math.PI * (220.0 + 4.0 * vibrato) * i / waveFormat.SampleRate) * 16384); // Not too loud
			dataPart1.Write(value);
			dataPart1.Write(value);
		}

		// Unlock the buffer
		secondarySoundBuffer.Unlock(dataPart1, dataPart2);

		// Play the song
		secondarySoundBuffer.Play(0, PlayFlags.Looping);
	}
}