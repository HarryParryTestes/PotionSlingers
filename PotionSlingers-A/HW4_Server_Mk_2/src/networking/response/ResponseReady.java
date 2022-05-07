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
public class ResponseReady extends GameResponse {
    private Player player;
    private int readyStatus;

    public ResponseReady() {
        responseCode = Constants.SMSG_READY;
    }

    @Override
    public byte[] constructResponseInBytes() {
//        GamePacket packet = new GamePacket(responseCode);
//        packet.addInt32(player.getID());
//
//        Log.printf("Player with id %d is ready", player.getID());
//        player.setReadyStatusOn(true);
//        return packet.getBytes();

        GamePacket packet = new GamePacket(responseCode);
//        packet.addInt32(player.getID());
        if(readyStatus == 1) {
            player.setReadyStatusOn(true);
            Log.printf("Player with id %d is ready", player.getID());
        }
        else if (readyStatus == 0) {
            player.setReadyStatusOn(false);
            Log.printf("Player with id %d is NOT ready", player.getID());
        }

        /*
         * numPlayers
         * P1 id
         * P1 readyStatus(boolean)
         * P2 id
         * P2 readyStatus(boolean)
         * ...
         */

        GameServer gs = GameServer.getInstance();
        List<Player> activePlayers = gs.getActivePlayers();
        int numPlayers = 0;
        for(Player p : activePlayers) {
            numPlayers++;
        }
        packet.addInt32(numPlayers);
        for(Player p : activePlayers) {
            packet.addInt32(p.getID());
            packet.addBoolean(p.getReadyStatus());
        }
        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public void setReadyStatus(int readyStatus) {
        this.readyStatus = readyStatus;
    }
}