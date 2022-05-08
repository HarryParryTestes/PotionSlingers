package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseReady;
import core.NetworkManager;
import utility.DataReader;

public class RequestReady extends GameRequest {

    private int readyStatus;

    // Responses
    private ResponseReady responseReady;

    public RequestReady() {
        responses.add(responseReady = new ResponseReady());
    }

    @Override
    public void parse() throws IOException {
        readyStatus = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responseReady.setPlayer(player);
        responseReady.setReadyStatus(readyStatus);

        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseReady);
    }
}