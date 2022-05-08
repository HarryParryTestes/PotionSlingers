package networking.response;

import core.GameServer;
import metadata.Constants;
import model.Player;
import utility.GamePacket;
import utility.Log;

import java.util.List;

public class ResponseCharacter extends GameResponse{

    private Player player;

    public ResponseCharacter() {
        responseCode = Constants.SMSG_CHARACTER;
    }

    @Override
    public byte[] constructResponseInBytes() {
//        GamePacket packet = new GamePacket(responseCode);
//        packet.addInt32(player.getID());
//        packet.addString(player.getCharacter());
//
//        Log.printf("Character %s set in server. for player with id %d", player.getCharacter(), player.getID());
//
//        return packet.getBytes();

        /*
         * numPlayers
         * P1 id
         * P1 character (string)
         * P2 id
         * P2 character (string)
         * ...
         */

        GamePacket packet = new GamePacket(responseCode);
        GameServer gs = GameServer.getInstance();
        List<Player> activePlayers = gs.getActivePlayers();

        int numPlayers = activePlayers.size();
        packet.addInt32(numPlayers);

        for(Player p : activePlayers) {
            packet.addInt32(p.getID());
            packet.addString(p.getCharacter());
        }

        Log.printf("Character %s set in server. for player with id %d", player.getCharacter(), player.getID());

        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }
}
