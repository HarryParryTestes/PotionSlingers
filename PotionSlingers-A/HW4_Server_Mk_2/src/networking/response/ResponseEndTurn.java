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
    private int newCurrentPlayerId;

    public ResponseEndTurn() {
        responseCode = Constants.SMSG_END_TURN;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);

        if(newCurrentPlayerId == player.getID()) {
            Log.printf("ERROR: Current player hasn't been set to the next player!");
        }

        packet.addInt32(player.getID());
        packet.addInt32(newCurrentPlayerId);

        Log.printf("Player with id %d has ended their turn", player.getID());
        Log.printf("Starting turn for Player with id %d", newCurrentPlayerId);

        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public void setData(int newCurrentPlayerId) {
        this.newCurrentPlayerId = newCurrentPlayerId;
    }
}