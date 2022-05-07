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
public class ResponseName extends GameResponse {
    private Player player;

    public ResponseName() {
        responseCode = Constants.SMSG_SETNAME;
    }

    @Override
    public byte[] constructResponseInBytes() {
//        GamePacket packet = new GamePacket(responseCode);
//        packet.addInt32(player.getID());
//        packet.addString(player.getName());
//
//        Log.printf("Name %s set in server. for player with id %d", player.getName(), player.getID());
//
//        return packet.getBytes();
        GamePacket packet = new GamePacket(responseCode);
        GameServer gs = GameServer.getInstance();
        List<Player> activePlayers = gs.getActivePlayers();

        /*
         * numPlayers
         * P1 id
         * P1 name
         * P2 id
         * P2 name
         * */

        int numPlayers = 0;
        for(Player p : activePlayers) {
            numPlayers++;
        }
        packet.addInt32(numPlayers);
        for(Player p : activePlayers) {
            packet.addInt32(p.getID());
            packet.addString(p.getName());
        }

        Log.printf("Name %s set in server. for player with id %d", player.getName(), player.getID());

        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }
}