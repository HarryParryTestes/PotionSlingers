package networking.response;

// Other Imports
import metadata.Constants;
import model.Player;
import utility.GamePacket;
import utility.Log;
/**
 * The ResponseLogin class contains information about the authentication
 * process.
 */
public class ResponseEndTurn extends GameResponse {
    private Player player;
    private int w;

    public ResponseEndTurn() {
        responseCode = Constants.SMSG_END_TURN;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(player.getID());
        packet.addInt32(w);

        Log.printf("Player with id %d has ended their turn", player.getID());

        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public void setData(int w) {
        this.w = w;
    }
}