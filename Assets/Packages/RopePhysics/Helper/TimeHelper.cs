using UnityEngine;
using System.Collections;
using System;

public interface ITimeHelper {
	float CurrentTime { get; }
	void Update();
	float DeltaTime { get; }
}

public abstract class TimeHelper {
	public const float TICK_2_SECOND = 1e-7f;

	public static ITimeHelper Create() {
		return Application.isPlaying ? (ITimeHelper)new PlayingTimeHelper() : (ITimeHelper)new EditorTimeHelper();
		//return new PlayingTimeHelper();
		//return new EditorTimeHelper();
	}

	private class PlayingTimeHelper : ITimeHelper {
		public float CurrentTime { get { return Time.time; } }
		public float DeltaTime { get { return Time.deltaTime; } }
		public void Update() { }
	}

	private class EditorTimeHelper : ITimeHelper {
		private long _startTime;
		private float _prevTime;
		private float _currTime;
		private float _deltaTime;

		public EditorTimeHelper() {
			_startTime = DateTime.Now.Ticks;
			_prevTime = _currTime = Now;
			_deltaTime = 0f;
		}

		public float Now { get { return (DateTime.Now.Ticks - _startTime) * TICK_2_SECOND; } }
		public float CurrentTime { get { return _currTime; } }
		public float DeltaTime { get { return _deltaTime; } }

		public void Update () {
			_prevTime = _currTime;
			_currTime = Now;
			_deltaTime = _currTime - _prevTime;
		}
	}
}
