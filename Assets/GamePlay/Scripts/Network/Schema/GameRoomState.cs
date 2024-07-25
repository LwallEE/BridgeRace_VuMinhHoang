// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.34
// 

using Colyseus.Schema;
using Action = System.Action;

namespace MyGame.Schema {
	public partial class GameRoomState : Colyseus.Schema.Schema {
		[Type(0, "map", typeof(MapSchema<PlayerData>))]
		public MapSchema<PlayerData> players = new MapSchema<PlayerData>();

		[Type(1, "ref", typeof(MapData))]
		public MapData map = new MapData();

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<MapSchema<PlayerData>> __playersChange;
		public Action OnPlayersChange(PropertyChangeHandler<MapSchema<PlayerData>> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.players));
			__playersChange += __handler;
			if (__immediate && this.players != null) { __handler(this.players, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(players));
				__playersChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<MapData> __mapChange;
		public Action OnMapChange(PropertyChangeHandler<MapData> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.map));
			__mapChange += __handler;
			if (__immediate && this.map != null) { __handler(this.map, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(map));
				__mapChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(players): __playersChange?.Invoke((MapSchema<PlayerData>) change.Value, (MapSchema<PlayerData>) change.PreviousValue); break;
				case nameof(map): __mapChange?.Invoke((MapData) change.Value, (MapData) change.PreviousValue); break;
				default: break;
			}
		}
	}
}
