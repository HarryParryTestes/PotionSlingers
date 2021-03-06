package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseEndTurn;
import utility.DataReader;
import core.NetworkManager;

public class RequestEndTurn extends GameRequest {
    private int newCurrentPlayerId;
    // Responses
    private ResponseEndTurn responseEndTurn;

    public RequestEndTurn() {
        responses.add(responseEndTurn = new ResponseEndTurn());
    }

    @Override
    public void parse() throws IOException {
        newCurrentPlayerId = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responseEndTurn.setPlayer(player);
        responseEndTurn.setData(newCurrentPlayerId);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseEndTurn);
    }
}