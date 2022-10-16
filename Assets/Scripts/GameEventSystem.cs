public class GameEventSystem {
    public delegate void EventRaised(GAME_EVENT_TYPE type, System.Object data);
    public static event EventRaised EventHandler;
    public static void RaiseEvent(GAME_EVENT_TYPE type, System.Object data = null) {
        if (EventHandler != null) {
            EventHandler(type, data);
        }
    }
}

public enum GAME_EVENT_TYPE {
    ENEMY_KILL
}
