package networking.response;

import metadata.Constants;
import model.Player;
import utility.GamePacket;
import utility.Log;

public class ResponseCharacter extends GameResponse{

    private Player player;

    public ResponseCharacter() {
        responseCode = Constants.SMSG_CHARACTER;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(player.getID());
        packet.addString(player.getCharacter());

        Log.printf("Character %s set in server. for player with id %d", player.getCharacter(), player.getID());

        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }
}
