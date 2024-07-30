// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.34
// 

using Colyseus.Schema;
using Action = System.Action;

namespace MyGame.Schema {
	public partial class MapData : Colyseus.Schema.Schema {
		[Type(0, "array", typeof(ArraySchema<StageData>))]
		public ArraySchema<StageData> stages = new ArraySchema<StageData>();
		
		[Type(1,"ref",typeof(Vect3))]
		public Vect3 winPosition = new Vect3();
		
		[Type(2, "map", typeof(MapSchema<BrickData>))]
		public MapSchema<BrickData> greyBricks = new MapSchema<BrickData>();

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<ArraySchema<StageData>> __stagesChange;
		public Action OnStagesChange(PropertyChangeHandler<ArraySchema<StageData>> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.stages));
			__stagesChange += __handler;
			if (__immediate && this.stages != null) { __handler(this.stages, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(stages));
				__stagesChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(stages): __stagesChange?.Invoke((ArraySchema<StageData>) change.Value, (ArraySchema<StageData>) change.PreviousValue); break;
				default: break;
			}
		}
	}
}
