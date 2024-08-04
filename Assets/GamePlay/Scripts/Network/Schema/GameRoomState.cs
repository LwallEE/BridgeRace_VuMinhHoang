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

		[Type(2, "ref", typeof(PlayerData))] public PlayerData winPlayer = new PlayerData();
		
		[Type(3, "uint8")]
		public byte gameState = default(byte);
		
		[Type(4,"map", typeof(MapSchema<NetworkUserData>))]
		public MapSchema<NetworkUserData> networkUsers = new MapSchema<NetworkUserData>();

		[Type(5, "uint8")]
		public byte minPlayerToStart = default(byte);

		[Type(6, "uint16")]
		public ushort countDownTime = default(ushort);
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
		
		protected event PropertyChangeHandler<byte> __gameStateChange;
		public Action OnGameStateChange(PropertyChangeHandler<byte> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.gameState));
			__gameStateChange += __handler;
			if (__immediate && this.gameState != default(byte)) { __handler(this.gameState, default(byte)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(gameState));
				__gameStateChange -= __handler;
			};
		}
		protected event PropertyChangeHandler<ushort> __countDownTimeChange;
		public Action OnCountDownTimeChange(PropertyChangeHandler<ushort> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.countDownTime));
			__countDownTimeChange += __handler;
			if (__immediate && this.countDownTime != default(ushort)) { __handler(this.countDownTime, default(ushort)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(countDownTime));
				__countDownTimeChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(players): __playersChange?.Invoke((MapSchema<PlayerData>) change.Value, (MapSchema<PlayerData>) change.PreviousValue); break;
				case nameof(map): __mapChange?.Invoke((MapData) change.Value, (MapData) change.PreviousValue); break;
				case nameof(gameState): __gameStateChange?.Invoke((byte)change.Value, (byte) change.PreviousValue);
					break;
				case nameof(countDownTime): __countDownTimeChange?.Invoke((ushort)change.Value, (ushort)change.PreviousValue);
					break;
				default: break;
			}
		}
	}
}
