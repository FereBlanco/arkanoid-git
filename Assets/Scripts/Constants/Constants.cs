namespace Scripts.Game
{
    public enum PowerUpType
    {
        None = -1, Break = 0, Catch = 1, Disruption = 2, Enlarge = 3, Laser = 4, Player = 5, Slow = 6
    }

    public enum VausState
    {
        Normal, Enlarged, Catch, Laser, Destroyed
    }

    public static class Constants
    {
        public static string AXIS_HORIZONTAL = "Horizontal";
        public static string AXIS_FIRE = "Jump";

        public static string TAG_VAUS = "Vaus";
        public static string TAG_BALL = "Ball";
        public static string TAG_BRICK = "Brick";
        public static string TAG_BULLET = "Bullet";
        public static string TAG_DOH_BULLET = "DohBullet";
        public static string TAG_POWER_UP = "PowerUp";
        public static string TAG_ENEMY = "Enemy";
        public static string TAG_LIMIT = "Limit";

        public static string PARAMETER_NORMAL = "Normal";
        public static string PARAMETER_ENLARGED = "Enlarged";
        public static string PARAMETER_CATCH = "Catch";
        public static string PARAMETER_LASER = "Laser";
        public static string PARAMETER_DESTROYED = "Destroyed";
        public static string PARAMETER_ATTACK = "Attack";
        public static string PARAMETER_HURT = "Hurt";

        public static string BOOL_DOOR_STATE = "isOpened";
    }

}