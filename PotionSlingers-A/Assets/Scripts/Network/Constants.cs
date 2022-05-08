public class Constants
{
	// Constants
	public static readonly string CLIENT_VERSION = "1.00";
	public static readonly string REMOTE_HOST = "localhost";
	public static readonly int REMOTE_PORT = 1729;

	// Request (1xx) + Response (2xx)
	public static readonly short CMSG_JOIN = 101;
	public static readonly short SMSG_JOIN = 201;
	public static readonly short CMSG_LEAVE = 102;
	public static readonly short SMSG_LEAVE = 202;
	public static readonly short CMSG_SETNAME = 103;
	public static readonly short SMSG_SETNAME = 203;
	public static readonly short CMSG_READY = 104;
	public static readonly short SMSG_READY = 204;
	public static readonly short CMSG_MOVE = 108;
	public static readonly short SMSG_MOVE = 208;
	public static readonly short CMSG_INTERACT = 109;
	public static readonly short SMSG_INTERACT = 209;
	public static readonly short CMSG_CHARACTER = 107;
	public static readonly short SMSG_CHARACTER = 207;
    public static readonly short CMSG_P_THROW = 105;
    public static readonly short SMSG_P_THROW = 205;
    public static readonly short CMSG_END_TURN = 106;
    public static readonly short SMSG_END_TURN = 206;
    public static readonly short CMSG_BUY = 110;
    public static readonly short SMSG_BUY = 210;
    public static readonly short CMSG_SELL = 112;
    public static readonly short SMSG_SELL = 212;
    public static readonly short CMSG_CYCLE = 113;
    public static readonly short SMSG_CYCLE = 213;
    public static readonly short CMSG_TRASH = 114;
    public static readonly short SMSG_TRASH = 214;
    public static readonly short CMSG_HEARTBEAT = 111;

	public static int USER_ID = -1;
	public static int OP1_ID = -1;
	public static int OP2_ID = -1;
	public static int OP3_ID = -1;
}