package networking.response;

// Other Imports
import core.GameServer;
import metadata.Constants;
import model.Player;
import utility.GamePacket;
import utility.Log;

import java.util.List;

/**
 * The ResponseLogin class contains information about the authentication
 * process.
 */
public class ResponsePotionThrow extends GameResponse {
    private Player player;
    private int throwerId;
    private int cardPosition;
    private int targetId;
    private int damage;
    private boolean isArtifact;
    private boolean isVessel;

    public ResponsePotionThrow() {
        responseCode = Constants.SMSG_P_THROW;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);

        if(player.getID() != throwerId) {
            Log.printf("ERROR: throwerId is NOT equal to this client's player in RequestPotionThrow");
        }

        packet.addInt32(player.getID());
        packet.addInt32(cardPosition);
        packet.addInt32(targetId);
        packet.addInt32(damage);
        packet.addBoolean(isArtifact);
        packet.addBoolean(isVessel);

        Log.printf("Player with id %d has thrown potion in card slot %d", player.getID(), cardPosition);

        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public void setData(int throwerId, int cardPosition, int targetId, int damage, boolean isArtifact, boolean isVessel) {
        this.throwerId = throwerId;
        this.cardPosition = cardPosition;
        this.targetId = targetId;
        this.damage = damage;
        this.isArtifact = isArtifact;
        this.isVessel = isVessel;
    }
}