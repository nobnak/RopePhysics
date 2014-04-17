using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine : MonoBehaviour {
	private Queue<IState> _sequence = new Queue<IState>();

	public void Change(IState next) {
		_sequence.Enqueue(next);
	}

	void OnEnable() {
		StartCoroutine(Process());
	}

	IEnumerator Process() {
		while (enabled) {
			if (_sequence.Count == 0) {
				yield return null;
				continue;
			}
			var next = _sequence.Dequeue();

			var iter = next.Enter(this);
			while (iter.MoveNext())
				yield return iter.Current;

			while (_sequence.Count == 0) {
				iter = next.Stay(this);
				while (iter.MoveNext())
					yield return iter.Current;
				yield return null;
			}

			iter = next.Exit(this);
			while (iter.MoveNext())
				yield return iter.Current;
		}
	}

	public interface IState {
		IEnumerator Enter(StateMachine sm);
		IEnumerator Stay(StateMachine sm);
		IEnumerator Exit(StateMachine sm);
	}

	public abstract class AbstractState : IState {
		public virtual IEnumerator Enter(StateMachine sm) { yield break; }
		public virtual IEnumerator Stay(StateMachine sm) { yield break; }
		public virtual IEnumerator Exit(StateMachine sm) { yield break; }
	}
	public abstract class AbstractStateBehaviour : MonoBehaviour, IState {
		#region IState implementation
		public virtual IEnumerator Enter (StateMachine sm) { yield break; }
		public virtual IEnumerator Stay (StateMachine sm) { yield break; }
		public virtual IEnumerator Exit (StateMachine sm) { yield break; }
		#endregion
	}
}

