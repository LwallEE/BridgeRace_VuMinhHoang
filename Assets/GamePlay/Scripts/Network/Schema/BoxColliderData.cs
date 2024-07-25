// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.34
// 

using Colyseus.Schema;
using Action = System.Action;

namespace MyGame.Schema {
	public partial class BoxColliderData : Colyseus.Schema.Schema {
		[Type(0, "ref", typeof(Vect3))]
		public Vect3 centerPosition = new Vect3();

		[Type(1, "ref", typeof(Vect3))]
		public Vect3 size = new Vect3();

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<Vect3> __centerPositionChange;
		public Action OnCenterPositionChange(PropertyChangeHandler<Vect3> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.centerPosition));
			__centerPositionChange += __handler;
			if (__immediate && this.centerPosition != null) { __handler(this.centerPosition, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(centerPosition));
				__centerPositionChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<Vect3> __sizeChange;
		public Action OnSizeChange(PropertyChangeHandler<Vect3> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.size));
			__sizeChange += __handler;
			if (__immediate && this.size != null) { __handler(this.size, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(size));
				__sizeChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				//case nameof(centerPosition): __centerPositionChange?.Invoke((Vect3) change.Value, (Vect3) change.PreviousValue); break;
				//case nameof(size): __sizeChange?.Invoke((Vect3) change.Value, (Vect3) change.PreviousValue); break;
				default: break;
			}
		}
	}
}
