package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseCharacter;
import utility.DataReader;
import core.NetworkManager;

public class RequestCharacter extends GameRequest{

    private String character;

    // Responses
    private ResponseCharacter responseCharacter;

    public RequestCharacter() {
        responses.add(responseCharacter = new ResponseCharacter());
    }

    @Override
    public void parse() throws IOException {
        character = DataReader.readString(dataInput).trim();
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        player.setCharacter(character);
        responseCharacter.setPlayer(player);

        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseCharacter);
    }
}
